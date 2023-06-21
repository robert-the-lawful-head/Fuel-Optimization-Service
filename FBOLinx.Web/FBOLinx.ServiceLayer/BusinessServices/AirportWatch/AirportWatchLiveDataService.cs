using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.Core.Enums.TableStorage;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Models.ServiceLogs;
using FBOLinx.DB.Projections.AirportWatch;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.DB.Specifications.ServiceLogs;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.AirportWatch;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using FBOLinx.TableStorage.Entities;
using FBOLinx.TableStorage.EntityServices;
using Fuelerlinx.SDK;
using Geolocation;
using Mapster;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public interface
        IAirportWatchLiveDataService : IBaseDTOService<AirportWatchLiveDataDto, DB.Models.AirportWatchLiveData>
    {
        Task<List<AirportWatchLiveDataWithHistoricalStatusDto>> GetAirportWatchLiveDataWithHistoricalStatuses(
            string airportIdentifier = null, int pastMinutesForLiveData = 1, int pastDaysForHistoricalData = 1);

        Task<List<AirportWatchLiveDataDto>> GetAirportWatchLiveDataRecordsFromTableStorage(IEnumerable<string> boxNames, DateTime startDate, DateTime endDate);
        Task<AirportWatchIntegrityCheckResult> CheckAirportWatchDataIntegrity(DateTime day);
        Task SaveAirportWatchLiveDataToTableStorage(IEnumerable<AirportWatchLiveDataDto> data);
    }

    public class AirportWatchLiveDataService : BaseDTOService<AirportWatchLiveDataDto, DB.Models.AirportWatchLiveData, FboLinxContext>, IAirportWatchLiveDataService
    {
        private const int _DistanceInNauticalMiles = 250;
        private IAirportWatchHistoricalDataService _AirportWatchHistoricalDataService;
        private IAirportService _AirportService;
        private IFboService _FboService;
        private readonly AirportWatchDataTableEntityService _airportWatchDataTableEntityService;
        private readonly TableStorageLogEntityService _TableStorageLogEntityService;

        public AirportWatchLiveDataService(IRepository<DB.Models.AirportWatchLiveData, 
                FboLinxContext> entityService,
            IAirportWatchHistoricalDataService airportWatchHistoricalDataService,
                IAirportService airportService,
            IFboService fboService, AirportWatchDataTableEntityService airportWatchDataTableEntityService, TableStorageLogEntityService tableStorageLogEntityService) : base(entityService)
        {
            _FboService = fboService;
            _AirportService = airportService;
            _AirportWatchHistoricalDataService = airportWatchHistoricalDataService;
            _airportWatchDataTableEntityService = airportWatchDataTableEntityService;
            _TableStorageLogEntityService = tableStorageLogEntityService;
        }

        public async Task<List<AirportWatchLiveDataWithHistoricalStatusDto>> GetAirportWatchLiveDataWithHistoricalStatuses(string airportIdentifier = null, int pastMinutesForLiveData = 1, int pastDaysForHistoricalData = 1)
        {
            //Load live data
            List<AirportWatchLiveDataDto> liveData = await GetLiveData(airportIdentifier, pastMinutesForLiveData);

            //Load historical data
            List<AirportWatchHistoricalDataDto> historicalData =
                await GetHistoricalData(liveData, airportIdentifier, pastDaysForHistoricalData);
            

            //Group the live data with past historical events over the last day
            var result = (from live in liveData
                          join historical in historicalData on new { live.AtcFlightNumber, live.AircraftHexCode } equals new { historical.AtcFlightNumber, historical.AircraftHexCode }
                              into leftJoinHistoricalData
                          from historical in leftJoinHistoricalData.DefaultIfEmpty()
                          group new { live, historical } by new { live.AtcFlightNumber, live.TailNumber, live.AircraftHexCode } into groupedResult
                          select new AirportWatchLiveDataWithHistoricalStatusDto
                          {
                              Oid = groupedResult.Max(x => x.live.Oid),
                              TailNumber = groupedResult.Key.TailNumber,
                              AtcFlightNumber = groupedResult.Key.AtcFlightNumber,
                              Latitude = groupedResult.FirstOrDefault().live.Latitude,
                              Longitude = groupedResult.FirstOrDefault().live.Longitude,
                              AircraftPositionDateTimeUtc = groupedResult.Max(x => x.live.AircraftPositionDateTimeUtc),
                              IsAircraftOnGround = groupedResult.FirstOrDefault().live.IsAircraftOnGround,
                              RecentAirportWatchHistoricalDataCollection = groupedResult.Where(x => x.historical != null).Select(x => x.historical).ToList(),
                              AirportWatchLiveData = groupedResult.Where(x => x.live != null).Select(x => x.live).FirstOrDefault()
                          }
                )
                .ToList();

            return result;
        }
        
        public async Task SaveAirportWatchLiveDataToTableStorage(IEnumerable<AirportWatchLiveDataDto> data)
        {
            if (data == null || !data.Any())
            {
                return;
            }

            string dataBlob = string.Empty;
            foreach (AirportWatchLiveDataDto airportWatchLiveDataDto in data)
            {
                dataBlob += airportWatchLiveDataDto.ToCsvString() + Environment.NewLine;
            }
            AirportWatchDataTableEntity airportWatchDataTableEntity = new AirportWatchDataTableEntity();
            airportWatchDataTableEntity.BoxTransmissionDateTimeUtc = DateTime.SpecifyKind(data.Max(x => x.BoxTransmissionDateTimeUtc), DateTimeKind.Utc);
            airportWatchDataTableEntity.MinAircraftPositionDateTimeUtc = DateTime.SpecifyKind(data.Min(x => x.AircraftPositionDateTimeUtc), DateTimeKind.Utc);
            airportWatchDataTableEntity.MaxAircraftPositionDateTimeUtc = DateTime.SpecifyKind(data.Max(x => x.AircraftPositionDateTimeUtc), DateTimeKind.Utc);
            airportWatchDataTableEntity.DataBlob = dataBlob;
            
            try
            {
                await _airportWatchDataTableEntityService.BatchInsert(new List<AirportWatchDataTableEntity>() { airportWatchDataTableEntity });
            }
            catch (Exception ex)
            {
                await _TableStorageLogEntityService.AddAsync(new TableStorageLog()
                {
                    TableEntityType = TableEntityType.AirportWatchLiveData,
                    LogType = TableStorageLogType.FailedBatchInsert,
                    RequestData = JsonConvert.SerializeObject(data),
                    AdditionalData = ex.Message,
                });
            }
        }

        public async Task<List<AirportWatchLiveDataDto>> GetAirportWatchLiveDataRecordsFromTableStorage(IEnumerable<string> boxNames, DateTime startDate, DateTime endDate)
        {
            var airportWatchDataTableEntities = await _airportWatchDataTableEntityService.GetAirportWatchDataRecords(startDate, endDate);
            List<AirportWatchLiveDataDto> result = new List<AirportWatchLiveDataDto>();
            foreach (AirportWatchDataTableEntity airportWatchDataTableEntity in airportWatchDataTableEntities)
            {
                result.AddRange(ConvertToDTO(airportWatchDataTableEntity.DataBlob.Split(Environment.NewLine).ToList()));
            }

            if (boxNames != null && boxNames.Any())
            {
                result = result.Where(x => !string.IsNullOrWhiteSpace(x.BoxName) && boxNames.Contains(x.BoxName)).ToList();
            }
            return result;
        }

        public async Task<AirportWatchIntegrityCheckResult> CheckAirportWatchDataIntegrity(DateTime day)
        {
            var airportWatchDataTableEntities = await _airportWatchDataTableEntityService.GetAirportWatchDataRecords(day);
            List<AirportWatchLiveDataDto> airportWatchLiveDataRecords = new List<AirportWatchLiveDataDto>();
            foreach (AirportWatchDataTableEntity airportWatchDataTableEntity in airportWatchDataTableEntities)
            {
                airportWatchLiveDataRecords.AddRange(ConvertToDTO(airportWatchDataTableEntity.DataBlob.Split(Environment.NewLine).ToList()));
            }

            AirportWatchIntegrityCheckResult result = new AirportWatchIntegrityCheckResult();
            var airportWatchRecordGroupings = airportWatchLiveDataRecords.GroupBy(x => new { x.BoxTransmissionDateTimeUtc, x.AircraftHexCode, x.BoxName });
            result.DistinctRecordsCount = airportWatchRecordGroupings.Count();
            result.DuplicateRecordsCount = airportWatchRecordGroupings.Count(x => x.Count() > 1);
            return result;
        }

        private async Task<List<AirportWatchLiveDataDto>> GetLiveData(string airportIdentifier = null, int pastMinutesForLiveData = 1)
        {
            List<AirportWatchLiveDataDto> result = new List<AirportWatchLiveDataDto>();
            if (!string.IsNullOrEmpty(airportIdentifier))
            {
                var airportPosition = await _AirportService.GetAirportPositionByAirportIdentifier(airportIdentifier);
                CoordinateBoundaries boundaries = new CoordinateBoundaries(airportPosition.GetFboCoordinate(), _DistanceInNauticalMiles, DistanceUnit.Miles);
                result = await GetListbySpec(new AirportWatchLiveDataByBoundarySpecification(
                    DateTime.UtcNow.AddMinutes(-pastMinutesForLiveData), DateTime.UtcNow,
                    boundaries.MinLatitude, boundaries.MaxLatitude, boundaries.MinLongitude, boundaries.MaxLongitude));
            }
            else
            {
                result = await GetListbySpec(
                    new AirportWatchLiveDataSpecification(DateTime.UtcNow.AddMinutes(-pastMinutesForLiveData), DateTime.UtcNow));
            }

            return result;
        }

        private async Task<List<AirportWatchHistoricalDataDto>> GetHistoricalData(
            List<AirportWatchLiveDataDto> liveData, string airportIdentifier = null, int pastDaysForHistoricalData = 1)
        {
            if (pastDaysForHistoricalData <= 0)
                return new List<AirportWatchHistoricalDataDto>();

            var aircraftHexCodes = liveData.Where(x => !string.IsNullOrEmpty(x.AircraftHexCode)).Select(x => x.AircraftHexCode).ToList();

            var historicalData = await _AirportWatchHistoricalDataService.GetHistoricalData(
                DateTime.UtcNow.AddDays(-pastDaysForHistoricalData), DateTime.UtcNow,
                string.IsNullOrEmpty(airportIdentifier) ? null : new List<string>() { airportIdentifier },
                aircraftHexCodes);
            return historicalData;
        }

        private List<AirportWatchLiveDataDto> ConvertToDTO(List<string> csvData)
        {
            List<AirportWatchLiveDataDto> airportWatchData = new List<AirportWatchLiveDataDto>();

            foreach (string airportWatchDataScvRecord in csvData)
            {
                if (string.IsNullOrWhiteSpace(airportWatchDataScvRecord))
                {
                    continue;
                }

                var airportWatchDataFields = airportWatchDataScvRecord.Split(",");

                var airportWatchDataDto = new AirportWatchLiveDataDto();

                airportWatchDataDto.AircraftHexCode = airportWatchDataFields[0];
                airportWatchDataDto.AtcFlightNumber = airportWatchDataFields[1];
                airportWatchDataDto.BoxName = airportWatchDataFields[2];
                airportWatchDataDto.AircraftTypeCode = airportWatchDataFields[3];
                airportWatchDataDto.BoxTransmissionDateTimeUtc = DateTime.Parse(airportWatchDataFields[4]);
                airportWatchDataDto.AircraftPositionDateTimeUtc = DateTime.Parse(airportWatchDataFields[5]);
                airportWatchDataDto.AltitudeInStandardPressure = GetIntIfNotEmpty(airportWatchDataFields[6]);
                airportWatchDataDto.GroundSpeedKts = GetIntIfNotEmpty(airportWatchDataFields[7]);
                string trackingDegree = airportWatchDataFields[8];
                if (!string.IsNullOrEmpty(trackingDegree))
                {
                    airportWatchDataDto.TrackingDegree = double.Parse(trackingDegree);
                }
                airportWatchDataDto.Latitude = double.Parse(airportWatchDataFields[9]);
                airportWatchDataDto.Longitude = double.Parse(airportWatchDataFields[10]);
                airportWatchDataDto.VerticalSpeedKts = GetIntIfNotEmpty(airportWatchDataFields[11]);
                airportWatchDataDto.TransponderCode = GetIntIfNotEmpty(airportWatchDataFields[12]);
                airportWatchDataDto.GpsAltitude = GetIntIfNotEmpty(airportWatchDataFields[13]);
                airportWatchDataDto.IsAircraftOnGround = bool.Parse(airportWatchDataFields[14]);

                airportWatchData.Add(airportWatchDataDto);
            }

            return airportWatchData;
        }

        private int? GetIntIfNotEmpty(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return int.Parse(value);
        }
    }
}