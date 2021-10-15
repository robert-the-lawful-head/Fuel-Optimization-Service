using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    public class FboFeeAndTaxOmitsByPricingTemplateController : Controller
    {
        private FboLinxContext _context;

        public FboFeeAndTaxOmitsByPricingTemplateController(FboLinxContext context)
        {
            _context = context;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FboFeeAndTaxOmitsByPricingTemplate>> GetFboFeeAndTaxOmitsByPricingTemplate([FromRoute] int id)
        {
            var result = await _context.FboFeeAndTaxOmitsByPricingTemplate.FirstOrDefaultAsync(x => x.Oid == id);
            return Ok(result);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<FboFeeAndTaxOmitsByPricingTemplate>> Post([FromBody] FboFeeAndTaxOmitsByPricingTemplate fboFeeAndTaxOmitsByPricingTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.FboFeeAndTaxOmitsByPricingTemplate.Add(fboFeeAndTaxOmitsByPricingTemplate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFboFeeAndTaxOmitsByPricingTemplate", new { id = fboFeeAndTaxOmitsByPricingTemplate.Oid }, fboFeeAndTaxOmitsByPricingTemplate);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FboFeeAndTaxOmitsByPricingTemplate fboFeeAndTaxOmitsByPricingTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.Entry(fboFeeAndTaxOmitsByPricingTemplate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                if (!(await FboFeeAndTaxOmitsByPricingTemplateExists(id)))
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
        public async Task<ActionResult<FboFeeAndTaxOmitsByPricingTemplate>> Delete(int id)
        {
            var fboFeeAndTaxOmitsByPricingTemplate = await _context.FboFeeAndTaxOmitsByPricingTemplate.FindAsync(id);
            if (fboFeeAndTaxOmitsByPricingTemplate == null)
            {
                return NotFound();
            }

            _context.FboFeeAndTaxOmitsByPricingTemplate.Remove(fboFeeAndTaxOmitsByPricingTemplate);
            await _context.SaveChangesAsync();

            return fboFeeAndTaxOmitsByPricingTemplate;
        }

        #region Private Methods
        private async Task<bool> FboFeeAndTaxOmitsByPricingTemplateExists(int id)
        {
            return await _context.FboFeeAndTaxOmitsByPricingTemplate.AnyAsync(e => e.Oid == id);
        }
        #endregion
    }
}
