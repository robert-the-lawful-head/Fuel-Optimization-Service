using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.Core.Utilities.DatesAndTimes;
using FBOLinx.Core.Utilities.Geography;
using FBOLinx.DB;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.AcukwikAirport;
using FBOLinx.DB.Specifications.Aircraft;
using FBOLinx.DB.Specifications.AircraftHexTailMapping;
using FBOLinx.DB.Specifications.SWIM;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.Logging;
using Mapster;

namespace FBOLinx.ServiceLayer.BusinessServices.SWIM
{
    public class SWIMService : ISWIMService
    {
        private const int MessageThresholdSec = 30;
        private const int FlightLegsFetchingThresholdMins = -30;

        private readonly SWIMFlightLegEntityService _FlightLegEntityService;
        private readonly SWIMFlightLegDataEntityService _FlightLegDataEntityService;
        private readonly AirportWatchLiveDataEntityService _AirportWatchLiveDataEntityService;
        private readonly AircraftHexTailMappingEntityService _AircraftHexTailMappingEntityService;
        private readonly AirportWatchHistoricalDataEntityService _AirportWatchHistoricalDataEntityService;
        private readonly AcukwikAirportEntityService _AcukwikAirportEntityService;
        private readonly ICustomerAircraftEntityService _CustomerAircraftEntityService;
        private readonly AircraftEntityService _AircraftEntityService;
        private readonly ILoggingService _LoggingService;

        public SWIMService(SWIMFlightLegEntityService flightLegEntityService, SWIMFlightLegDataEntityService flightLegDataEntityService,
            AirportWatchLiveDataEntityService airportWatchLiveDataEntityService, AircraftHexTailMappingEntityService aircraftHexTailMappingEntityService,
            AirportWatchHistoricalDataEntityService airportWatchHistoricalDataEntityService, AcukwikAirportEntityService acukwikAirportEntityService,
            ICustomerAircraftEntityService customerAircraftEntityService, AircraftEntityService aircraftEntityService, ILoggingService loggingService)
        {
            _FlightLegEntityService = flightLegEntityService;
            _FlightLegDataEntityService = flightLegDataEntityService;
            _AirportWatchLiveDataEntityService = airportWatchLiveDataEntityService;
            _AircraftHexTailMappingEntityService = aircraftHexTailMappingEntityService;
            _AirportWatchHistoricalDataEntityService = airportWatchHistoricalDataEntityService;
            _AcukwikAirportEntityService = acukwikAirportEntityService;
            _CustomerAircraftEntityService = customerAircraftEntityService;
            _AircraftEntityService = aircraftEntityService;
            _LoggingService = loggingService;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetDepartures(string icao)
        {
            IEnumerable<SWIMFlightLeg> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(icao, null, DateTime.UtcNow.AddMinutes(FlightLegsFetchingThresholdMins)));
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs, false);

            return result;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetArrivals(string icao)
        {
            IEnumerable<SWIMFlightLeg> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(null, icao, DateTime.UtcNow.AddMinutes(FlightLegsFetchingThresholdMins)));
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs, true);

