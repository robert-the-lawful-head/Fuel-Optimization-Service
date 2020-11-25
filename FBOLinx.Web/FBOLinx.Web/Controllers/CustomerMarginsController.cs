using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Web.Models.Requests;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerMarginsController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public CustomerMarginsController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/CustomerMargins
        [HttpGet]
        public IEnumerable<CustomerMargins> GetCustomerMargins()
        {
            return _context.CustomerMargins;
        }

        // GET: api/CustomerMargins/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerMargins([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerMargins = await _context.CustomerMargins.FindAsync(id);

            if (customerMargins == null)
            {
                return NotFound();
            }

            return Ok(customerMargins);
        }

        // GET: api/CustomerMargins/5
        [HttpGet("pricingtemplate/{pricingTemplateId}")]
        public async Task<IActionResult> GetCustomerMarginsByPricingTemplateId([FromRoute] int pricingTemplateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerMargins = await GetAllCustomerMargins().Include("PricingTemplate").Include("PriceTier").Where((x => x.TemplateId == pricingTemplateId)).ToListAsync();

            var customerMarginsVM = customerMargins.Select((c => new CustomerMarginsGridViewModel
            {
                PriceTierId = c.PriceTierId,
                TemplateId = c.TemplateId,
                Amount = c.Amount,
                Oid = c.Oid,
                Min = c.PriceTier.Min,
                Max = c.PriceTier.Max,
                MaxEntered = c.PriceTier.MaxEntered
            })).OrderBy(x => x.Min);

            List<CustomerMarginsGridViewModel> updatedModelWithMax = new List<CustomerMarginsGridViewModel>();

            int lastIndex = customerMarginsVM.Count() - 1;

            for (int i = 0; i < customerMarginsVM.Count(); i++)
            {
                var singleMc = customerMarginsVM.ToArray()[i];

                if(i == lastIndex)
                {
                    singleMc.Max = 99999;
                }
                else
                {
                    var nextTemplate = customerMarginsVM.ToArray()[i + 1];
                    singleMc.Max = nextTemplate.Min - 1;
                }

                updatedModelWithMax.Add(singleMc);
            }

            return Ok(updatedModelWithMax);
            //return Ok(customerMarginsVM);
        }

        // PUT: api/CustomerMargins/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerMargins([FromRoute] int id, [FromBody] CustomerMargins customerMargins)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerMargins.Oid)
            {
                return BadRequest();
            }

            _context.Entry(customerMargins).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerMarginsExists(id))
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

        // POST: api/CustomerMargins
        [HttpPost]
        public async Task<IActionResult> PostCustomerMargins([FromBody] CustomerMargins customerMargins)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CustomerMargins.Add(customerMargins);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerMargins", new { id = customerMargins.Oid }, customerMargins);
        }

        // DELETE: api/CustomerMargins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerMargins([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerMargins = await _context.CustomerMargins.FindAsync(id);
            if (customerMargins == null)
            {
                return NotFound();
            }

            _context.CustomerMargins.Remove(customerMargins);
            await _context.SaveChangesAsync();

            return Ok(customerMargins);
        }

        [HttpPost("bulkremove")]
        public async Task<IActionResult> DeleteBulkCustomerMargins([FromBody] List<CustomerMargins> margins)
        {
            try
            {
                var priceTierIds = margins.Select(x => x.PriceTierId).Where(x => x > 0);
                var customerMarginIds = margins.Select(x => x.Oid).Where(x => x > 0);
                var priceTiersToDelete = _context.PriceTiers.Where(x => priceTierIds.Any(p => p == x.Oid));
                var customerMarginsToDelete = _context.CustomerMargins.Where(x => customerMarginIds.Any(c => c == x.Oid));

                _context.CustomerMargins.RemoveRange(customerMarginsToDelete);
                _context.PriceTiers.RemoveRange(priceTiersToDelete);

                await _context.SaveChangesAsync();
            }
            catch (System.Exception exception)
            {
                //Do nothing - concurrency error due to fast-removal of margins that already occurred
                //TODO: find a better way of handling this.  Looks to occur during the auto-save in the UI.  Not causing any actual issues.
            }

            return Ok();
        }

        private bool CustomerMarginsExists(int id)
        {
            return _context.CustomerMargins.Any(e => e.Oid == id);
        }

        private IQueryable<CustomerMargins> GetAllCustomerMargins()
        {
            return _context.CustomerMargins.AsQueryable();
        }

        // POST: api/CustomerMargins
        [HttpPost("updatecustomermargin")]
        public async Task<IActionResult> UpdateCustomerMargin(CustomerPricingTemplateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerMarginObject = _context.PricingTemplate.FirstOrDefault(s => s.Oid == model.pricingTemplateId && s.Fboid == model.fboid);

            if (customerMarginObject != null)
            {
                var customerMargin = _context.CustomCustomerTypes.FirstOrDefault(s => s.CustomerId == model.id && s.Fboid == model.fboid);

                if(customerMargin != null)
                {
                    customerMargin.CustomerType = customerMarginObject.Oid;
                }
                else
                {
                    CustomCustomerTypes newType = new CustomCustomerTypes();
                    newType.CustomerType = customerMarginObject.Oid;
                    newType.Fboid = model.fboid;
                    newType.CustomerId = model.id;
                    _context.CustomCustomerTypes.Add(newType);
                }

                var groupInfo = _context.Fbos.FirstOrDefault(s => s.Oid == model.fboid).GroupId;
                var customerInfo = _context.CustomerInfoByGroup.FirstOrDefault(s => s.CustomerId == model.id && s.GroupId == groupInfo);

                if(customerInfo != null)
                {
                    customerInfo.PricingTemplateRemoved = false;
                    _context.CustomerInfoByGroup.Update(customerInfo);
                }

                await _context.SaveChangesAsync();
            }

            
            return Ok("");
            // return CreatedAtAction("GetCustomerMargins", new { id = customerMargins.Oid }, customerMargins);
        }

        // POST: api/CustomerMargins
        [HttpPost("updatemultiplecustomermargin")]
        public async Task<IActionResult> UpdateMultipleCustomerMargins(List<CustomerPricingTemplateViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach(var m in model)
            {
                var customerMarginObject = _context.PricingTemplate.FirstOrDefault(s => s.Oid == m.pricingTemplateId && s.Fboid == m.fboid);

                if (customerMarginObject != null)
                {
                    var customerMargin = _context.CustomCustomerTypes.FirstOrDefault(s => s.CustomerId == m.id && s.Fboid == m.fboid);

                    if (customerMargin != null)
                    {
                        customerMargin.CustomerType = customerMarginObject.Oid;
                    }
                    else
                    {
                        CustomCustomerTypes newType = new CustomCustomerTypes();
                        newType.CustomerType = customerMarginObject.Oid;
                        newType.Fboid = m.fboid;
                        newType.CustomerId = m.id;
                        _context.CustomCustomerTypes.Add(newType);
                    }

                    var groupInfo = _context.Fbos.FirstOrDefault(s => s.Oid == m.fboid).GroupId;
                    var customerInfo = _context.CustomerInfoByGroup.FirstOrDefault(s => s.CustomerId == m.id && s.GroupId == groupInfo);

                    if (customerInfo != null)
                    {
                        customerInfo.PricingTemplateRemoved = false;
                        _context.CustomerInfoByGroup.Update(customerInfo);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok("");
        }
    }
}