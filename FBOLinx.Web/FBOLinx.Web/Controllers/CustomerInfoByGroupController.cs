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
using FBOLinx.Web.Services;

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

        private Tuple<double, double> getPrices(double prevIntoPlanePrice, double? prevYourMargin, int oid, object margins, double margin, PricingTemplate.MarginTypes? marginType, 
                                        double? jetARetail, double? jetACost)
        {
            double intoPlanePrice = prevIntoPlanePrice;
            double? yourMargin = prevYourMargin;

            if (oid != 0)
            {
                if (margins != null)
                {
                    System.Reflection.PropertyInfo pi = margins.GetType().GetProperty("maxPrice");
                    double amount = (double)(pi.GetValue(margins, null));
                    string marginTypeDescription = Utilities.Enum.GetDescription(marginType ?? PricingTemplate.MarginTypes.CostPlus);
                    if (marginTypeDescription == "Retail -")
                    {
                        if (jetARetail != null)
                        {
                            intoPlanePrice = jetARetail.Value - Convert.ToDouble(amount);
                        }
                        else
                        {
                            intoPlanePrice = Convert.ToDouble(amount) + 0;
                        }

                        if (jetACost != null)
                        {
                            yourMargin = margin;
                        }
                        else
                        {
                            yourMargin = intoPlanePrice - 0;
                        }
                    }
                    else if (marginTypeDescription == "Cost +")
                    {
                        if (jetACost != null)
                        {
                            intoPlanePrice = Convert.ToDouble(amount) + jetACost.Value;
                            yourMargin = intoPlanePrice - jetACost.Value;
                        }
                        else
                        {
                            intoPlanePrice = Convert.ToDouble(amount) + 0;
                            yourMargin = intoPlanePrice - 0;
                        }
                    }
                }
            }
            return Tuple.Create(intoPlanePrice, yourMargin.GetValueOrDefault());
        }

        private async Task<List<CustomersGridViewModel>> FetchCustomersViewModelByGroupAndFbo(int groupId, int fboId)
        {
            try
            {
                Fboprices jetaACostRecord = await _context.Fboprices.Where(x => x.Fboid == fboId && x.Product == "JetA Cost").FirstOrDefaultAsync();
                var resultPrices =
                          from p in Utilities.Enum.GetDescriptions(typeof(Fboprices.FuelProductPriceTypes))
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
                                         s.FboId
                                     }
                              into tmpJoin
                          from s in tmpJoin.DefaultIfEmpty()
                          select new
                          {
                              Product = p.Description,
                              f?.Price
                          };

                double? jetACost = resultPrices.FirstOrDefault(s => s.Product == "JetA Cost").Price;
                double? jetARetail = resultPrices.FirstOrDefault(s => s.Product == "JetA Retail").Price;
                List<PricingTemplatesGridViewModel> result = 
                    (from p in
                        (
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
                        select new
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
                            YourMargin = jetaACostRecord == null || jetaACostRecord.Price.GetValueOrDefault() <= 0 ? 0 : ((fp == null ? 0 : fp.Price.GetValueOrDefault()) + (cm == null ? 0 : cm.maxPrice)) - (jetaACostRecord.Price.GetValueOrDefault()),
                            CustomerMargin = cm
                        })
                        select new PricingTemplatesGridViewModel
                        {
                            CustomerId = p.CustomerId,
                            Default = p.Default,
                            Fboid = p.Fboid,
                            Margin = p.Margin,
                            MarginType = p.MarginType,
                            Name = p.Name,
                            Notes = p.Notes,
                            Oid = p.Oid,
                            Type = p.Type,
                            Subject = p.Subject,
                            Email = p.Email,
                            IsInvalid = p.IsInvalid,
                            IsPricingExpired = p.IsPricingExpired,
                            IntoPlanePrice = getPrices(p.IntoPlanePrice, p.YourMargin, p.Oid, p.CustomerMargin, p.Margin, p.MarginType, jetARetail, jetACost).Item1,
                            YourMargin = getPrices(p.IntoPlanePrice, p.YourMargin, p.Oid, p.CustomerMargin, p.Margin, p.MarginType, jetARetail, jetACost).Item2
                        }
                    )
                    .GroupBy(x => x.Oid)
                    .Select(x => x.FirstOrDefault())
                    .ToList();

                PricingTemplate defaultPricingTemplate = await _context.PricingTemplate
                    .Where(x => x.Fboid == fboId && x.Default.GetValueOrDefault()).FirstOrDefaultAsync();
                int defaultPricingTemplateId = 0;
                if (defaultPricingTemplate != null)
                    defaultPricingTemplateId = defaultPricingTemplate.Oid;

                var customerAircraft =
                    from aircraftByCustomer in _context.CustomerAircrafts
                    where aircraftByCustomer.GroupId == groupId
                    group aircraftByCustomer by new { aircraftByCustomer.CustomerId }
                    into results
                    select new
                    {
                        results.Key.CustomerId,
                        Tails = string.Join(",", results.Select(x => x.TailNumber)),
                        Count = results.Count()
                    };

                List<CustomersGridViewModel> customerGridVM = await (
                                                    from cg in _context.CustomerInfoByGroup
                                                    join c in _context.Customers on cg.CustomerId equals c.Oid
                                                    join cct in _context.CustomCustomerTypes on new { customerId = cg.CustomerId, fboId } equals
                                                        new
                                                        {
                                                            customerId = cct.CustomerId,
                                                            fboId = cct.Fboid
                                                        } into leftJoinCCT
                                                    from cct in leftJoinCCT.DefaultIfEmpty()
                                                    join pt in _context.PricingTemplate on (cct == null || cct.CustomerType == 0 ? InsertAndGetDefaultPricingTemplateId(cg.CustomerId, fboId, defaultPricingTemplateId) : cct.CustomerType) equals pt.Oid into leftJoinPT
                                                    from pt in leftJoinPT.DefaultIfEmpty()
                                                    join ff in _context.Fbofees on new { fboId, feeType = 8 } equals new
                                                    {
                                                        fboId = ff.Fboid,
                                                        feeType = ff.FeeType
                                                    } into leftJoinFF
                                                    from ff in leftJoinFF.DefaultIfEmpty()
                                                    join fp in _context.Fboprices.Where((x =>
                                                        x.EffectiveFrom.Value < DateTime.Now &&
                                                        (!x.EffectiveTo.HasValue || x.EffectiveTo > DateTime.Now))) on new
                                                        {
                                                            fboId = (pt != null ? pt.Fboid : 0),
                                                            product = (pt != null ? pt.MarginTypeProduct : "")
                                                        } equals new
                                                        {
                                                            fboId = fp.Fboid.GetValueOrDefault(),
                                                            product = fp.Product
                                                        } into leftJoinFP
                                                    from fp in leftJoinFP.DefaultIfEmpty()
                                                    join cvf in _context.CustomersViewedByFbo on new { cg.CustomerId, Fboid = fboId } equals new
                                                    {
                                                        cvf.CustomerId,
                                                        cvf.Fboid
                                                    } into letJoinCVF
                                                    from cvf in letJoinCVF.DefaultIfEmpty()
                                                    join ccot in _context.CustomerCompanyTypes on new {
                                                            CustomerCompanyType = cg.CustomerCompanyType.GetValueOrDefault(),
                                                            cg.GroupId 
                                                        } equals new { 
                                                            CustomerCompanyType = ccot.Oid,
                                                            GroupId = ccot.GroupId == 0 ? groupId : ccot.GroupId
                                                        }
                                                    into leftJoinCCOT
                                                    from ccot in leftJoinCCOT.DefaultIfEmpty()
                                                    join ca in customerAircraft on cg.CustomerId equals ca.CustomerId
                                                    into leftJoinCA
                                                    from ca in leftJoinCA.DefaultIfEmpty()
                                                    join cc in _context.CustomerContacts on cg.CustomerId equals cc.CustomerId
                                                    into leftJoinCC
                                                    from cc in leftJoinCC.DefaultIfEmpty()
                                                    join cibg in _context.ContactInfoByGroup on new { ContactId = (cc == null ? 0 : cc.ContactId), GroupId = groupId } equals new { cibg.ContactId, cibg.GroupId }
                                                    into leftJoinCIBG
                                                    from cibg in leftJoinCIBG.DefaultIfEmpty()
                                                    join ai in result on new { Name = (string.IsNullOrEmpty(pt.Name) ? defaultPricingTemplate.Name : pt.Name) } equals new { ai.Name}
                                                    into leftJoinAi
                                                    from ai in leftJoinAi.DefaultIfEmpty()
                                                    where cg.GroupId == groupId
                                                    group cg by new
                                                    {
                                                        cg.CustomerId,
                                                        CustomerInfoByGroupId = cg.Oid,
                                                        cg.Company,
                                                        DefaultCustomerType = cg.CustomerType.GetValueOrDefault(),
                                                        FboPrice = fp == null || fp.Price == null ? 0 : fp.Price,
                                                        FboFeeAmount = ff == null || ff.FeeAmount == null ? 0 : ff.FeeAmount,
                                                        Suspended = cg.Suspended.GetValueOrDefault(),
                                                        FuelerLinxId = c.FuelerlinxId.GetValueOrDefault(),
                                                        Network = cg.Network.GetValueOrDefault(),
                                                        cg.GroupId,
                                                        CustomerCompanyType = cg.CustomerCompanyType.GetValueOrDefault(),
                                                        CustomerCompanyTypeName = ccot.Name,
                                                        ca.Tails,
                                                        CertificateType = cg.CertificateType.GetValueOrDefault(),
                                                        ContactExists = cibg == null || !cibg.CopyAlerts.HasValue ? false : cibg.CopyAlerts,
                                                        PricingTemplateName = string.IsNullOrEmpty(pt.Name) ? defaultPricingTemplate.Name : pt.Name,
                                                        HasBeenViewed = (cvf != null && cvf.Oid > 0),
                                                        IsPricingExpired = ai == null ? false : ai.IsPricingExpired,
                                                        Active = cg.Active.GetValueOrDefault(),
                                                        FleetSize = ca == null ? 0 : ca.Count,
                                                        AllInPrice = ai == null ? 0 : ai.IntoPlanePrice
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
                                                        CustomerCompanyType = resultsGroup.Key.CustomerCompanyType,
                                                        CustomerCompanyTypeName = resultsGroup.Key.CustomerCompanyTypeName,
                                                        TailNumbers = resultsGroup.Key.Tails,
                                                        HasBeenViewed = resultsGroup.Key.HasBeenViewed,
                                                        CertificateType = resultsGroup.Key.CertificateType,
                                                        IsPricingExpired = resultsGroup.Key.IsPricingExpired,
                                                        PricingTemplateName = resultsGroup.Key.PricingTemplateName,
                                                        Active = resultsGroup.Key.Active,
                                                        NeedsAttention = resultsGroup.Key.PricingTemplateName.Equals(defaultPricingTemplate.Name) ? true : false,
                                                        ContactExists = resultsGroup.Key.ContactExists,
                                                        FleetSize = resultsGroup.Key.FleetSize,
                                                        AllInPrice = resultsGroup.Key.AllInPrice,
                                                        PricingTemplatesList = result,
                                                        SelectAll = false,
                                                    })
                                                    .GroupBy(p => p.CustomerId)
                                                    .Select(g => g.FirstOrDefault())
                                                    .OrderByDescending(s => s.FleetSize.GetValueOrDefault())
                                                    .ToListAsync();
                await _context.SaveChangesAsync();

                return customerGridVM;
            }
            catch(Exception ex)
            {
                string sss = ex.Message;
                return null;
            }
        }

        private int InsertAndGetDefaultPricingTemplateId(int customerId, int fboId, int pricingTemplateId)
        {
            CustomCustomerTypes newCCT = new CustomCustomerTypes
            {
                CustomerId = customerId,
                Fboid = fboId,
                CustomerType = pricingTemplateId
            };
            _context.CustomCustomerTypes.Add(newCCT);
            return pricingTemplateId;
        }
    }
}
