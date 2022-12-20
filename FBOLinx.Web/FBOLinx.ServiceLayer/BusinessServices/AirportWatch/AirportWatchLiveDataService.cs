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
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using FBOLinx.TableStorage.Entities;
using FBOLinx.TableStorage.EntityServices;
using Fuelerlinx.SDK;
using Geolocation;
using Mapster;
using Newtonsoft.Json;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public interface
        IAirportWatchLiveDataService : IBaseDTOService<AirportWatchLiveDataDto, DB.Models.AirportWatchLiveData>
    {
        Task<List<AirportWatchLiveDataWithHistoricalStatusDto>> GetAirportWatchLiveDataWithHistoricalStatuses(
            string airportIdentifier = null, int pastMinutesForLiveData = 1, int pastDaysForHistoricalData = 1);

        Task<List<AirportWatchLiveDataDto>> GetAirportWatchLiveDataRecordsFromTableStorage(IEnumerable<string> boxNames, DateTime startDate, DateTime endDate);
        Task SaveAirportWatchLiveDataToTableStorage(IEnumerable<AirportWatchLiveDataDto> data);
    }

    public class AirportWatchLiveDataService : BaseDTOService<AirportWatchLiveDataDto, DB.Models.AirportWatchLiveData, FboLinxContext>, IAirportWatchLiveDataService
    {
        private const int _DistanceInNauticalMiles = 250;
        private AirportWatchHistoricalDataEntityService _AirportWatchHistoricalDataEntityService;
        private IAirportService _AirportService;
        private IFboService _FboService;
        private readonly AirportWatchLiveDataTableEntityService _airportWatchLiveDataTableEntityService;
        private readonly TableStorageLogEntityService _TableStorageLogEntityService;

        public AirportWatchLiveDataService(IRepository<DB.Models.AirportWatchLiveData, 
                FboLinxContext> entityService, 
            AirportWatchHistoricalDataEntityService airportWatchHistoricalDataEntityService,
                IAirportService airportService,
            IFboService fboService, AirportWatchLiveDataTableEntityService airportWatchLiveDataTableEntityService, TableStorageLogEntityService tableStorageLogEntityService) : base(entityService)
        {
            _FboService = fboService;
            _AirportService = airportService;
            _AirportWatchHistoricalDataEntityService = airportWatchHistoricalDataEntityService;
            _airportWatchLiveDataTableEntityService = airportWatchLiveDataTableEntityService;
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

        public async Task<List<AirportWatchLiveDataDto>> GetAirportWatchLiveDataRecordsFromTableStorage(IEnumerable<string> boxNames, DateTime startDate, DateTime endDate)
        {
            var airportWatchLiveDataTableEntities = await _airportWatchLiveDataTableEntityService.GetAirportWatchLiveDataRecords(boxNames, startDate, endDate);
            return airportWatchLiveDataTableEntities.Select(x => x.Adapt<AirportWatchLiveDataDto>()).ToList();
        }

        public async Task SaveAirportWatchLiveDataToTableStorage(IEnumerable<AirportWatchLiveDataDto> data)
        {
            IList<AirportWatchLiveDataTableEntity> airportWatchTableEntities = data.Select(x => new AirportWatchLiveDataTableEntity()
            {
                BoxName = x.BoxName,
                BoxTransmissionDateTimeUtc = DateTime.SpecifyKind(x.BoxTransmissionDateTimeUtc, DateTimeKind.Utc),
                AtcFlightNumber = x.AtcFlightNumber,
                AltitudeInStandardPressure = x.AltitudeInStandardPressure,
                GroundSpeedKts = x.GroundSpeedKts,
                TrackingDegree = x.TrackingDegree,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                VerticalSpeedKts = x.VerticalSpeedKts,
                TransponderCode = x.TransponderCode,
                AircraftPositionDateTimeUtc = DateTime.SpecifyKind(x.AircraftPositionDateTimeUtc, DateTimeKind.Utc),
                AircraftTypeCode = x.AircraftTypeCode,
                GpsAltitude = x.GpsAltitude,
                IsAircraftOnGround = x.IsAircraftOnGround,
                AircraftHexCode = x.AircraftHexCode,
            }).ToList();

            try
            {
                await _airportWatchLiveDataTableEntityService.BatchInsert(airportWatchTableEntities);
                await UpdateStatistics(airportWatchTableEntities);
            }
            catch (Exception ex)
            {
                await _TableStorageLogEntityService.AddAsync(new TableStorageLog()
                {
                    TableEntityType = TableEntityType.AirportWatchLiveData,
                    LogType = TableStorageLogType.FailedBatchInsert,
                    RequestData = JsonConvert.SerializeObject(airportWatchTableEntities),
                    AdditionalData = ex.Message,
                });
            }
        }

        private async Task UpdateStatistics(IEnumerable<AirportWatchLiveDataTableEntity> entities)
        {
            foreach (IGrouping<string, AirportWatchLiveDataTableEntity> entitiesGroup in entities.GroupBy(x => x.PartitionKey))
            {
                string partitionKey = entitiesGroup.First().PartitionKey;

                TableStorageLog tableStorageLogStatistics = await _TableStorageLogEntityService.GetSingleBySpec(new TableStorageLogSpecification(partitionKey));

                if (tableStorageLogStatistics != null)
                {
                    int recordsCount = int.Parse(tableStorageLogStatistics.AdditionalData);
                    recordsCount += entitiesGroup.Count();
                    tableStorageLogStatistics.AdditionalData = recordsCount.ToString();

                    await _TableStorageLogEntityService.UpdateAsync(tableStorageLogStatistics);
                }
                else
                {
                    await _TableStorageLogEntityService.AddAsync(new TableStorageLog()
                    {
                        TableEntityType = TableEntityType.AirportWatchLiveData,
                        LogType = TableStorageLogType.Statistics,
                        PartitionKey = partitionKey,
                        AdditionalData = entitiesGroup.Count().ToString(),
                    });
                }
            }
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
            
            if (string.IsNullOrEmpty(airportIdentifier))
            {
                var aircraftHexCodes = liveData.Where(x => !string.IsNullOrEmpty(x.AircraftHexCode)).Select(x => x.AircraftHexCode).ToList();
                var historicalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(
                    new AirportWatchHistoricalDataByHexCodeSpecification(aircraftHexCodes,
                        DateTime.UtcNow.AddDays(-pastDaysForHistoricalData)));
                return historicalData.Select(x => x.Adapt<AirportWatchHistoricalDataDto>()).ToList();
            }
            else
            {
                var tailNumbers = liveData.Where(x => !string.IsNullOrEmpty(x.TailNumber)).Select(x => x.TailNumber).ToList();
                var projectedHistoricalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec<AirportWatchHistoricalDataSimplifiedProjection>(
                    new AirportWatchHistoricalDataByIcaoSpecification(airportIdentifier,
                        DateTime.UtcNow.AddDays(-pastDaysForHistoricalData), DateTime.UtcNow));
                return projectedHistoricalData.Select(x => x.Adapt<AirportWatchHistoricalDataDto>()).ToList();
            }
        }
    }
}