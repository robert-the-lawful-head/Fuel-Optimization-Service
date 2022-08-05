using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using FBOLinx.Web.Services;
using FBOLinx.Web.Models.Responses;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Newtonsoft.Json;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.Dto.Responses;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.FboFeesAndTaxesService;
using FBOLinx.Core.Utilities.Extensions;
using FBOLinx.ServiceLayer.BusinessServices.FuelPricing;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.DTO;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerInfoByGroupController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly FboLinxContext _context;
        private readonly IPriceFetchingService _priceFetchingService;
        private readonly CustomerService _customerService;
        private readonly IFboService _fboService;
        private readonly AirportWatchService _airportWatchService;
        private readonly IPriceDistributionService _priceDistributionService;
        private readonly FuelerLinxApiService _fuelerLinxService;
        private readonly IPricingTemplateService _pricingTemplateService;
        private readonly AircraftService _aircraftService;
        private readonly DegaContext _degaContext;
        private readonly FbopricesService _fboPricesService;
        private readonly IFboFeesAndTaxesService _fboFeesAndTaxesService;

        public CustomerInfoByGroupController(IWebHostEnvironment hostingEnvironment, FboLinxContext context, CustomerService customerService, IPriceFetchingService priceFetchingService, IFboService fboService, AirportWatchService airportWatchService, IPriceDistributionService priceDistributionService, FuelerLinxApiService fuelerLinxService, IPricingTemplateService pricingTemplateService, AircraftService aircraftService, DegaContext degaContext, FbopricesService fbopricesService, IFboFeesAndTaxesService fboFeesAndTaxesService)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _priceFetchingService = priceFetchingService;
            _customerService = customerService;
            _fboService = fboService;
            _airportWatchService = airportWatchService;
            _priceDistributionService = priceDistributionService;
            _fuelerLinxService = fuelerLinxService;
            _degaContext = degaContext;
            _pricingTemplateService = pricingTemplateService;
            _aircraftService = aircraftService;
            _fboPricesService = fbopricesService;
            _fboFeesAndTaxesService = fboFeesAndTaxesService;
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

            var customerInfoByGroup = await (_context.CustomerInfoByGroup.Where(cg => cg.Oid.Equals(id))
                                                        .Include(cg => cg.Customer))

                                                        .FirstOrDefaultAsync<CustomerInfoByGroup>();

            return Ok(customerInfoByGroup);
        }

        [HttpGet("group/{groupId}/customers-with-contacts")]
        public async Task<IActionResult> GetCustomersWithContactsByGroup([FromRoute] int groupId)
        {
            var contacts = await (
                            from c in _context.Customers
                            join cc in _context.CustomerContacts on c.Oid equals cc.CustomerId
                            join t in _context.Contacts on cc.ContactId equals t.Oid
                            join cibg in _context.ContactInfoByGroup on t.Oid equals cibg.ContactId
                            where cibg.GroupId == groupId
                            select new GroupCustomerContact
                            {
                                FirstName = cibg.FirstName,
                                LastName = cibg.LastName,
                                Email = cibg.Email,
                                CustomerId = c.Oid,
                            })
                            .Distinct()
                            .ToListAsync();

            var result = await (
                            from cg in _context.CustomerInfoByGroup
                            join c in _context.Customers on cg.CustomerId equals c.Oid
                            where cg.GroupId == groupId && (!c.Suspended.HasValue || !c.Suspended.Value)
                            group cg by new
                            {
                                cg.CustomerId,
                                cg.Company
                            } into groupedResult
                            select new GroupCustomerWithContactsResponse
                            {
                                CustomerId = groupedResult.Key.CustomerId,
                                Company = groupedResult.Key.Company,
                            })
                            .Distinct()
                            .ToListAsync();

            result.ForEach(row =>
            {
                row.Contacts = contacts.Where(c => c.CustomerId == row.CustomerId).ToList();
            });

            return Ok(result);
        }


        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetCustomersByGroup([FromRoute] int groupId)
        {
            List<CustomersGridViewModel> customerGridVM = await (
                    from cg in _context.CustomerInfoByGroup
                    join c in _context.Customers on cg.CustomerId equals c.Oid
                    join cct in _context.CustomCustomerTypes on c.Oid equals cct.CustomerId
                    where cg.GroupId == groupId && c.Suspended != true
                    group cg by new
                    {
                        cg.CustomerId,
                        CustomerInfoByGroupId = cg.Oid,
                        cg.Company,
                        FuelerLinxId = c.FuelerlinxId,
                        CertificateType = cg.CertificateType,
                        Active = cg.Active,
                    }
                    into resultsGroup
                    select new CustomersGridViewModel()
                    {
                        CustomerInfoByGroupId = resultsGroup.Key.CustomerInfoByGroupId,
                        Active = resultsGroup.Key.Active,
                        CertificateType = resultsGroup.Key.CertificateType,
                        CustomerId = resultsGroup.Key.CustomerId,
                        Company = resultsGroup.Key.Company,
                        IsFuelerLinxCustomer = resultsGroup.Key.FuelerLinxId > 0,
                    }).ToListAsync();

            return Ok(customerGridVM);
        }

        // GET: api/CustomerInfoByGroup/group/5/fbo/6
        [HttpGet("group/{groupId}/fbo/{fboId}")]
        public async Task<IActionResult> GetCustomerInfoByGroupAndFBO([FromRoute] int groupId, [FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerGridVM = (await FetchCustomersViewModelByGroupAndFbo(groupId, fboId)).OrderBy(x => x.Company).ToList();

            return Ok(customerGridVM);
        }

        [HttpGet("group/{groupId}/fbo/{fboId}/needs-attention")]
        public async Task<IActionResult> GetNeedsAttentionCustomerInfoByGroupAndFBO([FromRoute] int groupId, [FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var needsAttentionCustomers = await _customerService.GetCustomersNeedingAttentionByGroupFbo(groupId, fboId);

            return Ok(needsAttentionCustomers);
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

            return Ok(new { fileLocation = fileName });
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
                             where (cg.Active ?? false)
                                   && (cc.CustomerType == pricingTemplateId || pricingTemplateId == 0)
                             select new
                             {
                                 Oid = cg.Oid,
                                 cg.Company,
                                 cg.CustomerId,
                                 CustomerCompanyType = (cg.CustomerCompanyType ?? 0),
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

            //View model for customers grid
            var customerCount = (from cg in _context.CustomerInfoByGroup
                                 join c in _context.Customers on cg.CustomerId equals c.Oid
                                 join cvf in _context.CustomersViewedByFbo on new { cg.CustomerId, Fboid = fboId } equals new
                                 {
                                     cvf.CustomerId,
                                     cvf.Fboid
                                 } into letJoinCVF
                                 from cvf in letJoinCVF.DefaultIfEmpty()
                                 where cg.GroupId == groupId && !(c.Suspended ?? false)
                                 select new
                                 {
                                     Oid = cg.Oid
                                 }).Count();

            return Ok(customerCount);
        }

        [HttpGet("group/{groupId}/fbo/{fboId}/list")]
        public async Task<IActionResult> GetCustomersListAsync([FromRoute] int groupId, [FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //View model for customers dropdown list
            var list = await _customerService.GetCustomersListByGroupAndFbo(groupId, fboId);

            return Ok(list);
        }

        // GET: api/CustomerInfoByGroup/CertificateTypes
        [HttpGet("CertificateTypes")]
        public IEnumerable<Core.Utilities.Enum.EnumDescriptionValue> GetCertificateTypes()
        {
            return Core.Utilities.Enum.GetDescriptions(typeof(CertificateTypes));
        }

        // GET: api/CustomerInfoByGroup/CustomerSources
        [HttpGet("CustomerSources")]
        public IEnumerable<Core.Utilities.Enum.EnumDescriptionValue> GetCustomerSources()
        {
            return Core.Utilities.Enum.GetDescriptions(typeof(CustomerSources));
        }

        //GET : api/CustomerInfoByGroup/GetCustomerLogger
        [HttpGet("CustomerLogger/customer/{customerId}/fbo/{fboId}")]
        public async Task<IActionResult> GetCustomerLogger([FromRoute] int customerId, int fboId)
        {
            try
            {
                CustomerInfoByGroup customerInfoByGroup = await _context.CustomerInfoByGroup.FirstOrDefaultAsync(x => x.Oid == customerId);
                List<CustomerInfoLoggerVM> customerInfoLoggerList = new List<CustomerInfoLoggerVM>();

                if (customerInfoByGroup != null)
                {
                    var dataLog = await _context.AuditsLogs.Where(a => a.CustomerId == customerInfoByGroup.CustomerId && a.GroupId == customerInfoByGroup.GroupId).ToListAsync();

                    if (dataLog.Count > 0)
                    {
                        var pricingTemplates = await _context.PricingTemplate.Where(p => p.Fboid == fboId).ToListAsync();
                        var aircrafts = await _degaContext.AirCrafts.ToListAsync();

                        foreach (var item in dataLog)
                        {
                            CustomerInfoLoggerVM infoLoggerVM = new CustomerInfoLoggerVM();
                            User user = await _context.User.FirstOrDefaultAsync(u => u.Oid == item.UserId);

                            if (item.TableName == "CustomCustomerTypes")
                            {
                                if (item.Type == AuditType.Update.ToString())
                                {
                                    CustomCustomerTypes OldCustomCustomerTypes = JsonConvert.DeserializeObject<CustomCustomerTypes>(item.OldValues);
                                    CustomCustomerTypes NewCustomCustomerTypes = JsonConvert.DeserializeObject<CustomCustomerTypes>(item.NewValues);

                                    if (OldCustomCustomerTypes != null && NewCustomCustomerTypes != null)
                                    {

                                        infoLoggerVM.OldPricingTemplate = pricingTemplates.FirstOrDefault(x => x.Oid == OldCustomCustomerTypes.CustomerType);
                                        infoLoggerVM.NewPricingTemplate = pricingTemplates.FirstOrDefault(x => x.Oid == NewCustomCustomerTypes.CustomerType);

                                        infoLoggerVM.Action = FBOLinx.Core.Utilities.Enum.GetDescription(AuditEntryType.ItpTemplateAssigned);
                                    }
                                }
                            }
                            else if (item.TableName == "CustomerAircrafts")
                            {
                                //New CustomerAircraft 
                                if (item.Type == AuditType.Create.ToString())
                                {
                                    infoLoggerVM.NewCustomerAircrafts = JsonConvert.DeserializeObject<CustomerAircrafts>(item.NewValues);
                                    if (infoLoggerVM.NewCustomerAircrafts != null)
                                    {
                                        infoLoggerVM.NewAircaft = _degaContext.AirCrafts.FirstOrDefault(a => a.AircraftId == infoLoggerVM.NewCustomerAircrafts.AircraftId);
                                        infoLoggerVM.Action = FBOLinx.Core.Utilities.Enum.GetDescription(AuditEntryType.AircaftAdded);
                                    }
                                }

                                //Delete CustomerAircraft 
                                else if (item.Type == AuditType.Delete.ToString())
                                {
                                    infoLoggerVM.OldCustomerAircrafts = JsonConvert.DeserializeObject<CustomerAircrafts>(item.OldValues);
                                    if (infoLoggerVM.OldCustomerAircrafts != null)
                                    {
                                        infoLoggerVM.OldAircaft = aircrafts.FirstOrDefault(a => a.AircraftId == infoLoggerVM.OldCustomerAircrafts.AircraftId);
                                        infoLoggerVM.Action = FBOLinx.Core.Utilities.Enum.GetDescription(AuditEntryType.AircraftDeleted);
                                    }
                                }
                            }
                            else if (item.TableName == "ContactInfoByGroup")
                            {
                                //New Contact
                                if (item.Type == AuditType.Create.ToString())
                                {
                                    infoLoggerVM.NewContact = JsonConvert.DeserializeObject<ContactInfoByGroup>(item.NewValues);
                                    infoLoggerVM.Action = FBOLinx.Core.Utilities.Enum.GetDescription(AuditEntryType.ContactAdded);
                                }

                                //Delete Contact
                                if (item.Type == AuditType.Delete.ToString())
                                {

                                    infoLoggerVM.OldContact = JsonConvert.DeserializeObject<ContactInfoByGroup>(item.OldValues);
                                    infoLoggerVM.Action = FBOLinx.Core.Utilities.Enum.GetDescription(AuditEntryType.ContactDeleted);
                                }
                            }
                            else if (item.TableName == "CustomerInfoByGroup")
                            {
                                //in Case Add New Customer 
                                if (item.Type == AuditType.Create.ToString())
                                {
                                    infoLoggerVM.NewCustomerInfoByGroup = JsonConvert.DeserializeObject<CustomerInfoByGroup>(item.NewValues);
                                    infoLoggerVM.Action = infoLoggerVM.NewCustomerInfoByGroup != null ? FBOLinx.Core.Utilities.Enum.GetDescription(AuditEntryType.Created) : null;

                                }
                                //in Case Edit Exsit Customer 
                                else if (item.Type == AuditType.Update.ToString())
                                {
                                    infoLoggerVM.OldCustomerInfoByGroup = JsonConvert.DeserializeObject<CustomerInfoByGroup>(item.OldValues);
                                    infoLoggerVM.NewCustomerInfoByGroup = JsonConvert.DeserializeObject<CustomerInfoByGroup>(item.NewValues);

                                    if (infoLoggerVM.OldCustomerInfoByGroup != null && infoLoggerVM.NewCustomerInfoByGroup != null)
                                    {
                                        if (infoLoggerVM.OldCustomerInfoByGroup.Active.GetValueOrDefault() && !infoLoggerVM.NewCustomerInfoByGroup.Active.GetValueOrDefault())
                                        {
                                            infoLoggerVM.Action = FBOLinx.Core.Utilities.Enum.GetDescription(AuditEntryType.Deactivated);
                                        }
                                        else if (!infoLoggerVM.OldCustomerInfoByGroup.Active.GetValueOrDefault() && infoLoggerVM.NewCustomerInfoByGroup.Active.GetValueOrDefault())
                                        {
                                            infoLoggerVM.Action = FBOLinx.Core.Utilities.Enum.GetDescription(AuditEntryType.Acitviated);
                                        }
                                        else
                                        {
                                            infoLoggerVM.Action = FBOLinx.Core.Utilities.Enum.GetDescription(AuditEntryType.Edited);
                                        }
                                    }
                                }
                            }

                            //Add Item to list
                            if (infoLoggerVM.Action != null)
                            {
                                if (user != null)
                                {
                                    infoLoggerVM.Username = user.FirstName;
                                    infoLoggerVM.Role = FBOLinx.Core.Utilities.Enum.GetDescription(user.Role);
                                }
                                infoLoggerVM.TableName = item.TableName;
                                infoLoggerVM.Type = item.Type;
                                infoLoggerVM.DateTime = item.DateTime;
                                customerInfoLoggerList.Add(infoLoggerVM);
                            }
                        }
                    }
                }

                return Ok(customerInfoLoggerList.OrderByDescending(x => x.DateTime));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }

        // PUT: api/CustomerInfoByGroup/5
        [HttpPut("{id}/{userId}")]
        public async Task<IActionResult> PutCustomerInfoByGroup([FromRoute] int id, [FromRoute] int userId, [FromBody] CustomerInfoByGroup customerInfoByGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerInfoByGroup.Oid)
            {
                return BadRequest();
            }


            try
            {
                CustomerInfoByGroup oldCustomer = _context.CustomerInfoByGroup.FirstOrDefault(c => c.Oid == customerInfoByGroup.Oid);

                if (oldCustomer != null)
                {
                    if (_customerService.CompareCustomers(oldCustomer, customerInfoByGroup) == false)
                    {
                        oldCustomer.Active = customerInfoByGroup.Active;
                        oldCustomer.Address = customerInfoByGroup.Address;
                        oldCustomer.CertificateType = customerInfoByGroup.CertificateType;
                        oldCustomer.City = customerInfoByGroup.City;
                        oldCustomer.Company = customerInfoByGroup.Company;
                        oldCustomer.Country = customerInfoByGroup.Country;
                        oldCustomer.CustomerCompanyType = customerInfoByGroup.CustomerCompanyType;
                        oldCustomer.Distribute = customerInfoByGroup.Distribute;
                        oldCustomer.EmailSubscription = customerInfoByGroup.EmailSubscription;
                        oldCustomer.MainPhone = customerInfoByGroup.MainPhone;
                        oldCustomer.Show100Ll = customerInfoByGroup.Show100Ll;
                        oldCustomer.ShowJetA = customerInfoByGroup.ShowJetA;
                        oldCustomer.State = customerInfoByGroup.State;
                        oldCustomer.Website = customerInfoByGroup.Website;
                        oldCustomer.Website = customerInfoByGroup.Website;

                        await _context.SaveChangesAsync(userId, customerInfoByGroup.CustomerId, customerInfoByGroup.GroupId);
                    }
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerInfoByGroupExists(id))
                {
                    return NotFound();
                }
            }

            return NoContent();

        }


        // POST: api/CustomerInfoByGroup
        [HttpPost("{userId}")]
        public async Task<IActionResult> PostCustomerInfoByGroup([FromRoute] int userId, [FromBody] CustomerInfoByGroup customerInfoByGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            customerInfoByGroup.Active = true;
            _context.CustomerInfoByGroup.Add(customerInfoByGroup);
            await _context.SaveChangesAsync(userId, customerInfoByGroup.CustomerId, customerInfoByGroup.GroupId);

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
                                                                       where cct == null
                                                                       select cg).ToListAsync();

            if (count == 0)
                return Ok(customersWithoutMargins);
            return Ok(customersWithoutMargins.Take(count));
        }

        [HttpGet("matchcustomerinfo/customerId/{customerId}/groupId/{groupId}")]
        public PotentialCustomerMatchVM MatchCustomerInfo(int customerId, int groupId)
        {
            if (customerId != 0)
            {
                var isMergeRejected = _context.CustomerInfoByGroup.FirstOrDefault(s => s.CustomerId == customerId).MergeRejected;
                if (isMergeRejected == null || isMergeRejected.Equals(false))
                {
                    // var custAircrafts = _context.CustomerAircrafts.Where(s => s.CustomerId == customerId && s.GroupId == groupId).Select(s => s.TailNumber).ToList();

                    var custAircrafts = (from ca in _context.CustomerAircrafts
                                         join c in _context.CustomerInfoByGroup
                                         on ca.CustomerId equals c.CustomerId
                                         where ca.GroupId == groupId
                                       && ca.CustomerId == customerId
                                         select new
                                         {
                                             CustomerId = ca.CustomerId,
                                             TailNumber = ca.TailNumber,
                                             Customer = c.Company
                                         }).ToList();

                    var custInfo = _context.CustomerInfoByGroup.FirstOrDefault(s => s.CustomerId == customerId);
                    PotentialCustomerMatchVM newCustomerMatch = new PotentialCustomerMatchVM();
                    if (custAircrafts.Count > 1)
                    {
                        try
                        {
                            var ooAircrafts = (from ca in _context.CustomerAircrafts
                                               join c in _context.Customers
                                               on ca.CustomerId equals c.Oid
                                               where ca.GroupId == groupId
                                               && ca.CustomerId != customerId
                                               && custAircrafts.ToList().Select(x => x.TailNumber).Contains(ca.TailNumber)
                                               select new
                                               {
                                                   ca.CustomerId,
                                                   ca.TailNumber
                                               }).ToList();

                            var result = (from f in custAircrafts
                                          join o in ooAircrafts
                                          on f.TailNumber equals o.TailNumber
                                          select o).ToList();

                            if (result.Count() > 0)
                            {
                                newCustomerMatch.AircraftTails = result.Select(s => s.TailNumber).ToList();
                                newCustomerMatch.CurrentCustomerId = customerId;
                                newCustomerMatch.CurrentCustomerName = custAircrafts[0].Customer;

                                var matchCustomerName = _context.CustomerInfoByGroup.FirstOrDefault(s => s.CustomerId == result[0].CustomerId && s.GroupId == groupId);

                                newCustomerMatch.MatchCustomerId = matchCustomerName.CustomerId;
                                newCustomerMatch.MatchCustomerOid = matchCustomerName.Oid;
                                newCustomerMatch.MatchCustomerName = matchCustomerName.Company;

                                newCustomerMatch.IsAircraftMatch = true;

                            }
                        }
                        catch (Exception ex)
                        {
                            var ee = ex;
                        }

                    }

                    var custNameCheck = _context.CustomerInfoByGroup.FirstOrDefault(s => s.Company == custInfo.Company && s.CustomerId != custInfo.CustomerId && s.GroupId == groupId);

                    if (custNameCheck != null)
                    {
                        newCustomerMatch.IsNameMatch = true;

                        newCustomerMatch.MatchNameCustomerId = custNameCheck.CustomerId;
                        newCustomerMatch.MatchNameCustomerOid = custNameCheck.Oid;
                        newCustomerMatch.MatchNameCustomer = custNameCheck.Company;
                    }

                    var custContacts = _context.CustomerContacts.Where(s => s.CustomerId == custInfo.CustomerId).ToList();

                    if (custContacts != null)
                    {
                        var contactEmails = _context.ContactInfoByGroup.Where(s => s.ContactId == custContacts[0].ContactId).Select(s => s.Email).ToList();

                        if (contactEmails.Count > 0)
                        {
                            var checkOtherContacts = _context.ContactInfoByGroup.Where(s => contactEmails.Contains(s.Email)).Select(s => s.ContactId).ToList();

                            if (checkOtherContacts.Count > 0)
                            {
                                var otherContacts = _context.CustomerContacts.Where(s => checkOtherContacts.Contains(s.ContactId) && s.CustomerId != customerId).Select(s => s.CustomerId).FirstOrDefault();

                                if (otherContacts != 0)
                                {
                                    var custInfoValue = _context.CustomerInfoByGroup.FirstOrDefault(s => s.CustomerId == otherContacts && s.GroupId == groupId);
                                    newCustomerMatch.IsContactMatch = true;
                                    newCustomerMatch.MatchContactCustomerId = custInfoValue.CustomerId;
                                    newCustomerMatch.MatchContactCustomerOid = custInfoValue.Oid;
                                }
                            }
                        }
                    }

                    return newCustomerMatch;
                }
            }

            return null;
        }

        [HttpGet("fuelvendors")]
        public async Task<IActionResult> GetFuelVendors()
        {
            var customerFuelVendors = await _fuelerLinxService.GetCustomerFuelVendors();
            var fuelVendors = customerFuelVendors
                .Select(cf => cf.FuelVendors.Split(";"))
                .SelectMany(i => i)
                .Distinct()
                .Select(i => i.Trim())
                .OrderBy(i => i)
                .ToList();
            return Ok(fuelVendors);
        }

        [HttpPost("rejectmergeforcustomer/customerId/{customerId}/groupId/{groupId}")]
        public async Task<IActionResult> RejectMergeForCustomer(int customerId, int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = _context.CustomerInfoByGroup.Where(s => s.CustomerId == customerId).ToList();
            foreach (var cust in customer)
            {
                cust.MergeRejected = true;
                _context.CustomerInfoByGroup.Update(cust);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpPost("mergecustomers/customerId/{customerId}/flcustomerId/{flcustomerId}/groupId/{groupId}")]
        public async Task<IActionResult> MergeCustomers(int customerId, int flcustomerid, int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = _context.CustomerInfoByGroup.FirstOrDefault(s => s.CustomerId == customerId);

            var flCustAircrafts = await (from ca in _context.CustomerAircrafts
                                         where ca.GroupId == groupId
                                         && ca.CustomerId == flcustomerid
                                         select new
                                         {
                                             CustomerId = ca.CustomerId,
                                             TailNumber = ca.TailNumber,
                                             AircraftId = ca.Oid
                                         }).ToListAsync();

            var customerAircrafts = await (from ca in _context.CustomerAircrafts
                                           where ca.GroupId == groupId
                                           && ca.CustomerId == customerId
                                           select new
                                           {
                                               CustomerId = ca.CustomerId,
                                               TailNumber = ca.TailNumber,
                                               AircraftId = ca.Oid
                                           }).ToListAsync();

            var result = customerAircrafts.Where(p => !flCustAircrafts.Any(p2 => p2.TailNumber == p.TailNumber)).ToList();

            if (result.Count > 0)
            {
                foreach (var aircraft in result.ToList())
                {
                    var aircraftProp = await _context.CustomerAircrafts.FirstOrDefaultAsync(s => s.Oid == aircraft.AircraftId);

                    if (aircraftProp != null)
                    {
                        aircraftProp.CustomerId = flcustomerid;
                        _context.CustomerAircrafts.Update(aircraftProp);
                        await _context.SaveChangesAsync();
                    }
                }
            }


            var custContacts = await _context.CustomerContacts.Where(s => s.CustomerId == customerId).ToListAsync();

            var custContactsList = await (from cc in _context.CustomerContacts
                                          join c in _context.ContactInfoByGroup
                                          on cc.ContactId equals c.ContactId
                                          where cc.CustomerId == customerId && c.GroupId == groupId
                                          select new
                                          {
                                              ContactId = c.Oid,
                                              ContactEmail = c.Email,
                                              ContactOid = cc.Oid
                                          }).ToListAsync();

            var custContactsListFl = await (from cc in _context.CustomerContacts
                                            join c in _context.ContactInfoByGroup
                                            on cc.ContactId equals c.ContactId
                                            where cc.CustomerId == flcustomerid && c.GroupId == groupId
                                            select new
                                            {
                                                ContactId = c.Oid,
                                                ContactEmail = c.Email,
                                                ContactOid = cc.Oid
                                            }).ToListAsync();

            var resultContact = custContactsList.Where(p => !custContactsListFl.Any(p2 => p2.ContactEmail == p.ContactEmail)).ToList();

            if (resultContact.Count > 0)
            {
                foreach (var contact in resultContact)
                {
                    var custContact = await _context.CustomerContacts.FirstOrDefaultAsync(s => s.Oid == contact.ContactOid);

                    if (custContact != null)
                    {
                        custContact.CustomerId = flcustomerid;
                        _context.CustomerContacts.Update(custContact);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            if (customer != null)
            {
                var customerEntry = await _context.Customers.FirstOrDefaultAsync(s => s.Oid == customer.CustomerId);

                _context.CustomerInfoByGroup.Remove(customer);
                _context.Customers.Remove(customerEntry);
                await _context.SaveChangesAsync();
            }

            var Flcustomerobject = await _context.CustomerInfoByGroup.FirstOrDefaultAsync(s => s.CustomerId == flcustomerid);

            //return NoContent();
            return new JsonResult(Flcustomerobject.Oid);
        }

        [HttpPost("group-analytics/group/{groupId}")]
        public async Task<IActionResult> GetGroupAnalytics([FromRoute] int groupId, [FromBody] int customerId)
        {
            var result = await GetCustomerPricingAnalytics(groupId, customerId);

            return Ok(result);
        }

        [HttpPost("group-analytics/email/group/{groupId}")]
        public async Task<IActionResult> EmailGroupAnalytics([FromRoute] int groupId, [FromBody] int customerId)
        {
            var contacts = await (
                                from c in _context.Customers
                                join cc in _context.CustomerContacts on c.Oid equals cc.CustomerId
                                join t in _context.Contacts on cc.ContactId equals t.Oid
                                join cibg in _context.ContactInfoByGroup on t.Oid equals cibg.ContactId
                                where cibg.GroupId == groupId && c.Oid == customerId
                                select cibg.Email)
                            .Distinct()
                            .ToListAsync();

            if (contacts.Count > 0)
            {
                var result = await GetCustomerPricingAnalytics(groupId, customerId);
                await _priceDistributionService.SendCustomerPriceEmail(groupId, result, contacts);
            }

            return Ok(true);
        }


        private bool CustomerInfoByGroupExists(int id)
        {
            return _context.CustomerInfoByGroup.Any(e => e.Oid == id);
        }

        private async Task<List<CustomersGridViewModel>> FetchCustomersViewModelByGroupAndFbo(int groupId, int fboId)
        {
            try
            {
                List<PricingTemplateGrid> pricingTemplatesCollection = await _pricingTemplateService.GetPricingTemplates(fboId, groupId);

                var fboIcao = await _fboService.GetFBOIcao(fboId);

                PricingTemplateDto defaultPricingTemplate = await _pricingTemplateService.GetDefaultTemplate(fboId);
                if (defaultPricingTemplate == null)
                    defaultPricingTemplate = new PricingTemplateDto();

                await _pricingTemplateService.FixCustomCustomerTypes(groupId, fboId);

                var companyTypes = await _context.CustomerCompanyTypes.Where(x => x.Fboid == fboId || x.Fboid == 0).ToListAsync();

                var customerAircraft =
                    (from aircraftByCustomer in (await _context.CustomerAircrafts.Where(x => x.GroupId == groupId).ToListAsync())
                     where aircraftByCustomer.GroupId == groupId
                     group aircraftByCustomer by new { aircraftByCustomer.CustomerId }
                     into results
                     select new
                     {
                         results.Key.CustomerId,
                         Tails = string.Join(",", results.Select(x => x.TailNumber)),
                         Count = results.Count()
                     }
                     ).ToList();

                var customerTags = await _context.CustomerTag.Where(x => x.GroupId == groupId).ToListAsync();

                var needsAttentionCustomers = await _customerService.GetCustomersNeedingAttentionByGroupFbo(groupId, fboId);

                var customerInfoByGroupCollection = await _customerService.GetCustomersByGroupAndFbo(groupId, fboId);

                var contactInfoByFboForAlerts =
                    await (from cibg in _context.ContactInfoByGroup
                           join c in _context.Contacts on cibg.ContactId equals c.Oid
                           join cibf in _context.Set<ContactInfoByFbo>() on new { ContactId = c.Oid, FboId = fboId } equals new { ContactId = cibf.ContactId.GetValueOrDefault(), FboId = cibf.FboId.GetValueOrDefault() } into leftJoinCIBF
                           from cibf in leftJoinCIBF.DefaultIfEmpty()
                           where cibg.GroupId == groupId
                           select new { cibg.ContactId, CopyAlerts = cibf.ContactId == null ? cibg.CopyAlerts : cibf.CopyAlerts }).ToListAsync();

                //var historicalData = await _airportWatchService.GetHistoricalDataAssociatedWithGroupOrFbo(groupId, fboId, new AirportWatchHistoricalDataRequest { StartDateTime = null, EndDateTime = null });

                var customerFuelVendors = await _fuelerLinxService.GetCustomerFuelVendors();

                var customerMargins = await (from pt in _context.PricingTemplate
                                             join cm in _context.CustomerMargins on pt.Oid equals cm.TemplateId
                                                  into leftJoinCm
                                             from cm in leftJoinCm.DefaultIfEmpty()
                                             join prt in _context.PriceTiers on
                                                 new { PriceTierId = cm.PriceTierId, Min = 1.0 } equals
                                                 new { PriceTierId = prt.Oid, Min = prt.Min.GetValueOrDefault() }
                                             into leftJoinPrt
                                             from prt in leftJoinPrt.DefaultIfEmpty()
                                             where pt.Fboid == fboId && prt != null
                                             select new { PricingTemplateId = pt.Oid, cm.Amount, cm.PriceTier.Min, cm.PriceTier.Max }).ToListAsync();

                List<CustomersGridViewModel> customerGridVM = (
                        from cg in customerInfoByGroupCollection
                        join ccot in companyTypes on cg.CustomerCompanyType equals ccot.Oid into leftJoinCCOT
                        from ccot in leftJoinCCOT.DefaultIfEmpty()
                        join ai in pricingTemplatesCollection on new
                        {
                            TemplateId = ((cg.Customer?.CustomCustomerType?.CustomerType).GetValueOrDefault() == 0
                                    ? defaultPricingTemplate.Oid
                                    : cg.Customer?.CustomCustomerType?.CustomerType).GetValueOrDefault()
                        } equals new { TemplateId = ai.Oid }
                            into leftJoinAi
                        from ai in leftJoinAi.DefaultIfEmpty()
                        join cm in customerMargins on (ai == null ? 0 : ai.Oid) equals cm.PricingTemplateId
                        into leftJoinCm
                        from cm in leftJoinCm.DefaultIfEmpty()
                            //join hd in historicalData on cg.CustomerId equals hd.CustomerId into leftJoinHd
                            //from hd in leftJoinHd.DefaultIfEmpty()
                        join cv in customerFuelVendors on cg.Customer.FuelerlinxId equals cv.FuelerLinxId into leftJoinCv
                        from cv in leftJoinCv.DefaultIfEmpty()
                        where cg.GroupId == groupId && !(cg.Suspended ?? false)

                        group new { cg } by new //, hd 
                        {
                            cg.CustomerId,
                            CustomerInfoByGroupId = cg.Oid,
                            cg.Company,
                            FuelerLinxId = (cg.Customer?.FuelerlinxId ?? 0),
                            CustomerCompanyTypeName = ccot?.Name,
                            CertificateType = (cg.CertificateType ?? CertificateTypes.NotSet),
                            ContactExists = contactInfoByFboForAlerts.Any(c =>
                           (cg.Customer?.CustomerContacts?.Any(cc => cc.ContactId == c.ContactId)) == true && c.CopyAlerts == true),
                            PricingTemplateName = string.IsNullOrEmpty(ai?.Name) ? defaultPricingTemplate.Name : ai.Name,
                            IsPricingExpired = ai != null && ai.IsPricingExpired,
                            Active = (cg.Active ?? false),
                            Tails = customerAircraft.FirstOrDefault(x => x.CustomerId == cg.CustomerId)?.Tails,
                            FleetSize = customerAircraft.FirstOrDefault(x => x.CustomerId == cg.CustomerId)?.Count,
                            PricingTemplateId = (ai?.Oid).GetValueOrDefault() == 0 ? defaultPricingTemplate.Oid : ai.Oid,
                            FuelVendors = cv == null ? "" : cv.FuelVendors,
                            Tags = customerTags.Where(x => x.CustomerId == cg.CustomerId),
                            PricingFormula = ai == null ?
                                                (FBOLinx.Core.Utilities.Enums.EnumHelper.GetDescription(defaultPricingTemplate.MarginType) + " " +
                                                    (defaultPricingTemplate.DiscountType == DiscountTypes.Percentage ?
                                                        defaultPricingTemplate.DefaultAmount.ToString() + "%"
                                                        : string.Format("{0:C}", defaultPricingTemplate.DefaultAmount.GetValueOrDefault())))
                                                : (ai.MarginTypeDescription + " " + (ai.DiscountType == DiscountTypes.Percentage ?
                                                    cm == null ? "0" : cm.Amount.ToString() + "%"
                                                    : string.Format("{0:C}", (cm == null ? 0 : cm.Amount.GetValueOrDefault()))))
                        }
                        into resultsGroup
                        select new CustomersGridViewModel()
                        {
                            CustomerInfoByGroupId = resultsGroup.Key.CustomerInfoByGroupId,
                            Active = resultsGroup.Key.Active,
                            CertificateType = resultsGroup.Key.CertificateType,
                            CustomerId = resultsGroup.Key.CustomerId,
                            CustomerCompanyTypeName = resultsGroup.Key.CustomerCompanyTypeName,
                            ContactExists = resultsGroup.Key.ContactExists,
                            Company = resultsGroup.Key.Company,
                            FleetSize = resultsGroup.Key.FleetSize,
                            IsFuelerLinxCustomer = resultsGroup.Key.FuelerLinxId > 0,
                            IsPricingExpired = resultsGroup.Key.IsPricingExpired,
                            PricingTemplateId = resultsGroup.Key.PricingTemplateId,
                            PricingTemplateName = resultsGroup.Key.PricingTemplateName,
                            SelectAll = false,
                            TailNumbers = resultsGroup.Key.Tails,
                            //AircraftsVisits = resultsGroup.Count(a => a.hd != null && a.hd.AircraftStatus == AirportWatchHistoricalData.AircraftStatusType.Landing),
                            FuelVendors = resultsGroup.Key.FuelVendors
                                .Split(";")
                                .Where(a => !string.IsNullOrEmpty(a))
                                .OrderBy(a => a)
                                .ToList(),
                            Tags = resultsGroup.Key.Tags.ToList(),
                            PricingFormula = resultsGroup.Key.PricingFormula
                        })
                    .GroupBy(p => p.CustomerId)
                    .Select(g => g.FirstOrDefault())
                    .OrderByDescending(s => (s.FleetSize ?? 0))
                    .ToList();


                List<CustomerWithPricing> result = new List<CustomerWithPricing>();
                var fboPrices = await _fboPricesService.GetPrices(fboId);
                var customersViewedByFbo = await _customerService.GetCustomersViewedByFbo(fboId);
                var customerCompanyTypes = await _customerService.GetCustomerCompanyTypes(groupId, fboId);
                var priceBreakdownDisplayType = await _priceFetchingService.GetPriceBreakdownDisplayType(fboId);
                fboPrices = fboPrices.Where(f => f.Product.Contains("JetA")).ToList();
                List<PricingTemplate> templates = await _pricingTemplateService.GetStandardPricingTemplatesForAllCustomers(fboId, groupId);
                var flightTypeClassifications = FlightTypeClassifications.Private;
                var departureType = ApplicableTaxFlights.DomesticOnly;

                //Prepare fees/taxes based on the provided departure type and flight type
                List<FboFeesAndTaxes> feesAndTaxes = await _fboFeesAndTaxesService.GetFboFeesAndTaxes(fboId);
                feesAndTaxes = feesAndTaxes.Where(x =>
                        (x.FlightTypeClassification == FlightTypeClassifications.All ||
                         x.FlightTypeClassification == flightTypeClassifications) &&
                        (x.DepartureType == departureType ||
                         departureType == FBOLinx.Core.Enums.ApplicableTaxFlights.All ||
                         x.DepartureType == FBOLinx.Core.Enums.ApplicableTaxFlights.All)).ToList();

                customerGridVM.ForEach(x =>
                {
                    var customerInfoByGroup = customerInfoByGroupCollection.Where(c => c.CustomerId == x.CustomerId).ToList();
                    List<int> pricingTemplateIds = templates.Where(t => t.CustomerId == x.CustomerId).Select(c => c.Oid).ToList();
                    var pricingTemplates = pricingTemplatesCollection.Where(p => pricingTemplateIds.Contains(p.Oid)).ToList();

                    //Fetch the customer pricing results
                    var customerPricingResults = (from cg in customerInfoByGroup
                                                  join pt in pricingTemplates on fboId equals pt.Fboid into leftJoinPT
                                                  from pt in leftJoinPT.DefaultIfEmpty()
                                                  join ppt in customerMargins on (pt != null ? pt.Oid : 0) equals ppt.PricingTemplateId
                                                      into leftJoinPPT
                                                  from ppt in leftJoinPPT.DefaultIfEmpty()
                                                  join fp in fboPrices on
                                                  new
                                                  {
                                                      Fboid = pt != null ? pt.Fboid : 0,
                                                      Product = pt != null ? pt.MarginTypeProduct : ""
                                                  } equals new
                                                  {
                                                      Fboid = fp.Fboid,
                                                      Product = fp.GenericProduct
                                                  }
                                                  into letJoinFP
                                                  from fp in letJoinFP.DefaultIfEmpty()
                                                  join cvf in customersViewedByFbo on new { cg.CustomerId, Fboid = fboId } equals new
                                                  {
                                                      cvf.CustomerId,
                                                      cvf.Fboid
                                                  } into letJoinCVF
                                                  from cvf in letJoinCVF.DefaultIfEmpty()
                                                  join ccot in customerCompanyTypes on new
                                                  { CustomerCompanyType = cg.CustomerCompanyType ?? 0, cg.GroupId } equals new
                                                  { CustomerCompanyType = ccot.Oid, GroupId = ccot.GroupId == 0 ? groupId : ccot.GroupId }
                                                      into leftJoinCCOT
                                                  from ccot in leftJoinCCOT.DefaultIfEmpty()

                                                  select new CustomerWithPricing()
                                                  {
                                                      CustomerId = cg.CustomerId,
                                                      Company = cg.Company,
                                                      MarginType = (pt == null ? 0 : pt.MarginType),
                                                      DiscountType = (pt == null ? 0 : pt.DiscountType),
                                                      FboPrice = (fp == null || fp.Oid == 0 ? 0 : fp.Price),
                                                      CustomerMarginAmount = pt == null ? 0 : (pt.MarginTypeProduct == "Retail"
                                                          ? (ppt == null ? 0 : ppt.Amount) + (fp == null || fp.TempJet == null ? 0 : (double)fp.TempJet)
                                                          : (ppt == null ? 0 : ppt.Amount)),
                                                      amount = ppt == null ? 0 : ppt.Amount,
                                                      CustomerCompanyType = cg.CustomerCompanyType,
                                                      PriceBreakdownDisplayType = priceBreakdownDisplayType
                                                  }).OrderBy(y => y.Company).ThenBy(y => y.PricingTemplateId).ThenBy(y => y.Product).ThenBy(y => y.MinGallons).ToList();

                    if (feesAndTaxes.Count > 0 && customerPricingResults[0].FboPrice > 0)
                    {
                        //Add domestic-departure-only price options
                        List<CustomerWithPricing> domesticOptions = new List<CustomerWithPricing>();
                        if ((feesAndTaxes.Any(y => y.DepartureType == ApplicableTaxFlights.DomesticOnly) &&
                            departureType == ApplicableTaxFlights.All) || departureType == ApplicableTaxFlights.DomesticOnly)
                        {
                            domesticOptions = customerPricingResults.Clone<CustomerWithPricing>().ToList();
                            domesticOptions.ForEach(y =>
                            {
                                y.Product = "Jet A (Domestic Departure)";
                                y.FeesAndTaxes = feesAndTaxes.Where(fee =>
                                    fee.DepartureType == ApplicableTaxFlights.DomesticOnly ||
                                    fee.DepartureType == ApplicableTaxFlights.All)
                                    .ToList().Clone<FboFeesAndTaxes>().ToList();
                            });
                        }

                        //Add international-departure-only price options
                        List<CustomerWithPricing> internationalOptions = new List<CustomerWithPricing>();
                        if ((feesAndTaxes.Any(y => y.DepartureType == ApplicableTaxFlights.InternationalOnly) &&
                             departureType == ApplicableTaxFlights.All) ||
                            departureType == ApplicableTaxFlights.InternationalOnly)
                        {
                            internationalOptions = customerPricingResults.Clone<CustomerWithPricing>().ToList();
                            internationalOptions.ForEach(y =>
                            {
                                y.Product = "Jet A (International Departure)";
                                y.FeesAndTaxes = feesAndTaxes.Where(fee =>
                                    fee.DepartureType == ApplicableTaxFlights.InternationalOnly ||
                                    fee.DepartureType == ApplicableTaxFlights.All)
                                    .Where(fee => fee.OmitsByPricingTemplate == null || fee.OmitsByPricingTemplate.All(o => o.PricingTemplateId != y.PricingTemplateId))
                                    .ToList();
                                y.FeesAndTaxes = feesAndTaxes.Where(fee =>
                                        fee.DepartureType == ApplicableTaxFlights.InternationalOnly ||
                                        fee.DepartureType == ApplicableTaxFlights.All)
                                    .ToList().Clone<FboFeesAndTaxes>().ToList();
                            });
                        }

                        //Add price options for all departure types
                        List<CustomerWithPricing> allDepartureOptions = new List<CustomerWithPricing>();
                        if ((feesAndTaxes.Any(y => y.DepartureType == ApplicableTaxFlights.All) &&
                           departureType == ApplicableTaxFlights.All) &&
                          (domesticOptions.Count == 0 || internationalOptions.Count == 0))
                        {
                            allDepartureOptions = customerPricingResults.Clone<CustomerWithPricing>().ToList();
                            allDepartureOptions.ForEach(y =>
                            {
                                var productName = y.Product;
                                if (domesticOptions.Count == 0)
                                    productName += " (Domestic Departure)";
                                y.Product = productName;
                                y.FeesAndTaxes = feesAndTaxes.Where(fee => fee.DepartureType == ApplicableTaxFlights.All)
                                    .Where(fee => fee.OmitsByPricingTemplate == null || fee.OmitsByPricingTemplate.All(o => o.PricingTemplateId != y.PricingTemplateId))
                                    .ToList().Clone<FboFeesAndTaxes>().ToList();
                            });
                        }

                        List<CustomerWithPricing> resultsWithFees = new List<CustomerWithPricing>();
                        resultsWithFees.AddRange(domesticOptions);
                        resultsWithFees.AddRange(internationalOptions);
                        resultsWithFees.AddRange(allDepartureOptions);

                        x.AllInPrice = resultsWithFees.Count > 0 ? resultsWithFees.FirstOrDefault().AllInPrice : 0;
                    }
                    else
                    {
                        x.AllInPrice = customerPricingResults.Count > 0 && customerPricingResults[0].FboPrice > 0 ? customerPricingResults.FirstOrDefault().AllInPrice : 0;
                    }

                    var needsAttentionRecord = needsAttentionCustomers.FirstOrDefault(n => n.Oid == x.CustomerInfoByGroupId);
                    if (needsAttentionRecord == null)
                        return;
                    x.NeedsAttention = true;
                    x.NeedsAttentionReason = needsAttentionRecord.NeedsAttentionReason;
                });

                return customerGridVM;
            }
            catch (Exception ex)
            {
                string sss = ex.Message;
                return null;
            }
        }

        private async Task<List<GroupCustomerAnalyticsResponse>> GetCustomerPricingAnalytics(int groupId, int customerId)
        {
            var airports = await _context.Fbos
                .Where(x => x.GroupId == groupId && x.Active == true)
                .Include(x => x.FboAirport)
                .ToListAsync();

            var customerInfoByGroup = await _context.CustomerInfoByGroup.FirstOrDefaultAsync(c => c.CustomerId == customerId && c.GroupId == groupId);
            if (customerInfoByGroup == null)
                return new List<GroupCustomerAnalyticsResponse>();

            List<string> icaos = new List<string>();
            icaos.AddRange(airports.Select(x => x.FboAirport?.Icao).Where(x => !string.IsNullOrEmpty(x)));

            Dictionary<FlightTypeClassifications, List<CustomerWithPricing>> priceResults =
                new Dictionary<FlightTypeClassifications, List<CustomerWithPricing>>();
            priceResults.Add(FlightTypeClassifications.Commercial, new List<CustomerWithPricing>());
            priceResults.Add(FlightTypeClassifications.Private, new List<CustomerWithPricing>());


            List<CustomerWithPricing> commercialValidPricing =
                await _priceFetchingService.GetCustomerPricingByLocationAsync(string.Join(",", icaos), customerId, FlightTypeClassifications.Commercial, ApplicableTaxFlights.All, null, 0, groupId, true);
            if (commercialValidPricing != null)
            {
                commercialValidPricing.RemoveAll(x => x.GroupId != groupId);
                priceResults[FlightTypeClassifications.Commercial].AddRange(commercialValidPricing);
            }

            List<CustomerWithPricing> privateValidPricing =
                await _priceFetchingService.GetCustomerPricingByLocationAsync(string.Join(",", icaos), customerId, FlightTypeClassifications.Private, ApplicableTaxFlights.All, null, 0, groupId, true);
            if (privateValidPricing != null)
            {
                privateValidPricing.RemoveAll(x => x.GroupId != groupId);
                priceResults[FlightTypeClassifications.Private].AddRange(privateValidPricing);
            }

            var results = priceResults[FlightTypeClassifications.Commercial]
                .GroupBy(x => new { x.Company, x.TailNumbers, x.CustomerId, x.FboId })
                .Select(x => new GroupCustomerAnalyticsResponse
                {
                    CustomerId = x.Key.CustomerId,
                    Company = x.Key.Company,
                    TailNumbers = x.Key.TailNumbers,
                    FboId = x.Key.FboId,
                    GroupCustomerFbos = new List<GroupedFboPrices>()
                }).ToList();

            if (results != null)
            {
                results.ForEach(result =>
                {
                    result.AddGroupedFboPrices(
                        FlightTypeClassifications.Commercial,
                        priceResults[FlightTypeClassifications.Commercial]
                            .Where(x => x.Company == result.Company && x.TailNumbers == result.TailNumbers && x.FboId == result.FboId)
                            .ToList()
                    );
                    result.AddGroupedFboPrices(
                        FlightTypeClassifications.Private,
                        priceResults[FlightTypeClassifications.Private]
                            .Where(x => x.Company == result.Company && x.TailNumbers == result.TailNumbers && x.FboId == result.FboId)
                            .ToList()
                    );

                    var maxPriceType = result.GroupCustomerFbos.Max(y => y.Prices.Max(z => z.PriceBreakdownDisplayType));
                    if (maxPriceType == PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly)
                        maxPriceType = PriceBreakdownDisplayTypes.FourColumnsAllRules;

                    result.GroupCustomerFbos?.ForEach(g =>
                    {
                        g.Prices?.ForEach(p => p.PriceBreakdownDisplayType = maxPriceType);
                    });
                });
            }
            else
            {
                results = new List<GroupCustomerAnalyticsResponse>();
                results.Add(new GroupCustomerAnalyticsResponse
                {
                    Company = null,
                    TailNumbers = null,
                    GroupCustomerFbos = new List<GroupedFboPrices>()
                });
            }

            return results;
        }
    }
}

