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
using FBOLinx.Web.Services;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FboairportsController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly FboService _fboService;

        public FboairportsController(FboLinxContext context, FboService fboService)
        {
            _context = context;
            _fboService = fboService;
        }

        // GET: api/Fboairports
        [HttpGet]
        public IEnumerable<Fboairports> GetFboairports()
        {
            return _context.Fboairports;
        }

        // GET: api/Fboairports/Fbo/5
        [HttpGet("fbo/{fboId}")]
        public async Task<IActionResult> GetFboairportsByFbo([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fboairport = await GetAllFBOAirports().Where((x => x.Fboid == fboId)).FirstOrDefaultAsync();

            if (fboairport == null)
            {
                return NotFound();
            }

            return Ok(fboairport);
        }

        // GET: api/Fboairports/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFboairports([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fboairports = await _context.Fboairports.FindAsync(id);

            if (fboairports == null)
            {
                return NotFound();
            }

            return Ok(fboairports);
        }

        // PUT: api/Fboairports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFboairports([FromRoute] int id, [FromBody] Fboairports fboairports)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fboairports.Oid)
            {
                return BadRequest();
            }

            _context.Entry(fboairports).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FboairportsExists(id))
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

        // POST: api/Fboairports
        [HttpPost]
        public async Task<IActionResult> PostFboairports([FromBody] Fboairports fboairports)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Fboairports.Add(fboairports);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFboairports", new { id = fboairports.Oid }, fboairports);
        }

        // DELETE: api/Fboairports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFboairports([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fboairports = await _context.Fboairports.FindAsync(id);
            if (fboairports == null)
            {
                return NotFound();
            }

            _context.Fboairports.Remove(fboairports);
            await _context.SaveChangesAsync();

            return Ok(fboairports);
        }

        // GET: api/Fboairports/local-datetime-now/fbo/5
        [HttpGet("local-datetime-now/fbo/{fboId}")]
        public async Task<IActionResult> GetLocalDateTimeNowByFboId([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var localDateTime = await _fboService.GetAirportLocalDateTimeByFboId(fboId);

            return Ok(localDateTime);
        }

        // GET: api/Fboairports/local-timezone/fbo/5
        [HttpGet("local-timezone/fbo/{fboId}")]
        public async Task<IActionResult> GetLocalTimeZoneByFboId([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var localTimeZone = await _fboService.GetAirportTimeZoneByFboId(fboId);

            return Ok(localTimeZone);
        }

        private bool FboairportsExists(int id)
        {
            return _context.Fboairports.Any(e => e.Oid == id);
        }

        private IQueryable<Fboairports> GetAllFBOAirports()
        {
            return _context.Fboairports.AsQueryable();
        }
    }
}