using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Extensions;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.SWIM;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class SWIMFlightLegEntityService : Repository<SWIMFlightLeg, DegaContext>
    {
        public SWIMFlightLegEntityService(DegaContext context) : base(context)
        {
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

        private IQueryable<SWIMFlightLeg> GetSWIMFlightLegsQueryable(List<string> gufiList)
        {
            var query = (from swim in context.SWIMFlightLegs
                join gufi in context.AsTable(gufiList) on swim.Gufi equals gufi.Value
                select swim);
            return query;
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
