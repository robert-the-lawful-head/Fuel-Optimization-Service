using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Utilities.DatesAndTimes;
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

namespace FBOLinx.ServiceLayer.BusinessServices.SWIM
{
    public class SWIMService: ISWIMService
    {
        private const int MessageThresholdSec = 30;

        private readonly SWIMFlightLegEntityService _FlightLegEntityService;
        private readonly SWIMFlightLegDataEntityService _FlightLegDataEntityService;
        private readonly AirportWatchLiveDataEntityService _AirportWatchLiveDataEntityService;
        private readonly AircraftHexTailMappingEntityService _AircraftHexTailMappingEntityService;
        private readonly AirportWatchHistoricalDataEntityService _AirportWatchHistoricalDataEntityService;
        private readonly AcukwikAirportEntityService _AcukwikAirportEntityService;
        private readonly ICustomerAircraftEntityService _CustomerAircraftEntityService;
        private readonly AircraftEntityService _AircraftEntityService;

        public SWIMService(SWIMFlightLegEntityService flightLegEntityService, SWIMFlightLegDataEntityService flightLegDataEntityService, 
            AirportWatchLiveDataEntityService airportWatchLiveDataEntityService, AircraftHexTailMappingEntityService aircraftHexTailMappingEntityService, 
            AirportWatchHistoricalDataEntityService airportWatchHistoricalDataEntityService, AcukwikAirportEntityService acukwikAirportEntityService,
            ICustomerAircraftEntityService customerAircraftEntityService, AircraftEntityService aircraftEntityService)
        {
            _FlightLegEntityService = flightLegEntityService;
            _FlightLegDataEntityService = flightLegDataEntityService;
            _AirportWatchLiveDataEntityService = airportWatchLiveDataEntityService;
            _AircraftHexTailMappingEntityService = aircraftHexTailMappingEntityService;
            _AirportWatchHistoricalDataEntityService = airportWatchHistoricalDataEntityService;
            _AcukwikAirportEntityService = acukwikAirportEntityService;
            _CustomerAircraftEntityService = customerAircraftEntityService;
            _AircraftEntityService = aircraftEntityService;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetDepartures(string icao)
        {
            IEnumerable<SWIMFlightLegDTO> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(icao, null, DateTime.UtcNow));
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs, false);

            return result;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetArrivals(string icao)
        {
            IEnumerable<SWIMFlightLegDTO> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(null, icao, DateTime.UtcNow));
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
            List<SWIMFlightLegDTO> existingFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(departureICAOs, arrivalICAOs, atdMin, atdMax));
            
            List<SWIMFlightLegDataDTO> flightLegDataMessagesToInsert = new List<SWIMFlightLegDataDTO>();
            List<SWIMFlightLegDTO> flightLegsToUpdate = new List<SWIMFlightLegDTO>();
            
