using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class FbofeesController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public FbofeesController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/Fbofees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fbofees>>> GetFbofees()
        {
            return await _context.Fbofees.ToListAsync();
        }

        // GET: api/Fbofees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fbofees>> GetFbofees(int id)
        {
            var fbofees = await _context.Fbofees.FindAsync(id);

            if (fbofees == null)
            {
                return NotFound();
            }

            return fbofees;
        }

        // GET: api/Fbofees/5
        [HttpGet("fbo/{fboId}")]
        public async Task<ActionResult<IEnumerable<Fbofees>>> GetFbofeesForFbo(int fboId)
        {
            var fbofees = await _context.Fbofees.Where((x => x.Fboid == fboId)).ToListAsync();

            if (fbofees == null)
            {
                return NotFound();
            }

            return fbofees;
        }

        // PUT: api/Fbofees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFbofees(int id, Fbofees fbofees)
        {
            if (id != fbofees.Oid)
            {
                return BadRequest();
            }

            _context.Entry(fbofees).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FbofeesExists(id))
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

        // POST: api/Fbofees
        [HttpPost]
        public async Task<ActionResult<Fbofees>> PostFbofees(Fbofees fbofees)
        {
            _context.Fbofees.Add(fbofees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFbofees", new { id = fbofees.Oid }, fbofees);
        }

        // DELETE: api/Fbofees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Fbofees>> DeleteFbofees(int id)
        {
            var fbofees = await _context.Fbofees.FindAsync(id);
            if (fbofees == null)
            {
                return NotFound();
            }

            _context.Fbofees.Remove(fbofees);
            await _context.SaveChangesAsync();

            return fbofees;
        }

        private bool FbofeesExists(int id)
        {
            return _context.Fbofees.Any(e => e.Oid == id);
        }
    }
}
