using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Queries;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
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
        Task<List<SWIMFlightLegDTO>> GetSwimFlightLegsByGufi(List<string> gufiList);
        Task<List<SWIMFlightLegDTO>> GetSwimFlightLegs(DateTime minArrivalOrDepartureDateTimeUtc,
            DateTime maxArrivalOrDepartureDateTimeUtc, List<string> departureAirportIcaos = null,
            List<string> arrivalAirportIcaos = null,
            List<string> aircraftIdentifications = null,
            bool? isPlaceHolder = null);
        Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(int maxRecords, int pastMinutesForDepartureOrArrival = 30);

        Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(List<string> airportIdentifiers,
            int pastMinutesForDepartureOrArrival = 30);
        Task<IEnumerable<SWIMFlightLegDTO>> GetSWIMFlightLegs(List<string> airportsForArrivalsAndDepartures, bool isFlightWatchMapData = false);
    }

    public class SWIMFlightLegService : BaseDTOService<SWIMFlightLegDTO, DB.Models.SWIMFlightLeg, DegaContext>, ISWIMFlightLegService
    {
        private SWIMFlightLegEntityService _SwimFlightLegEntityService;
        private IRepository<DB.Models.SWIMFlightLeg, DegaContext> _repository;
        private int flightWatchMaxRecords = 30000;
        private int minutesThreshold = 30;

        public SWIMFlightLegService(SWIMFlightLegEntityService swimFlightLegEntityService, IRepository<DB.Models.SWIMFlightLeg, DegaContext> repository) : base(swimFlightLegEntityService)
        {
            _SwimFlightLegEntityService = swimFlightLegEntityService;
            _repository = repository;
        }

        public async Task<List<SWIMFlightLegDTO>> GetSwimFlightLegsByGufi(List<string> gufiList)
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

        public async Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(int maxRecords, int pastMinutesForDepartureOrArrival = 30)
        {
            var queryOptions = new QueryableOptions<SWIMFlightLeg>();
            queryOptions.MaxRecords = maxRecords;
            queryOptions.OrderByDescendingExpression = (x => x.Oid);

            var result = await _repository.GetAsync(queryOptions);

            var minimumDateTime = DateTime.UtcNow.AddMinutes(-pastMinutesForDepartureOrArrival);
            result = result.Where(x => !string.IsNullOrEmpty(x.AircraftIdentification) && ((x.ATD > minimumDateTime) || (x.ETA.HasValue && x.ETA.Value > minimumDateTime)))
                .ToList();

            return result.Adapt<List<SWIMFlightLegDTO>>();
        }
        public async Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(List<string> airportIdentifiers,
           int pastMinutesForDepartureOrArrival = 30)
        {
            List<SWIMFlightLegDTO> result = new List<SWIMFlightLegDTO>();

            var departures = await GetSwimFlightLegs(DateTime.UtcNow.AddMinutes(-pastMinutesForDepartureOrArrival),
                DateTime.UtcNow.AddMinutes(pastMinutesForDepartureOrArrival), airportIdentifiers);
            var arrivals = await GetSwimFlightLegs(DateTime.UtcNow.AddMinutes(-pastMinutesForDepartureOrArrival),
                DateTime.UtcNow.AddMinutes(pastMinutesForDepartureOrArrival), null, airportIdentifiers);

            result = departures;
            result.AddRange(arrivals.Where(x => !departures.Any(d => d.Oid == x.Oid)));
            return result
                .OrderByDescending(row => row.LastUpdated)
                .GroupBy(row => row.AircraftIdentification)
                .Select(grouped => grouped.First())
                .ToList();
        }

        public async Task<IEnumerable<SWIMFlightLegDTO>> GetSWIMFlightLegs(List<string> airportsForArrivalsAndDepartures, bool isFlightWatchMapData = false)
        {
            if (isFlightWatchMapData)
            {
                return (await _SwimFlightLegEntityService.GetSWIMFlightLegsForFlightWatchMap(airportsForArrivalsAndDepartures.FirstOrDefault(), minutesThreshold)).Adapt<List<SWIMFlightLegDTO>>();
            }
           else if ((airportsForArrivalsAndDepartures?.Count).GetValueOrDefault() > 0)
                return (await GetRecentSWIMFlightLegs(airportsForArrivalsAndDepartures)).Where(x => !string.IsNullOrEmpty(x.AircraftIdentification));   
            else
                return (await GetRecentSWIMFlightLegs(flightWatchMaxRecords)).Where(x => !string.IsNullOrEmpty(x.AircraftIdentification));
        }
    }
}
