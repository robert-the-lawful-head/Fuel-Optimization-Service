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

namespace FBOLinx.ServiceLayer.BusinessServices.SWIM
{
    public class SWIMService: ISWIMService
    {
        private readonly SWIMFlightLegEntityService _FlightLegEntityService;
        private readonly SWIMFlightLegDataEntityService _FlightLegDataEntityService;
        private readonly AirportWatchLiveDataEntityService _AirportWatchLiveDataEntityService;
        private readonly AircraftHexTailMappingEntityService _AircraftHexTailMappingEntityService;

        public SWIMService(SWIMFlightLegEntityService flightLegEntityService, SWIMFlightLegDataEntityService flightLegDataEntityService, 
            AirportWatchLiveDataEntityService airportWatchLiveDataEntityService, AircraftHexTailMappingEntityService aircraftHexTailMappingEntityService)
        {
            _FlightLegEntityService = flightLegEntityService;
            _FlightLegDataEntityService = flightLegDataEntityService;
            _AirportWatchLiveDataEntityService = airportWatchLiveDataEntityService;
            _AircraftHexTailMappingEntityService = aircraftHexTailMappingEntityService;
        }

        public async Task SaveFlightLegData(IEnumerable<SWIMFlightLegDTO> flightLegs)
        {
            if (flightLegs == null || !flightLegs.Any())
            {
                return;
            }

            IList<string> aircraftIdentificationNumbers = flightLegs.Select(x => x.AircraftIdentification).Distinct().ToList();
            DateTime atdMin = flightLegs.Min(x => x.ATD);
            DateTime atdMax = flightLegs.Max(x => x.ATD);
            List<SWIMFlightLegDTO> existingFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(aircraftIdentificationNumbers, atdMin, atdMax));
            List<AirportWatchLiveDataDto> antennaData = await _AirportWatchLiveDataEntityService.GetListBySpec(new AirportWatchLiveDataSpecification(
                flightLegs.Where(x => !x.AircraftIdentification.StartsWith('N')).Select(x => x.AircraftIdentification).ToList()));
            List<SWIMFlightLegDataDTO> flightLegDataMessagesToInsert = new List<SWIMFlightLegDataDTO>();
            foreach (SWIMFlightLegDTO swimFlightLegDto in flightLegs)
            {
                if (swimFlightLegDto.SWIMFlightLegDataMessages == null || !swimFlightLegDto.SWIMFlightLegDataMessages.Any())
                {
                    continue;
                }

                if (!swimFlightLegDto.AircraftIdentification.StartsWith('N'))
                {
                    AirportWatchLiveDataDto antennaDataRecord = antennaData.FirstOrDefault(x => x.AtcFlightNumber == swimFlightLegDto.AircraftIdentification);
                    if (antennaDataRecord != null)
                    {
                        if (!string.IsNullOrEmpty(antennaDataRecord.TailNumber))
                        {
                            swimFlightLegDto.AircraftIdentification = antennaDataRecord.TailNumber;
                        }
                        else if (!string.IsNullOrEmpty(antennaDataRecord.AircraftHexCode))
                        {
                            List<AircraftHexTailMappingDTO> hexTailMappings = await _AircraftHexTailMappingEntityService.GetListBySpec(new AircraftHexTailMappingSpecification(antennaDataRecord.AircraftHexCode));
                            if (hexTailMappings != null && hexTailMappings.Any())
                            {
                                swimFlightLegDto.AircraftIdentification = hexTailMappings.First().TailNumber;
                            }
                        }
                    }
                }

                var existingFlightLeg = existingFlightLegs.SingleOrDefault(
                    x => x.AircraftIdentification == swimFlightLegDto.AircraftIdentification && x.ATD == swimFlightLegDto.ATD && x.DepartureICAO == swimFlightLegDto.DepartureICAO && x.ArrivalICAO == swimFlightLegDto.ArrivalICAO);
                if (existingFlightLeg != null)
                {
                    foreach (SWIMFlightLegDataDTO flightLegDataMessageDto in swimFlightLegDto.SWIMFlightLegDataMessages)
                    {
                        flightLegDataMessageDto.SWIMFlightLegId = existingFlightLeg.Oid;
                    }

                    flightLegDataMessagesToInsert.AddRange(swimFlightLegDto.SWIMFlightLegDataMessages);
                }
                else
                {
                    await _FlightLegEntityService.Add(swimFlightLegDto);
                }
            }

            if (flightLegDataMessagesToInsert.Count > 0)
            {
                await _FlightLegDataEntityService.BulkInsertOrUpdate(flightLegDataMessagesToInsert);
            }
        }
    }
}
