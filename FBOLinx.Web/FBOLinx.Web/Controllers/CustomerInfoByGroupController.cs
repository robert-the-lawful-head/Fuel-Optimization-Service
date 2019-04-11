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
        public async Task<IActionResult> GetCustomersByFBOAndPricing([FromRoute] int groupId, [FromRoute] int fboId, [FromRoute] int pricingTemplateId)
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
                    Oid = cg.Oid,
                    cg.Company,
                    CustomerId = cg.CustomerId,
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
        public async Task<IActionResult> GetCustomerCountByGroupAndFBO([FromRoute] int groupId, [FromRoute] int fboId)
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
                                      Oid = cg.Oid
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
            FBOLinx.Web.Services.PriceFetchingService priceFetchingService =
                new FBOLinx.Web.Services.PriceFetchingService(_context);
            var customerPricingResults = await priceFetchingService.GetCustomerPricingAsync(fboId, groupId);
            var customerGridVM = (from results in customerPricingResults
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
                    results.HasBeenViewed
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
                    NeedsAttention = resultsGroup.Key.NeedsAttention,
                    PricingTemplateName = (from customerPricing in resultsGroup where customerPricing.PricingTemplateId > 0 select customerPricing.PricingTemplateName).Distinct().Count() > 1 ? "-Multiple-" : (from customerPricing in resultsGroup select customerPricing.PricingTemplateName).First(),
                    CustomerCompanyType = resultsGroup.Key.CustomerCompanyType,
                    CustomerCompanyTypeName = resultsGroup.Key.CustomerCompanyTypeName,
                    HasBeenViewed = resultsGroup.Key.HasBeenViewed,
                    AllInPrice = (from customerPricing in resultsGroup select customerPricing.AllInPrice).Max()
                }).ToList();
            
            return customerGridVM;
        }
    }
}