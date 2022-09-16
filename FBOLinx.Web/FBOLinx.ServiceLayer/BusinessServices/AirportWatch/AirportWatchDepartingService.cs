using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.AcukwikAirport;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public interface IAirportWatchDepartingService
    {
        Task<IEnumerable<AirportWatchLiveDataWithHistoricalStatusDto>> GetAirportWatchLiveDataWithHistoricalStatuses(DateTime liveStartDate, DateTime historicalStartDate);
    }

    public class AirportWatchDepartingService : IAirportWatchDepartingService
    {
        private AirportWatchLiveDataEntityService _AirportWatchLiveDataEntityService;
        private IAirportService _AirportService;

        public AirportWatchDepartingService(AirportWatchLiveDataEntityService airportWatchLiveDataEntityService,
            IAirportService airportService)
        {
            _AirportService = airportService;
            _AirportWatchLiveDataEntityService = airportWatchLiveDataEntityService;
        }

        public async Task<List<AirportWatchLiveDataWithHistoricalStatusDto>> GetLiveDepartingStatuses()
        {
            var liveAircraftDataWithHistoricalStatuses =
                await _AirportWatchLiveDataEntityService.GetLiveDataWithRecentHistoryStatuses(DateTime.UtcNow.AddHours(-5),
                    DateTime.UtcNow.AddHours(-24));

            foreach (var liveAircraftData in liveAircraftDataWithHistoricalStatuses)
            {
                var mostRecentStatus = liveAircraftData.GetMostRecentStatus();
                if (liveAircraftData.IsAircraftOnGround)
                {
                    if (!mostRecentStatus.HasValue || mostRecentStatus == AircraftStatusType.Parking)
                        liveAircraftData.FlightLegStatus = FlightLegStatus.TaxiingOrigin;
                    else if (liveAircraftData.GetMinutesSinceLastHistoricalStatusChange() > 30)
                        liveAircraftData.FlightLegStatus = FlightLegStatus.TaxiingOrigin;
                    else
                    {
                        var nearestAirportPosition = await _AirportService.GetNearestAirportPosition(liveAircraftData.Latitude, liveAircraftData.Longitude);
                    }
                }

                if (liveAircraftData.GetMostRecentStatus().GetValueOrDefault() != AircraftStatusType.Parking).get
            }

            List<AcukwikAirport> airports = await _AcukwikAirportEntityService.GetListBySpec(new AcukwikAirportSpecification());
        }
        

        public async Task<IEnumerable<AirportWatchLiveDataWithHistoricalStatusDto>> GetAirportWatchLiveDataWithHistoricalStatuses(DateTime liveStartDate, DateTime historicalStartDate)
        {
            var liveAircraftDataWithHistoricalStatuses =
                await _AirportWatchLiveDataEntityService.GetLiveDataWithRecentHistoryStatuses(liveStartDate,
                    historicalStartDate);

            return liveAircraftDataWithHistoricalStatuses;
        }
    }
}
