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

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcukwikAirportsController : ControllerBase
    {
        private readonly DegaContext _context;
     

        public AcukwikAirportsController(DegaContext context)
        {
            _context = context;
        }

        // GET: api/AcukwikAirports
        [HttpGet]
        public async Task<ActionResult<List<AcukwikAirport>>> GetAcukwikAirports()
        {
            return Ok(await _context.AcukwikAirports.Where(x => x.Latitude != null && x.Longitude != null).ToListAsync());
        }

        // GET: api/AcukwikAirports/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAcukwikAirports([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var acukwikAirports = await _context.AcukwikAirports.FindAsync(id);

            if (acukwikAirports == null)
            {
                return NotFound();
            }

            return Ok(acukwikAirports);
        }

        // GET: api/AcukwikAirports/5
        [HttpGet("search/{value}")]
        public async Task<IActionResult> GetAcukwikAirports([FromRoute] string value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            value = value.ToLower();

            var acukwikAirports = await GetAllAirports().Where((x => (x != null && x.Icao.Contains(value))
                                                                        || (x.FullAirportName != null && x.FullAirportName.ToLower().Contains(value))
                                                                        || (x.Iata != null && x.Iata.ToLower().Contains(value))
                                                                        || (x.Faa != null && x.Faa.ToLower().Contains(value)))).ToListAsync();
            return Ok(acukwikAirports);
        }

        // GET: api/AcukwikAirports/icao/5
        [HttpGet("byicao/{icao}")]
        public async Task<IActionResult> GetAcukwikAirport([FromRoute] string icao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            icao = icao.ToLower();

            var acukwikAirport = await GetAllAirports().Where(x => (x != null && x.Icao == icao)).FirstOrDefaultAsync();
            return Ok(acukwikAirport);
        }

        // PUT: api/AcukwikAirports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAcukwikAirports([FromRoute] int id, [FromBody] AcukwikAirport acukwikAirport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != acukwikAirport.Oid)
            {
                return BadRequest();
            }

            _context.Entry(acukwikAirport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AcukwikAirportsExists(id))
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

        // POST: api/AcukwikAirports
        [HttpPost]
        public async Task<IActionResult> PostAcukwikAirports([FromBody] AcukwikAirport acukwikAirport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AcukwikAirports.Add(acukwikAirport);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AcukwikAirportsExists(acukwikAirport.Oid))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAcukwikAirports", new { id = acukwikAirport.Oid }, acukwikAirport);
        }

        // DELETE: api/AcukwikAirports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAcukwikAirports([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var acukwikAirports = await _context.AcukwikAirports.FindAsync(id);
            if (acukwikAirports == null)
            {
                return NotFound();
            }

            _context.AcukwikAirports.Remove(acukwikAirports);
            await _context.SaveChangesAsync();

            return Ok(acukwikAirports);
        }

        //GET: api/AcukwikAirports/KVNY/fbo-handler-detail/1234
        [HttpGet("{icao}/fbo-handler-detail")]
        public async Task<IActionResult> GetAcukwikFboHandlerDetailByIcao([FromRoute] string icao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var airport = await _context.AcukwikAirports
                                            .Where(x => !string.IsNullOrEmpty(x.Icao) && x.Icao.ToLower() == icao.ToLower())
                                            .FirstOrDefaultAsync();

            if (airport == null || airport.Oid == 0)
                return NotFound("No record found for that icao.");

            var results = await _context.AcukwikFbohandlerDetail
                                            .Where(x => x.AirportId == airport.Oid)
                                            .ToListAsync();

            if (results == null)
                return NotFound("No record found for that handler Id.");

            return Ok(results);
        }

        private bool AcukwikAirportsExists(int id)
        {
            return _context.AcukwikAirports.Any(e => e.Oid == id);
        }

        private IQueryable<AcukwikAirport> GetAllAirports()
        {
            return _context.AcukwikAirports.AsQueryable();
        }
    }
}