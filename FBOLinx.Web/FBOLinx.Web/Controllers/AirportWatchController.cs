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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportWatchController : ControllerBase
    {
        private readonly AirportWatchService _airportWatchService;
        private readonly FboService _fboService;

        public AirportWatchController(AirportWatchService airportWatchService, FboService fboService)
        {
            _airportWatchService = airportWatchService;
            _fboService = fboService;
        }

        [HttpGet("list/fbo/{fboId}")]
        public async Task<IActionResult> GetAirportLiveData([FromRoute] int fboId)
        {
            var fboLocation = await _fboService.GetFBOLocaiton(fboId);
            var data = await _airportWatchService.GetAirportWatchLiveData();
            return Ok(new
            {
                FBOLocation = fboLocation,
                FlightWatchData = data,
            });
        }

        [HttpPost("group/{groupId}/historical-data")]
        public async Task<IActionResult> GetHistoricalData([FromRoute] int groupId, [FromBody] AirportWatchHistoricalDataRequest request)
        {
            var data = await _airportWatchService.GetHistoricalData(groupId, request);
            return Ok(data);
        }

        [AllowAnonymous]
        [HttpPost("list")]
        public ActionResult<AiriportWatchDataPostResponse> PostDataList([FromBody] List<AirportWatchLiveData> data)
        {
            try
            {
                _airportWatchService.ProcessAirportWatchData(data);
                return Ok(new AiriportWatchDataPostResponse(true));
            }
            catch (Exception exception)
            {
                return Ok(new AiriportWatchDataPostResponse(false, exception.Message));
            }
        }
    }
}
