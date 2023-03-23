using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Web.Services;
using FBOLinx.ServiceLayer.Logging;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AirCraftsController : FBOLinxControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly AircraftService _aircraftService;

       
       
        public AirCraftsController(FboLinxContext context, AircraftService aircraftService, ILoggingService logger) : base(logger)
        {
            _context = context;
            _aircraftService = aircraftService;
        }

        // GET: api/AirCrafts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AirCrafts>>> GetAirCrafts()
        {
            var result = await _aircraftService.GetAllAircrafts();
            return Ok(result.OrderBy(x => x.Make).ThenBy(x => x.Model));
        }

        [HttpGet("Sizes")]
        public IEnumerable<FBOLinx.Core.Utilities.Enum.EnumDescriptionValue> GetAircraftSizes()
        {
            return Core.Utilities.Enum.GetDescriptions(typeof(AircraftSizes));
        }

        // GET: api/AirCrafts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAirCrafts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAircraft = await _context.CustomerAircrafts.Where(x => x.Oid == id).FirstOrDefaultAsync();

            if (customerAircraft == null)
            {
                return NotFound();
            }

            var aircraft = await _aircraftService.GetAircrafts(customerAircraft.AircraftId);
            var result = new
            {
                Oid = customerAircraft.Oid,
                customerAircraft.AircraftId,
                customerAircraft.TailNumber,
                customerAircraft.GroupId,
                customerAircraft.CustomerId,
                aircraft?.Make,
                aircraft?.Model,
                customerAircraft.Size
            };

           
           return Ok(result);
        }

        [HttpGet("customers-by-tail/group/{groupId}/tail/{tailNumber}")]
        public async Task<ActionResult<CustomerInfoByGroup>> GetCustomersByTail([FromRoute] int groupId, [FromRoute] string tailNumber)
        {
            if (string.IsNullOrEmpty(tailNumber))
                return Ok(new List<CustomerInfoByGroup>());
            
            var customerAircraft = await _context.CustomerAircrafts.Where(x => x.TailNumber == tailNumber && x.GroupId == groupId).ToListAsync();
            if (customerAircraft == null)
                return Ok(new List<CustomerInfoByGroup>());

            List<int> customerIds = customerAircraft.Select(x => x.CustomerId).Distinct().ToList();

            var result = await _context.CustomerInfoByGroup.Where(x => x.GroupId == groupId && customerIds.Contains(x.CustomerId)).Include(x => x.Customer).Where(x => (x.Customer != null && x.Customer.Suspended != true)).ToListAsync();

            return Ok(result);
        }

        // PUT: api/AirCrafts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAirCrafts([FromRoute] int id, [FromBody] AirCraftsDto airCrafts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != airCrafts.AircraftId)
            {
                return BadRequest();
            }

            try
            {
                await _aircraftService.UpdateAirCrafts(airCrafts);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok();
        }

        // POST: api/AirCrafts
        [HttpPost]
        public async Task<IActionResult> PostAirCrafts([FromBody] AirCraftsDto airCrafts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _aircraftService.AddAirCrafts(airCrafts);

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

            var airCrafts = await _aircraftService.GetAircrafts(id);
            if (airCrafts == null)
            {
                return NotFound();
            }

            await _aircraftService.RemoveAirCrafts(airCrafts);

            return Ok(airCrafts);
        }

       
    }
}