using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Utilities.DatesAndTimes;
using FBOLinx.DB.Specifications.AcukwikAirport;
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

        public SWIMService(SWIMFlightLegEntityService flightLegEntityService, SWIMFlightLegDataEntityService flightLegDataEntityService, 
            AirportWatchLiveDataEntityService airportWatchLiveDataEntityService, AircraftHexTailMappingEntityService aircraftHexTailMappingEntityService, 
            AirportWatchHistoricalDataEntityService airportWatchHistoricalDataEntityService, AcukwikAirportEntityService acukwikAirportEntityService)
        {
            _FlightLegEntityService = flightLegEntityService;
            _FlightLegDataEntityService = flightLegDataEntityService;
            _AirportWatchLiveDataEntityService = airportWatchLiveDataEntityService;
            _AircraftHexTailMappingEntityService = aircraftHexTailMappingEntityService;
            _AirportWatchHistoricalDataEntityService = airportWatchHistoricalDataEntityService;
            _AcukwikAirportEntityService = acukwikAirportEntityService;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetDepartures(string icao)
        {
            IEnumerable<SWIMFlightLegDTO> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(icao, null, DateTime.UtcNow));
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs);

            return result;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetArrivals(string icao)
        {
            IEnumerable<SWIMFlightLegDTO> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(null, icao, DateTime.UtcNow));
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
                    DateTime latestExistingETA = existingFlightLeg.SWIMFlightLegDataMessages.OrderByDescending(x => x.Id).First().ETA;
                    foreach (SWIMFlightLegDataDTO flightLegDataMessageDto in swimFlightLegDto.SWIMFlightLegDataMessages)
                    {
                        double messageTimestampThreshold = Math.Abs((flightLegDataMessageDto.MessageTimestamp - latestMessageTimestamp).TotalSeconds);
                        if (messageTimestampThreshold > MessageThresholdSec && 
                            (flightLegDataMessageDto.ActualSpeed != null ||
                             flightLegDataMessageDto.Altitude != null || flightLegDataMessageDto.Latitude != null ||
                             flightLegDataMessageDto.Longitude != null || flightLegDataMessageDto.ETA != latestExistingETA))
                        {
                            flightLegDataMessageDto.SWIMFlightLegId = existingFlightLeg.Id;
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

            List<AirportWatchLiveDataDto> antennaLiveData = await _AirportWatchLiveDataEntityService.GetListBySpec(new AirportWatchLiveDataSpecification(swimFlightLegDto.AircraftIdentification));
            AirportWatchLiveDataDto antennaLiveDataRecord = antennaLiveData.FirstOrDefault();
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
                List<AirportWatchHistoricalDataDto> antennaHistoricalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(new AirportWatchHistoricalDataSpecification(swimFlightLegDto.AircraftIdentification));
                AirportWatchHistoricalDataDto antennaHistoricalDataRecord = antennaHistoricalData.FirstOrDefault();
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

        private async Task<IEnumerable<FlightLegDTO>> GetFlightLegs(IEnumerable<SWIMFlightLegDTO> swimFlightLegs)
        {
            List<string> airportICAOs = swimFlightLegs.Select(x => x.DepartureICAO).ToList();
            airportICAOs.AddRange(swimFlightLegs.Select(x => x.ArrivalICAO).ToList());
            airportICAOs = airportICAOs.Distinct().ToList();
            List<AcukwikAirportDTO> airports = await _AcukwikAirportEntityService.GetListBySpec(new AcukwikAirportSpecification(airportICAOs));
            IList<FlightLegDTO> result = new List<FlightLegDTO>();
            foreach (SWIMFlightLegDTO swimFlightLegDto in swimFlightLegs)
            {
                FlightLegDTO dto = new FlightLegDTO();
                dto.Id = swimFlightLegDto.Id;
                dto.FlightNumber = swimFlightLegDto.AircraftIdentification;
                dto.DepartureICAO = swimFlightLegDto.DepartureICAO;
                dto.ArrivalICAO = swimFlightLegDto.ArrivalICAO;
                
                AcukwikAirportDTO departureAirport = airports.FirstOrDefault(x => x.Icao == dto.DepartureICAO);
                if (departureAirport != null)
                {
                    dto.ATD = DateTimeHelper.GetLocalTime(dto.ATD, departureAirport.IntlTimeZone, departureAirport.DaylightSavingsYn?.ToLower() == "y");
                }
                else
                {
                    dto.ATD = swimFlightLegDto.ATD;
                }

                AcukwikAirportDTO arrivalAirport = airports.FirstOrDefault(x => x.Icao == dto.ArrivalICAO);
                if (arrivalAirport != null)
                {
                    dto.ETA = DateTimeHelper.GetLocalTime(dto.ETA, arrivalAirport.IntlTimeZone, arrivalAirport.DaylightSavingsYn?.ToLower() == "y");
                }
                else
                {
                    dto.ETA = swimFlightLegDto.ETA;
                }
                
                SWIMFlightLegDataDTO latestSWIMMessage =
                    swimFlightLegDto.SWIMFlightLegDataMessages.OrderByDescending(x => x.Id).First();
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
