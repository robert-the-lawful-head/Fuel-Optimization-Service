using System;
using static FBOLinx.DB.Models.AirportWatchHistoricalData;

namespace FBOLinx.Web.Models.Responses.AirportWatch
{
    public class AirportWatchHistoricalDataResponse
    {
        public int CustomerInfoByGroupID { get; set; }
        public int CompanyId { get; set; }
        public string Company { get; set; }
        public DateTime DateTime { get; set; }
        public string TailNumber { get; set; }
        public string FlightNumber { get; set; }
        public string HexCode { get; set; }
        public string AircraftType { get; set; }
        public string AircraftTypeCode { get; set; }
        public string Status { get; set; }
        public int? PastVisits { get; set; }
        public string Originated { get; set; }
        public string AirportIcao { get; set; }
        public int? VisitsToMyFbo { get; set; }
        public double? PercentOfVisits { get; set; }
    }
}
