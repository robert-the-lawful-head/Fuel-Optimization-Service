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
    public class RampFeeSettingsController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public RampFeeSettingsController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/RampFeeSettings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RampFeeSettings>>> GetRampFeeSettings()
        {
            return await _context.RampFeeSettings.ToListAsync();
        }

        // GET: api/RampFeeSettings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RampFeeSettings>> GetRampFeeSettings(int id)
        {
            var rampFeeSettings = await _context.RampFeeSettings.FindAsync(id);

            if (rampFeeSettings == null)
            {
                return NotFound();
            }

            return rampFeeSettings;
        }

        // GET: api/RampFeeSettings/5
        [HttpGet("fbo/{fboId}")]
        public async Task<ActionResult<RampFeeSettings>> GetRampFeeSettingsForFbo([FromRoute] int fboId)
        {
            var rampFeeSettings = await _context.RampFeeSettings.Where((x => x.Fboid == fboId)).FirstOrDefaultAsync();

            if (rampFeeSettings == null)
            {
                return NotFound();
            }

            return rampFeeSettings;
        }

        // PUT: api/RampFeeSettings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRampFeeSettings(int id, RampFeeSettings rampFeeSettings)
        {
            if (id != rampFeeSettings.Oid)
            {
                return BadRequest();
            }

            _context.Entry(rampFeeSettings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RampFeeSettingsExists(id))
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

        // POST: api/RampFeeSettings
        [HttpPost]
        public async Task<ActionResult<RampFeeSettings>> PostRampFeeSettings(RampFeeSettings rampFeeSettings)
        {
            _context.RampFeeSettings.Add(rampFeeSettings);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRampFeeSettings", new { id = rampFeeSettings.Oid }, rampFeeSettings);
        }

        // DELETE: api/RampFeeSettings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RampFeeSettings>> DeleteRampFeeSettings(int id)
        {
            var rampFeeSettings = await _context.RampFeeSettings.FindAsync(id);
            if (rampFeeSettings == null)
            {
                return NotFound();
            }

            _context.RampFeeSettings.Remove(rampFeeSettings);
            await _context.SaveChangesAsync();

            return rampFeeSettings;
        }

        private bool RampFeeSettingsExists(int id)
        {
            return _context.RampFeeSettings.Any(e => e.Oid == id);
        }
    }
}