            foreach (SWIMFlightLegDTO swimFlightLegDto in flightLegs)
            {
                var existingFlightLeg = existingFlightLegs.SingleOrDefault(
                    x => x.DepartureICAO == swimFlightLegDto.DepartureICAO && x.ArrivalICAO == swimFlightLegDto.ArrivalICAO && x.ATD == swimFlightLegDto.ATD);
                if (existingFlightLeg != null)
                {
                    DateTime latestMessageTimestamp = existingFlightLeg.SWIMFlightLegDataMessages.Max(x => x.MessageTimestamp);
                    DateTime latestExistingETA = existingFlightLeg.SWIMFlightLegDataMessages.OrderByDescending(x => x.Oid).First().ETA;
                    foreach (SWIMFlightLegDataDTO flightLegDataMessageDto in swimFlightLegDto.SWIMFlightLegDataMessages)
                    {
                        double messageTimestampThreshold = Math.Abs((flightLegDataMessageDto.MessageTimestamp - latestMessageTimestamp).TotalSeconds);
                        if (messageTimestampThreshold > MessageThresholdSec && 
                            (flightLegDataMessageDto.ActualSpeed != null ||
                             flightLegDataMessageDto.Altitude != null || flightLegDataMessageDto.Latitude != null ||
                             flightLegDataMessageDto.Longitude != null || flightLegDataMessageDto.ETA != latestExistingETA))
                        {
                            flightLegDataMessageDto.SWIMFlightLegId = existingFlightLeg.Oid;
                            flightLegDataMessagesToInsert.Add(flightLegDataMessageDto);
                        }
                    }

                    if (latestExistingETA != swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA)
                    {
                        existingFlightLeg.ETA = swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA;
                        flightLegsToUpdate.Add(existingFlightLeg);
                    }
                }
                else
                {
                    await SetTailNumber(swimFlightLegDto);
                    swimFlightLegDto.ETA = swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA;
                    await _FlightLegEntityService.Add(swimFlightLegDto);
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

        private async Task SetTailNumber(SWIMFlightLegDTO swimFlightLegDto)
        {
            if (swimFlightLegDto.AircraftIdentification.ToUpperInvariant().StartsWith('N'))
            {
                return;
            }

            List<AirportWatchLiveDataDto> antennaLiveData = await _AirportWatchLiveDataEntityService.GetListBySpec(new AirportWatchLiveDataSpecification(swimFlightLegDto.AircraftIdentification, DateTime.UtcNow.AddHours(-1)));
            AirportWatchLiveDataDto antennaLiveDataRecord = antennaLiveData.OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
            if (antennaLiveDataRecord != null)
            {
                if (!string.IsNullOrEmpty(antennaLiveDataRecord.TailNumber))
                {
                    swimFlightLegDto.AircraftIdentification = antennaLiveDataRecord.TailNumber;
                }
                else if (!string.IsNullOrEmpty(antennaLiveDataRecord.AircraftHexCode))
                {
                    List<AircraftHexTailMappingDTO> hexTailMappings = await _AircraftHexTailMappingEntityService.GetListBySpec(new AircraftHexTailMappingSpecification(antennaLiveDataRecord.AircraftHexCode));
                    if (hexTailMappings != null && hexTailMappings.Any())
                    {
                        swimFlightLegDto.AircraftIdentification = hexTailMappings.First().TailNumber;
                    }
                }
            }
            else
            {
                List<AirportWatchHistoricalDataDto> antennaHistoricalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(new AirportWatchHistoricalDataSpecification(swimFlightLegDto.AircraftIdentification, DateTime.UtcNow.AddDays(-1)));
                AirportWatchHistoricalDataDto antennaHistoricalDataRecord = antennaHistoricalData.OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
                if (antennaHistoricalDataRecord != null)
                {
                    if (!string.IsNullOrEmpty(antennaHistoricalDataRecord.TailNumber))
                    {
                        swimFlightLegDto.AircraftIdentification = antennaHistoricalDataRecord.TailNumber;
                    }
                    else if (!string.IsNullOrEmpty(antennaHistoricalDataRecord.AircraftHexCode))
                    {
                        List<AircraftHexTailMappingDTO> hexTailMappings = await _AircraftHexTailMappingEntityService.GetListBySpec(new AircraftHexTailMappingSpecification(antennaHistoricalDataRecord.AircraftHexCode));
                        if (hexTailMappings != null && hexTailMappings.Any())
                        {
                            swimFlightLegDto.AircraftIdentification = hexTailMappings.First().TailNumber;
                        }
                    }
                }
            }
        }

        private async Task<IEnumerable<FlightLegDTO>> GetFlightLegs(IEnumerable<SWIMFlightLegDTO> swimFlightLegs, bool isArrivals)
        {
            List<string> airportICAOs = swimFlightLegs.Select(x => x.DepartureICAO).ToList();
            airportICAOs.AddRange(swimFlightLegs.Select(x => x.ArrivalICAO).ToList());
            airportICAOs = airportICAOs.Distinct().ToList();
            List<AcukwikAirportDTO> airports = await _AcukwikAirportEntityService.GetListBySpec(new AcukwikAirportSpecification(airportICAOs));
            var flightDepartmentsByTailNumbers = 
                await _CustomerAircraftEntityService.GetAircraftsByFlightDepartments(swimFlightLegs.Select(x => x.AircraftIdentification).ToList());
            var aircrafts = await _AircraftEntityService.GetListBySpec(new AircraftSpecification(flightDepartmentsByTailNumbers.Select(x => x.Item1).Distinct().ToList()));
            IList<FlightLegDTO> result = new List<FlightLegDTO>();
            foreach (SWIMFlightLegDTO swimFlightLegDto in swimFlightLegs)
            {
                FlightLegDTO dto = new FlightLegDTO();
                dto.Id = swimFlightLegDto.Oid;
                dto.FlightNumber = swimFlightLegDto.AircraftIdentification;
                dto.DepartureICAO = swimFlightLegDto.DepartureICAO;
                dto.ArrivalICAO = swimFlightLegDto.ArrivalICAO;
                
                var aircraftByFlightDepartment = flightDepartmentsByTailNumbers.FirstOrDefault(x => x.Item2 == swimFlightLegDto.AircraftIdentification);
                if (aircraftByFlightDepartment != null)
                {
                    dto.FlightDepartment = aircraftByFlightDepartment.Item3;
                    var aircraft = aircrafts.FirstOrDefault(x => x.AircraftId == aircraftByFlightDepartment.Item1);
                    if (aircraft != null)
                    {
                        dto.Make = aircraft.Make;
                        dto.Model = aircraft.Model;
                    }
                }

                dto.ATDZulu = swimFlightLegDto.ATD;
                AcukwikAirportDTO departureAirport = airports.FirstOrDefault(x => x.Icao == dto.DepartureICAO);
                if (departureAirport != null)
                {
                    dto.ATDLocal = DateTimeHelper.GetLocalTime(swimFlightLegDto.ATD, departureAirport.IntlTimeZone, departureAirport.DaylightSavingsYn?.ToLower() == "y");
                    dto.DepartureCity = departureAirport.AirportCity;
                }
                else
                {
                    dto.ATDLocal = swimFlightLegDto.ATD;
                }

                dto.ETALocal = swimFlightLegDto.ETA;
                AcukwikAirportDTO arrivalAirport = airports.FirstOrDefault(x => x.Icao == dto.ArrivalICAO);
                if (arrivalAirport != null)
                {
                    dto.ETALocal = DateTimeHelper.GetLocalTime(swimFlightLegDto.ETA, arrivalAirport.IntlTimeZone, arrivalAirport.DaylightSavingsYn?.ToLower() == "y");
                    dto.ArrivalCity = arrivalAirport.AirportCity;
                }
                else
                {
                    dto.ETALocal = swimFlightLegDto.ETA;
                }

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
                
                SWIMFlightLegDataDTO latestSWIMMessage =
                    swimFlightLegDto.SWIMFlightLegDataMessages.OrderByDescending(x => x.Oid).First();
                dto.ActualSpeed = latestSWIMMessage.ActualSpeed;
                dto.Altitude = latestSWIMMessage.Altitude;
                dto.Latitude = latestSWIMMessage.Latitude;
                dto.Longitude = latestSWIMMessage.Longitude;

                result.Add(dto);
            }

            return result;
        }
    }
}
