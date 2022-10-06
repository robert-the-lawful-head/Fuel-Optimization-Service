using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.BusinessServices.SWIM
{
    public interface ISWIMService
    {
        Task<IEnumerable<FlightLegDTO>> GetDepartures(int groupId, int fboId, string icao);
        Task<IEnumerable<FlightLegDTO>> GetArrivals(int groupId, int fboId, string icao);
        Task SaveFlightLegData(IEnumerable<SWIMFlightLegDTO> flightLegs);
        Task SyncRecentAndUpcomingFlightLegs();
    }
}
