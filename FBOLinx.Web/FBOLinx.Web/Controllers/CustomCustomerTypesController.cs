using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;

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
        public async Task<ActionResult<List<CustomCustomerTypes>>> GetCustomCustomerTypesForCustomer([FromRoute] int fboId, [FromRoute] int customerId)
        {
            var customCustomerTypes = await _context.CustomCustomerTypes.Where((x => x.CustomerId == customerId && x.Fboid == fboId)).ToListAsync();

            if (customCustomerTypes != null && customCustomerTypes.Count > 0)
                return customCustomerTypes;

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
            var defaultResult = new List<CustomCustomerTypes>();
            defaultResult.Add(defaultCustomerType);
            return defaultResult;
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
    }
}
