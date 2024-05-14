using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FBOLinx.Core.Extensions;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Fuelerlinx.SDK;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class SWIMFlightLegEntityService : Repository<SWIMFlightLeg, DegaContext>
    {
        private FboLinxContext _fboLinxContext;
        public SWIMFlightLegEntityService(DegaContext context, FboLinxContext fboLinxContext) : base(context)
        {
            _fboLinxContext = fboLinxContext;
        }

        public async Task<long> GetMaximumOID()
        {
            var result = await context.SWIMFlightLegs.MaxAsync(x => x.Oid);
            return result;
        }

        public async Task<List<SWIMFlightLeg>> GetSWIMFlightLegs(List<string> gufiList)
        {
            var query = GetSWIMFlightLegsQueryable(gufiList);
            return await query.ToListAsync();
        }

        public async Task<List<SWIMFlightLeg>> GetSWIMFlightLegs(DateTime minArrivalOrDepartureDateTimeUtc, DateTime maxArrivalOrDepartureDateTimeUtc, List<string> departureAirportIcaos = null,
            List<string> arrivalAirportIcaos = null,
            List<string> aircraftIdentifications = null,
            bool? isPlaceHolder = null)
        {
            var query = GetSWIMFlightLegsQueryable(minArrivalOrDepartureDateTimeUtc, maxArrivalOrDepartureDateTimeUtc,
                departureAirportIcaos, arrivalAirportIcaos, aircraftIdentifications, isPlaceHolder);
            return await query.ToListAsync();
        }

        public IQueryable<SWIMFlightLeg> GetSWIMFlightLegsQueryable(List<string> tailNumbersList, List<string> atdsList)
        {
            var query = (from swim in context.SWIMFlightLegs
                         join tailNumbers in context.AsTable(tailNumbersList) on swim.AircraftIdentification equals tailNumbers.Value
                         join atds in context.AsTable(atdsList) on new { swim.ATD, Id = Convert.ToInt64(tailNumbers.Id.ToString()) } equals new { ATD = DateTime.Parse(atds.Value), Id = Convert.ToInt64(atds.Id.ToString()) }
                         select swim);
            return query;
        }

        public IQueryable<SWIMFlightLeg> GetSWIMFlightLegsByIdsQueryable(List<string> idsList)
        {
            var query = (from swim in context.SWIMFlightLegs
                         join id in context.AsTable(idsList) on swim.Oid equals Convert.ToInt64(id.Value)
                         select swim);
            return query;
        }

        private IQueryable<SWIMFlightLeg> GetSWIMFlightLegsQueryable(List<string> gufiList)
        {
            var query = (from swim in context.SWIMFlightLegs
                join gufi in context.AsTable(gufiList) on swim.Gufi equals gufi.Value
                select swim);
            return query;
        }

        private Expression<Func<SWIMFlightLeg, bool>> ArrivalsAndDeparturesQuerylogic(int etaTimeMinutesThreshold, int atdTimeMinutesThreshold, int lastUpdateThreshold)
        {
            var atdDateTimeThreshold = DateTime.UtcNow.AddMinutes(-atdTimeMinutesThreshold);
            var etaDateTimeThreshold = DateTime.UtcNow.AddMinutes(-etaTimeMinutesThreshold);
            var lastUpdateDateTime = DateTime.UtcNow.AddMinutes(-lastUpdateThreshold);

            return swim => swim.LastUpdated >= lastUpdateDateTime &&
                    (swim.ATD >= atdDateTimeThreshold || (swim.ETA.HasValue && swim.ETA.Value >= etaDateTimeThreshold));
        }

        public async Task<IList<SWIMFlightLeg>> GetSWIMFlightLegsForFlightWatchMap(string icao, int etaTimeMinutesThreshold, int atdTimeMinutesThreshold, int lastUpdateThreshold)
        {
            var query = context.SWIMFlightLegs
                .Where(ArrivalsAndDeparturesQuerylogic(etaTimeMinutesThreshold, atdTimeMinutesThreshold, lastUpdateThreshold));

            var arrivals = query.Where(swim => swim.ArrivalICAO == icao);

            var departures = query.Where(swim => swim.DepartureICAO == icao);

            return await arrivals.Concat(departures).ToListAsync();
        }

        private IQueryable<SWIMFlightLeg> GetSWIMFlightLegsQueryable(DateTime minArrivalOrDepartureDateTimeUtc,
        DateTime maxArrivalOrDepartureDateTimeUtc, List<string> departureAirportIcaos = null,
        List<string> arrivalAirportIcaos = null,
        List<string> aircraftIdentifications = null,
        bool? isPlaceHolder = null
        )
        {
            var query = (from swim in context.SWIMFlightLegs
                join departureAirport in context.AsTable(departureAirportIcaos) on swim.DepartureICAO equals departureAirport.Value
                into departureAirportJoin
                from departureAirport in departureAirportJoin.DefaultIfEmpty()
                join arrivalAirport in context.AsTable(arrivalAirportIcaos) on swim.ArrivalICAO equals arrivalAirport.Value
                into arrivalAirportJoin
                from arrivalAirport in arrivalAirportJoin.DefaultIfEmpty()
                join aircraftIdentification in context.AsTable(aircraftIdentifications) on swim.AircraftIdentification equals aircraftIdentification.Value
                into aircraftIdentificationJoin
                from aircraftIdentification in aircraftIdentificationJoin.DefaultIfEmpty()
                    where ((swim.ATD > minArrivalOrDepartureDateTimeUtc) || (swim.ETA.HasValue && swim.ETA.Value > minArrivalOrDepartureDateTimeUtc))
                                                && ((swim.ATD < maxArrivalOrDepartureDateTimeUtc) || (swim.ETA.HasValue && swim.ETA.Value < maxArrivalOrDepartureDateTimeUtc))
                                                && (departureAirportIcaos == null || !string.IsNullOrEmpty(departureAirport.Value))
                                                && (arrivalAirportIcaos == null || !string.IsNullOrEmpty(arrivalAirport.Value))
                                                && (aircraftIdentifications == null || !string.IsNullOrEmpty(aircraftIdentification.Value))
                                                && (isPlaceHolder == null || isPlaceHolder.Value == swim.IsPlaceholder)
                                                                        select swim);

            return query;
        }
    }
}
