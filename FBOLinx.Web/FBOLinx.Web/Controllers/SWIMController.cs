using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.SWIM;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.Web.Models.Responses.SWIM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SWIMController : ControllerBase
    {
        private readonly ISWIMService _SWIMService;

        public SWIMController(ISWIMService swimService)
        {
            _SWIMService = swimService;
        }

        [HttpGet("departures/{icao}")]
        public async Task<ActionResult<FlightLegsResponse>> GetDepartures([FromRoute] string icao)
        {
            IEnumerable<FlightLegDTO> result = await _SWIMService.GetDepartures(icao);

            return Ok(new FlightLegsResponse(result));
        }

        [HttpGet("arrivals/{icao}")]
        public async Task<ActionResult<FlightLegsResponse>> GetArrivals([FromRoute] string icao)
        {
            IEnumerable<FlightLegDTO> result = await _SWIMService.GetArrivals(icao);

            return Ok(new FlightLegsResponse(result));
        }

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
                return BadRequest(ex.Message);
            }
        }
    }
}