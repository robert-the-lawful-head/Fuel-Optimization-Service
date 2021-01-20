using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using FBOLinx.Web.Models.Responses.AirportWatch;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportWatchController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("list")]
        public async Task<ActionResult<AiriportWatchDataPostResponse>> PostDataList([FromBody] List<AirportWatchHistoricalData> data)
        {
            try
            {
                return Ok(new AiriportWatchDataPostResponse(true));
            }
            catch (System.Exception exception)
            {
                return Ok(new AiriportWatchDataPostResponse(false, exception.Message));
            }
        }
    }
}
