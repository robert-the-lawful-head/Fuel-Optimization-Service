using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Services;
using FBOLinx.ServiceLayer.Logging;

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerCompanyTypesController : FBOLinxControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public CustomerCompanyTypesController(FboLinxContext context, IHttpContextAccessor httpContextAccessor, ILoggingService logger) : base(logger)
        {
            _context = context;
            _HttpContextAccessor = httpContextAccessor;
        }

        // GET: api/CustomerCompanyTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerCompanyTypes>>> GetCustomerCompanyTypes()
        {
            return await _context.CustomerCompanyTypes.ToListAsync();
        }

        // GET: api/CustomerCompanyTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerCompanyTypes>> GetCustomerCompanyTypes(int id)
        {
            var customerCompanyTypes = await _context.CustomerCompanyTypes.FindAsync(id);

            if (customerCompanyTypes == null)
            {
                return NotFound();
            }

            return customerCompanyTypes;
        }

        // GET: api/CustomerCompanyTypes/5
        [HttpGet("fbo/{fboId}")]
        public async Task<ActionResult<CustomerCompanyTypes>> GetCustomerCompanyTypesForFbo([FromRoute] int fboId)
        {
            var customerCompanyTypes = await _context.CustomerCompanyTypes
                                                        .Where((x => x.Fboid == fboId || x.Fboid == 0))
                                                        .OrderBy((x => x.Name))
                                                        .ToListAsync();

            if (customerCompanyTypes == null)
            {
                return NotFound();
            }

            return Ok(customerCompanyTypes);
        }

        [HttpGet("fbo/{fboId}/nofuelerlinx")]
        public async Task<ActionResult<CustomerCompanyTypes>> GetNoFuelerlinxCustomerCompanyTypesForFbo([FromRoute] int fboId)
        {
            var customerCompanyTypes = await _context.CustomerCompanyTypes
                                                        .Where(x => (x.Fboid == fboId || x.Fboid == 0) && (x.Name != "FuelerLinx"))
                                                        .OrderBy((x => x.Name))
                                                        .ToListAsync();

            if (customerCompanyTypes == null)
            {
                return NotFound();
            }

            return Ok(customerCompanyTypes);
        }

        // PUT: api/CustomerCompanyTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerCompanyTypes(int id, CustomerCompanyTypes customerCompanyTypes)
        {
            if (id != customerCompanyTypes.Oid)
            {
                return BadRequest();
            }

            _context.Entry(customerCompanyTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerCompanyTypesExists(id))
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

        // POST: api/CustomerCompanyTypes
        [HttpPost]
        public async Task<ActionResult<CustomerCompanyTypes>> PostCustomerCompanyTypes(CustomerCompanyTypes customerCompanyTypes)
        {
            _context.CustomerCompanyTypes.Add(customerCompanyTypes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerCompanyTypes", new { id = customerCompanyTypes.Oid }, customerCompanyTypes);
        }

        // DELETE: api/CustomerCompanyTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerCompanyTypes>> DeleteCustomerCompanyTypes(int id)
        {
            var customerCompanyTypes = await _context.CustomerCompanyTypes.FindAsync(id);
            if (customerCompanyTypes == null)
            {
                return NotFound();
            }

            _context.CustomerCompanyTypes.Remove(customerCompanyTypes);
            await _context.SaveChangesAsync();

            return customerCompanyTypes;
        }

        private bool CustomerCompanyTypesExists(int id)
        {
            return _context.CustomerCompanyTypes.Any(e => e.Oid == id);
        }
    }
}
