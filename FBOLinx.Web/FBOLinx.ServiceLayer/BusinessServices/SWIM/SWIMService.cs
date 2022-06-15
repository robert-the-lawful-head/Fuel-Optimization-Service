using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public SWIMService(SWIMFlightLegEntityService flightLegEntityService, SWIMFlightLegDataEntityService flightLegDataEntityService, 
            AirportWatchLiveDataEntityService airportWatchLiveDataEntityService, AircraftHexTailMappingEntityService aircraftHexTailMappingEntityService, 
            AirportWatchHistoricalDataEntityService airportWatchHistoricalDataEntityService)
        {
            _FlightLegEntityService = flightLegEntityService;
            _FlightLegDataEntityService = flightLegDataEntityService;
            _AirportWatchLiveDataEntityService = airportWatchLiveDataEntityService;
            _AircraftHexTailMappingEntityService = aircraftHexTailMappingEntityService;
            _AirportWatchHistoricalDataEntityService = airportWatchHistoricalDataEntityService;
        }

        public async Task SaveFlightLegData(IEnumerable<SWIMFlightLegDTO> flightLegs)
        {
            if (flightLegs == null || !flightLegs.Any())
            {
                return;
            }
            
            // List<AirportWatchLiveDataDto> antennaData = await _AirportWatchLiveDataEntityService.GetListBySpec(new AirportWatchLiveDataSpecification(
            //     flightLegs.Where(x => !x.AircraftIdentification.StartsWith('N')).Select(x => x.AircraftIdentification).ToList()));
            // List<SWIMFlightLegDataDTO> flightLegDataMessagesToInsert = new List<SWIMFlightLegDataDTO>();
            // foreach (SWIMFlightLegDTO swimFlightLegDto in flightLegs)
            // {
            //     if (swimFlightLegDto.SWIMFlightLegDataMessages == null || !swimFlightLegDto.SWIMFlightLegDataMessages.Any())
            //     {
            //         continue;
            //     }
            //
            //     if (!swimFlightLegDto.AircraftIdentification.StartsWith('N'))
            //     {
            //         AirportWatchLiveDataDto antennaDataRecord = antennaData.FirstOrDefault(x => x.AtcFlightNumber == swimFlightLegDto.AircraftIdentification);
            //         if (antennaDataRecord != null)
            //         {
            //             if (!string.IsNullOrEmpty(antennaDataRecord.TailNumber))
            //             {
            //                 swimFlightLegDto.AircraftIdentification = antennaDataRecord.TailNumber;
            //             }
            //             else if (!string.IsNullOrEmpty(antennaDataRecord.AircraftHexCode))
            //             {
            //                 List<AircraftHexTailMappingDTO> hexTailMappings = await _AircraftHexTailMappingEntityService.GetListBySpec(new AircraftHexTailMappingSpecification(antennaDataRecord.AircraftHexCode));
            //                 if (hexTailMappings != null && hexTailMappings.Any())
            //                 {
            //                     swimFlightLegDto.AircraftIdentification = hexTailMappings.First().TailNumber;
            //                 }
            //             }
            //         }
            //         else
            //         {
            //
            //         }
            //     }
            // }

            DateTime atdMin = flightLegs.Min(x => x.ATD);
            DateTime atdMax = flightLegs.Max(x => x.ATD);
            IList<string> aircraftIdentificationNumbers = flightLegs.Select(x => x.AircraftIdentification).Distinct().ToList();
            List<SWIMFlightLegDTO> existingFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(aircraftIdentificationNumbers, atdMin, atdMax));
            
            List<SWIMFlightLegDataDTO> flightLegDataMessagesToInsert = new List<SWIMFlightLegDataDTO>();
            
            foreach (SWIMFlightLegDTO swimFlightLegDto in flightLegs)
            {
                var existingFlightLeg = existingFlightLegs.SingleOrDefault(
                    x => x.DepartureICAO == swimFlightLegDto.DepartureICAO && x.ArrivalICAO == swimFlightLegDto.ArrivalICAO && x.ATD == swimFlightLegDto.ATD);
                if (existingFlightLeg != null)
                {
                    DateTime latestMessageTimestamp = existingFlightLeg.SWIMFlightLegDataMessages.Max(x => x.MessageTimestamp);
                    foreach (SWIMFlightLegDataDTO flightLegDataMessageDto in swimFlightLegDto.SWIMFlightLegDataMessages)
                    {
                        double messageTimestampThreshold = Math.Abs((flightLegDataMessageDto.MessageTimestamp - latestMessageTimestamp).TotalSeconds);
                        if (messageTimestampThreshold > MessageThresholdSec)
                        {
                            flightLegDataMessageDto.SWIMFlightLegId = existingFlightLeg.Oid;
                            flightLegDataMessagesToInsert.Add(flightLegDataMessageDto);
                        }
                    }
                }
                else
                {
                    await SetTailNumber(swimFlightLegDto);
                    await _FlightLegEntityService.Add(swimFlightLegDto);
                }
            }

            if (flightLegDataMessagesToInsert.Count > 0)
            {
                await _FlightLegDataEntityService.BulkInsertOrUpdate(flightLegDataMessagesToInsert);
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
    }
}
