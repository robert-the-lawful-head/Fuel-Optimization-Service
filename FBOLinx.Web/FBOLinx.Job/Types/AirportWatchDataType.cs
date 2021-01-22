using CsvHelper.Configuration.Attributes;

namespace FBOLinx.Job.Types
{
    public class AirportWatchDataType
    {
        public int BoxTransmissionDateTimeUtc { get; set; }
        public string AircraftHexCode { get; set; }
        public string AtcFlightNumber { get; set; }
        [Optional]
        public int AltitudeInStandardPressure { get; set; }
        [Optional]
        public int GroundSpeedKts { get; set; }
        public double TrackingDegree { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Optional]
        public int VerticalSpeedKts { get; set; }
        [Optional]
        public int TransponderCode { get; set; }
        public string BoxName { get; set; }
        public int AircraftPositionDateTimeUtc { get; set; }
        public string AircraftTypeCode { get; set; }
        [Optional]
        public int GpsAltitude { get; set; }
        public bool IsAircraftOnGround { get; set; }
    }
}
