using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses.AirportWatch
{
    public class AirportWatchHistoricalDataResponse
    {
        public string Company { get; set; }
        public DateTime DateTime { get; set; }
        public string TailNumber { get; set; }
        public string FlightNumber { get; set; }
        public string HexCode { get; set; }
        public string AircraftType { get; set; }
        public string Status { get; set; }
        public int PastVisits { get; set; }
        public string Originated { get; set; }
    }
}
