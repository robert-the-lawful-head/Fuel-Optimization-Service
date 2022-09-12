using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;

namespace FBOLinx.DB.Projections.AirportWatch
{
    public class AirportWatchHistoricalDataSimplifiedProjection
    {
        public int Oid { get; set; }
        public string AircraftHexCode { get; set; }
        public string AtcFlightNumber { get; set; }
        public DateTime AircraftPositionDateTimeUtc { get; set; }
        public AircraftStatusType AircraftStatus { get; set; }
        public string AirportICAO { get; set; }
        public string AircraftTypeCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string TailNumber { get; set; }
    }
}
