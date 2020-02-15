using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerInfoByGroupController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly FboLinxContext _context;

        public CustomerInfoByGroupController(IHostingEnvironment hostingEnvironment, FboLinxContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        // GET: api/CustomerInfoByGroup
        [HttpGet]
        public IEnumerable<CustomerInfoByGroup> GetCustomerInfoByGroup()
        {
            return _context.CustomerInfoByGroup;
        }

        // GET: api/CustomerInfoByGroup/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerInfoByGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerInfoByGroup = await _context.CustomerInfoByGroup.FindAsync(id);

            if (customerInfoByGroup == null)
            {
                return NotFound();
            }

            return Ok(customerInfoByGroup);
        }

        // GET: api/CustomerInfoByGroup/group/5/fbo/6
        [HttpGet("group/{groupId}/fbo/{fboId}")]
        public async Task<IActionResult> GetCustomerInfoByGroupAndFBO([FromRoute] int groupId, [FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var customerInfoByGroup = await GetAllCustomersInfoByGroup().Include("Customer").Include("CustomCustomerType").Where((x => x.GroupId == groupId)).ToListAsync();

            var customerGridVM = await FetchCustomersViewModelByGroupAndFbo(groupId, fboId);

            return Ok(customerGridVM);
        }

        // GET: api/CustomerInfoByGroup/group/5/fbo/6
        [HttpGet("group/{groupId}/fbo/{fboId}/export")]
        public async Task<IActionResult> GetCustomerInfoByGroupAndFBOExport([FromRoute] int groupId, [FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //View model for customers grid
            var customerList = await FetchCustomersViewModelByGroupAndFbo(groupId, fboId);

            string rootFolder = _hostingEnvironment.WebRootPath;
            string fileName = @"ExportCustomers_" + fboId + ".xlsx";

            if (System.IO.File.Exists(Path.Combine(rootFolder, fileName)))
                System.IO.File.Delete(Path.Combine(rootFolder, fileName));


            FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));

            using (ExcelPackage package = new ExcelPackage(file))
            {

                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Customers");
                int totalRows = customerList.Count;

                worksheet.Cells[1, 1].Value = "Company";
                worksheet.Cells[1, 2].Value = "Source";
                worksheet.Cells[1, 3].Value = "Assigned Price Tier";
                worksheet.Cells[1, 4].Value = "Price";
                int i = 0;
                for (int row = 2; row <= totalRows + 1; row++)
                {
                    worksheet.Cells[row, 1].Value = customerList[i].Company;
                    worksheet.Cells[row, 2].Value = customerList[i].CustomerCompanyTypeName;
                    worksheet.Cells[row, 3].Value = customerList[i].PricingTemplateName;
                    worksheet.Cells[row, 4].Value = customerList[i].AllInPrice;
                    i++;
                }

                package.Save();

            }

            return Ok(new {fileLocation = fileName });
        }



        // GET: api/CustomerInfoByGroup/group/5/fbo/6/pricingtemplate/7
        [HttpGet("group/{groupId}/fbo/{fboId}/pricingtemplate/{pricingTemplateId}")]
        public IActionResult GetCustomersByFBOAndPricing([FromRoute] int groupId, [FromRoute] int fboId, [FromRoute] int pricingTemplateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customers = (from cg in _context.CustomerInfoByGroup.Where((x => x.GroupId == groupId))
                             join c in _context.Customers on cg.CustomerId equals c.Oid
                             join cc in _context.CustomCustomerTypes.Where((x => x.Fboid == fboId)) on cg.CustomerId equals cc.CustomerId
                             where cg.Active.GetValueOrDefault()
                                 && cg.Active.GetValueOrDefault()
                                   && (cc.CustomerType == pricingTemplateId || pricingTemplateId == 0)
                             select new
                             {
                                 cg.Oid,
                                 cg.Company,
                                 cg.CustomerId,
                                 CustomerCompanyType = cg.CustomerCompanyType.GetValueOrDefault(),
                                 cg.Address,
                                 cg.CertificateType,
                                 cg.City,
                                 cg.Country,
                                 cg.CustomerType,
                                 cg.DefaultTemplate,
                                 cg.Distribute,
                                 cg.EmailSubscription,
                                 cg.GroupId,
                                 cg.Joined,
                                 cg.MainPhone,
                                 cg.Network,
                                 cg.Suspended,
                                 cg.Sfid,
                                 cg.Show100Ll,
                                 cg.ShowJetA,
                                 cg.State,
                                 cg.Username,
                                 cg.Website,
                                 cg.ZipCode
                             }).OrderBy((x => x.Company)).Distinct();

            return Ok(customers);
        }

        // GET: api/CustomerInfoByGroup/group/5/fbo/6/count
        [HttpGet("group/{groupId}/fbo/{fboId}/count")]
        public IActionResult GetCustomerCountByGroupAndFBO([FromRoute] int groupId, [FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var customerInfoByGroup = await GetAllCustomersInfoByGroup().Include("Customer").Include("CustomCustomerType").Where((x => x.GroupId == groupId)).ToListAsync();

            //View model for customers grid
            var customerCount = (from cg in _context.CustomerInfoByGroup
                                 join c in _context.Customers on cg.CustomerId equals c.Oid
                                 where cg.GroupId == groupId
                                 && cg.Active.GetValueOrDefault()
                                 select new
                                 {
                                     cg.Oid
                                 }).Count();

            return Ok(customerCount);
        }

        // GET: api/CustomerInfoByGroup/CertificateTypes
        [HttpGet("CertificateTypes")]
        public IEnumerable<Utilities.Enum.EnumDescriptionValue> GetCertificateTypes()
        {
            return Utilities.Enum.GetDescriptions(typeof(Models.CustomerInfoByGroup.CertificateTypes));
        }

        // GET: api/CustomerInfoByGroup/CustomerSources
        [HttpGet("CustomerSources")]
        public IEnumerable<Utilities.Enum.EnumDescriptionValue> GetCustomerSources()
        {
            return Utilities.Enum.GetDescriptions(typeof(Models.Customers.CustomerSources));
        }

        // PUT: api/CustomerInfoByGroup/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerInfoByGroup([FromRoute] int id, [FromBody] CustomerInfoByGroup customerInfoByGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerInfoByGroup.Oid)
            {
                return BadRequest();
            }

            _context.Entry(customerInfoByGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerInfoByGroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CustomerInfoByGroup
        [HttpPost]
        public async Task<IActionResult> PostCustomerInfoByGroup([FromBody] CustomerInfoByGroup customerInfoByGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CustomerInfoByGroup.Add(customerInfoByGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerInfoByGroup", new { id = customerInfoByGroup.Oid }, customerInfoByGroup);
        }

        // DELETE: api/CustomerInfoByGroup/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerInfoByGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerInfoByGroup = await _context.CustomerInfoByGroup.FindAsync(id);
            if (customerInfoByGroup == null)
            {
                return NotFound();
            }

            _context.CustomerInfoByGroup.Remove(customerInfoByGroup);
            await _context.SaveChangesAsync();

            return Ok(customerInfoByGroup);
        }

        //GET: api/CustomerInfoByGroup/
        [HttpGet("group/{groupId}/fbo/{fboId}/nomargin/{count}")]
        public async Task<IActionResult> GetCustomersWithoutMarginsByGroupAndFBO([FromRoute] int groupId, [FromRoute] int fboId, [FromRoute] int count)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<CustomerInfoByGroup> customersWithoutMargins = await (from cg in _context.CustomerInfoByGroup
                                                 join c in _context.Customers on cg.CustomerId equals c.Oid
                                                 join cct in _context.CustomCustomerTypes on new { customerId = cg.CustomerId, fboId = fboId } equals
                                                     new
                                                     {
                                                         customerId = cct.CustomerId,
                                                         fboId = cct.Fboid
                                                     } into leftJoinCCT
                                                 from cct in leftJoinCCT.DefaultIfEmpty()
                                                 where cg.GroupId == groupId && cct == null
                                                 //where cct == null
                                                 select cg).ToListAsync();

            if (count == 0)
                return Ok(customersWithoutMargins);
            return Ok(customersWithoutMargins.Take(count));
        }

        private bool CustomerInfoByGroupExists(int id)
        {
            return _context.CustomerInfoByGroup.Any(e => e.Oid == id);
        }

        private IQueryable<CustomerInfoByGroup> GetAllCustomersInfoByGroup()
        {
            return _context.CustomerInfoByGroup.AsQueryable();
        }

        private async Task<List<CustomersGridViewModel>> FetchCustomersViewModelByGroupAndFbo(int groupId, int fboId)
        {
            try
            {
                FBOLinx.Web.Services.PriceFetchingService priceFetchingService = new FBOLinx.Web.Services.PriceFetchingService(_context);
                List<DTO.CustomerWithPricing> customerPricingResults = await priceFetchingService.GetCustomerPricingAsync(fboId, groupId);

                var customerAircraft =
                    from aircraftByCustomer in _context.CustomerAircrafts
                    where aircraftByCustomer.GroupId == groupId
                    group aircraftByCustomer by new { aircraftByCustomer.CustomerId }
                    into results
                    select new {
                        results.Key.CustomerId,
                        Tails = string.Join(",", results.Select(x => x.TailNumber))
                    };

                List<CustomerInfoByGroup> allCustomerInfoByGroups = await _context.CustomerInfoByGroup.ToListAsync();
                Fboprices jetaACostRecord = await _context.Fboprices.Where(x => x.Fboid == fboId && x.Product == "JetA Cost").FirstOrDefaultAsync();

                List<CustomersGridViewModel> customerGridVM = (
                    from results in customerPricingResults
                    join ci in allCustomerInfoByGroups on results.CustomerInfoByGroupId equals ci.Oid
                    join ca in customerAircraft on results.CustomerId equals ca.CustomerId
                    into leftJoinCA
                    from ca in leftJoinCA.DefaultIfEmpty()
                    group results by new
                    {
                        results.CustomerId,
                        results.CustomerInfoByGroupId,
                        results.Company,
                        results.DefaultCustomerType,
                        results.FboPrice,
                        results.FboFeeAmount,
                        results.Suspended,
                        results.FuelerLinxId,
                        results.Network,
                        results.GroupId,
                        results.NeedsAttention,
                        results.CustomerCompanyType,
                        results.CustomerCompanyTypeName,
                        results.HasBeenViewed,
                        Tails = ca?.Tails,
                        results.IsPricingExpired,
                        ci.CertificateType
                    }
                    into resultsGroup
                    select new CustomersGridViewModel()
                    {
                        CustomerId = resultsGroup.Key.CustomerId,
                        CustomerInfoByGroupId = resultsGroup.Key.CustomerInfoByGroupId,
                        Company = resultsGroup.Key.Company,
                        DefaultCustomerType = resultsGroup.Key.DefaultCustomerType,
                        FboPrice = resultsGroup.Key.FboPrice,
                        FboFeeAmount = resultsGroup.Key.FboFeeAmount,
                        Suspended = resultsGroup.Key.Suspended,
                        FuelerLinxId = resultsGroup.Key.FuelerLinxId,
                        Network = resultsGroup.Key.Network,
                        GroupId = resultsGroup.Key.GroupId,
                        PricingTemplateName = (
                        from customerPricing in resultsGroup
                        where customerPricing.PricingTemplateId > 0
                        select customerPricing.PricingTemplateName
                        ).Distinct().Count() > 1 ? "-Multiple-" : (from customerPricing in resultsGroup select customerPricing.PricingTemplateName).First(),
                        CustomerCompanyType = resultsGroup.Key.CustomerCompanyType,
                        CustomerCompanyTypeName = resultsGroup.Key.CustomerCompanyTypeName,
                        HasBeenViewed = resultsGroup.Key.HasBeenViewed,
                        SelectAll = false,
                        TailNumbers = resultsGroup.Key.Tails,
                        IsPricingExpired = resultsGroup.Key.IsPricingExpired,
                        CertificateType = resultsGroup.Key.CertificateType
                    })
                    .GroupBy(p => p.CustomerId)
                    .Select(g => g.First())
                    .ToList();

                List<PricingTemplatesGridViewModel> result = (
                    from p in _context.PricingTemplate
                    join f in (_context.Fbos.Include("Preferences")) on p.Fboid equals f.Oid
                    join cm in (
                        from c in _context.CustomerMargins
                        join tm in (_context.PriceTiers)
                        on c.PriceTierId equals tm.Oid
                        group c by new { c.TemplateId }
                        into cmResults
                        select new
                        {
                            templateId = cmResults.Key.TemplateId,
                            maxPrice = cmResults.FirstOrDefault().Amount
                        }
                    ) on p.Oid equals cm.templateId
                    into leftJoinCustomerMargins
                    from cm in leftJoinCustomerMargins.DefaultIfEmpty()
                    join fp in (
                        from f in _context.Fboprices
                        where f.EffectiveTo > DateTime.Now.AddDays(-1) && f.Fboid.GetValueOrDefault() == fboId
                        select f
                    ) on p.MarginTypeProduct equals fp.Product
                    into leftJoinFboPrices
                    from fp in leftJoinFboPrices.DefaultIfEmpty()
                    where p.Fboid == fboId
                    select new PricingTemplatesGridViewModel
                    {
                        CustomerId = p.CustomerId.GetValueOrDefault(),
                        Default = p.Default.GetValueOrDefault(),
                        Fboid = p.Fboid,
                        Margin = cm == null ? 0 : cm.maxPrice.Value,
                        MarginType = p.MarginType.GetValueOrDefault(),
                        Name = p.Name,
                        Notes = p.Notes,
                        Oid = p.Oid,
                        Type = p.Type.GetValueOrDefault(),
                        Subject = p.Subject,
                        Email = p.Email,
                        IntoPlanePrice = (jetaACostRecord == null ? 0 : jetaACostRecord.Price.GetValueOrDefault()) + (cm == null ? 0 : cm.maxPrice.Value),
                        IsInvalid = (f != null && f.Preferences != null && ((f.Preferences.OmitJetACost.GetValueOrDefault() && p.MarginType.GetValueOrDefault() == Models.PricingTemplate.MarginTypes.CostPlus) || f.Preferences.OmitJetARetail.GetValueOrDefault() && p.MarginType.GetValueOrDefault() == Models.PricingTemplate.MarginTypes.RetailMinus)) ? true : false,
                        IsPricingExpired = (fp == null && (p.MarginType == null || p.MarginType != PricingTemplate.MarginTypes.FlatFee)),
                        YourMargin = jetaACostRecord == null || jetaACostRecord.Price.GetValueOrDefault() <= 0 ? 0 : ((fp == null ? 0 : fp.Price.GetValueOrDefault()) + (cm == null ? 0 : cm.maxPrice)) - (jetaACostRecord.Price.GetValueOrDefault())
                    }).ToList();

                IEnumerable<Utilities.Enum.EnumDescriptionValue> products = FBOLinx.Web.Utilities.Enum.GetDescriptions(typeof(Models.Fboprices.FuelProductPriceTypes));
                var resultPrices =
                          from p in products
                          join f in (
                                     from f in _context.Fboprices
                                     where f.EffectiveTo > DateTime.Now.AddDays(-1) && f.Fboid == fboId
                                     select f
                          ) on new { Product = p.Description, FboId = fboId } equals new { f.Product, FboId = f.Fboid.GetValueOrDefault() }
                          into leftJoinFBOPrices
                          from f in leftJoinFBOPrices.DefaultIfEmpty()
                          join s in (from s in _context.TempAddOnMargin
                                     where s.FboId == fboId && s.EffectiveTo >= DateTime.Today.ToUniversalTime()
                                     select s) on new { FboId = fboId } equals new
                                     {
                                         FboId = s.FboId
                                     }
                              into tmpJoin
                          from s in tmpJoin.DefaultIfEmpty()
                          select new
                          {
                              Oid = f?.Oid ?? 0,
                              Fboid = fboId,
                              Product = p.Description,
                              Price = f?.Price,
                              EffectiveFrom = f?.EffectiveFrom ?? DateTime.Now,
                              EffectiveTo = f?.EffectiveTo,
                              TimeStamp = f?.Timestamp,
                              SalesTax = f?.SalesTax,
                              Currency = f?.Currency,
                              tempJet = s?.MarginJet,
                              tempAvg = s?.MarginAvgas,
                              tempId = s?.Id,
                              tempDateFrom = s?.EffectiveFrom,
                              tempDateTo = s?.EffectiveTo
                          };

                double? jetACost = resultPrices.FirstOrDefault(s => s.Product == "JetA Cost").Price;
                double? jetARetail = resultPrices.FirstOrDefault(s => s.Product == "JetA Retail").Price;
                foreach (PricingTemplatesGridViewModel res in result)
                {
                    if (res.Oid != 0)
                    {
                        CustomerMargins margins = _context.CustomerMargins.FirstOrDefault(s => s.TemplateId == res.Oid && s.PriceTierId != 0);

                        if (margins != null)
                        {
                            if (res.MarginTypeDescription == "Retail -")
                            {
                                if (jetARetail != null)
                                {
                                    res.IntoPlanePrice = jetARetail.Value - Convert.ToDouble(margins.Amount);
                                }
                                else
                                {
                                    res.IntoPlanePrice = Convert.ToDouble(margins.Amount) + 0;
                                }

                                if (jetACost != null)
                                {
                                    res.YourMargin = res.Margin;
                                }
                                else
                                {
                                    res.YourMargin = res.IntoPlanePrice - 0;
                                }
                            }
                            else if (res.MarginTypeDescription == "Cost +")
                            {
                                if (jetACost != null)
                                {
                                    res.IntoPlanePrice = Convert.ToDouble(margins.Amount) + jetACost.Value;
                                    res.YourMargin = res.IntoPlanePrice - jetACost.Value;
                                }
                                else
                                {
                                    res.IntoPlanePrice = Convert.ToDouble(margins.Amount) + 0;
                                    res.YourMargin = res.IntoPlanePrice - 0;
                                }
                            }
                        }
                    }
                }
                foreach (CustomersGridViewModel model in customerGridVM)
                {
                    model.PricingTemplatesList = result;
                    model.AllInPrice = result.Where(pt => pt.Name.Equals(model.PricingTemplateName)).Select(pt => pt.IntoPlanePrice).FirstOrDefault();
                    model.FleetSize = (from ca in _context.CustomerAircrafts
                                       where ca.GroupId.GetValueOrDefault() == groupId && ca.CustomerId == model.CustomerId
                                       select ca).Count();

                    model.Active = _context.CustomerInfoByGroup.FirstOrDefault(s => s.CustomerId == model.CustomerId && s.GroupId == model.GroupId).Active;
                    model.NeedsAttention = model.PricingTemplateName.Equals("Default Template") ? true : false;
                }

                customerGridVM = customerGridVM.OrderByDescending(s => s.FleetSize).ToList();

                return customerGridVM;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
