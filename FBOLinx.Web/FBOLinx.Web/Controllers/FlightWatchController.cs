using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.FlightWatch;
using FBOLinx.ServiceLayer.DTO.Responses.FlightWatch;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch;
using FBOLinx.ServiceLayer.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FBOLinx.Web.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FlightWatchController : FBOLinxControllerBase
    {
        private IFlightWatchService _FlightWatchService;

        public FlightWatchController(IFlightWatchService flightWatchService, ILoggingService logger) : base(logger)
        {
            _FlightWatchService = flightWatchService;
        }

        [HttpGet("list/fbo/{fboId}")]
        public async Task<ActionResult<FlightWatchListResponse>> GetFlightWatchDataForFbo([FromRoute] int fboId, int nauticalMileRange = 250)
        {
            try
            {
                var result = await _FlightWatchService.GetCurrentFlightWatchData(new FlightWatchDataRequestOptions()
                {
                    FboIdForCenterPoint = fboId,
                    NauticalMileRadiusForData = nauticalMileRange,
                    IncludeCustomerAircraftInformation = true,
                    IncludeFuelOrderInformation = true,
                    IncludeVisitsAtFbo = true,
                    IncludeCompanyPricingLogLastQuoteDate = true
                });
                return Ok(new FlightWatchListResponse(result));
            }
            catch (System.Exception exception)
            {
                return Ok(new FlightWatchListResponse(false, exception.Message));
            }
        }

        [HttpGet("list/fbo/{fboId}/airport/{icao}")]
        public async Task<ActionResult<FlightWatchListResponse>> GetFlightWatchDataForFboAndAirport([FromRoute] int fboId, [FromRoute] string icao, int nauticalMileRange = 250)
        {
            try
            {
                var result = await _FlightWatchService.GetCurrentFlightWatchData(new FlightWatchDataRequestOptions()
                {
                    FboIdForCenterPoint = fboId,
                    NauticalMileRadiusForData = nauticalMileRange,
                    AirportIdentifier = icao,
                    IncludeCustomerAircraftInformation = true,
                    IncludeFuelOrderInformation = true,
                    IncludeVisitsAtFbo = true,
                    IncludeCompanyPricingLogLastQuoteDate = true
                });
                return Ok(new FlightWatchListResponse(result));
            }
            catch (System.Exception exception)
            {
                return Ok(new FlightWatchListResponse(false, exception.Message));
            }
        }

        [HttpGet("leg/{swimFlightLegId}")]
        public async Task<ActionResult<FlightWatchLegAdditionalDetailsResponse>> GetFlightWatchLegAdditionalDetails(
            [FromRoute] int swimFlightLegId)
        {
            try
            {
                var result = await _FlightWatchService.GetAdditionalDetailsForLeg(swimFlightLegId);
                return Ok(new FlightWatchLegAdditionalDetailsResponse(result));
            }
            catch (System.Exception exception)
            {
                return Ok(new FlightWatchLegAdditionalDetailsResponse(false, exception.Message));
            }
        }
    }
}