            return result;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetHistoricalFlightLegs(string icao, bool isArrivals, DateTime historicalETD, DateTime historicalETA, DateTime historicalAircraftPositionDateTime)
        {
            IEnumerable<SWIMFlightLeg> swimFlightLegs;
            if (isArrivals)
            {
                swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(null, icao, DateTime.UtcNow.AddMinutes(FlightLegsFetchingThresholdMins)));
            }
            else
            {
                swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(icao, null, DateTime.UtcNow.AddMinutes(FlightLegsFetchingThresholdMins)));
            }
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs, isArrivals, true, historicalETD, historicalETA, historicalAircraftPositionDateTime);

            return result;
        }

        public async Task SaveFlightLegData(IEnumerable<SWIMFlightLegDTO> flightLegs)
        {
            if (flightLegs == null || !flightLegs.Any())
            {
                return;
            }

            DateTime atdMin = flightLegs.Min(x => x.ATD);
            DateTime atdMax = flightLegs.Max(x => x.ATD);
            IList<string> departureICAOs = flightLegs.Select(x => x.DepartureICAO).Distinct().ToList();
            IList<string> arrivalICAOs = flightLegs.Select(x => x.ArrivalICAO).Distinct().ToList();
            

            List<string> flightNumbers = flightLegs.Where(x => !x.AircraftIdentification.ToUpperInvariant().StartsWith('N')).Select(x => x.AircraftIdentification).ToList();
            List<AirportWatchLiveData> antennaLiveData = await _AirportWatchLiveDataEntityService.GetListBySpec(
                new AirportWatchLiveDataByFlightNumberSpecification(flightNumbers, DateTime.UtcNow.AddHours(-1)));
            List<AirportWatchHistoricalData> antennaHistoricalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(
                new AirportWatchHistoricalDataSpecification(flightNumbers, DateTime.UtcNow.AddDays(-1)));

            List<SWIMFlightLeg> existingFlightLegs = (await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(departureICAOs, arrivalICAOs, atdMin, atdMax))).OrderByDescending(x => x.ATD).ToList();
            var existingFlightLegMessages = await _FlightLegDataEntityService.GetListBySpec(new SWIMFlightLegDataSpecification(existingFlightLegs.Select(x => x.Oid).ToList()));

            List<SWIMFlightLegData> flightLegDataMessagesToInsert = new List<SWIMFlightLegData>();
            List<SWIMFlightLeg> flightLegsToUpdate = new List<SWIMFlightLeg>();
            List<SWIMFlightLeg> flightLegsToAdd = new List<SWIMFlightLeg>();
            
            foreach (SWIMFlightLegDTO swimFlightLegDto in flightLegs)
            {
                var existingLeg = existingFlightLegs.FirstOrDefault(
                    x => x.DepartureICAO == swimFlightLegDto.DepartureICAO && x.ArrivalICAO == swimFlightLegDto.ArrivalICAO && x.ATD == swimFlightLegDto.ATD);
                if (existingLeg != null)
                {
                    var exisingLegMessages = existingFlightLegMessages.Where(x => x.SWIMFlightLegId == existingLeg.Oid).ToList();
                    DateTime latestMessageTimestamp = exisingLegMessages.Max(x => x.MessageTimestamp);
                    DateTime latestExistingETA = exisingLegMessages.OrderByDescending(x => x.Oid).First().ETA;
                    foreach (SWIMFlightLegDataDTO flightLegDataMessageDto in swimFlightLegDto.SWIMFlightLegDataMessages)
                    {
                        double messageTimestampThreshold = Math.Abs((flightLegDataMessageDto.MessageTimestamp - latestMessageTimestamp).TotalSeconds);
                        if (messageTimestampThreshold > MessageThresholdSec &&
                            (flightLegDataMessageDto.ActualSpeed != null ||
                             flightLegDataMessageDto.Altitude != null || flightLegDataMessageDto.Latitude != null ||
                             flightLegDataMessageDto.Longitude != null || flightLegDataMessageDto.ETA != latestExistingETA))
                        {
                            flightLegDataMessageDto.SWIMFlightLegId = existingLeg.Oid;
                            flightLegDataMessagesToInsert.Add(flightLegDataMessageDto.Adapt<SWIMFlightLegData>());
                        }
                    }

                    if (latestExistingETA != swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA)
                    {
                        existingLeg.ETA = swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA;
                        flightLegsToUpdate.Add(existingLeg);
                    }
                }
                else
                {
                    await SetTailNumber(swimFlightLegDto, antennaLiveData, antennaHistoricalData);
                    swimFlightLegDto.ETA = swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA;
                    flightLegsToAdd.Add(swimFlightLegDto.Adapt<SWIMFlightLeg>());
                }
            }

            if (flightLegsToAdd.Count > 0)
            {
                LogMissedTailNumbers(flightLegsToAdd);

                await _FlightLegEntityService.BulkInsertOrUpdate(flightLegsToAdd);

                foreach (SWIMFlightLeg flightLeg in flightLegsToAdd)
                {
                    foreach (SWIMFlightLegData legDataMessage in flightLeg.SWIMFlightLegDataMessages)
                    {
                        legDataMessage.SWIMFlightLegId = flightLeg.Oid;
                    }

                    flightLegDataMessagesToInsert.AddRange(flightLeg.SWIMFlightLegDataMessages.Select(x => x.Adapt<SWIMFlightLegData>()));
                }
            }

            if (flightLegDataMessagesToInsert.Count > 0)
            {
                await _FlightLegDataEntityService.BulkInsertOrUpdate(flightLegDataMessagesToInsert);
            }

            if (flightLegsToUpdate.Count > 0)
            {
                await _FlightLegEntityService.BulkInsertOrUpdate(flightLegsToUpdate);
            }
        }

        private async Task SetTailNumber(SWIMFlightLegDTO swimFlightLegDto, List<AirportWatchLiveData> antennaLiveData, List<AirportWatchHistoricalData> antennaHistoricalData)
        {
            if (IsCorrectTailNumber(swimFlightLegDto.AircraftIdentification))
            {
                return;
            }

            AirportWatchLiveData antennaLiveDataRecord = antennaLiveData.OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
            if (antennaLiveDataRecord != null)
            {
                if (!string.IsNullOrEmpty(antennaLiveDataRecord.TailNumber))
                {
                    swimFlightLegDto.AircraftIdentification = antennaLiveDataRecord.TailNumber;
                }
                else if (!string.IsNullOrEmpty(antennaLiveDataRecord.AircraftHexCode))
                {
                    List<AircraftHexTailMapping> hexTailMappings = await _AircraftHexTailMappingEntityService.GetListBySpec(new AircraftHexTailMappingSpecification(antennaLiveDataRecord.AircraftHexCode));
                    if (hexTailMappings != null && hexTailMappings.Any())
                    {
                        swimFlightLegDto.AircraftIdentification = hexTailMappings.First().TailNumber;
                    }
                }
            }
            else
            {
                AirportWatchHistoricalData antennaHistoricalDataRecord = antennaHistoricalData.OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
                if (antennaHistoricalDataRecord != null)
                {
                    if (!string.IsNullOrEmpty(antennaHistoricalDataRecord.TailNumber))
                    {
                        swimFlightLegDto.AircraftIdentification = antennaHistoricalDataRecord.TailNumber;
                    }
                    else if (!string.IsNullOrEmpty(antennaHistoricalDataRecord.AircraftHexCode))
                    {
                        List<AircraftHexTailMapping> hexTailMappings = await _AircraftHexTailMappingEntityService.GetListBySpec(new AircraftHexTailMappingSpecification(antennaHistoricalDataRecord.AircraftHexCode));
                        if (hexTailMappings != null && hexTailMappings.Any())
                        {
                            swimFlightLegDto.AircraftIdentification = hexTailMappings.First().TailNumber;
                        }
                    }
                }
            }
        }

        private async Task<IEnumerable<FlightLegDTO>> GetFlightLegs(IEnumerable<SWIMFlightLeg> swimFlightLegs, bool isArrivals, bool useHistoricalData = false, DateTime? historicalETD = null, DateTime? historicalETA = null, DateTime? historicalAircraftPositionDateTime = null)
        {
            List<SWIMFlightLegData> swimFlightLegMessages = await _FlightLegDataEntityService.GetListBySpec(new SWIMFlightLegDataSpecification(swimFlightLegs.Select(x => x.Oid).ToList()));

            List<string> airportICAOs = swimFlightLegs.Select(x => x.DepartureICAO).ToList();
            airportICAOs.AddRange(swimFlightLegs.Select(x => x.ArrivalICAO).ToList());
            airportICAOs = airportICAOs.Distinct().ToList();
            List<AcukwikAirport> airports = await _AcukwikAirportEntityService.GetListBySpec(new AcukwikAirportSpecification(airportICAOs));
            List<string> tailNumbers = swimFlightLegs.Select(x => x.AircraftIdentification).ToList();
            var flightDepartmentsByTailNumbers = await _CustomerAircraftEntityService.GetAircraftsByFlightDepartments(tailNumbers);
            var pricingTemplates = await _CustomerAircraftEntityService.GetPricingTemplates(tailNumbers);
            var aircrafts = await _AircraftEntityService.GetListBySpec(new AircraftSpecification(flightDepartmentsByTailNumbers.Select(x => x.Item1).Distinct().ToList()));
            List<AirportWatchLiveData> antennaLiveData = await _AirportWatchLiveDataEntityService.GetListBySpec(new AirportWatchLiveDataByFlightNumberSpecification(tailNumbers, DateTime.UtcNow.AddHours(-1)));
            List<AirportWatchHistoricalData> antennaHistoricalData;
            if (useHistoricalData)
            {
                antennaHistoricalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(new AirportWatchHistoricalDataSpecification(tailNumbers, historicalETD.Value, historicalETA.Value));
            }
            else
            {
                antennaHistoricalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(new AirportWatchHistoricalDataSpecification(tailNumbers, DateTime.UtcNow.AddDays(-1)));
            }
            IList<FlightLegDTO> result = new List<FlightLegDTO>();
            foreach (SWIMFlightLeg swimFlightLeg in swimFlightLegs)
            {
                FlightLegDTO flightLegDto = new FlightLegDTO();
                flightLegDto.Id = swimFlightLeg.Oid;
                flightLegDto.TailNumber = swimFlightLeg.AircraftIdentification;
                flightLegDto.DepartureICAO = swimFlightLeg.DepartureICAO;
                flightLegDto.ArrivalICAO = swimFlightLeg.ArrivalICAO;

                bool isAircraftOnGround = false;
                if (useHistoricalData)
                {
                    AirportWatchHistoricalData airportWatchHistoricalData = antennaHistoricalData.Where(x => x.AtcFlightNumber == swimFlightLeg.AircraftIdentification && 
                        x.AircraftPositionDateTimeUtc >= historicalAircraftPositionDateTime.Value.AddMinutes(-5) && x.AircraftPositionDateTimeUtc <= historicalAircraftPositionDateTime.Value.AddMinutes(5)).FirstOrDefault();
                    if (airportWatchHistoricalData != null)
                    {
                        isAircraftOnGround = airportWatchHistoricalData.IsAircraftOnGround;
                    }
                }
                else
                {
                    AirportWatchLiveData latestAntennaLiveDataRecord = antennaLiveData.Where(x => x.AtcFlightNumber == swimFlightLeg.AircraftIdentification).OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
                    if (latestAntennaLiveDataRecord != null)
                    {
                        isAircraftOnGround = latestAntennaLiveDataRecord.IsAircraftOnGround;
                    }
                }
                flightLegDto.IsAircraftOnGround = isAircraftOnGround;

                var aircraftByFlightDepartment = flightDepartmentsByTailNumbers.FirstOrDefault(x => x.Item2 == swimFlightLeg.AircraftIdentification);
                if (aircraftByFlightDepartment != null)
                {
                    flightLegDto.FlightDepartment = aircraftByFlightDepartment.Item3;
                    var aircraft = aircrafts.FirstOrDefault(x => x.AircraftId == aircraftByFlightDepartment.Item1);
                    if (aircraft != null)
                    {
                        flightLegDto.Make = aircraft.Make;
                        flightLegDto.Model = aircraft.Model;
                        flightLegDto.FuelCapacityGal = aircraft.FuelCapacityGal;
                    }
                }

                flightLegDto.ITPMarginTemplate = "Company Pricing";
                var pricingTemplate = pricingTemplates.FirstOrDefault(x => x.Item2 == swimFlightLeg.AircraftIdentification);
                if (pricingTemplate != null && !string.IsNullOrWhiteSpace(pricingTemplate.Item3))
                {
                    flightLegDto.ITPMarginTemplate = pricingTemplate.Item3;
                }

                flightLegDto.ATDZulu = swimFlightLeg.ATD;
                AcukwikAirport departureAirport = airports.FirstOrDefault(x => x.Icao == flightLegDto.DepartureICAO);
                if (departureAirport != null)
                {
                    flightLegDto.ATDLocal = DateTimeHelper.GetLocalTime(swimFlightLeg.ATD, departureAirport.IntlTimeZone, departureAirport.DaylightSavingsYn?.ToLower() == "y");
                    flightLegDto.DepartureCity = departureAirport.AirportCity;
                }
                else
                {
                    flightLegDto.ATDLocal = swimFlightLeg.ATD;
                }

                flightLegDto.ETAZulu = swimFlightLeg.ETA;
                AcukwikAirport arrivalAirport = airports.FirstOrDefault(x => x.Icao == flightLegDto.ArrivalICAO);
                if (arrivalAirport != null)
                {
                    flightLegDto.ETALocal = DateTimeHelper.GetLocalTime(swimFlightLeg.ETA, arrivalAirport.IntlTimeZone, arrivalAirport.DaylightSavingsYn?.ToLower() == "y");
                    flightLegDto.ArrivalCity = arrivalAirport.AirportCity;
                }
                else
                {
                    flightLegDto.ETALocal = swimFlightLeg.ETA;
                }
                
                flightLegDto.ETE = flightLegDto.ETAZulu - DateTime.UtcNow;

                if (isArrivals)
                {
                    flightLegDto.Origin = flightLegDto.DepartureICAO;
                    flightLegDto.City = flightLegDto.DepartureCity;
                }
                else
                {
                    flightLegDto.Origin = flightLegDto.ArrivalICAO;
                    flightLegDto.City = flightLegDto.ArrivalCity;
                }

                flightLegDto.Status = GetFlightStatus(swimFlightLeg, swimFlightLegMessages, arrivalAirport, departureAirport, isAircraftOnGround, antennaHistoricalData);

                SWIMFlightLegData latestSWIMMessage =
                    swimFlightLegMessages.Where(x => x.SWIMFlightLegId == swimFlightLeg.Oid).OrderByDescending(x => x.Oid).First();
                flightLegDto.ActualSpeed = latestSWIMMessage.ActualSpeed;
                flightLegDto.Altitude = latestSWIMMessage.Altitude;
                flightLegDto.Latitude = latestSWIMMessage.Latitude;
                flightLegDto.Longitude = latestSWIMMessage.Longitude;

                result.Add(flightLegDto);
            }

            return result;
        }

        private bool IsCorrectTailNumber(string aircraftIdentification)
        {
            if (string.IsNullOrEmpty(aircraftIdentification))
            {
                return false;
            }

            return aircraftIdentification.ToUpperInvariant().StartsWith('N');
        }

        private void LogMissedTailNumbers(IEnumerable<SWIMFlightLeg> flightLegs)
        {
            var missedTailNumbers = flightLegs.Where(x => !IsCorrectTailNumber(x.AircraftIdentification)).Select(x => x.AircraftIdentification).ToList();
            if (missedTailNumbers.Any())
            {
                _LoggingService.LogError("Missed Tail Numbers", string.Join(",", missedTailNumbers), LogLevel.Warning);
            }
        }

        private FlightLegStatus GetFlightStatus(SWIMFlightLeg swimFlightLeg, List<SWIMFlightLegData> swimFlightLegMessages, AcukwikAirport arrivalAirport, AcukwikAirport departureAirport, 
            bool isAircraftOnGround, List<AirportWatchHistoricalData> antennaHistoricalData)
        {
            FlightLegStatus flightLegStatus = FlightLegStatus.EnRoute;

            SWIMFlightLegData latestSWIMFlightLegMessage =
                swimFlightLegMessages.Where(x => x.Latitude != null && x.Longitude != null).OrderByDescending(x => x.MessageTimestamp).FirstOrDefault();

            if (arrivalAirport == null || string.IsNullOrEmpty(arrivalAirport.Latitude) || string.IsNullOrEmpty(arrivalAirport.Longitude) || 
                departureAirport == null || string.IsNullOrEmpty(departureAirport.Latitude) || string.IsNullOrEmpty(departureAirport.Longitude) ||
                latestSWIMFlightLegMessage == null)
            {
                return flightLegStatus;
            }

            double arrivalAirportLatitude = LocationHelper.GetLatitudeGeoLocationFromGPS(arrivalAirport.Latitude);
            double arrivalAirportLongitude = LocationHelper.GetLongitudeGeoLocationFromGPS(arrivalAirport.Longitude);
            var distanceFromArrivalAirport = new Coordinates(arrivalAirportLatitude, arrivalAirportLongitude)
                .DistanceTo(
                    new Coordinates(latestSWIMFlightLegMessage.Latitude.Value, latestSWIMFlightLegMessage.Longitude.Value),
                    UnitOfLength.Miles
                );
            double departureAirportLatitude = LocationHelper.GetLatitudeGeoLocationFromGPS(departureAirport.Latitude);
            double departureAirportLongitude = LocationHelper.GetLongitudeGeoLocationFromGPS(departureAirport.Longitude);
            var distanceFromDepartureAirport = new Coordinates(departureAirportLatitude, departureAirportLongitude)
                .DistanceTo(
                    new Coordinates(latestSWIMFlightLegMessage.Latitude.Value, latestSWIMFlightLegMessage.Longitude.Value),
                    UnitOfLength.Miles
                );
            
            if (!isAircraftOnGround)
            {
                if (distanceFromArrivalAirport < 5)
                {
                    flightLegStatus = FlightLegStatus.Landing;
                }
                else if (distanceFromDepartureAirport < 5)
                {
                    flightLegStatus = FlightLegStatus.Departing;
                }
                else
                {
                    flightLegStatus = FlightLegStatus.EnRoute;
                }
            }
            else
            {
                AirportWatchHistoricalData antennaHistoricalDataRecord = antennaHistoricalData.Where(x => x.AtcFlightNumber == swimFlightLeg.AircraftIdentification).OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
                
                if (antennaHistoricalDataRecord != null && antennaHistoricalDataRecord.AircraftStatus == AircraftStatusType.Parking)
                {
                    if (antennaHistoricalDataRecord.AircraftStatus == AircraftStatusType.Parking)
                    {
                        flightLegStatus = FlightLegStatus.Arrived;
                    }
                    else if (antennaHistoricalDataRecord.AircraftStatus == AircraftStatusType.Landing && antennaHistoricalDataRecord.AircraftPositionDateTimeUtc > DateTime.UtcNow.AddMinutes(-30))
                    {
                        flightLegStatus = FlightLegStatus.Taxiing;
                    }

                }
            }

            return flightLegStatus;
        }
    }
}
