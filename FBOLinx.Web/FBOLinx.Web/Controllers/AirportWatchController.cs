﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Models.Responses.AirportWatch;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Web.Services;
using FBOLinx.DB.Context;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.Service.Mapping.Dto;
using Microsoft.EntityFrameworkCore;
using FBOLinx.ServiceLayer.Dto.Responses;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.AirportWatch;
using FBOLinx.ServiceLayer.DTO.Requests.AirportWatch;
using FBOLinx.ServiceLayer.DTO.Responses.AirportWatch;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Analytics;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.ServiceLayer.BusinessServices.Analytics;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch;
using Newtonsoft.Json;
using YamlDotNet.Core.Events;
using Fuelerlinx.SDK;

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
        private IIntraNetworkAntennaDataReportService _IntraNetworkAntennaDataReportService;
        private IAirportWatchHistoricalDataService _AirportWatchHistoricalDataService;
        private IAirportWatchHistoricalParkingService _AirportWatchHistoricalParkingService;
        private ILoggingService _logger;
        public AirportWatchController(AirportWatchService airportWatchService, IFboService fboService,
            FboLinxContext context, DBSCANService dBSCANService,
            IAirportWatchLiveDataService airportWatchLiveDataService, ILoggingService logger,
            IIntraNetworkAntennaDataReportService intraNetworkAntennaDataReportService,
            IAirportWatchHistoricalDataService airportWatchHistoricalDataService,
            IAirportWatchHistoricalParkingService airportWatchHistoricalParkingService) : base(logger)
        {
            _AirportWatchHistoricalDataService = airportWatchHistoricalDataService;
            _AirportWatchHistoricalParkingService = airportWatchHistoricalParkingService;
            _IntraNetworkAntennaDataReportService = intraNetworkAntennaDataReportService;
            _airportWatchService = airportWatchService;
            _fboService = fboService;
            _context = context;
            _dBSCANService = dBSCANService;
            _airportWatchLiveDataService = airportWatchLiveDataService;
            _logger = logger;
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
        public async Task<IActionResult> GetArrivalsDepartures([FromRoute] int groupId, [FromRoute] int fboId, [FromBody] AirportWatchHistoricalDataRequest request, [FromQuery] string icao = null)
        {
            var data2 = await _airportWatchService.GetArrivalsDeparturesRefactored(groupId, fboId, request, icao);
            return Ok(data2);
        }

        [HttpPost("fbo/{fboId}/arrivals-depatures-swim")]
        public async Task<IActionResult> GetArrivalsDeparturesSwim([FromRoute] int groupId, [FromRoute] int fboId, [FromBody] AirportWatchHistoricalDataRequest request, [FromQuery] string icao = null)
        {
            var data2 = await _airportWatchService.GetArrivalsDeparturesSwim(fboId, request.StartDateTime.GetValueOrDefault(), request.EndDateTime.GetValueOrDefault(), icao);
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
            await _airportWatchService.ProcessAirportWatchData(data);
            return Ok(new AirportWatchDataPostResponse(true));
        }
        [AllowAnonymous]
        [HttpPost("log-backwards")]
        public IActionResult logBakcwards([FromBody]FlightWatchModel flightWatch)
        {
            _logger.LogError($"tailnumber {flightWatch.TailNumber} went backwards  => {JsonConvert.SerializeObject(flightWatch)}",string.Empty,LogLevel.Info,LogColorCode.Blue);
            return Ok();
        }

        [HttpGet("get-airport-watch-live-data-from-table-storage")]
        public async Task<ActionResult<AirportWatchLiveDataResponse>> GetAirportWatchLiveDataFromTableStorage([FromQuery] IEnumerable<string> boxNames, DateTime startDate, DateTime endDate)
        {
            List<AirportWatchLiveDataDto> result = await _airportWatchLiveDataService.GetAirportWatchLiveDataRecordsFromTableStorage(boxNames, startDate, endDate);
            return Ok(new AirportWatchLiveDataResponse(result));
        }

        [AllowAnonymous]
        [HttpGet("check-airport-watch-data-integrity/{day}")]
        public async Task<ActionResult<AirportWatchIntegrityCheckResult>> CheckAirportWatchDataIntegrity([FromRoute] DateTime day)
        {
            AirportWatchIntegrityCheckResult result = await _airportWatchLiveDataService.CheckAirportWatchDataIntegrity(day);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("post-live-data-to-table-storage")]
        public async Task<ActionResult<AirportWatchDataPostResponse>> PostAirportWatchLiveDataToTableStorage([FromBody] List<AirportWatchLiveDataDto> data)
        {
            await _airportWatchLiveDataService.SaveAirportWatchLiveDataToTableStorage(data);
            return Ok(new AirportWatchDataPostResponse(true));
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
            if (startDateTime == null)
                startDateTime = DateTime.UtcNow.AddDays(-7);
            if (endDateTime == null)
                endDateTime = DateTime.UtcNow;
            var result = await _airportWatchService.GetParkingOccurencesByAirport(icao,
                startDateTime.GetValueOrDefault(), endDateTime.GetValueOrDefault());
            return result.Take(50).ToList();
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

        [HttpGet("intra-network/visits-report/group/{groupId}/fbo/{fboId}")]
        public async Task<ActionResult<List<IntraNetworkVisitsReportItem>>> GetIntraNetworkVisitsReportForGroup(
            [FromRoute] int groupId, [FromRoute] int fboId, DateTime? startDateTimeUtc = null, DateTime? endDateTimeUtc = null)
        {
            try
            {
                if (!startDateTimeUtc.HasValue || !endDateTimeUtc.HasValue)
                {
                    throw new System.Exception("Start and end date time are required.  Please provide startDateTimeUtc and endDateTimeUtc values in the querystring.");
                }

                var result = await _IntraNetworkAntennaDataReportService.GenerateReportForNetwork(groupId,fboId, startDateTimeUtc.Value, endDateTimeUtc.Value);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                HandleException(ex);
                return StatusCode(500, "Error getting intra network visits report for group {groupId}");
            }
        }

        [HttpGet("historical-parking/{id}")]
        public async Task<ActionResult<AirportWatchHistoricalParkingDto>> GetHistoricalParkingById([FromRoute] int id)
        {
            var result = await _AirportWatchHistoricalParkingService.GetSingleBySpec(new AirportWatchHistoricalParkingById(id));
            return Ok(result);
        }

        [HttpPost("historical-parking")]
        public async Task<ActionResult<AirportWatchHistoricalDataResponse>> CreateHistoricalParking(
            [FromBody] AirportWatchHistoricalDataResponse dto)
        {
            var historicalData = await  _AirportWatchHistoricalDataService.FindAsync(dto.AirportWatchHistoricalDataId);

            historicalData.AircraftStatus = Core.Enums.AircraftStatusType.Parking;
            historicalData.AircraftPositionDateTimeUtc = historicalData.AircraftPositionDateTimeUtc.AddMinutes(10);
            historicalData.Oid = 0;
            var parking = await _AirportWatchHistoricalDataService.AddAsync(historicalData);

            dto.AirportWatchHistoricalParking.AirportWatchHistoricalDataId = parking.Oid;
            var result = await _AirportWatchHistoricalParkingService.AddAsync(dto.AirportWatchHistoricalParking);
            parking.AirportWatchHistoricalParking = result;
            return Ok(parking);
        }

        [HttpPut("historical-parking")]
        public async Task<ActionResult<AirportWatchHistoricalDataResponse>> UpdateHistoricalParking(
            [FromBody] AirportWatchHistoricalDataResponse dto)
        {
            await _AirportWatchHistoricalParkingService.UpdateAsync(dto.AirportWatchHistoricalParking);

            return Ok();
        }

        [HttpDelete("historical-parking/{id}")]
        public async Task<ActionResult<AirportWatchHistoricalParkingDto>> DeleteHistoricalParking([FromRoute] int id)
        {
            var row = await _AirportWatchHistoricalParkingService.GetSingleBySpec(new AirportWatchHistoricalParkingById(id));
            if (row == null || row.Oid == 0)
            {
                return NotFound();
            }
            await _AirportWatchHistoricalParkingService.DeleteAsync(row);
            return Ok();
        }
    }
}
