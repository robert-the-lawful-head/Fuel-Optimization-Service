using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.SWIM;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.Web.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FBOLinx.Web.Controllers
{
    [APIKey]
    [Route("api/[controller]")]
    [ApiController]
    public class SWIMController : ControllerBase
    {
        private readonly ISWIMService _SWIMService;

        public SWIMController(ISWIMService swimService)
        {
            _SWIMService = swimService;
        }

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
                return Ok(ex.Message);
            }
        }
    }
}
