using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Specifications.SWIM;
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
        Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(int pastMinutesForDepartureOrArrival = 30);

        Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(List<string> airportIdentifiers,
            int pastMinutesForDepartureOrArrival = 30);
        Task<List<SWIMFlightLegDTO>> GetSwimFlightLegsForFlightWatchMap(string icao, int etaTimeMinutesThreshold, int atdTimeMinutesThreshold, int lastUpdated);
    }

    public class SWIMFlightLegService : BaseDTOService<SWIMFlightLegDTO, DB.Models.SWIMFlightLeg, FlightDataContext>, ISWIMFlightLegService
    {
        private SWIMFlightLegEntityService _SwimFlightLegEntityService;
        private IRepository<DB.Models.SWIMFlightLeg, FlightDataContext> _repository;
        private int flightWatchMaxRecords = 30000;

        public SWIMFlightLegService(SWIMFlightLegEntityService swimFlightLegEntityService, IRepository<DB.Models.SWIMFlightLeg, FlightDataContext> repository) : base(swimFlightLegEntityService)
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

        public async Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(int pastMinutesForDepartureOrArrival = 30)
        {
            //Use the maximum OID to get the last 30 minutes of data 
            var minimumLastUpdatedDateTime = DateTime.UtcNow.AddMinutes(-30);
            var currentMaxOID = await _SwimFlightLegEntityService.GetMaximumOID();
            var result = await _SwimFlightLegEntityService.GetListBySpec(
                new SWIMFlightLegByLastUpdatedSpecification(minimumLastUpdatedDateTime, currentMaxOID - 300000));

            //Filter from the last updated record to find arrivals or departures that are within 30 minutes
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
        public async Task<List<SWIMFlightLegDTO>> GetSwimFlightLegsForFlightWatchMap(string icao, int etaTimeMinutesThreshold, int atdTimeMinutesThreshold, int lastUpdateThreshold) => (await _SwimFlightLegEntityService.GetSWIMFlightLegsForFlightWatchMap(icao, etaTimeMinutesThreshold, atdTimeMinutesThreshold, lastUpdateThreshold)).Adapt<List<SWIMFlightLegDTO>>();
    }
}
