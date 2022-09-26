using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.ServiceLayer.EntityServices;
using Geolocation;
using Mapster;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public interface
        IAirportWatchLiveDataService : IBaseDTOService<AirportWatchLiveDataDto, DB.Models.AirportWatchLiveData>
    {
        Task<List<AirportWatchLiveDataWithHistoricalStatusDto>> GetAirportWatchLiveDataWithHistoricalStatuses(
            int? centerfboId = null, int pastMinutesForLiveData = 1, int pastDaysForHistoricalData = 1);
    }

    public class AirportWatchLiveDataService : BaseDTOService<AirportWatchLiveDataDto, DB.Models.AirportWatchLiveData, FboLinxContext>, IAirportWatchLiveDataService
    {
        private const int _DistanceInNauticalMiles = 250;
        private AirportWatchHistoricalDataEntityService _AirportWatchHistoricalDataEntityService;
        private IAirportService _AirportService;
        private IFboService _FboService;

        public AirportWatchLiveDataService(IRepository<DB.Models.AirportWatchLiveData, 
                FboLinxContext> entityService, 
            AirportWatchHistoricalDataEntityService airportWatchHistoricalDataEntityService,
                IAirportService airportService,
            IFboService fboService) : base(entityService)
        {
            _FboService = fboService;
            _AirportService = airportService;
            _AirportWatchHistoricalDataEntityService = airportWatchHistoricalDataEntityService;
        }

        public async Task<List<AirportWatchLiveDataWithHistoricalStatusDto>> GetAirportWatchLiveDataWithHistoricalStatuses(int? centerfboId = null, int pastMinutesForLiveData = 1, int pastDaysForHistoricalData = 1)
        {
            //Load live data
            List<AirportWatchLiveDataDto> liveData = await GetLiveData(centerfboId, pastMinutesForLiveData);
            var aircraftHexCodes = liveData.Where(x => !string.IsNullOrEmpty(x.AircraftHexCode)).Select(x => x.AircraftHexCode).ToList();
            
            //Load historical data
            List<DB.Models.AirportWatchHistoricalData> historicalData = new List<AirportWatchHistoricalData>();
            if (pastDaysForHistoricalData > 0)
                historicalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(
                    new AirportWatchHistoricalDataByHexCodeSpecification(aircraftHexCodes,
                        DateTime.UtcNow.AddDays(-pastDaysForHistoricalData)));

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
                              RecentAirportWatchHistoricalDataCollection = groupedResult.Where(x => x.historical != null).Select(x => x.historical.Adapt<AirportWatchHistoricalDataDto>()).ToList(),
                              AirportWatchLiveData = groupedResult.Where(x => x.live != null).Select(x => x.live).FirstOrDefault()
                          }
                )
                .ToList();

            return result;
        }

        private async Task<List<AirportWatchLiveDataDto>> GetLiveData(int? centerfboId = null, int pastMinutesForLiveData = 1)
        {
            List<AirportWatchLiveDataDto> result = new List<AirportWatchLiveDataDto>();
            if (centerfboId.GetValueOrDefault() > 0)
            {
                var airportPosition = await _AirportService.GetAirportPositionForFbo(centerfboId.GetValueOrDefault());
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
    }
}