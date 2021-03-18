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
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerInfoByGroupController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly FboLinxContext _context;

        private readonly PriceFetchingService _priceFetchingService;
        private readonly CustomerService _customerService;

        public CustomerInfoByGroupController(IWebHostEnvironment hostingEnvironment, FboLinxContext context, CustomerService customerService, PriceFetchingService priceFetchingService)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _priceFetchingService = priceFetchingService;
            _customerService = customerService;
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

        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetCustomersByGroup([FromRoute] int groupId)
        {
            var test = (from cg in _context.CustomerInfoByGroup
                        select cg);
            
            var result = await (from cg in _context.CustomerInfoByGroup
                          join c in _context.Customers on cg.CustomerId equals c.Oid
                          where cg.GroupId == groupId
                          && (!c.Suspended.HasValue || !c.Suspended.Value)
                          group cg by new
                          {
                              cg.CustomerId,
                              cg.Company
                          } into groupedResult
                          select new
                          {
                              groupedResult.Key.CustomerId,
                              groupedResult.Key.Company,
                          })
                          .Distinct()
                          .ToListAsync();

            return Ok(result);
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
            customerInfoByGroup.Active = true;
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
        public async Task<IActionResult> GetGroupAnalytics([FromRoute]int groupId, [FromBody] List<int> customerIds)
        {
            var customerFbos = (await (from cig in _context.CustomerInfoByGroup.Where(cig => customerIds.Contains(cig.CustomerId))
                                       join fbo in _context.Fbos on cig.GroupId equals fbo.GroupId
                                       join fa in _context.Fboairports on fbo.Oid equals fa.Fboid
                                       join cct in _context.CustomCustomerTypes on new { cig.CustomerId, Fboid = fbo.Oid } equals new { cct.CustomerId, cct.Fboid }
                                       into leftJoinCCT
                                       from cct in leftJoinCCT.DefaultIfEmpty()
                                       join pt in _context.PricingTemplate on cct == null ? 0 : cct.CustomerType equals pt.Oid
                                       into leftJoinPT
                                       from pt in leftJoinPT.DefaultIfEmpty()
                                       where cig.GroupId == groupId
                                       select new
                                       {
                                           CustomerInfoByGroupId = cig.Oid,
                                           cig.Company,
                                           FboId = fbo.Oid,
                                           fa.Icao,
                                           PricingTemplateId = pt == null ? 0 : pt.Oid
                                       })
                                       .ToListAsync())
                                       .GroupBy(cf => cf.Company)
                                       .ToList();

            var result = new List<GroupCustomerAnalyticsResponse>();

            foreach (var customerfbo in customerFbos)
            {
                var record = new GroupCustomerAnalyticsResponse
                {
                    Company = customerfbo.Key,
                    GroupCustomerFbos = new List<GroupedFboPrices>()
                };
                var customerfbosData = customerfbo.ToList();
                foreach(var customerFbo in customerfbosData)
                {
                    var groupFboPrices = new GroupedFboPrices
                    {
                        Icao = customerFbo.Icao,
                        Prices = new List<Prices>(),
                    };
                    var commercialInternationalPricingResults = await _priceFetchingService
                        .GetCustomerPricingAsync(customerFbo.FboId, groupId, customerFbo.CustomerInfoByGroupId, new List<int> { customerFbo.PricingTemplateId}, FBOLinx.Core.Enums.FlightTypeClassifications.Commercial, FBOLinx.Core.Enums.ApplicableTaxFlights.InternationalOnly);
                    var privateInternationalPricingResults = await _priceFetchingService
                        .GetCustomerPricingAsync(customerFbo.FboId, groupId, customerFbo.CustomerInfoByGroupId, new List<int> { customerFbo.PricingTemplateId }, FBOLinx.Core.Enums.FlightTypeClassifications.Private, FBOLinx.Core.Enums.ApplicableTaxFlights.InternationalOnly);
                    var commercialDomesticPricingResults = await _priceFetchingService
                        .GetCustomerPricingAsync(customerFbo.FboId, groupId, customerFbo.CustomerInfoByGroupId, new List<int> { customerFbo.PricingTemplateId }, FBOLinx.Core.Enums.FlightTypeClassifications.Commercial, FBOLinx.Core.Enums.ApplicableTaxFlights.DomesticOnly);
                    var privateDomesticPricingResults = await _priceFetchingService
                        .GetCustomerPricingAsync(customerFbo.FboId, groupId, customerFbo.CustomerInfoByGroupId, new List<int> { customerFbo.PricingTemplateId }, FBOLinx.Core.Enums.FlightTypeClassifications.Private, FBOLinx.Core.Enums.ApplicableTaxFlights.DomesticOnly);
                    privateDomesticPricingResults = privateDomesticPricingResults.OrderBy(s => s.MinGallons).ToList();

                    int loopIndex = 0;
                    foreach (CustomerWithPricing model in privateDomesticPricingResults)
                    {
                        string volumeTier;
                        if ((loopIndex + 1) < commercialInternationalPricingResults.Count)
                        {
                            string minGallon, maxGallon;
                            minGallon = model.MinGallons.GetValueOrDefault().ToString();
                            var next = commercialInternationalPricingResults[loopIndex + 1];
                            double maxValue = Convert.ToDouble(next.MinGallons) - 1;

                            if (maxValue > 999)
                            {
                                string output = Convert.ToDouble(next.MinGallons).ToString("#,##", CultureInfo.InvariantCulture);
                                maxGallon = output;
                            }
                            else
                            {
                                maxGallon = maxValue.ToString();
                            }
                            volumeTier = minGallon + " - " + maxGallon;
                        }
                        else
                        {
                            string output = Convert.ToDouble(model.MinGallons).ToString("#,##", CultureInfo.InvariantCulture);

                            volumeTier = output + "+";
                        }
                        groupFboPrices.Prices.Add(new Prices
                        {
                            VolumeTier = volumeTier,
                            IntComm = (commercialInternationalPricingResults.Where(s => s.MinGallons == model.MinGallons).Select(s => s.AllInPrice.GetValueOrDefault())).FirstOrDefault(),
                            IntPrivate = (privateInternationalPricingResults.Where(s => s.MinGallons == model.MinGallons).Select(s => s.AllInPrice.GetValueOrDefault())).FirstOrDefault(),
                            DomComm = (commercialDomesticPricingResults.Where(s => s.MinGallons == model.MinGallons).Select(s => s.AllInPrice.GetValueOrDefault())).FirstOrDefault(),
                            DomPrivate = (privateDomesticPricingResults.Where(s => s.MinGallons == model.MinGallons).Select(s => s.AllInPrice.GetValueOrDefault())).FirstOrDefault(),
                        });
                    }

                    record.GroupCustomerFbos.Add(groupFboPrices);
                }
                result.Add(record);
            }

            return Ok(result);
        }

        private bool CustomerInfoByGroupExists(int id)
        {
            return _context.CustomerInfoByGroup.Any(e => e.Oid == id);
        }

        private async Task<List<CustomersGridViewModel>> FetchCustomersViewModelByGroupAndFbo(int groupId, int fboId)
        {
            try
            {
                List<PricingTemplatesGridViewModel> pricingTemplates = await _priceFetchingService.GetPricingTemplates(fboId, groupId);

                PricingTemplate defaultPricingTemplate = await _context.PricingTemplate
                    .Where(x => x.Fboid == fboId && (x.Default ?? false)).FirstOrDefaultAsync();

                PricingTemplateService pricingTemplateService = new PricingTemplateService(_context);

                await pricingTemplateService.FixCustomCustomerTypes(groupId, fboId);
                
                var companyTypes = await _context.CustomerCompanyTypes.Where(x => x.GroupId == groupId).ToListAsync();

                
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

                var needsAttentionCustomers = await _customerService.GetCustomersNeedingAttentionByGroupFbo(groupId, fboId);

                var customerInfoByGroup = await _customerService.GetCustomersByGroupAndFbo(groupId, fboId);
                var contactInfoByGroupForAlerts =
                    await _context.ContactInfoByGroup.Where(x => x.GroupId == groupId && x.CopyAlerts == true).Include(x => x.Contact).ToListAsync();

                List<CustomersGridViewModel> customerGridVM = (
                        from cg in customerInfoByGroup
                        join ccot in companyTypes on new
                            {
                                CustomerCompanyType = (cg.CustomerCompanyType ?? 0),
                                cg.GroupId
                            }
                            equals new
                            {
                                CustomerCompanyType = ccot.Oid,
                                GroupId = ccot.GroupId == 0 ? groupId : ccot.GroupId
                            }
                            into leftJoinCCOT
                        from ccot in leftJoinCCOT.DefaultIfEmpty()
                        join ai in pricingTemplates on new
                            {
                                TemplateId = ((cg.Customer?.CustomCustomerType?.CustomerType).GetValueOrDefault() == 0
                                    ? defaultPricingTemplate.Oid
                                    : cg.Customer?.CustomCustomerType?.CustomerType).GetValueOrDefault()
                            } equals new { TemplateId = ai.Oid}
                            into leftJoinAi
                        from ai in leftJoinAi.DefaultIfEmpty()
                        where cg.GroupId == groupId && !(cg.Suspended ?? false)

                        group cg by new
                        {
                            cg.CustomerId,
                            CustomerInfoByGroupId = cg.Oid,
                            cg.Company,
                            FuelerLinxId = (cg.Customer?.FuelerlinxId ?? 0),
                            CustomerCompanyTypeName = ccot?.Name,
                            CertificateType = (cg.CertificateType ?? CustomerInfoByGroup.CertificateTypes.NotSet),
                            ContactExists = contactInfoByGroupForAlerts.Any(c =>
                                (cg.Customer?.CustomerContacts?.Any(cc => cc.ContactId == c.ContactId)) == true),
                            PricingTemplateName = string.IsNullOrEmpty(ai?.Name) ? defaultPricingTemplate.Name : ai.Name,
                            IsPricingExpired = ai != null && ai.IsPricingExpired,
                            Active = (cg.Active ?? false),
                            Tails = customerAircraft.FirstOrDefault(x => x.CustomerId == cg.CustomerId)?.Tails,
                            FleetSize = customerAircraft.FirstOrDefault(x => x.CustomerId == cg.CustomerId)?.Count,
                            AllInPrice = ai == null ? 0 : ai.IntoPlanePrice,
                            PricingTemplateId = (ai?.Oid).GetValueOrDefault() == 0 ? defaultPricingTemplate.Oid : ai.Oid
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

    }

}
