using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using FBOLinx.Web.Models.Responses.AirportWatch;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Web.Services;
using FBOLinx.Web.Models.Requests;
using FBOLinx.DB.Context;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportWatchController : ControllerBase
    {
        private readonly AirportWatchService _airportWatchService;
        private readonly FboService _fboService;
        private readonly FboLinxContext _context;

        public AirportWatchController(AirportWatchService airportWatchService, FboService fboService, FboLinxContext context)
        {
            _airportWatchService = airportWatchService;
            _fboService = fboService;
            _context = context;
        }

        [HttpGet("list/fbo/{fboId}")]
        public async Task<IActionResult> GetAirportLiveData([FromRoute] int fboId)
        {
            var fboLocation = await _fboService.GetFBOLocaiton(fboId);
            var data = await _airportWatchService.GetAirportWatchLiveData(fboLocation);
            return Ok(new
            {
                FBOLocation = fboLocation,
                FlightWatchData = data,
            });
        }

        [HttpPost("group/{groupId}/fbo/{fboId}/arrivals-depatures")]
        public async Task<IActionResult> GetArrivalsDepartures([FromRoute] int groupId, [FromRoute] int fboId, [FromBody] AirportWatchHistoricalDataRequest request)
        {
            var data = await _airportWatchService.GetArrivalsDepartures(groupId, fboId, request);
            return Ok(data);
        }

        [HttpPost("group/{groupId}/fbo/{fboId}/visits")]
        public async Task<IActionResult> GetVisits([FromRoute] int groupId, [FromRoute] int fboId, [FromBody] AirportWatchHistoricalDataRequest request)
        {
            var data = await _airportWatchService.GetVisits(groupId, fboId, request);
            return Ok(data);
        }

        [AllowAnonymous]
        [HttpPost("list")]
        public async Task<ActionResult<AiriportWatchDataPostResponse>> PostDataList([FromBody] List<AirportWatchLiveData> data)
        {
            try
            {
                await _airportWatchService.ProcessAirportWatchData(data);
                return Ok(new AiriportWatchDataPostResponse(true));
            }
            catch (Exception exception)
            {
                return Ok(new AiriportWatchDataPostResponse(false, exception.Message));
            }
        }

        [HttpGet("start-date")]
        public async Task<IActionResult> GetAirportWatchStartDate()
        {
            var startRecord = await _context.AirportWatchHistoricalData.OrderBy(item => item.AircraftPositionDateTimeUtc).FirstOrDefaultAsync();

            return Ok(startRecord.AircraftPositionDateTimeUtc);
        }
    }
}
