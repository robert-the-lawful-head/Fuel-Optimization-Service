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
    public class AirCraftsController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public AirCraftsController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/AirCrafts
        [HttpGet]
        public IEnumerable<AirCrafts> GetAirCrafts()
        {
            return _context.Aircrafts.OrderBy((x => x.Make)).ThenBy((x => x.Model));
        }

        [HttpGet("Sizes")]
        public IEnumerable<Utilities.Enum.EnumDescriptionValue> GetAircraftSizes()
        {
            return Utilities.Enum.GetDescriptions(typeof(Models.AirCrafts.AircraftSizes));
        }

        // GET: api/AirCrafts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAirCrafts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAircrafts = await (from ca in _context.CustomerAircrafts
                                           join ac in _context.Aircrafts on ca.AircraftId equals ac.AircraftId into leftJoinAircrafts
                                           from ac in leftJoinAircrafts.DefaultIfEmpty()
                                           where ca.Oid == id
                                           select new
                                           {
                                               ca.Oid,
                                               ca.AircraftId,
                                               ca.TailNumber,
                                               ca.GroupId,
                                               ca.CustomerId,
                                               ac.Make,
                                               ac.Model,
                                               ca.Size
                                           }).FirstOrDefaultAsync();

           // var airCrafts = await _context.Aircrafts.FindAsync(id);

            if (customerAircrafts == null)
            {
                return NotFound();
            }

            return Ok(customerAircrafts);
        }

        // PUT: api/AirCrafts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAirCrafts([FromRoute] int id, [FromBody] AirCrafts airCrafts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != airCrafts.AircraftId)
            {
                return BadRequest();
            }

            _context.Entry(airCrafts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AirCraftsExists(id))
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

        // POST: api/AirCrafts
        [HttpPost]
        public async Task<IActionResult> PostAirCrafts([FromBody] AirCrafts airCrafts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Aircrafts.Add(airCrafts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAirCrafts", new { id = airCrafts.AircraftId }, airCrafts);
        }

        // DELETE: api/AirCrafts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirCrafts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var airCrafts = await _context.Aircrafts.FindAsync(id);
            if (airCrafts == null)
            {
                return NotFound();
            }

            _context.Aircrafts.Remove(airCrafts);
            await _context.SaveChangesAsync();

            return Ok(airCrafts);
        }

        private bool AirCraftsExists(int id)
        {
            return _context.Aircrafts.Any(e => e.AircraftId == id);
        }
    }
}