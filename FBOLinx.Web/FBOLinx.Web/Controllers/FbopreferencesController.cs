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
    public class FbopreferencesController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public FbopreferencesController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/Fbopreferences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fbopreferences>>> GetFbopreferences()
        {
            return await _context.Fbopreferences.ToListAsync();
        }

        // GET: api/Fbopreferences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fbopreferences>> GetFbopreferences(int id)
        {
            var fbopreferences = await _context.Fbopreferences.FindAsync(id);

            if (fbopreferences == null)
            {
                return NotFound();
            }

            return fbopreferences;
        }

        // GET: api/Fbopreferences/5
        [HttpGet("fbo/{fboId}")]
        public async Task<ActionResult<Fbopreferences>> GetFbopreferencesForFbo(int fboId)
        {
            var fbopreferences = await _context.Fbopreferences.Where((x => x.Fboid == fboId)).FirstOrDefaultAsync();

            if (fbopreferences == null)
            {
                fbopreferences = new Fbopreferences() { Fboid = fboId, EnableJetA = true, EnableSaf = true };
                _context.Fbopreferences.Add(fbopreferences);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetFbopreferences", new { id = fbopreferences.Oid }, fbopreferences);
            }

            return fbopreferences;
        }

        // PUT: api/Fbopreferences/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFbopreferences(int id, Fbopreferences fbopreferences)
        {
            if (id != fbopreferences.Oid)
            {
                return BadRequest();
            }

            _context.Entry(fbopreferences).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FbopreferencesExists(id))
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

        // POST: api/Fbopreferences
        [HttpPost]
        public async Task<ActionResult<Fbopreferences>> PostFbopreferences(Fbopreferences fbopreferences)
        {
            _context.Fbopreferences.Add(fbopreferences);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFbopreferences", new { id = fbopreferences.Oid }, fbopreferences);
        }

        // DELETE: api/Fbopreferences/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Fbopreferences>> DeleteFbopreferences(int id)
        {
            var fbopreferences = await _context.Fbopreferences.FindAsync(id);
            if (fbopreferences == null)
            {
                return NotFound();
            }

            _context.Fbopreferences.Remove(fbopreferences);
            await _context.SaveChangesAsync();

            return fbopreferences;
        }

        private bool FbopreferencesExists(int id)
        {
            return _context.Fbopreferences.Any(e => e.Oid == id);
        }
    }
}
