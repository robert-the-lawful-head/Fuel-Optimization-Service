using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Utilities.DatesAndTimes;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.AcukwikAirport;
using FBOLinx.DB.Specifications.AircraftHexTailMapping;
using FBOLinx.DB.Specifications.SWIM;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.Service.Mapping.Dto;
using Mapster;

namespace FBOLinx.ServiceLayer.BusinessServices.SWIM
{
    public class SWIMService: ISWIMService
    {
        private const int MessageThresholdSec = 30;

        private readonly SWIMFlightLegRepository _flightLegRepository;
        private readonly SWIMFlightLegDataRepository _flightLegDataRepository;
        private readonly AirportWatchLiveDataRepository _airportWatchLiveDataRepository;
        private readonly AircraftHexTailMappingEntityService _AircraftHexTailMappingEntityService;
        private readonly AirportWatchHistoricalDataRepository _airportWatchHistoricalDataRepository;
        private readonly AcukwikAirportEntityService _AcukwikAirportEntityService;

        public SWIMService(SWIMFlightLegRepository flightLegRepository, SWIMFlightLegDataRepository flightLegDataRepository, 
            AirportWatchLiveDataRepository airportWatchLiveDataRepository, AircraftHexTailMappingEntityService aircraftHexTailMappingEntityService, 
            AirportWatchHistoricalDataRepository airportWatchHistoricalDataRepository, AcukwikAirportEntityService acukwikAirportEntityService)
        {
            _flightLegRepository = flightLegRepository;
            _flightLegDataRepository = flightLegDataRepository;
            _airportWatchLiveDataRepository = airportWatchLiveDataRepository;
            _AircraftHexTailMappingEntityService = aircraftHexTailMappingEntityService;
            _airportWatchHistoricalDataRepository = airportWatchHistoricalDataRepository;
            _AcukwikAirportEntityService = acukwikAirportEntityService;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetDepartures(string icao)
        {
            IEnumerable<SWIMFlightLeg> swimFlightLegs = await _flightLegRepository.GetListBySpec(new SWIMFlightLegSpecification(icao, null, DateTime.UtcNow));
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs);

            return result;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetArrivals(string icao)
        {
            IEnumerable<SWIMFlightLeg> swimFlightLegs = await _flightLegRepository.GetListBySpec(new SWIMFlightLegSpecification(null, icao, DateTime.UtcNow));
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs);
            
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
            List<SWIMFlightLeg> existingFlightLegs = await _flightLegRepository.GetListBySpec(new SWIMFlightLegSpecification(departureICAOs, arrivalICAOs, atdMin, atdMax));
            
            List<SWIMFlightLegData> flightLegDataMessagesToInsert = new List<SWIMFlightLegData>();
            List<SWIMFlightLeg> flightLegsToUpdate = new List<SWIMFlightLeg>();
            List<SWIMFlightLeg> flightLegsToAdd = new List<SWIMFlightLeg>();
            
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
                            flightLegDataMessagesToInsert.Add(flightLegDataMessageDto.Adapt<SWIMFlightLegData>());
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
                    flightLegsToAdd.Add(swimFlightLegDto.Adapt<SWIMFlightLeg>());
                }
            }

            if (flightLegsToAdd.Count > 0)
            {
                await _flightLegRepository.BulkInsertOrUpdate(flightLegsToAdd);

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
                await _flightLegDataRepository.BulkInsertOrUpdate(flightLegDataMessagesToInsert);
            }

            if (flightLegsToUpdate.Count > 0)
            {
                await _flightLegRepository.BulkInsertOrUpdate(flightLegsToUpdate);
            }
        }

        private async Task SetTailNumber(SWIMFlightLegDTO swimFlightLegDto)
        {
            if (swimFlightLegDto.AircraftIdentification.ToUpperInvariant().StartsWith('N'))
            {
                return;
            }

            List<AirportWatchLiveData> antennaLiveData = await _airportWatchLiveDataRepository.GetListBySpec(new AirportWatchLiveDataByFlightNumberSpecification(swimFlightLegDto.AircraftIdentification, DateTime.UtcNow.AddHours(-1)));
            AirportWatchLiveData antennaLiveDataRecord = antennaLiveData.OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
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
                List<AirportWatchHistoricalData> antennaHistoricalData = await _airportWatchHistoricalDataRepository.GetListBySpec(new AirportWatchHistoricalDataSpecification(swimFlightLegDto.AircraftIdentification, DateTime.UtcNow.AddDays(-1)));
                AirportWatchHistoricalData antennaHistoricalDataRecord = antennaHistoricalData.OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
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

        private async Task<IEnumerable<FlightLegDTO>> GetFlightLegs(IEnumerable<SWIMFlightLeg> swimFlightLegs)
        {
            List<string> airportICAOs = swimFlightLegs.Select(x => x.DepartureICAO).ToList();
            airportICAOs.AddRange(swimFlightLegs.Select(x => x.ArrivalICAO).ToList());
            airportICAOs = airportICAOs.Distinct().ToList();
            List<AcukwikAirportDTO> airports = await _AcukwikAirportEntityService.GetListBySpec(new AcukwikAirportSpecification(airportICAOs));
            IList<FlightLegDTO> result = new List<FlightLegDTO>();
            foreach (SWIMFlightLeg swimFlightLeg in swimFlightLegs)
            {
                FlightLegDTO dto = new FlightLegDTO();
                dto.Id = swimFlightLeg.Oid;
                dto.FlightNumber = swimFlightLeg.AircraftIdentification;
                dto.DepartureICAO = swimFlightLeg.DepartureICAO;
                dto.ArrivalICAO = swimFlightLeg.ArrivalICAO;
                
                AcukwikAirportDTO departureAirport = airports.FirstOrDefault(x => x.Icao == dto.DepartureICAO);
                if (departureAirport != null)
                {
                    dto.ATD = DateTimeHelper.GetLocalTime(swimFlightLeg.ATD, departureAirport.IntlTimeZone, departureAirport.DaylightSavingsYn?.ToLower() == "y");
                }
                else
                {
                    dto.ATD = swimFlightLeg.ATD;
                }

                AcukwikAirportDTO arrivalAirport = airports.FirstOrDefault(x => x.Icao == dto.ArrivalICAO);
                if (arrivalAirport != null)
                {
                    dto.ETA = DateTimeHelper.GetLocalTime(swimFlightLeg.ETA, arrivalAirport.IntlTimeZone, arrivalAirport.DaylightSavingsYn?.ToLower() == "y");
                }
                else
                {
                    dto.ETA = swimFlightLeg.ETA;
                }
                
                SWIMFlightLegData latestSWIMMessage =
                    swimFlightLeg.SWIMFlightLegDataMessages.OrderByDescending(x => x.Oid).First();
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
