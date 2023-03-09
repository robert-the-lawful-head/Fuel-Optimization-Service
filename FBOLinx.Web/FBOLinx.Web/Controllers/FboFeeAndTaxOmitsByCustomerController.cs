using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    public class FboFeeAndTaxOmitsByCustomerController : FBOLinxControllerBase
    {
        private FboLinxContext _context;

        public FboFeeAndTaxOmitsByCustomerController(FboLinxContext context, ILoggingService logger) : base(logger)
        {
            _context = context;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FboFeeAndTaxOmitsByCustomer>> GetFboFeeAndTaxOmitsByCustomer([FromRoute] int id)
        {
            var result = await _context.FboFeeAndTaxOmitsByCustomer.FirstOrDefaultAsync(x => x.Oid == id);
            return Ok(result);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<FboFeeAndTaxOmitsByCustomer>> Post([FromBody]FboFeeAndTaxOmitsByCustomer fboFeeAndTaxOmitsByCustomer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (fboFeeAndTaxOmitsByCustomer.Oid == 0)
            {
                _context.FboFeeAndTaxOmitsByCustomer.Add(fboFeeAndTaxOmitsByCustomer);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetFboFeeAndTaxOmitsByCustomer", new { id = fboFeeAndTaxOmitsByCustomer.Oid }, fboFeeAndTaxOmitsByCustomer);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FboFeeAndTaxOmitsByCustomer fboFeeAndTaxOmitsByCustomer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.Entry(fboFeeAndTaxOmitsByCustomer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                if (!(await FboFeeAndTaxOmitsByCustomerExists(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw exception;
                }
            }

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FboFeeAndTaxOmitsByCustomer>> Delete(int id)
        {
            var fboFeeAndTaxOmitsByCustomer = await _context.FboFeeAndTaxOmitsByCustomer.FindAsync(id);
            if (fboFeeAndTaxOmitsByCustomer == null)
            {
                return NotFound();
            }

            _context.FboFeeAndTaxOmitsByCustomer.Remove(fboFeeAndTaxOmitsByCustomer);
            await _context.SaveChangesAsync();

            return fboFeeAndTaxOmitsByCustomer;
        }

        #region Private Methods
        private async Task<bool> FboFeeAndTaxOmitsByCustomerExists(int id)
        {
            return await _context.FboFeeAndTaxOmitsByCustomer.AnyAsync(e => e.Oid == id);
        }
        #endregion
    }
}
