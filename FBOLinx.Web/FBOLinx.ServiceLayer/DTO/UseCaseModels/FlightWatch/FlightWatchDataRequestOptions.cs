using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch
{
    public class FlightWatchDataRequestOptions
    {
        public int? FboIdForCenterPoint { get; set; }
        public bool IncludeRecentHistoricalRecords { get; set; } = true;
        public bool IncludeNearestAirportPosition { get; set; } = true;
        public bool IncludeFuelOrderInformation { get; set; }
        public bool IncludeCustomerAircraftInformation { get; set; }
    }
}
