using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Projections.AirportWatch
{
    public class AirportWatchLiveHexTailMapping
    {
        public string AircraftHexCode { get; set; }
        public string TailNumber { get; set; }
        public string AtcFlightNumber { get; set; }
        public DateTime AircraftPositionDateTimeUtc { get; set; }
    }
}