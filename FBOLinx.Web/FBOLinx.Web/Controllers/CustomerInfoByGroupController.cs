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
using FBOLinx.Web.DTO;
using FBOLinx.Web.Models.Responses;
using static FBOLinx.DB.Models.PricingTemplate;
using System.Globalization;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Services.Interfaces;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;

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
        private readonly FboService _fboService;
        private readonly AirportWatchService _airportWatchService;
        private readonly IPriceDistributionService _priceDistributionService;
        private readonly FuelerLinxService _fuelerLinxService;
        private readonly IPricingTemplateService _pricingTemplateService;
        private readonly AircraftService _aircraftService;

        public CustomerInfoByGroupController(IWebHostEnvironment hostingEnvironment, FboLinxContext context, CustomerService customerService, IPriceFetchingService priceFetchingService, FboService fboService, AirportWatchService airportWatchService, IPriceDistributionService priceDistributionService, FuelerLinxService fuelerLinxService, IPricingTemplateService pricingTemplateService , AircraftService aircraftService)
        {    _hostingEnvironment = hostingEnvironment;
            _context = context;
            _priceFetchingService = priceFetchingService;
            _customerService = customerService;
            _fboService = fboService;
            _airportWatchService = airportWatchService;
            _priceDistributionService = priceDistributionService;
            _fuelerLinxService = fuelerLinxService;
            _pricingTemplateService = pricingTemplateService;
            _aircraftService = aircraftService;
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
            List < CustomersGridViewModel > customerGridVM = await (
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
                             where (cg.Active ?? false)
                                   && (cc.CustomerType == pricingTemplateId || pricingTemplateId == 0)
                             select new
                             {
                                 cg.Oid,
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
                                     cg.Oid
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
            return Core.Utilities.Enum.GetDescriptions(typeof(CustomerInfoByGroup.CertificateTypes));
        }

        // GET: api/CustomerInfoByGroup/CustomerSources
        [HttpGet("CustomerSources")]
        public IEnumerable<Core.Utilities.Enum.EnumDescriptionValue> GetCustomerSources()
        {
            return Core.Utilities.Enum.GetDescriptions(typeof(Customers.CustomerSources));
        }

        // PUT: api/CustomerInfoByGroup/5
        [HttpPut("{id}/{userId}")]
        public async Task<IActionResult> PutCustomerInfoByGroup([FromRoute] int id , [FromRoute] int userId, [FromBody] CustomerInfoByGroup customerInfoByGroup )
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
                EditCustomerToLogger(customerInfoByGroup, customerInfoByGroup.Oid, userId);
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
        [HttpPost("{userId}")]
        public async Task<IActionResult> PostCustomerInfoByGroup([FromRoute] int userId , [FromBody] CustomerInfoByGroup customerInfoByGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            customerInfoByGroup.Active = true;
            _context.CustomerInfoByGroup.Add(customerInfoByGroup);
            await _context.SaveChangesAsync();
            AddCustomerInfoGroupLog(customerInfoByGroup , userId);
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
            if(customerId != 0)
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
                        catch(Exception ex)
                        {
                            var ee = ex;
                        }
                       
                    }

                    var custNameCheck = _context.CustomerInfoByGroup.FirstOrDefault(s => s.Company == custInfo.Company && s.CustomerId != custInfo.CustomerId && s.GroupId == groupId);

                    if(custNameCheck != null)
                    {
                        newCustomerMatch.IsNameMatch = true;

                        newCustomerMatch.MatchNameCustomerId = custNameCheck.CustomerId;
                        newCustomerMatch.MatchNameCustomerOid = custNameCheck.Oid;
                        newCustomerMatch.MatchNameCustomer = custNameCheck.Company;
                    }

                    var custContacts = _context.CustomerContacts.Where(s => s.CustomerId == custInfo.CustomerId).ToList();

                    if(custContacts != null)
                    {
                        var contactEmails = _context.ContactInfoByGroup.Where(s => s.ContactId == custContacts[0].ContactId).Select(s => s.Email).ToList();

                        if(contactEmails.Count > 0)
                        {
                            var checkOtherContacts = _context.ContactInfoByGroup.Where(s => contactEmails.Contains(s.Email)).Select(s =>s.ContactId).ToList();

                            if(checkOtherContacts.Count > 0)
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
            foreach(var cust in customer)
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

            var customerAircrafts =await  (from ca in _context.CustomerAircrafts
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

            if(resultContact.Count> 0)
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
        public async Task<IActionResult> GetGroupAnalytics([FromRoute]int groupId, [FromBody] int customerId)
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
       
        [HttpPost("GetCustomerLogger/{id}")]
        public async Task<IActionResult> GetCustomerLogger ([FromRoute] int id)
       {
          
            List<CustomerByGroupLogVM> customers = new List<CustomerByGroupLogVM>();

            try
            {
                //Get customer Info Group Log 
                var CustomerInfoGroup =  _context.CustomerInfoByGroupLog.Where(c => c.customerId.Equals(id)).AsNoTracking().ToList();
                
                foreach (var item in CustomerInfoGroup)
                {
                    customers.Add(new CustomerByGroupLogVM
                    {
                        Oid = item.OID,
                        Action = item.Action.ToString(),
                        Location = item.Location.ToString(),                       
                        Role = item.Role.ToString(),
                        Time = item.Time,
                        username =  _context.User.FirstOrDefault(u => u.Oid == item.userId).FirstName , 
                        logType = CustomerByGroupLogVM.LogType.CustomerInfo
                         
                    });
                }




                //Get customer Contact  Log 
                var CustomerContact =   _context.CustomerContactLog.Where(c => c.customerId.Equals(id)).AsNoTracking().ToList();

                foreach (var item in CustomerContact)
                {
                    customers.Add(new CustomerByGroupLogVM
                    {
                        Oid = item.OID,
                        Action = item.Action.ToString(),
                        Location = item.Location.ToString(),
                        Role = item.Role.ToString(),
                        Time = item.Time,
                        username = _context.User.FirstOrDefault(u => u.Oid == item.userId).FirstName , 
                        logType = CustomerByGroupLogVM.LogType.CustoemrContact
                    });
                }



                //Get customer AirCaft  Log 
                var CustomerAircraft =  _context.CustomerAircraftLog.Where(c => c.customerId.Equals(id)).AsNoTracking().ToList();

                foreach (var item in CustomerAircraft)
                {
                    customers.Add(new CustomerByGroupLogVM
                    { 
                        Oid = item.OID ,
                        Action = item.Action.ToString(),
                        Location = item.Location.ToString(),
                        Role = item.Role.ToString(),
                        Time = item.Time,
                        username = _context.User.FirstOrDefault(u => u.Oid == item.userId).FirstName , 
                        logType = CustomerByGroupLogVM.LogType.CustomerAircarft
                    });
                }




                //Get customer ITP MArgin  Log 
                var CustomerITPMargin =  _context.CustomCustomerTypeLog.Where(c => c.customerId.Equals(id)).AsNoTracking().ToList();

                foreach (var item in CustomerITPMargin)
                {
                    customers.Add(new CustomerByGroupLogVM
                    {
                        Oid = item.OID,
                        Action = item.Action.ToString(),
                        Location = item.Location.ToString(),
                        Role = item.Role.ToString(),
                        Time = item.Time,
                        username = _context.User.FirstOrDefault(u => u.Oid == item.userId).FirstName , 
                        logType = CustomerByGroupLogVM.LogType.CustomerItpMargin
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(customers.OrderByDescending(d=>d.Time));
        }


        [HttpPost("GetCustomerLoggerDetails/id/{id}/logType/{logType}")]
        public async Task<IActionResult> GetCustomerLoggerDetails ([FromRoute] int id , [FromRoute] int logType)
        {
            CustomerInfoByGroupLogDataVM customerInfo = new CustomerInfoByGroupLogDataVM();
            CustomerContactLogDataVM customerContact = new CustomerContactLogDataVM();
            CustomerAircraftLogDataVM customerAircraft = new CustomerAircraftLogDataVM();
            CustomerMarginLogDataVM customerMargin = new CustomerMarginLogDataVM();

           try
            {
           
                if(logType == ((int)CustomerByGroupLogVM.LogType.CustomerInfo))
                {
                    var customerInfoLog = await _context.CustomerInfoByGroupLog.FirstOrDefaultAsync(x=>x.OID == id);
                    if(customerInfoLog != null)
                    {
                        customerInfo.customerInfoByGroupLogData = customerInfoLog.oldcustomerId.ToString() != "" ?
                                     _context.CustomerInfoByGroupLogData.FirstOrDefault(c => c.Oid == customerInfoLog.oldcustomerId) :null;

                        customerInfo.customerInfoByGroup = customerInfoLog.newcustomerId.ToString() != "" ?
                                          _context.CustomerInfoByGroup.FirstOrDefault(c => c.Oid == customerInfoLog.newcustomerId) : null;


                    }

                    return Ok(customerInfo);

                }
                else if (logType == ((int)CustomerByGroupLogVM.LogType.CustoemrContact))
                {

                    var customerContactLog = await _context.CustomerContactLog.FirstOrDefaultAsync(x => x.OID == id);
                    if (customerContactLog != null)
                    {
                        customerContact.customerContactLogData = customerContactLog.oldcustomercontactId.ToString() != "" ?
                                     _context.CustomerContactLogData.FirstOrDefault(c => c.Oid == customerContactLog.oldcustomercontactId) : null;

                        customerContact.customerContact = customerContactLog.newcustomercontactId.ToString() != "" ?
                                          _context.CustomerContacts.FirstOrDefault(c => c.Oid == customerContactLog.newcustomercontactId) : null;

                        if(customerContact.customerContactLogData != null  )
                        {
                            customerContact.OldContact = _context.Contacts.FirstOrDefault(c => c.Oid == customerContact.customerContactLogData.ContactId);
                           
                        }

                        if(customerContact.customerContact != null)
                        {
                            customerContact.NewContact = _context.Contacts.FirstOrDefault(c => c.Oid == customerContact.customerContact.ContactId);

                        }

                    }

                    return Ok(customerContact);

                }
                else if (logType == ((int)CustomerByGroupLogVM.LogType.CustomerAircarft))
                {

                    var customeraircraftLog = await _context.CustomerAircraftLog.FirstOrDefaultAsync(x => x.OID == id);
                    if (customeraircraftLog != null)
                    {
                        customerAircraft.customerAircraftLogData = customeraircraftLog.oldcustomeraircraftId.ToString() != "" ?
                                     _context.CustomerAircraftLogData.FirstOrDefault(c => c.Oid == customeraircraftLog.oldcustomeraircraftId) : null;

                        customerAircraft.customerAircrafts = customeraircraftLog.newcustomeraircraftId.ToString() != "" ?
                                          _context.CustomerAircrafts.FirstOrDefault(c => c.Oid == customeraircraftLog.newcustomeraircraftId) : null;


                        if (customerAircraft.customerAircrafts != null )
                        {
                            customerAircraft.NewAircraft = _aircraftService.GetAircrafts(customerAircraft.customerAircrafts.AircraftId).Result;
                           

                        }

                        if(customerAircraft.customerAircraftLogData != null)
                        {
                            customerAircraft.oldAircraft = _aircraftService.GetAircrafts(customerAircraft.customerAircraftLogData.AircraftId).Result;
                        }

                    }

                    return Ok(customerAircraft);
                }
                else if (logType == ((int)CustomerByGroupLogVM.LogType.CustomerItpMargin))
                {


                    var customerMarginLog = await _context.CustomCustomerTypeLog.FirstOrDefaultAsync(x => x.OID == id);
                    if (customerMarginLog != null)
                    {
                        customerMargin.customerTypesLogData = customerMarginLog.oldcustomertypeId.ToString() != "" ?
                                     _context.CustomCustomerTypesLogData.FirstOrDefault(c => c.Oid == customerMarginLog.oldcustomertypeId) : null;

                        customerMargin.customerTypes = customerMarginLog.newcustomertypetId.ToString() != "" ?
                                          _context.CustomCustomerTypes.FirstOrDefault(c => c.Oid == customerMarginLog.newcustomertypetId) : null;

                        if(customerMargin.customerTypes != null && customerMargin.customerTypesLogData != null)
                        {
                            customerMargin.NewMarginName = _context.PricingTemplate.
                                                          FirstOrDefault(p => p.Oid == customerMargin.customerTypes.CustomerType).Name;

                            customerMargin.oldMarginName = _context.PricingTemplate.
                                                         FirstOrDefault(p => p.Oid == customerMargin.customerTypesLogData.CustomerType).Name;

                        }

                    }

                    return Ok(customerMargin);
                }
                else
                {
                    return NoContent();
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        private void AddCustomerInfoGroupLog(CustomerInfoByGroup customer , int userId = 0)
        {
            var newCustomer = _context.CustomerInfoByGroup
                                      .FirstOrDefault(c=>c.Company.Equals(customer.Company) 
                                                      && c.CustomerId.Equals(customer.CustomerId)
                                                      && c.GroupId.Equals(customer.GroupId));

            if(newCustomer != null)
            {
                try
                {
                    _context.CustomerInfoByGroupLog.Add(new CustomerInfoByGroupLog
                    {
                        Action = CustomerInfoByGroupLog.Actions.Created , 
                        Location =CustomerInfoByGroupLog.Locations.Customer,
                        Role = (CustomerInfoByGroupLog.UserRoles)_context.User.FirstOrDefault(u => u.Oid == userId).Role, 
                        userId  = userId, 
                        Time = DateTime.Now , 
                        newcustomerId = newCustomer.Oid , 
                        customerId = newCustomer.Oid
                    });
                    _context.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        private  void EditCustomerToLogger ( CustomerInfoByGroup customer , int customerId , int userId = 0 )
        {
            var oldCustomerInfoByGroup = _context.CustomerInfoByGroup.FirstOrDefault(c => c.Oid.Equals(customer.Oid));
            if (oldCustomerInfoByGroup != null)
            {
                //Add the old info into DataLog
                _context.CustomerInfoByGroupLogData.Add(new CustomerInfoByGroupLogData
                {
                    Active = oldCustomerInfoByGroup.Active,
                    Address = oldCustomerInfoByGroup.Address,
                    //CertificateType = oldCustomerInfoByGroup.CertificateType , 
                    City = oldCustomerInfoByGroup.City,
                    Company = oldCustomerInfoByGroup.Company,
                    Country = oldCustomerInfoByGroup.Country,
                    CustomerCompanyType = oldCustomerInfoByGroup.CustomerCompanyType,
                    CustomerId = oldCustomerInfoByGroup.CustomerId,
                    CustomerType = oldCustomerInfoByGroup.CustomerType,
                    DefaultTemplate = oldCustomerInfoByGroup.DefaultTemplate,
                    Distribute = oldCustomerInfoByGroup.Distribute,
                    EmailSubscription = oldCustomerInfoByGroup.EmailSubscription,
                    GroupId = oldCustomerInfoByGroup.GroupId,
                    Joined = oldCustomerInfoByGroup.Joined,
                    MainPhone = oldCustomerInfoByGroup.MainPhone,
                    MergeRejected = oldCustomerInfoByGroup.MergeRejected,
                    Network = oldCustomerInfoByGroup.Network,
                    Password = oldCustomerInfoByGroup.Password,
                    Sfid = oldCustomerInfoByGroup.Sfid,
                    Show100Ll = oldCustomerInfoByGroup.Show100Ll,
                    ShowJetA = oldCustomerInfoByGroup.ShowJetA,
                    State = oldCustomerInfoByGroup.State,
                    Suspended = oldCustomerInfoByGroup.Suspended,
                    Username = oldCustomerInfoByGroup.Username,
                    Website = oldCustomerInfoByGroup.Website,
                    ZipCode = oldCustomerInfoByGroup.ZipCode,
                    PricingTemplateRemoved = oldCustomerInfoByGroup.PricingTemplateRemoved,

                });

                try
                {
                    _context.SaveChanges();
                    int OldcustomerId = _context.CustomerInfoByGroupLogData.Where(c => (c.GroupId.Equals(customer.GroupId)) && (c.CustomerId.Equals(customer.CustomerId)))
                        .OrderByDescending(c => c.Oid).FirstOrDefault().Oid;

                    CustomerInfoByGroupLog customerlog = new CustomerInfoByGroupLog();
                    customerlog.newcustomerId = customer.Oid;
                    customerlog.Time = DateTime.Now;
                    customerlog.userId = userId;
                    customerlog.Role = (CustomerInfoByGroupLog.UserRoles)_context.User.FirstOrDefault(u => u.Oid == userId).Role;
                    customerlog.Location = CustomerInfoByGroupLog.Locations.EditCustomer;
                    customerlog.oldcustomerId = OldcustomerId;
                    customerlog.customerId = customerId;

                    if (oldCustomerInfoByGroup.Active == true && customer.Active == false)
                        customerlog.Action = CustomerInfoByGroupLog.Actions.Deactivated;
                    else if (oldCustomerInfoByGroup.Active == false && customer.Active == true)
                        customerlog.Action = CustomerInfoByGroupLog.Actions.Activated;
                    else
                        customerlog.Action = CustomerInfoByGroupLog.Actions.Edited;

                    _context.CustomerInfoByGroupLog.Add(customerlog); 

                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

            }

            
           
        }

        private bool CustomerInfoByGroupExists(int id)
        {
            return _context.CustomerInfoByGroup.Any(e => e.Oid == id);
        }

        private async Task<List<CustomersGridViewModel>> FetchCustomersViewModelByGroupAndFbo(int groupId, int fboId)
        {
            try
            {
                List<PricingTemplatesGridViewModel> pricingTemplates = await _pricingTemplateService.GetPricingTemplates(fboId, groupId);

                var fboIcao = await _fboService.GetFBOIcao(fboId);

                PricingTemplate defaultPricingTemplate = await _context.PricingTemplate
                    .Where(x => x.Fboid == fboId && (x.Default ?? false)).FirstOrDefaultAsync();

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

                var customerInfoByGroup = await _customerService.GetCustomersByGroupAndFbo(groupId, fboId);
                
                var contactInfoByFboForAlerts =
                    await (from cibg in _context.ContactInfoByGroup
                           join c in _context.Contacts on cibg.ContactId equals c.Oid
                           join cibf in _context.Set<ContactInfoByFbo>() on new { ContactId = c.Oid, FboId = fboId } equals new { ContactId = cibf.ContactId.GetValueOrDefault(), FboId = cibf.FboId.GetValueOrDefault() } into leftJoinCIBF
                           from cibf in leftJoinCIBF.DefaultIfEmpty()
                           where cibg.GroupId == groupId 
                           select new { cibg.ContactId, CopyAlerts = cibf.ContactId == null ? cibg.CopyAlerts : cibf.CopyAlerts }).ToListAsync();

                //var historicalData = await _airportWatchService.GetHistoricalDataAssociatedWithGroupOrFbo(groupId, fboId, new AirportWatchHistoricalDataRequest { StartDateTime = null, EndDateTime = null });

                var customerFuelVendors = await _fuelerLinxService.GetCustomerFuelVendors();

                List<CustomersGridViewModel> customerGridVM = (
                        from cg in customerInfoByGroup
                        join ccot in companyTypes on cg.CustomerCompanyType equals ccot.Oid into leftJoinCCOT
                        from ccot in leftJoinCCOT.DefaultIfEmpty()
                        join ai in pricingTemplates on new
                            {
                                TemplateId = ((cg.Customer?.CustomCustomerType?.CustomerType).GetValueOrDefault() == 0
                                    ? defaultPricingTemplate.Oid
                                    : cg.Customer?.CustomCustomerType?.CustomerType).GetValueOrDefault()
                            } equals new { TemplateId = ai.Oid}
                            into leftJoinAi
                        from ai in leftJoinAi.DefaultIfEmpty()
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
                            CertificateType = (cg.CertificateType ?? CustomerInfoByGroup.CertificateTypes.NotSet),
                            ContactExists =  contactInfoByFboForAlerts.Any(c =>
                                (cg.Customer?.CustomerContacts?.Any(cc => cc.ContactId == c.ContactId)) == true && c.CopyAlerts == true),
                            PricingTemplateName = string.IsNullOrEmpty(ai?.Name) ? defaultPricingTemplate.Name : ai.Name,
                            IsPricingExpired = ai != null && ai.IsPricingExpired,
                            Active = (cg.Active ?? false),
                            Tails = customerAircraft.FirstOrDefault(x => x.CustomerId == cg.CustomerId)?.Tails,
                            FleetSize = customerAircraft.FirstOrDefault(x => x.CustomerId == cg.CustomerId)?.Count,
                            AllInPrice = ai == null ? 0 : ai.IntoPlanePrice,
                            PricingTemplateId = (ai?.Oid).GetValueOrDefault() == 0 ? defaultPricingTemplate.Oid : ai.Oid,
                            FuelVendors = cv == null ? "" : cv.FuelVendors,
                            Tags = customerTags.Where(x=>x.CustomerId == cg.CustomerId)
                        }
                        into resultsGroup
                        select new CustomersGridViewModel()
                        {
                            CustomerInfoByGroupId = resultsGroup.Key.CustomerInfoByGroupId,
                            Active = resultsGroup.Key.Active,
                            AllInPrice = resultsGroup.Key.AllInPrice,
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
                            Tags = resultsGroup.Key.Tags.ToList()
                        })
                    .GroupBy(p => p.CustomerId)
                    .Select(g => g.FirstOrDefault())
                    .OrderByDescending(s => (s.FleetSize ?? 0))
                    .ToList();

                customerGridVM.ForEach(x =>
                {
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
                .Include(x => x.fboAirport)
                .ToListAsync();

            List<string> icaos = new List<string>();
            icaos.AddRange(airports.Select(x => x.fboAirport?.Icao).Where(x => !string.IsNullOrEmpty(x)));

            Dictionary<FlightTypeClassifications, List<CustomerWithPricing>> priceResults =
                new Dictionary<FlightTypeClassifications, List<CustomerWithPricing>>();
            priceResults.Add(FlightTypeClassifications.Commercial, new List<CustomerWithPricing>());
            priceResults.Add(FlightTypeClassifications.Private, new List<CustomerWithPricing>());


            List<CustomerWithPricing> commercialValidPricing =
                await _priceFetchingService.GetCustomerPricingByLocationAsync(string.Join(",", icaos), customerId, FlightTypeClassifications.Commercial, ApplicableTaxFlights.All, null, 0, groupId);
            if (commercialValidPricing != null)
            {
                commercialValidPricing.RemoveAll(x => x.GroupId != groupId);
                priceResults[FlightTypeClassifications.Commercial].AddRange(commercialValidPricing);
            }

            List<CustomerWithPricing> privateValidPricing =
                await _priceFetchingService.GetCustomerPricingByLocationAsync(string.Join(",", icaos), customerId, FlightTypeClassifications.Private, ApplicableTaxFlights.All, null, 0, groupId);
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
                    if (maxPriceType == PriceDistributionService.PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly)
                        maxPriceType = PriceDistributionService.PriceBreakdownDisplayTypes.FourColumnsAllRules;

                    result.GroupCustomerFbos?.ForEach(g =>
                    {
                        g.Prices?.ForEach(p => p.PriceBreakdownDisplayType = maxPriceType);
                    });
                });
            } else
            {
                results = new List<GroupCustomerAnalyticsResponse>();
                results.Add(new GroupCustomerAnalyticsResponse{
                    Company = null,
                    TailNumbers = null,
                    GroupCustomerFbos = new List<GroupedFboPrices>()
                });
            }

            return results;
        }
    
    }
}
