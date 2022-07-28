using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.BusinessServices.SWIM
{
    public interface ISWIMService
    {
        Task<IEnumerable<FlightLegDTO>> GetDepartures(string icao);
        Task<IEnumerable<FlightLegDTO>> GetArrivals(string icao);
        Task SaveFlightLegData(IEnumerable<SWIMFlightLegDTO> flightLegs);

        Task<IEnumerable<FlightLegDTO>> GetHistoricalFlightLegs(string icao, bool isArrivals, DateTime historicalETD,
            DateTime historicalETA, DateTime historicalAircraftPositionDateTime);
    }
}
