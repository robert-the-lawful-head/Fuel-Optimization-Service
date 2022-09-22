using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.Core.Utilities.Geography;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.AcukwikAirport;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.DB.Specifications.SWIM;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using Mapster;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public interface IAirportWatchFlightLegStatusService
    {
        Task<List<AirportWatchLiveDataWithHistoricalStatusDto>> GetAirportWatchLiveDataWithFlightLegStatuses();
    }

    public class AirportWatchFlightLegStatusService : IAirportWatchFlightLegStatusService
    {
        private AirportWatchLiveDataEntityService _AirportWatchLiveDataEntityService;
        private IAirportService _AirportService;
        private SWIMFlightLegEntityService _FlightLegEntityService;
        private AirportWatchHistoricalDataEntityService _AirportWatchHistoricalDataEntityService;

        public AirportWatchFlightLegStatusService(AirportWatchLiveDataEntityService airportWatchLiveDataEntityService,
            IAirportService airportService,
            SWIMFlightLegEntityService flightLegEntityService,
            AirportWatchHistoricalDataEntityService airportWatchHistoricalDataEntityService)
        {
            _AirportWatchHistoricalDataEntityService = airportWatchHistoricalDataEntityService;
            _FlightLegEntityService = flightLegEntityService;
            _AirportService = airportService;
            _AirportWatchLiveDataEntityService = airportWatchLiveDataEntityService;
        }

        public async Task<List<AirportWatchLiveDataWithHistoricalStatusDto>> GetAirportWatchLiveDataWithFlightLegStatuses()
        {
            //First load all live records from our FlightWatch antenna from the last 5 minutes with their associated historical records from the past 24 hours.
            var liveAircraftDataWithHistoricalStatuses =
                await GetAirportWatchLiveDataWithHistoricalStatuses();
            var aircraftIdentifiers = liveAircraftDataWithHistoricalStatuses
                .Where(x => !string.IsNullOrEmpty(x.TailNumber)).Select(x => x.TailNumber).Distinct().ToList();
            aircraftIdentifiers.AddRange(liveAircraftDataWithHistoricalStatuses.Where(x => !string.IsNullOrEmpty(x.AtcFlightNumber)).Select(x => x.AtcFlightNumber).ToList());

            //Then load all SWIM flight legs that we have from the last hour.
            List<SWIMFlightLeg> existingFlightLegs = (await _FlightLegEntityService.GetListBySpec(
                new SWIMFlightLegByAircraftSpecification(aircraftIdentifiers, DateTime.UtcNow.AddHours(-1))));

            //For each live antenna record we will determine the current flight status
            foreach (var liveAircraftData in liveAircraftDataWithHistoricalStatuses)
            {
                //Get the latest matching leg from SWIM data if available
                liveAircraftData.SWIMFlightLeg = existingFlightLegs.Where(x =>
                    x.AircraftIdentification == liveAircraftData.TailNumber ||
                    x.AircraftIdentification == liveAircraftData.AtcFlightNumber).OrderByDescending(x => x.Oid).FirstOrDefault();
                await SetFlightLegStatus(liveAircraftData);
            }

            return liveAircraftDataWithHistoricalStatuses;
        }

        private async Task SetFlightLegStatus(AirportWatchLiveDataWithHistoricalStatusDto liveAircraftData)
        {
            //Prepare our airport position and SWIM flight leg association for the live data
            liveAircraftData.AirportPosition =
                await _AirportService.GetNearestAirportPosition(liveAircraftData.Latitude,
                    liveAircraftData.Longitude);
            
            //Finally, determine the proper FlightLegStatus of each result based on all of the data we have
            var mostRecentHistoricalRecord = liveAircraftData.GetMostRecentHistoricalRecord();
            FlightLegStatus flightLegStatus = FlightLegStatus.EnRoute;

            if (IsTaxiingForDeparture(liveAircraftData, mostRecentHistoricalRecord))
                flightLegStatus = FlightLegStatus.TaxiingOrigin;

            else if (IsTaxiingToDestination(liveAircraftData, mostRecentHistoricalRecord))
                flightLegStatus = FlightLegStatus.TaxiingDestination;

            else if (IsLandingAtDestination(liveAircraftData, mostRecentHistoricalRecord))
                flightLegStatus = FlightLegStatus.Landing;

            else if (IsDepartingFromOrigin(liveAircraftData, mostRecentHistoricalRecord))
                flightLegStatus = FlightLegStatus.Departing;

            else if (HasArrivedAtDestination(liveAircraftData, mostRecentHistoricalRecord))
                flightLegStatus = FlightLegStatus.Arrived;

            liveAircraftData.FlightLegStatus = flightLegStatus;
            if (liveAircraftData.SWIMFlightLeg != null &&
                liveAircraftData.FlightLegStatus != liveAircraftData.SWIMFlightLeg.Status)
            {
                liveAircraftData.SWIMFlightLeg.Status = flightLegStatus;
                liveAircraftData.SWIMFlightLegStatusNeedsUpdate = true;
            }
        }


        private async Task<List<AirportWatchLiveDataWithHistoricalStatusDto>> GetAirportWatchLiveDataWithHistoricalStatuses()
        {
            var liveData = await _AirportWatchLiveDataEntityService.GetListBySpec(
                new AirportWatchLiveDataSpecification(DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow));
            var flightNumbers = liveData.Where(x => !string.IsNullOrEmpty(x.AtcFlightNumber)).Select(x => x.AtcFlightNumber).ToList();
            var historicalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(
                new AirportWatchHistoricalDataSpecification(flightNumbers, DateTime.UtcNow.AddDays(-1)));

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
                        RecentAirportWatchHistoricalDataCollection = groupedResult.Where(x => x.historical != null).Select(x => x.historical.Adapt<AirportWatchHistoricalDataDto>()).ToList()
                    }
                )
                .ToList();

            return result;
        }

        private bool IsTaxiingForDeparture(AirportWatchLiveDataWithHistoricalStatusDto liveAircraftData, AirportWatchHistoricalDataDto mostRecentHistoricalRecord)
        {
            if (!liveAircraftData.IsAircraftOnGround)
                return false;
            
            //No historical record exists, it's a new flight we haven't seen so must be prepping to depart
            if (mostRecentHistoricalRecord == null)
                return true;

            //If we have seen this aircraft then confirm we last saw it parked
            if (mostRecentHistoricalRecord.AircraftStatus != AircraftStatusType.Parking)
                return false;

            //If we have SWIM data for the upcoming flight leg and the departure airport doesn't match the one we are at then this is NOT taxi'ing for departure
            if (liveAircraftData.SWIMFlightLeg != null && liveAircraftData.SWIMFlightLeg.ATD > DateTime.UtcNow &&
                liveAircraftData.SWIMFlightLeg.DepartureICAO !=
                liveAircraftData.AirportPosition?.GetProperAirportIdentifier())
                return false;

            //Ensure the parking occurrence was registered within the past 10 minutes so it's still possibly moving
            if (Math.Abs((liveAircraftData.AircraftPositionDateTimeUtc -
                          mostRecentHistoricalRecord.AircraftPositionDateTimeUtc).TotalMinutes) < 10)
                return false;

            //Ensure the aircraft has moved from it's parking position
            if (Math.Abs(liveAircraftData.Latitude - mostRecentHistoricalRecord.Latitude) < 0.00001
                && Math.Abs(liveAircraftData.Longitude - mostRecentHistoricalRecord.Longitude) < 0.00001)
                return false;

            return true;
        }

        private bool IsTaxiingToDestination(AirportWatchLiveDataWithHistoricalStatusDto liveAircraftData,
            AirportWatchHistoricalDataDto mostRecentHistoricalRecord)
        {
            if (!liveAircraftData.IsAircraftOnGround)
                return false;

            //We haven't seen this flight yet.  It can't be taxi'ing into the arrival.
            if (mostRecentHistoricalRecord == null)
                return false;

            //Last known status was landing and the aircraft hasn't parked yet - this must be taxi'ing to the arrival destination.
            if (mostRecentHistoricalRecord.AircraftStatus == AircraftStatusType.Landing)
                return true;

            //Last known status was parking and it's been less than 10 minutes - if it's still moving then consider this taxi'ing to destination.
            if (mostRecentHistoricalRecord.AircraftStatus == AircraftStatusType.Parking && Math.Abs(
                    (liveAircraftData.AircraftPositionDateTimeUtc -
                     mostRecentHistoricalRecord.AircraftPositionDateTimeUtc).TotalMinutes) < 10)
                return true;
            return false;
        }

        private bool IsLandingAtDestination(AirportWatchLiveDataWithHistoricalStatusDto liveAircraftData,
            AirportWatchHistoricalDataDto mostRecentHistoricalRecord)
        {
            if (liveAircraftData.IsAircraftOnGround)
                return false;

            //Require the SWIM flight leg to confirm we know the actual destination of the aircraft.  Require the airport closest to the aircraft as well.
            if (liveAircraftData.SWIMFlightLeg == null || liveAircraftData.AirportPosition == null)
                return false;

            //If the arrival airport of the leg is not our nearest airport then we can't assume we are within landing range yet
            if (liveAircraftData.SWIMFlightLeg.ArrivalICAO !=
                liveAircraftData.AirportPosition.GetProperAirportIdentifier())
                return false;

            var distanceFromAirport =
                liveAircraftData.AirportPosition?.GetDistanceInMilesFromAirportPosition(liveAircraftData.Latitude,
                    liveAircraftData.Longitude);

            //We are considered "landing" if we are within 5 miles of the airport.  We've changed this because the ETA from the SWIM flight leg is a bit lagged in determining the actual arrival time.
            return (distanceFromAirport <= 5);
        }

        private bool IsDepartingFromOrigin(AirportWatchLiveDataWithHistoricalStatusDto liveAircraftData,
            AirportWatchHistoricalDataDto mostRecentHistoricalRecord)
        {
            if (liveAircraftData.IsAircraftOnGround)
                return false;

            //Ensure we have seen this aircraft actually depart the airport recently.
            if (mostRecentHistoricalRecord == null ||
                mostRecentHistoricalRecord.AircraftStatus != AircraftStatusType.Takeoff)
                return false;

            //The takeoff must have occurred in the last 5 minutes to be considered "departing" before turning into an "enroute" status.
            if (Math.Abs((liveAircraftData.AircraftPositionDateTimeUtc -
                          mostRecentHistoricalRecord.AircraftPositionDateTimeUtc).TotalMinutes) < 5)
                return false;
            
            return true;
        }

        private bool HasArrivedAtDestination(AirportWatchLiveDataWithHistoricalStatusDto liveAircraftData,
            AirportWatchHistoricalDataDto mostRecentHistoricalRecord)
        {
            if (!liveAircraftData.IsAircraftOnGround)
                return false;

            //We haven't seen this flight yet.  It can't be taxi'ing into the arrival.
            if (mostRecentHistoricalRecord == null)
                return false;

            //Last known status must be "parking".
            if (mostRecentHistoricalRecord.AircraftStatus != AircraftStatusType.Parking)
                return false;

            //Require a SWIM and AirportPosition record to ensure accuracy
            if (liveAircraftData.SWIMFlightLeg == null || liveAircraftData.AirportPosition == null)
                return false;

            //Confirm we have arrived at the airport that SWIM data claims we should be at.
            if (liveAircraftData.AirportPosition?.GetProperAirportIdentifier() !=
                liveAircraftData.SWIMFlightLeg?.ArrivalICAO)
                return false;

            //It's been over 10 minutes since our last parking update.
            if (Math.Abs(
                    (liveAircraftData.AircraftPositionDateTimeUtc -
                     mostRecentHistoricalRecord.AircraftPositionDateTimeUtc).TotalMinutes) >= 10)
                return true;
            return false;
        }
    }
}
