using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class AirportFboGeofenceClustersController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public IServiceScopeFactory _serviceScopeFactory;
        private readonly AirportFboGeofenceClustersService _airportFboGeofenceClustersService;

        public AirportFboGeofenceClustersController(FboLinxContext context, IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory, AirportFboGeofenceClustersService airportFboGeofenceClustersService)
        {
            _airportFboGeofenceClustersService = airportFboGeofenceClustersService;
            _context = context;
            _HttpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
        }

        // GET: api/airportfbogeofenceclusters
        [HttpGet]
        public async Task<IActionResult> GetAllFboGeofenceClusters()
        {
            try
            {
                if (JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.Conductor)
                {
                    return BadRequest(ModelState);
                }

                var airportFBOGeoFenceClusters = await _airportFboGeofenceClustersService.GetAllClusters();

                if (airportFBOGeoFenceClusters == null)
                {
                    return NotFound();
                }

                return Ok(airportFBOGeoFenceClusters);
            }
            catch (Exception ex)
            {
                return Ok("Get error: " + ex.Message);
            }

        }

        // GET: api/airportfbogeofenceclusters/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFboGeofenceClusters([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != JwtManager.GetClaimedGroupId(_HttpContextAccessor) && JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.Conductor)
                {
                    return BadRequest(ModelState);
                }

                var airportFBOGeoFenceClusters = await _context.AirportFboGeofenceClusters.FindAsync(id);

                if (airportFBOGeoFenceClusters == null)
                {
                    return NotFound();
                }

                return Ok(airportFBOGeoFenceClusters);
            }
            catch (Exception ex)
            {
                return Ok("Get error: " + ex.Message);
            }

        }

        // PUT: api/airportfbogeofenceclusters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssociation([FromRoute] int id, [FromBody] AirportFboGeofenceClusters airportFboGeofenceClusters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != JwtManager.GetClaimedGroupId(_HttpContextAccessor) && JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.Conductor)
            {
                return BadRequest(ModelState);
            }

            _context.AirportFboGeofenceClusters.Update(airportFboGeofenceClusters);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Group.Any(e => e.Oid == id))
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

        // POST: api/airportfbogeofenceclusters
        [HttpPost]
        public async Task<IActionResult> PostAirportFboGeofenceClusters([FromBody] AirportFboGeofenceClusters airportFboGeoFenceClusters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                airportFboGeoFenceClusters = await _airportFboGeofenceClustersService.CreateNewCluster(airportFboGeoFenceClusters);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

            return CreatedAtAction("GetFboGeofenceClusters", new { id = airportFboGeoFenceClusters.Oid }, airportFboGeoFenceClusters);
        }

        // DELETE: api/airportfbogeofenceclusters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirportFboGeofenceClusters([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var association = await _context.AirportFboGeofenceClusters.FindAsync(id);
            if (association == null)
            {
                return NotFound();
            }

            try
            {
                await _airportFboGeofenceClustersService.DeleteCluster(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(association);
        }
    }
}
