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
using FBOLinx.ServiceLayer.BusinessServices.FlightWatch;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using Mapster;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public interface IAirportWatchFlightLegStatusService
    {
        Task<List<FlightWatchModel>> GetAirportWatchLiveDataWithFlightLegStatuses();
    }

    public class AirportWatchFlightLegStatusService : IAirportWatchFlightLegStatusService
    {
        private IAirportService _AirportService;
        private SWIMFlightLegEntityService _FlightLegEntityService;
        private IAirportWatchLiveDataService _AirportWatchLiveDataService;
        private IFlightWatchService _FlightWatchService;

        public AirportWatchFlightLegStatusService(IAirportService airportService,
            SWIMFlightLegEntityService flightLegEntityService,
            IAirportWatchLiveDataService airportWatchLiveDataService,
            IFlightWatchService flightWatchService)
        {
            _FlightWatchService = flightWatchService;
            _AirportWatchLiveDataService = airportWatchLiveDataService;
            _FlightLegEntityService = flightLegEntityService;
            _AirportService = airportService;
        }

        public async Task<List<FlightWatchModel>> GetAirportWatchLiveDataWithFlightLegStatuses()
        {
            var flightWatchData = await _FlightWatchService.GetCurrentFlightWatchData(new FlightWatchDataRequestOptions()
            {
                IncludeRecentHistoricalRecords = true,
                IncludeNearestAirportPosition = true
            });

            ////First load all live records from our FlightWatch antenna from the last 5 minutes with their associated historical records from the past 24 hours.
            //var liveAircraftDataWithHistoricalStatuses =
            //    await _AirportWatchLiveDataService.GetAirportWatchLiveDataWithHistoricalStatuses();
            //var aircraftIdentifiers = liveAircraftDataWithHistoricalStatuses
            //    .Where(x => !string.IsNullOrEmpty(x.TailNumber)).Select(x => x.TailNumber).Distinct().ToList();
            //aircraftIdentifiers.AddRange(liveAircraftDataWithHistoricalStatuses.Where(x => !string.IsNullOrEmpty(x.AtcFlightNumber)).Select(x => x.AtcFlightNumber).ToList());

            ////Then load all SWIM flight legs that we have from the last hour.
            //List<SWIMFlightLeg> existingFlightLegs = (await _FlightLegEntityService.GetListBySpec(
            //    new SWIMFlightLegByAircraftSpecification(aircraftIdentifiers, DateTime.UtcNow.AddHours(-1))));

            //For each live antenna record we will determine the current flight status
            foreach (var liveAircraftData in flightWatchData)
            {
                await SetFlightLegStatus(liveAircraftData);
            }

            return flightWatchData;
        }

        private async Task SetFlightLegStatus(FlightWatchModel flightWatchModel)
        {
            ////Prepare our airport position and SWIM flight leg association for the live data
            //liveAircraftData.AirportPosition =
            //    await _AirportService.GetNearestAirportPosition(liveAircraftData.Latitude,
            //        liveAircraftData.Longitude);
            
            //Finally, determine the proper FlightLegStatus of each result based on all of the data we have
            var mostRecentHistoricalRecord = flightWatchModel.GetAirportWatchHistoricalDataCollection()?.FirstOrDefault();
            FlightLegStatus flightLegStatus = FlightLegStatus.EnRoute;

            if (IsTaxiingForDeparture(flightWatchModel, mostRecentHistoricalRecord))
                flightLegStatus = FlightLegStatus.TaxiingOrigin;

            else if (IsTaxiingToDestination(flightWatchModel, mostRecentHistoricalRecord))
                flightLegStatus = FlightLegStatus.TaxiingDestination;

            else if (IsLandingAtDestination(flightWatchModel, mostRecentHistoricalRecord))
                flightLegStatus = FlightLegStatus.Landing;

            else if (IsDepartingFromOrigin(flightWatchModel, mostRecentHistoricalRecord))
                flightLegStatus = FlightLegStatus.Departing;

            else if (HasArrivedAtDestination(flightWatchModel, mostRecentHistoricalRecord))
                flightLegStatus = FlightLegStatus.Arrived;

            flightWatchModel.Status = flightLegStatus;
            
        }

        private bool IsTaxiingForDeparture(FlightWatchModel flightWatchModel, AirportWatchHistoricalDataDto mostRecentHistoricalRecord)
        {
            if (!(flightWatchModel.IsAircraftOnGround).GetValueOrDefault())
                return false;

            //No historical record exists, it's a new flight we haven't seen so must be prepping to depart
            if (mostRecentHistoricalRecord == null)
                return true;

            //If we have seen this aircraft then confirm we last saw it parked
            if (mostRecentHistoricalRecord.AircraftStatus != AircraftStatusType.Parking)
                return false;

            //If we have SWIM data for the upcoming flight leg and the departure airport doesn't match the one we are at then this is NOT taxi'ing for departure
            if (flightWatchModel.ATDZulu.HasValue && flightWatchModel.ATDZulu.GetValueOrDefault() > DateTime.UtcNow &&
                flightWatchModel.DepartureICAO !=
                flightWatchModel.GetAirportPosition()?.GetProperAirportIdentifier())
                return false;

            //Ensure the parking occurrence was registered within the past 10 minutes so it's still possibly moving
            if (!flightWatchModel.AircraftPositionDateTimeUtc.HasValue || Math.Abs((flightWatchModel.AircraftPositionDateTimeUtc.GetValueOrDefault() -
                                                                       mostRecentHistoricalRecord.AircraftPositionDateTimeUtc).TotalMinutes) < 10)
                return false;

            //Ensure the aircraft has moved from it's parking position
            if (Math.Abs(flightWatchModel.Latitude.GetValueOrDefault() - mostRecentHistoricalRecord.Latitude) < 0.00001
                && Math.Abs(flightWatchModel.Longitude.GetValueOrDefault() - mostRecentHistoricalRecord.Longitude) < 0.00001)
                return false;

            return true;
        }

        private bool IsTaxiingToDestination(FlightWatchModel flightWatchModel,
            AirportWatchHistoricalDataDto mostRecentHistoricalRecord)
        {
            if (!flightWatchModel.IsAircraftOnGround.GetValueOrDefault())
                return false;

            //We haven't seen this flight yet.  It can't be taxi'ing into the arrival.
            if (mostRecentHistoricalRecord == null)
                return false;

            //Last known status was landing and the aircraft hasn't parked yet - this must be taxi'ing to the arrival destination.
            if (mostRecentHistoricalRecord.AircraftStatus == AircraftStatusType.Landing)
                return true;

            //Last known status was parking and it's been less than 10 minutes - if it's still moving then consider this taxi'ing to destination.
            if (mostRecentHistoricalRecord.AircraftStatus == AircraftStatusType.Parking 
                && flightWatchModel.AircraftPositionDateTimeUtc.HasValue
                && Math.Abs(
                    (flightWatchModel.AircraftPositionDateTimeUtc.Value -
                     mostRecentHistoricalRecord.AircraftPositionDateTimeUtc).TotalMinutes) < 10)
                return true;
            return false;
        }

        private bool IsLandingAtDestination(FlightWatchModel flightWatchModel,
            AirportWatchHistoricalDataDto mostRecentHistoricalRecord)
        {
            if (flightWatchModel.IsAircraftOnGround.GetValueOrDefault())
                return false;

            //Require the SWIM flight leg to confirm we know the actual destination of the aircraft.  Require the airport closest to the aircraft as well.
            if (flightWatchModel.SWIMFlightLegId.GetValueOrDefault() == 0 || flightWatchModel.GetAirportPosition() == null)
                return false;

            //If the arrival airport of the leg is not our nearest airport then we can't assume we are within landing range yet
            if (flightWatchModel.ArrivalICAO !=
                flightWatchModel.GetAirportPosition().GetProperAirportIdentifier())
                return false;

            var distanceFromAirport =
                flightWatchModel.GetAirportPosition().GetDistanceInMilesFromAirportPosition(flightWatchModel.Latitude.GetValueOrDefault(),
                    flightWatchModel.Longitude.GetValueOrDefault());

            //We are considered "landing" if we are within 5 miles of the airport.  We've changed this because the ETA from the SWIM flight leg is a bit lagged in determining the actual arrival time.
            return (distanceFromAirport <= 5);
        }

        private bool IsDepartingFromOrigin(FlightWatchModel flightWatchModel,
            AirportWatchHistoricalDataDto mostRecentHistoricalRecord)
        {
            if (flightWatchModel.IsAircraftOnGround.GetValueOrDefault())
                return false;

            //Ensure we have seen this aircraft actually depart the airport recently.
            if (mostRecentHistoricalRecord == null ||
                mostRecentHistoricalRecord.AircraftStatus != AircraftStatusType.Takeoff)
                return false;

            //The takeoff must have occurred in the last 5 minutes to be considered "departing" before turning into an "enroute" status.
            if (flightWatchModel.AircraftPositionDateTimeUtc.HasValue && Math.Abs((flightWatchModel.AircraftPositionDateTimeUtc.GetValueOrDefault() -
                                            mostRecentHistoricalRecord.AircraftPositionDateTimeUtc).TotalMinutes) <= 5)
                return true;
            
            return false;
        }

        private bool HasArrivedAtDestination(FlightWatchModel flightWatchModel,
            AirportWatchHistoricalDataDto mostRecentHistoricalRecord)
        {
            if (!flightWatchModel.IsAircraftOnGround.GetValueOrDefault())
                return false;

            //If we don't have a live look at the aircraft and SWIM data claims it has already arrived then trust it.
            if (flightWatchModel.AirportWatchLiveDataId <= 0 && flightWatchModel.ETAZulu.HasValue &&
                flightWatchModel.ETAZulu.Value < DateTime.UtcNow)
                return true;

            //We haven't seen this flight yet.  It can't be taxi'ing into the arrival.
            if (mostRecentHistoricalRecord == null)
                return false;

            //Last known status must be "parking".
            if (mostRecentHistoricalRecord.AircraftStatus != AircraftStatusType.Parking)
                return false;

            //Require a SWIM and AirportPosition record to ensure accuracy.
            if (flightWatchModel.SWIMFlightLegId.GetValueOrDefault() <= 0 || flightWatchModel.GetAirportPosition() == null)
                return false;

            //Confirm we have arrived at the airport that SWIM data claims we should be at.
            if (flightWatchModel.GetAirportPosition()?.GetProperAirportIdentifier() !=
                flightWatchModel.ArrivalICAO)
                return false;

            //It's been over 10 minutes since our last parking update.
            if (flightWatchModel.AircraftPositionDateTimeUtc.HasValue && Math.Abs(
                    (flightWatchModel.AircraftPositionDateTimeUtc.GetValueOrDefault() -
                     mostRecentHistoricalRecord.AircraftPositionDateTimeUtc).TotalMinutes) >= 10)
                return true;
            return false;
        }
    }
}
