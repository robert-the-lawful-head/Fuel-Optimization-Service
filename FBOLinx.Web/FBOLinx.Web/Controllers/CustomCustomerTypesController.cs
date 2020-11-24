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
using FBOLinx.Web.Models.Requests;

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomCustomerTypesController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public CustomCustomerTypesController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/CustomCustomerTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomCustomerTypes>>> GetCustomCustomerTypes()
        {
            return await _context.CustomCustomerTypes.ToListAsync();
        }

        // GET: api/CustomCustomerTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomCustomerTypes>> GetCustomCustomerTypes(int id)
        {
            var customCustomerTypes = await _context.CustomCustomerTypes.FindAsync(id);

            if (customCustomerTypes == null)
            {
                return NotFound();
            }

            return customCustomerTypes;
        }

        // GET: api/CustomCustomerTypes/5
        [HttpGet("fbo/{fboId}/customer/{customerId}")]
        public async Task<IActionResult> GetCustomCustomerTypesForCustomer([FromRoute] int fboId, [FromRoute] int customerId)
        {
            var customCustomerTypes = await _context.CustomCustomerTypes.FirstOrDefaultAsync((x => x.CustomerId == customerId && x.Fboid == fboId));

            if (customCustomerTypes != null)
                return Ok(customCustomerTypes);

            //If no previous pricing template was attached, grab the default
            var defaultPricingTemplate = await _context.PricingTemplate
                .Where(x => x.Fboid == fboId && x.Default.GetValueOrDefault()).FirstOrDefaultAsync();
            
            if (defaultPricingTemplate == null)
                return null;
            
            var defaultCustomerType = new CustomCustomerTypes()
            {
                CustomerId = customerId,
                CustomerType = defaultPricingTemplate.Oid,
                Fboid = fboId
            };
            return Ok(defaultCustomerType);
        }

        [HttpPut("update")]
        public async Task<IActionResult> PutCustomCustomerTypesForCustomer([FromBody] CustomCustomerTypeUpdateRequest request)
        {
            var customCustomerType = _context.CustomCustomerTypes
                                            .FirstOrDefault((x => x.CustomerId == request.CustomerId && x.Fboid == request.FboId));

            if (customCustomerType == null)
                return NotFound();

            customCustomerType.CustomerType = request.PricingTemplateId;
            _context.CustomCustomerTypes.Update(customCustomerType);

            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/CustomCustomerTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomCustomerTypes(int id, CustomCustomerTypes customCustomerTypes)
        {
            if (id != customCustomerTypes.Oid)
            {
                return BadRequest();
            }

            _context.Entry(customCustomerTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomCustomerTypesExists(id))
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

        // POST: api/CustomCustomerTypes
        [HttpPost]
        public async Task<ActionResult<CustomCustomerTypes>> PostCustomCustomerTypes(CustomCustomerTypes customCustomerTypes)
        {
            _context.CustomCustomerTypes.Add(customCustomerTypes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomCustomerTypes", new { id = customCustomerTypes.Oid }, customCustomerTypes);
        }

        [HttpPost("collection")]
        public async Task<ActionResult<List<CustomCustomerTypes>>> UpdateCustomCustomerTypeCollection(List<CustomCustomerTypes> customCustomerTypesCollection)
        {
            foreach (var customCustomerType in customCustomerTypesCollection)
            {
                if (customCustomerType.Oid > 0)
                    _context.CustomCustomerTypes.Update(customCustomerType);
                else
                    _context.CustomCustomerTypes.Add(customCustomerType);
            }
            await _context.SaveChangesAsync();
            return customCustomerTypesCollection;
        }

        // DELETE: api/CustomCustomerTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomCustomerTypes>> DeleteCustomCustomerTypes(int id)
        {
            var customCustomerTypes = await _context.CustomCustomerTypes.FindAsync(id);
            if (customCustomerTypes == null)
            {
                return NotFound();
            }

            _context.CustomCustomerTypes.Remove(customCustomerTypes);
            await _context.SaveChangesAsync();

            return customCustomerTypes;
        }

        private bool CustomCustomerTypesExists(int id)
        {
            return _context.CustomCustomerTypes.Any(e => e.Oid == id);
        }

        [HttpPost("updatedefaulttemplate")]
        public async Task<ActionResult<PricingTemplate>> UpdateCustomCustomerTypeCollection(UpdateTemplateVM updateTemplate)
        {
           if(updateTemplate != null)
            {
                var currentDefault = _context.PricingTemplate.FirstOrDefault(s => s.Fboid == updateTemplate.fboid && s.Oid == updateTemplate.currenttemplate);

                var newDefault = _context.PricingTemplate.FirstOrDefault(s => s.Fboid == updateTemplate.fboid && s.Oid == updateTemplate.newtemplate);

                if(newDefault != null)
                {
                    newDefault.Default = true;
                }

                if(currentDefault != null)
                {
                    _context.PricingTemplate.Remove(currentDefault);
                }

                if (newDefault != null && currentDefault != null)
                {
                    var customers = _context.CustomCustomerTypes
                    .Where(c => c.Fboid.Equals(updateTemplate.fboid) && c.CustomerType.Equals(updateTemplate.currenttemplate))
                    .Select(s => s.CustomerId)
                    .ToList();

                    var groupInfo = _context.Fbos.FirstOrDefault(s => s.Oid == updateTemplate.fboid).GroupId;
                    _context.CustomerInfoByGroup.Where(s => customers.Contains(s.CustomerId) && s.GroupId == groupInfo).ToList().ForEach(s => s.PricingTemplateRemoved = true);

                    var customCustomerTypes = _context.CustomCustomerTypes
                        .Where(c => c.Fboid.Equals(updateTemplate.fboid) && c.CustomerType.Equals(updateTemplate.currenttemplate))
                        .ToList();

                    customCustomerTypes.ForEach(c => c.CustomerType = newDefault.Oid);
                }
                
                _context.PricingTemplate.Update(newDefault);

                _context.SaveChanges();

                return newDefault;
            }

            return null;
        }
    }
}
