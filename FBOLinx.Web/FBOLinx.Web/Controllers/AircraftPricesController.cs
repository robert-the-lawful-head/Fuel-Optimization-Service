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
using FBOLinx.ServiceLayer.Logging;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftPricesController : FBOLinxControllerBase
    {
        private readonly FboLinxContext _context;

        public AircraftPricesController(FboLinxContext context, ILoggingService logger) : base(logger)
        {
            _context = context;
        }

        // GET: api/AircraftPrices
        [HttpGet]
        public IEnumerable<AircraftPrices> GetAircraftPrices()
        {
            return _context.AircraftPrices;
        }

        // GET: api/AircraftPrices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAircraftPrices([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var aircraftPrices = await _context.AircraftPrices.FindAsync(id);

            if (aircraftPrices == null)
            {
                return NotFound();
            }

            return Ok(aircraftPrices);
        }

        // GET: api/AircraftPrices/5
        [HttpGet("customeraircraft/{customerAircraftId}/fbo/{fboId}")]
        public async Task<IActionResult> GetAircraftPricesForCustomerAircraft([FromRoute] int customerAircraftId, [FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var aircraftPrices = await (from ca in _context.CustomerAircrafts
                join ap in _context.AircraftPrices on ca.Oid equals ap.CustomerAircraftId
                join pt in _context.PricingTemplate on ap.PriceTemplateId equals pt.Oid
                where ca.Oid == customerAircraftId
                      && pt.Fboid == fboId
                select ap).ToListAsync();

            if (aircraftPrices == null)
            {
                return NotFound();
            }

            return Ok(aircraftPrices);
        }

        // PUT: api/AircraftPrices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAircraftPrices([FromRoute] int id, [FromBody] AircraftPrices aircraftPrices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aircraftPrices.Oid)
            {
                return BadRequest();
            }

            _context.Entry(aircraftPrices).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AircraftPricesExists(id))
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

        // POST: api/AircraftPrices
        [HttpPost]
        public async Task<IActionResult> PostAircraftPrices([FromBody] AircraftPrices aircraftPrices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AircraftPrices.Add(aircraftPrices);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAircraftPrices", new { id = aircraftPrices.Oid }, aircraftPrices);
        }

        // DELETE: api/AircraftPrices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAircraftPrice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var aircraftPrices = await _context.AircraftPrices.FindAsync(id);
            if (aircraftPrices == null)
            {
                return NotFound();
            }

            _context.AircraftPrices.Remove(aircraftPrices);
            await _context.SaveChangesAsync();

            return Ok(aircraftPrices);
        }

        [HttpPost("delete-multiple")]
        public async Task<IActionResult> DeleteAircraftPricesByCustomerAircraftIds([FromBody] List<CustomerAircrafts> customerAircrafts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAircraftIds = customerAircrafts.Select(ca => ca.Oid).ToList();

            var aircraftPrices = await _context.AircraftPrices
                                            .Where(ap => customerAircraftIds.Contains(ap.CustomerAircraftId))
                                            .ToListAsync();

            _context.AircraftPrices.RemoveRange(aircraftPrices);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool AircraftPricesExists(int id)
        {
            return _context.AircraftPrices.Any(e => e.Oid == id);
        }
    }
}