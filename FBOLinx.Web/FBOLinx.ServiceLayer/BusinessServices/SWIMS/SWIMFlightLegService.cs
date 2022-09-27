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
        Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(int pastMinutesForDepartureOrArrival = -30);

        Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(string icao = "", int pastMinutesForDepartureOrArrival = 30);
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

        public async Task<List<SWIMFlightLegDTO>> GetRecentSWIMFlightLegs(string icao = "", int pastMinutesForDepartureOrArrival = 30)
        {
            if (string.IsNullOrEmpty(icao))
                return await GetRecentSWIMFlightLegs(pastMinutesForDepartureOrArrival);
            List<SWIMFlightLegDTO> result = new List<SWIMFlightLegDTO>();
                result = await GetListbySpec(new SWIMFlightLegByAirportSpecification(icao,
                    DateTime.UtcNow.AddMinutes(-pastMinutesForDepartureOrArrival)));

            return result;
        }
    }
}
