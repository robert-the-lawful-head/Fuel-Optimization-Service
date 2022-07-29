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

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SWIMController : ControllerBase
    {
        private readonly ISWIMService _SWIMService;
        private readonly ILoggingService _LoggingService;
        private readonly AirportWatchService _AirportWatchService;

        public SWIMController(ISWIMService swimService, ILoggingService loggingService, AirportWatchService airportWatchService)
        {
            _SWIMService = swimService;
            _AirportWatchService = airportWatchService;
            _LoggingService = loggingService;
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

        private async Task GetPastVisits(int groupId, int fboId)
        {
            // List<FboHistoricalDataModel> historicalData = await _AirportWatchService.GetAircraftsHistoricalDataAssociatedWithFboRefactored(
            //     groupId, fboId, new AirportWatchHistoricalDataRequest() { StartDateTime = DateTime.UtcNow.AddMonths(-1), EndDateTime = DateTime.UtcNow});
            // if (historicalData == null || !historicalData.Any())
            // {
            //     return;
            // }
            //
            // var customerVisitsData = historicalData
            //         .GroupBy(ah => new { ah.CustomerId, ah.AirportICAO, ah.AircraftHexCode, ah.AtcFlightNumber })
            //         .Select(g =>
            //         {
            //             var latest = g
            //                 .OrderByDescending(ah => ah.AircraftPositionDateTimeUtc).First();
            //
            //             var pastVisits = g
            //                .Where(ah => ah.AircraftStatus == AircraftStatusType.Parking).ToList();
            //
            //             var visitsToMyFbo = new List<FboHistoricalDataModel>();
            //             if (coordinates.Count > 0)
            //                 visitsToMyFbo = pastVisits.Where(p => IsPointInPolygon(new Coordinate(p.Latitude, p.Longitude), coordinates.ToArray())).ToList();
            //
            //             return new AirportWatchHistoricalDataResponse
            //             {
            //                 CustomerInfoByGroupID = latest.CustomerInfoByGroupID,
            //                 CompanyId = latest.CustomerId,
            //                 Company = latest.Company,
            //                 DateTime = latest.AircraftPositionDateTimeUtc,
            //                 TailNumber = latest.TailNumber,
            //                 FlightNumber = latest.AtcFlightNumber,
            //                 HexCode = latest.AircraftHexCode,
            //                 AircraftType = string.IsNullOrEmpty(latest.Make) ? null : latest.Make + " / " + latest.Model,
            //                 Status = latest.AircraftStatusDescription,
            //                 PastVisits = pastVisits.Count(),
            //                 AirportIcao = latest.AirportICAO,
            //                 AircraftTypeCode = latest.AircraftTypeCode,
            //                 VisitsToMyFbo = visitsToMyFbo.Count(),
            //                 PercentOfVisits = visitsToMyFbo.Count > 0 ? (double)(visitsToMyFbo.Count() / (double)pastVisits.Count()) : 0
            //             };
            //         })
            //         .ToList();
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
    }
}