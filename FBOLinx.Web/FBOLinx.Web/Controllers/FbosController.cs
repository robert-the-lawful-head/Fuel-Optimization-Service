using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FbosController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public FbosController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/Fbos/group/5
        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetFboByGroupID([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fbos = await GetAllFbos().Include("fboAirport").Where((x => x.GroupId == groupId)).ToListAsync();

            //FbosViewModel used to display FBO info in the grid
            var fbosVM = fbos.Select(f => new FbosGridViewModel
            {
                Active = f.Active,
                Fbo = f.Fbo,
                Icao = f.fboAirport.Icao,
                Oid = f.Oid
            }).ToList();
            return Ok(fbosVM);
        }

        // GET: api/Fbos
        [HttpGet]
        public async Task<IActionResult> GetFbos()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fbos = await GetAllFbos().Include("fboAirport").ToListAsync();
            if (fbos == null)
            {
                return NotFound();
            }

            //FbosViewModel used to display FBO info in the grid
            var fbosVM = fbos.Select(f => new FbosGridViewModel
            {
                Active = f.Active,
                Fbo = f.Fbo,
                Icao = f.fboAirport?.Icao,
                Oid = f.Oid
            }).ToList();
            return Ok(fbosVM);
        }

        // GET: api/Fbos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFbo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbos = await _context.Fbos.FindAsync(id);

            if (fbos == null)
            {
                return NotFound();
            }

            return Ok(fbos);
        }

        // PUT: api/Fbos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFbos([FromRoute] int id, [FromBody] Fbos fbos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fbos.Oid)
            {
                return BadRequest();
            }

            _context.Entry(fbos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FbosExists(id))
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

        // POST: api/Fbos
        [HttpPost]
        public async Task<IActionResult> PostFbos([FromBody] Fbos fbos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Fbos.Add(fbos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFbo", new { id = fbos.Oid }, fbos);
        }

        // DELETE: api/Fbos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFbos([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbos = await _context.Fbos.FindAsync(id);
            if (fbos == null)
            {
                return NotFound();
            }

            _context.Fbos.Remove(fbos);
            await _context.SaveChangesAsync();

            return Ok(fbos);
        }

        [AllowAnonymous]
        [APIKey(IntegrationPartners.IntegrationPartnerTypes.Internal)]
        [HttpGet("by-akukwik-record/{handlerId}")]
        public async Task<ActionResult<Fbos>> GetFboByAcukwikRecord([FromRoute] int handlerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbo = await _context.Fbos.Where(x => x.AcukwikFBOHandlerId == handlerId).Include(x => x.Group).Include(x => x.fboAirport).Include(x => x.Contacts).ThenInclude(c => c.Contact).FirstOrDefaultAsync();

            return Ok(fbo);
        }

        private bool FbosExists(int id)
        {
            return _context.Fbos.Any(e => e.Oid == id);
        }

        private IQueryable<Fbos> GetAllFbos()
        {
            return _context.Fbos.AsQueryable();
        }
    }
}