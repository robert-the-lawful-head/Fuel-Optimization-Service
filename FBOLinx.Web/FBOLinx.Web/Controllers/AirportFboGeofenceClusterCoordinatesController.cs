using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FBOLinx.ServiceLayer.Logging;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AirportFboGeofenceClusterCoordinatesController : FBOLinxControllerBase
    {
        private FboLinxContext _fboLinxContext;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public AirportFboGeofenceClusterCoordinatesController(FboLinxContext fboLinxContext, IHttpContextAccessor httpContextAccessor, ILoggingService logger) : base(logger)
        {
            _fboLinxContext = fboLinxContext;
            _HttpContextAccessor = httpContextAccessor;
        }
        

        // GET api/<AirportFboGeofenceClusterCoordinatesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DB.Models.AirportFboGeofenceClusterCoordinates>> GetAirportGeoFenceClusterCoordinatesById(int id)
        {
            var result =
                await _fboLinxContext.AirportFboGeoFenceClusterCoordinates.FirstOrDefaultAsync(x => x.Oid == id);
            return Ok(result);
        }

        // POST api/<AirportFboGeofenceClusterCoordinatesController>
        [HttpPost]
        public async Task<IActionResult> PostAirportGeoFenceClusterCoordinates([FromBody] AirportFboGeofenceClusterCoordinates airportFboGeofenceClusterCoordinates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                 _fboLinxContext.AirportFboGeoFenceClusterCoordinates.Add(
                        airportFboGeofenceClusterCoordinates);
                await _fboLinxContext.SaveChangesAsync();
                return Ok(airportFboGeofenceClusterCoordinates);
            }
            catch (System.Exception exception)
            {
                return Ok(null);
            }
        }

        // PUT api/<AirportFboGeofenceClusterCoordinatesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAirportGeoFenceClusterCoordinates(int id,
            [FromBody] AirportFboGeofenceClusterCoordinates airportFboGeofenceClusterCoordinates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRecord =
                await _fboLinxContext.AirportFboGeoFenceClusterCoordinates.FirstOrDefaultAsync(x => x.Oid == id);
            if (existingRecord == null)
                return BadRequest("No record found with that id.");
            existingRecord.Longitude = airportFboGeofenceClusterCoordinates.Longitude;
            existingRecord.Latitude = airportFboGeofenceClusterCoordinates.Latitude;
            existingRecord.ClusterID = airportFboGeofenceClusterCoordinates.ClusterID;
            _fboLinxContext.Entry(existingRecord).State = EntityState.Modified;
            await _fboLinxContext.SaveChangesAsync();
            return Ok();
        }

        // DELETE api/<AirportFboGeofenceClusterCoordinatesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingRecord =
                await _fboLinxContext.AirportFboGeoFenceClusterCoordinates.FirstOrDefaultAsync(x => x.Oid == id);
            if (existingRecord == null)
                return BadRequest("No record found with that id.");
            _fboLinxContext.Entry(existingRecord).State = EntityState.Deleted;
            await _fboLinxContext.SaveChangesAsync();
            return Ok();
        }
    }
}
