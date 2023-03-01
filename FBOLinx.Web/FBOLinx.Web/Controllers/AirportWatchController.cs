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
using FBOLinx.Service.Mapping.Dto;
using Microsoft.EntityFrameworkCore;
using FBOLinx.ServiceLayer.Dto.Responses;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.Requests.AirportWatch;
using FBOLinx.ServiceLayer.DTO.Responses.AirportWatch;
using FBOLinx.Web.Auth;
using Fuelerlinx.SDK;
using FBOLinx.ServiceLayer.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportWatchController : FBOLinxControllerBase
    {
        private readonly AirportWatchService _airportWatchService;
        private readonly IAirportWatchLiveDataService _airportWatchLiveDataService;
        private readonly IFboService _fboService;
        private readonly FboLinxContext _context;
        private readonly DBSCANService _dBSCANService;

        public AirportWatchController(AirportWatchService airportWatchService, IFboService fboService, FboLinxContext context , DBSCANService dBSCANService, IAirportWatchLiveDataService airportWatchLiveDataService, ILoggingService logger) : base(logger)
        {
            _airportWatchService = airportWatchService;
            _fboService = fboService;
            _context = context;
            _dBSCANService = dBSCANService;
            _airportWatchLiveDataService = airportWatchLiveDataService;
        }

        [HttpGet("list/group/{groupId}/fbo/{fboId}")]
        public async Task<IActionResult> GetAirportLiveData([FromRoute] int groupId, [FromRoute] int fboId)
        {
            var fboLocation = await _fboService.GetFBOLocation(fboId);
            var data2 = await _airportWatchService.GetAirportWatchLiveDataRefactored(groupId, fboId, fboLocation);
            return Ok(new
            {
                FBOLocation = fboLocation,
                FlightWatchData = data2,
            });
        }

        //where we work with DBSCAN
        [HttpPost("group/{groupId}/fbo/{fboId}/arrivals-depatures")]
        public async Task<IActionResult> GetArrivalsDepartures([FromRoute] int groupId, [FromRoute] int fboId, [FromBody] AirportWatchHistoricalDataRequest request)
        {
            var data2 = await _airportWatchService.GetArrivalsDeparturesRefactored(groupId, fboId, request);
            return Ok(data2);
        }

        [HttpPost("group/{groupId}/fbo/{fboId}/visits")]
        public async Task<IActionResult> GetVisits([FromRoute] int groupId, [FromRoute] int fboId, [FromBody] AirportWatchHistoricalDataRequest request)
        {
            var data = await _airportWatchService.GetVisits(groupId, fboId, request);
            return Ok(data);
        }

        [AllowAnonymous]
        [HttpPost("list")]
        public async Task<ActionResult<AirportWatchDataPostResponse>> PostDataList([FromBody] List<AirportWatchLiveDataDto> data)
        {
            try
            {
                await _airportWatchService.ProcessAirportWatchData(data);
                return Ok(new AirportWatchDataPostResponse(true));
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    return Ok(new AirportWatchDataPostResponse(false, exception.Message + "***" + exception.InnerException.StackTrace + "****" + exception.StackTrace));
                else
                    return Ok(new AirportWatchDataPostResponse(false, exception.Message + "****" + exception.StackTrace));
            }
        }
        
        [HttpGet("get-airport-watch-live-data-from-table-storage")]
        public async Task<ActionResult<AirportWatchLiveDataResponse>> GetAirportWatchLiveDataFromTableStorage([FromQuery] IEnumerable<string> boxNames, DateTime startDate, DateTime endDate)
        {
            try
            {
                List<AirportWatchLiveDataDto> result = await _airportWatchLiveDataService.GetAirportWatchLiveDataRecordsFromTableStorage(boxNames, startDate, endDate);
                return Ok(new AirportWatchLiveDataResponse(result));
            }
            catch (Exception exception)
            {
                return Ok(new AirportWatchLiveDataResponse(false, exception.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost("post-live-data-to-table-storage")]
        public async Task<ActionResult<AirportWatchDataPostResponse>> PostAirportWatchLiveDataToTableStorage([FromBody] List<AirportWatchLiveDataDto> data)
        {
            try
            {
                await _airportWatchLiveDataService.SaveAirportWatchLiveDataToTableStorage(data);
                return Ok(new AirportWatchDataPostResponse(true));
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    return Ok(new AirportWatchDataPostResponse(false, exception.Message + "***" + exception.InnerException.StackTrace + "****" + exception.StackTrace));
                else
                    return Ok(new AirportWatchDataPostResponse(false, exception.Message + "****" + exception.StackTrace));
            }
        }

        [HttpGet("start-date")]
        public async Task<IActionResult> GetAirportWatchStartDate()
        {
            var startRecordDateTimeUtc = await _context.AirportWatchHistoricalData.MinAsync(x => x.AircraftPositionDateTimeUtc);

            return Ok(startRecordDateTimeUtc);
        }

        [HttpGet(("parking-occurrences/{icao}"))]
        public async Task<ActionResult<List<AirportWatchHistoricalDataDto>>> GetParkingOccurrencesByAirportIcao(
            [FromRoute] string icao, DateTime? startDateTime, DateTime? endDateTime)
        {
            try
            {
                if (startDateTime == null)
                    startDateTime = DateTime.UtcNow.AddDays(-7);
                if (endDateTime == null)
                    endDateTime = DateTime.UtcNow;
                var result = await _airportWatchService.GetParkingOccurencesByAirport(icao,
                    startDateTime.GetValueOrDefault(), endDateTime.GetValueOrDefault());
                return result.Take(50).ToList();
            }
            catch (System.Exception exception)
            {
                return Ok(new List<AirportWatchHistoricalData>());
            }
        }
        [HttpGet("aircraftLiveData/{groupId}/{fboId}/{tailNumber}")]
        public async Task<ActionResult<AircraftWatchLiveData>> GetAircraftLiveData(int groupId,int fboId, string tailNumber)
        {
            var aircraftInfo = await _airportWatchService.GetAircraftWatchLiveData(groupId, fboId, tailNumber);

            return Ok(aircraftInfo);
        }

        [HttpGet("allAntennas")]
        public async Task<ActionResult<AirportWatchAntennaStatusGrid>> GetAllAntennas()
        {
            var airportsWithAntennaData = await _airportWatchService.GetAntennaStatusData();

            return Ok(airportsWithAntennaData);
        }

        [HttpGet("unassignedAntennas/{antennaName}")]
        public async Task<ActionResult<AirportWatchAntennaStatusGrid>> GetAllUnassignedAntennas(string antennaName)
        {
            var unassignedAntennas = await _airportWatchService.GetDistinctUnassignedAntennaBoxes(antennaName);

            return Ok(unassignedAntennas);
        }

        //[HttpGet("test")]
        //public async Task<ActionResult> Test()
        //{
        //    await _airportWatchService.GetAirportWatchTestData();
        //    return Ok();
        //}
    }
}
