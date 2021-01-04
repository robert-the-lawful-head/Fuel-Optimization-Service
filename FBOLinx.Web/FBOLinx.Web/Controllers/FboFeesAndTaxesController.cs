using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web;
using FBOLinx.Web.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FboFeesAndTaxesController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public FboFeesAndTaxesController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/FboFeesAndTaxes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FboFeesAndTaxes>>> GetFbofeesAndTaxes()
        {
            return await _context.FbofeesAndTaxes.ToListAsync();
        }

        // GET: api/FboFeesAndTaxes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FboFeesAndTaxes>> GetFboFeesAndTaxes(int id)
        {
            var fboFeesAndTaxes = await _context.FbofeesAndTaxes.FindAsync(id);

            if (fboFeesAndTaxes == null)
            {
                return NotFound();
            }

            return fboFeesAndTaxes;
        }

        // GET: api/fboFeesAndTaxes/fbo/5
        [HttpGet("fbo/{fboId}")]
        public async Task<ActionResult<List<FboFeesAndTaxes>>> GetFboFeesAndTaxesByFbo(int fboId)
        {
            var fboFeesandTaxes = await _context.FbofeesAndTaxes.Where(x => x.Fboid == fboId).ToListAsync();

            return Ok(fboFeesandTaxes);
        }

        // GET api/<controller>/fbo/5
        [HttpGet("fbo/{fboId}/customer/{customerId}")]
        public async Task<ActionResult<List<FboFeesAndTaxes>>> GetFboFeesAndTaxesFboAndCustomer([FromRoute] int fboId, [FromRoute] int customerId)
        {
            var result = await _context.FbofeesAndTaxes.Where(x => x.Fboid == fboId).Include(x => x.OmitsByCustomer).ToListAsync();
            result.ForEach(x =>
            {
                x.IsOmitted = (x.OmitsByCustomer != null && x.OmitsByCustomer.Any(o => o.CustomerId == customerId));
            });
            //var result = await _context.FboFeeAndTaxOmitsByCustomer.FirstOrDefaultAsync(x => x.Oid == id);
            return Ok(result);
        }

        // PUT: api/FboFeesAndTaxes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFboFeesAndTaxes(int id, FboFeesAndTaxes fboFeesAndTaxes)
        {
            if (id != fboFeesAndTaxes.Oid)
            {
                return BadRequest();
            }

            _context.Entry(fboFeesAndTaxes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FboFeesAndTaxesExists(id))
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

        // POST: api/FboFeesAndTaxes
        [HttpPost]
        public async Task<ActionResult<FboFeesAndTaxes>> PostFboFeesAndTaxes(FboFeesAndTaxes fboFeesAndTaxes)
        {
            _context.FbofeesAndTaxes.Add(fboFeesAndTaxes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFboFeesAndTaxes", new { id = fboFeesAndTaxes.Oid }, fboFeesAndTaxes);
        }

        // DELETE: api/FboFeesAndTaxes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FboFeesAndTaxes>> DeleteFboFeesAndTaxes(int id)
        {
            var fboFeesAndTaxes = await _context.FbofeesAndTaxes.FindAsync(id);
            if (fboFeesAndTaxes == null)
            {
                return NotFound();
            }

            _context.FbofeesAndTaxes.Remove(fboFeesAndTaxes);
            await _context.SaveChangesAsync();

            return fboFeesAndTaxes;
        }

        private bool FboFeesAndTaxesExists(int id)
        {
            return _context.FbofeesAndTaxes.Any(e => e.Oid == id);
        }
    }
}
