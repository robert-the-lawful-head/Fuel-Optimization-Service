using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.ServiceLayer.BusinessServices.SWIM;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Models.Responses.SWIM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Geolocation;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.DTO.Requests.AirportWatch;
using FBOLinx.ServiceLayer.DTO.Responses.AirportWatch;
// using Microsoft.Extensions.Logging;
// using System.Diagnostics; // do we need Stopwatch in sync-flight-legs function


namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SWIMController : FBOLinxControllerBase
    {
        private readonly ISWIMService _SWIMService;
        private readonly ILoggingService _LoggingService;

        public SWIMController(ISWIMService swimService, ILoggingService logger) : base(logger)
        {
            _SWIMService = swimService;
            _LoggingService = logger;
        }
        
        [HttpGet("departures/{icao}/group/{groupId}/fbo/{fboId}")]
        public async Task<ActionResult<FlightLegsResponse>> GetDepartures([FromRoute] int groupId, [FromRoute] int fboId, [FromRoute] string icao)
        {
            try
            {
                IEnumerable<FlightLegDTO> result = await _SWIMService.GetDepartures(groupId, fboId, icao);

                return Ok(new FlightLegsResponse(result));
            }
            catch (Exception ex)
            {
                _LoggingService.LogError(ex.Message, ex.StackTrace, LogLevel.Error, LogColorCode.Red);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("arrivals/{icao}/group/{groupId}/fbo/{fboId}")]
        public async Task<ActionResult<FlightLegsResponse>> GetArrivals([FromRoute] int groupId, [FromRoute] int fboId, [FromRoute] string icao)
        {
            try
            {
                IEnumerable<FlightLegDTO> result = await _SWIMService.GetArrivals(groupId, fboId, icao);

                return Ok(new FlightLegsResponse(result));
            }
            catch (Exception ex)
            {
                _LoggingService.LogError(ex.Message, ex.StackTrace, LogLevel.Error, LogColorCode.Red);
                return BadRequest(ex.Message);
            }
        }
        
        [AllowAnonymous]
        [APIKey(IntegrationPartnerTypes.Internal)]
        [HttpPost("flight-legs")]
        public async Task<ActionResult> PostFlightLeg([FromBody] IEnumerable<SWIMFlightLegDTO> flightLegs)
        {
            try
            {
                await _SWIMService.SaveFlightLegData(flightLegs);

                return Ok();
            }
            catch (Exception ex)
            {
                _LoggingService.LogError(ex.Message, ex.StackTrace, LogLevel.Error, LogColorCode.Red);
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [APIKey(IntegrationPartnerTypes.Internal)]
        [HttpPost("sync-flight-legs")]
        public async Task<ActionResult> SyncFlightLegs()
        {            
            
            try
            {
                await _SWIMService.SyncRecentAndUpcomingFlightLegs();

                return Ok();
            }
            catch (Exception ex)
            {
                _LoggingService.LogError(ex.Message, ex.StackTrace, LogLevel.Error, LogColorCode.Red);
                return BadRequest(ex.Message);
            }

        }
        

        
    }
}