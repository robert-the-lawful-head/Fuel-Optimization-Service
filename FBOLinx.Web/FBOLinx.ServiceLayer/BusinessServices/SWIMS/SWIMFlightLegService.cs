using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.SWIM;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.SWIMS
{
    public interface ISWIMFlightLegService : IBaseDTOService<SWIMFlightLegDTO, DB.Models.SWIMFlightLeg>
    {
        Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(int pastMinutesForDepartureOrArrival = 30);

        Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(List<string> airportIdentifiers = null,
            int pastMinutesForDepartureOrArrival = 30, bool removeRecordsNotRecentlyUpdated = false);
    }

    public class SWIMFlightLegService : BaseDTOService<SWIMFlightLegDTO, DB.Models.SWIMFlightLeg, DegaContext>, ISWIMFlightLegService
    {
        private IAirportService _AirportService;

        public SWIMFlightLegService(IRepository<SWIMFlightLeg, DegaContext> entityService, IAirportService airportService) : base(entityService)
        {
            _AirportService = airportService;
        }

        public async Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(int pastMinutesForDepartureOrArrival = 30)
        {
            List<SWIMFlightLegDTO> result = new List<SWIMFlightLegDTO>();
                result = await GetListbySpec(
                    new SWIMFlightLegByDateSpecification(
                        DateTime.UtcNow.AddMinutes(-pastMinutesForDepartureOrArrival)));

            return result;
        }

        public async Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(List<string> airportIdentifiers = null, int pastMinutesForDepartureOrArrival = 30, bool removeRecordsNotRecentlyUpdated = false)
        {
            List<SWIMFlightLegDTO> result = new List<SWIMFlightLegDTO>();
            if ((airportIdentifiers?.Count).GetValueOrDefault() <= 0)
            {
                return await GetRecentSWIMFlightLegs(pastMinutesForDepartureOrArrival);
            }
            else
            {
                var departures = await GetListbySpec(new SWIMFlightLegByDepartureAirportSpecification(
                    airportIdentifiers,
                    DateTime.UtcNow.AddMinutes(-pastMinutesForDepartureOrArrival)));
                var arrivals = await GetListbySpec(new SWIMFlightLegByArrivalAirportSpecification(airportIdentifiers,
                    DateTime.UtcNow.AddMinutes(-pastMinutesForDepartureOrArrival)));
                result = departures;
                result.AddRange(arrivals.Where(x => !departures.Any(d => d.Oid == x.Oid)));

                //Remove any SWIM data that hasn't seen a message in the last 30 minutes.
                if (removeRecordsNotRecentlyUpdated)
                    result.RemoveAll(x =>
                        !x.LastUpdated.HasValue || x.LastUpdated.Value < DateTime.UtcNow.AddMinutes(-30));
            }

            return result;
        }
    }
}
