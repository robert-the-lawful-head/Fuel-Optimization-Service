using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.SWIM;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using Mapster;

namespace FBOLinx.ServiceLayer.BusinessServices.SWIMS
{
    public interface ISWIMFlightLegService : IBaseDTOService<SWIMFlightLegDTO, DB.Models.SWIMFlightLeg>
    {
        Task<List<SWIMFlightLegDTO>> GetSwimFlightLegs(List<string> gufiList);
        Task<List<SWIMFlightLegDTO>> GetSwimFlightLegs(DateTime minArrivalOrDepartureDateTimeUtc,
            DateTime maxArrivalOrDepartureDateTimeUtc, List<string> departureAirportIcaos = null,
            List<string> arrivalAirportIcaos = null,
            List<string> aircraftIdentifications = null,
            bool? isPlaceHolder = null);
        Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(int pastMinutesForDepartureOrArrival = 30);

        Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(List<string> airportIdentifiers = null,
            int pastMinutesForDepartureOrArrival = 30);
    }

    public class SWIMFlightLegService : BaseDTOService<SWIMFlightLegDTO, DB.Models.SWIMFlightLeg, DegaContext>, ISWIMFlightLegService
    {
        private IAirportService _AirportService;
        private SWIMFlightLegEntityService _SwimFlightLegEntityService;
        
        public SWIMFlightLegService(SWIMFlightLegEntityService swimFlightLegEntityService, IAirportService airportService) : base(swimFlightLegEntityService)
        {
            _SwimFlightLegEntityService = swimFlightLegEntityService;
            _AirportService = airportService;
        }

        public async Task<List<SWIMFlightLegDTO>> GetSwimFlightLegs(List<string> gufiList)
        {
            var result = await _SwimFlightLegEntityService.GetSWIMFlightLegs(gufiList);
            return result == null ? null : result.Adapt<List<SWIMFlightLegDTO>>();
        }

        public async Task<List<SWIMFlightLegDTO>> GetSwimFlightLegs(DateTime minArrivalOrDepartureDateTimeUtc, DateTime maxArrivalOrDepartureDateTimeUtc, List<string> departureAirportIcaos = null,
            List<string> arrivalAirportIcaos = null,
            List<string> aircraftIdentifications = null,
            bool? isPlaceHolder = null)
        {
            var result = await _SwimFlightLegEntityService.GetSWIMFlightLegs(minArrivalOrDepartureDateTimeUtc, maxArrivalOrDepartureDateTimeUtc, departureAirportIcaos, arrivalAirportIcaos, aircraftIdentifications, isPlaceHolder);
            return result == null ? null : result.Adapt<List<SWIMFlightLegDTO>>();
        }

        public async Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(int pastMinutesForDepartureOrArrival = 30)
        {
            List<SWIMFlightLegDTO> result = new List<SWIMFlightLegDTO>();
                result = await GetListbySpec(
                    new SWIMFlightLegByDateSpecification(
                        DateTime.UtcNow.AddMinutes(-pastMinutesForDepartureOrArrival)));

            return result;
        }

        public async Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(List<string> airportIdentifiers = null, int pastMinutesForDepartureOrArrival = 30)
        {
            List<SWIMFlightLegDTO> result = new List<SWIMFlightLegDTO>();
            if ((airportIdentifiers?.Count).GetValueOrDefault() <= 0)
            {
                return await GetRecentSWIMFlightLegs(pastMinutesForDepartureOrArrival);
            }
            else
            {
                var departures = await GetSwimFlightLegs(DateTime.UtcNow.AddMinutes(-pastMinutesForDepartureOrArrival), DateTime.UtcNow, airportIdentifiers);
                var arrivals = await GetSwimFlightLegs(DateTime.UtcNow.AddMinutes(-pastMinutesForDepartureOrArrival),
                    DateTime.UtcNow, null, airportIdentifiers);

                result = departures;
                result.AddRange(arrivals.Where(x => !departures.Any(d => d.Oid == x.Oid)));
            }

            return result;
        }
    }
}
