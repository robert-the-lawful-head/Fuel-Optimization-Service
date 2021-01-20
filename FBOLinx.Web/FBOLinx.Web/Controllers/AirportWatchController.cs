using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using FBOLinx.Web.Models.Responses.AirportWatch;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Web.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportWatchController : ControllerBase
    {
        private readonly AirportWatchService _airportWatchService;

        public AirportWatchController(AirportWatchService airportWatchService)
        {
            _airportWatchService = airportWatchService;
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
