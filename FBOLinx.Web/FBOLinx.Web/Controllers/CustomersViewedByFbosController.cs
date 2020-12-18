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
using Microsoft.AspNetCore.Authorization;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersViewedByFbosController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public CustomersViewedByFbosController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/CustomersViewedByFboes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomersViewedByFbo>>> GetCustomersViewedByFbo()
        {
            return await _context.CustomersViewedByFbo.ToListAsync();
        }

        // GET: api/CustomersViewedByFboes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomersViewedByFbo>> GetCustomersViewedByFbo(int id)
        {
            var customersViewedByFbo = await _context.CustomersViewedByFbo.FindAsync(id);

            if (customersViewedByFbo == null)
            {
                return NotFound();
            }

            return customersViewedByFbo;
        }

        [HttpGet("needsattention/group/{groupId}/fbo/{fboId}")]
        public async Task<IActionResult> GetCustomerCountNeedingAttention([FromRoute] int groupId, [FromRoute] int fboId)
        {
            var customerCount = await (from cg in _context.CustomerInfoByGroup
                join cv in _context.CustomersViewedByFbo on new {cg.CustomerId, Fboid = fboId} equals new
                {
                    cv.CustomerId,
                    cv.Fboid
                } into leftJoinCV
                from cv in leftJoinCV.DefaultIfEmpty()
                where cg.GroupId == groupId
                      && (cv == null || cv.Oid == 0)
                select cg).CountAsync();

            return Ok(customerCount);
        }

        // PUT: api/CustomersViewedByFboes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomersViewedByFbo(int id, CustomersViewedByFbo customersViewedByFbo)
        {
            if (id != customersViewedByFbo.Oid)
            {
                return BadRequest();
            }

            _context.Entry(customersViewedByFbo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomersViewedByFboExists(id))
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

        // POST: api/CustomersViewedByFboes
        [HttpPost]
        public async Task<ActionResult<CustomersViewedByFbo>> PostCustomersViewedByFbo(CustomersViewedByFbo customersViewedByFbo)
        {
            var existingRecord = await _context.CustomersViewedByFbo.Where((x =>
                x.Fboid == customersViewedByFbo.Fboid && x.CustomerId == customersViewedByFbo.CustomerId)).FirstOrDefaultAsync();

            if (existingRecord != null && existingRecord.Oid > 0)
                return Ok(existingRecord);

            customersViewedByFbo.ViewDate = DateTime.Now;
            _context.CustomersViewedByFbo.Add(customersViewedByFbo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomersViewedByFbo", new { id = customersViewedByFbo.Oid }, customersViewedByFbo);
        }

        // DELETE: api/CustomersViewedByFboes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomersViewedByFbo>> DeleteCustomersViewedByFbo(int id)
        {
            var customersViewedByFbo = await _context.CustomersViewedByFbo.FindAsync(id);
            if (customersViewedByFbo == null)
            {
                return NotFound();
            }

            _context.CustomersViewedByFbo.Remove(customersViewedByFbo);
            await _context.SaveChangesAsync();

            return customersViewedByFbo;
        }

        private bool CustomersViewedByFboExists(int id)
        {
            return _context.CustomersViewedByFbo.Any(e => e.Oid == id);
        }
    }
}
