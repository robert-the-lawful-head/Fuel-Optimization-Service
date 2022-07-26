using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Utilities.DatesAndTimes;
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

        private readonly SWIMFlightLegEntityService _flightLegEntityService;
        private readonly SWIMFlightLegDataEntityService _flightLegDataEntityService;
        private readonly AirportWatchLiveDataEntityService _airportWatchLiveDataEntityService;
        private readonly AircraftHexTailMappingEntityService _AircraftHexTailMappingEntityService;
        private readonly AirportWatchHistoricalDataEntityService _airportWatchHistoricalDataEntityService;
        private readonly AcukwikAirportEntityService _AcukwikAirportEntityService;
        private readonly ICustomerAircraftEntityService _CustomerAircraftEntityService;
        private readonly AircraftEntityService _AircraftEntityService;
        private readonly ILoggingService _LoggingService;

        public SWIMService(SWIMFlightLegEntityService flightLegEntityService, SWIMFlightLegDataEntityService flightLegDataEntityService,
            AirportWatchLiveDataEntityService airportWatchLiveDataEntityService, AircraftHexTailMappingEntityService aircraftHexTailMappingEntityService,
            AirportWatchHistoricalDataEntityService airportWatchHistoricalDataEntityService, AcukwikAirportEntityService acukwikAirportEntityService,
            ICustomerAircraftEntityService customerAircraftEntityService, AircraftEntityService aircraftEntityService, ILoggingService loggingService)
        {
            _flightLegEntityService = flightLegEntityService;
            _flightLegDataEntityService = flightLegDataEntityService;
            _airportWatchLiveDataEntityService = airportWatchLiveDataEntityService;
            _AircraftHexTailMappingEntityService = aircraftHexTailMappingEntityService;
            _airportWatchHistoricalDataEntityService = airportWatchHistoricalDataEntityService;
            _AcukwikAirportEntityService = acukwikAirportEntityService;
            _CustomerAircraftEntityService = customerAircraftEntityService;
            _AircraftEntityService = aircraftEntityService;
            _LoggingService = loggingService;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetDepartures(string icao)
        {
            IEnumerable<SWIMFlightLeg> swimFlightLegs = await _flightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(icao, null, DateTime.UtcNow.AddMinutes(FlightLegsFetchingThresholdMins)));
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs, false);

            return result;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetArrivals(string icao)
        {
            IEnumerable<SWIMFlightLeg> swimFlightLegs = await _flightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(null, icao, DateTime.UtcNow.AddMinutes(FlightLegsFetchingThresholdMins)));
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs, true);

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
            List<FBOLinx.DB.Models.AirportWatchLiveData> antennaLiveData = await _airportWatchLiveDataEntityService.GetListBySpec(
                new AirportWatchLiveDataByFlightNumberSpecification(flightNumbers, DateTime.UtcNow.AddHours(-1)));
            List<AirportWatchHistoricalData> antennaHistoricalData = await _airportWatchHistoricalDataEntityService.GetListBySpec(
                new AirportWatchHistoricalDataSpecification(flightNumbers, DateTime.UtcNow.AddDays(-1)));

            List<SWIMFlightLeg> existingFlightLegs = (await _flightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(departureICAOs, arrivalICAOs, atdMin, atdMax))).OrderByDescending(x => x.ATD).ToList();
            var existingFlightLegMessages = await _flightLegDataEntityService.GetListBySpec(new SWIMFlightLegDataSpecification(existingFlightLegs.Select(x => x.Oid).ToList()));

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
                    if (!exisingLegMessages.Any())
                    {
                        continue;
                    }
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
                await _flightLegEntityService.BulkInsert(flightLegsToAdd, true);
            }

            if (flightLegDataMessagesToInsert.Count > 0)
            {
                await _flightLegDataEntityService.BulkInsert(flightLegDataMessagesToInsert);
            }

            if (flightLegsToUpdate.Count > 0)
            {
                await _flightLegEntityService.BulkUpdate(flightLegsToUpdate);
            }
        }

        private async Task SetTailNumber(SWIMFlightLegDTO swimFlightLegDto, List<FBOLinx.DB.Models.AirportWatchLiveData> antennaLiveData, List<AirportWatchHistoricalData> antennaHistoricalData)
        {
            if (IsCorrectTailNumber(swimFlightLegDto.AircraftIdentification))
            {
                return;
            }

            FBOLinx.DB.Models.AirportWatchLiveData antennaLiveDataRecord = antennaLiveData.OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
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

        private async Task<IEnumerable<FlightLegDTO>> GetFlightLegs(IEnumerable<SWIMFlightLeg> swimFlightLegs, bool isArrivals)
        {
            var swimFlightLegMessages = await _flightLegDataEntityService.GetListBySpec(new SWIMFlightLegDataSpecification(swimFlightLegs.Select(x => x.Oid).ToList()));

            List<string> airportICAOs = swimFlightLegs.Select(x => x.DepartureICAO).ToList();
            airportICAOs.AddRange(swimFlightLegs.Select(x => x.ArrivalICAO).ToList());
            airportICAOs = airportICAOs.Distinct().ToList();
            List<AcukwikAirport> airports = await _AcukwikAirportEntityService.GetListBySpec(new AcukwikAirportSpecification(airportICAOs));
            List<string> tailNumbers = swimFlightLegs.Select(x => x.AircraftIdentification).ToList();
            var flightDepartmentsByTailNumbers = await _CustomerAircraftEntityService.GetAircraftsByFlightDepartments(tailNumbers);
            var pricingTemplates = await _CustomerAircraftEntityService.GetPricingTemplates(tailNumbers);
            var aircrafts = await _AircraftEntityService.GetListBySpec(new AircraftSpecification(flightDepartmentsByTailNumbers.Select(x => x.Item1).Distinct().ToList()));
            List<FBOLinx.DB.Models.AirportWatchLiveData> antennaLiveData = await _airportWatchLiveDataEntityService.GetListBySpec(new AirportWatchLiveDataByFlightNumberSpecification(tailNumbers, DateTime.UtcNow.AddHours(-1)));
            IList<FlightLegDTO> result = new List<FlightLegDTO>();
            foreach (SWIMFlightLeg swimFlightLeg in swimFlightLegs)
            {
                FlightLegDTO dto = new FlightLegDTO();
                dto.Id = swimFlightLeg.Oid;
                dto.TailNumber = swimFlightLeg.AircraftIdentification;
                dto.DepartureICAO = swimFlightLeg.DepartureICAO;
                dto.ArrivalICAO = swimFlightLeg.ArrivalICAO;

                FBOLinx.DB.Models.AirportWatchLiveData antennaLiveDataRecord = antennaLiveData.Where(x => x.AtcFlightNumber == swimFlightLeg.AircraftIdentification).OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
                if (antennaLiveDataRecord != null)
                {
                    dto.IsAircraftOnGround = antennaLiveDataRecord.IsAircraftOnGround;
                }

                var aircraftByFlightDepartment = flightDepartmentsByTailNumbers.FirstOrDefault(x => x.Item2 == swimFlightLeg.AircraftIdentification);
                if (aircraftByFlightDepartment != null)
                {
                    dto.FlightDepartment = aircraftByFlightDepartment.Item3;
                    var aircraft = aircrafts.FirstOrDefault(x => x.AircraftId == aircraftByFlightDepartment.Item1);
                    if (aircraft != null)
                    {
                        dto.Make = aircraft.Make;
                        dto.Model = aircraft.Model;
                        dto.FuelCapacityGal = aircraft.FuelCapacityGal;
                    }
                }

                dto.ITPMarginTemplate = "Company Pricing";
                var pricingTemplate = pricingTemplates.FirstOrDefault(x => x.Item2 == swimFlightLeg.AircraftIdentification);
                if (pricingTemplate != null && !string.IsNullOrWhiteSpace(pricingTemplate.Item3))
                {
                    dto.ITPMarginTemplate = pricingTemplate.Item3;
                }

                dto.ATDZulu = swimFlightLeg.ATD;
                AcukwikAirport departureAirport = airports.FirstOrDefault(x => x.Icao == dto.DepartureICAO);
                if (departureAirport != null)
                {
                    dto.ATDLocal = DateTimeHelper.GetLocalTime(swimFlightLeg.ATD, departureAirport.IntlTimeZone, departureAirport.DaylightSavingsYn?.ToLower() == "y");
                    dto.DepartureCity = departureAirport.AirportCity;
                }
                else
                {
                    dto.ATDLocal = swimFlightLeg.ATD;
                }

                dto.ETAZulu = swimFlightLeg.ETA;
                AcukwikAirport arrivalAirport = airports.FirstOrDefault(x => x.Icao == dto.ArrivalICAO);
                if (arrivalAirport != null)
                {
                    dto.ETALocal = DateTimeHelper.GetLocalTime(swimFlightLeg.ETA, arrivalAirport.IntlTimeZone, arrivalAirport.DaylightSavingsYn?.ToLower() == "y");
                    dto.ArrivalCity = arrivalAirport.AirportCity;
                }
                else
                {
                    dto.ETALocal = swimFlightLeg.ETA;
                }

                dto.ETE = dto.ETAZulu - DateTime.UtcNow;

                if (isArrivals)
                {
                    dto.Origin = dto.DepartureICAO;
                    dto.City = dto.DepartureCity;
                }
                else
                {
                    dto.Origin = dto.ArrivalICAO;
                    dto.City = dto.ArrivalCity;
                }

                SWIMFlightLegData latestSWIMMessage =
                    swimFlightLegMessages.Where(x => x.SWIMFlightLegId == swimFlightLeg.Oid).OrderByDescending(x => x.Oid).First();
                dto.ActualSpeed = latestSWIMMessage.ActualSpeed;
                dto.Altitude = latestSWIMMessage.Altitude;
                dto.Latitude = latestSWIMMessage.Latitude;
                dto.Longitude = latestSWIMMessage.Longitude;

                result.Add(dto);
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
    }
}
