using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.Job.Types
{
    public class AirportWatchDataType
    {
        public int BoxTransmissionDateTimeUtc { get; set; }
        public string AircraftHexCode { get; set; }
        public string AtcFlightNumber { get; set; }
        public int AltitudeInStandardPressure { get; set; }
        public int GroundSpeedKts { get; set; }
        public double TrackingDegree { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int VerticalSpeedKts { get; set; }
        public int TransponderCode { get; set; }
        public string BoxName { get; set; }
        public int AircraftPositionDateTimeUtc { get; set; }
        public string AircraftTypeCode { get; set; }
        public int GpsAltitude { get; set; }
        public bool IsAircraftOnGround { get; set; }
    }
}
