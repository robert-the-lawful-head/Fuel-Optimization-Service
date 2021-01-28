namespace FBOLinx.Job.Types
{
    public class AirportWatchDataType
    {
        public string BoxTransmissionDateTimeUtc { get; set; }
        public string AircraftHexCode { get; set; }
        public string AtcFlightNumber { get; set; }
        public string AltitudeInStandardPressure { get; set; }
        public string GroundSpeedKts { get; set; }
        public string TrackingDegree { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string VerticalSpeedKts { get; set; }
        public string TransponderCode { get; set; }
        public string BoxName { get; set; }
        public string AircraftPositionDateTimeUtc { get; set; }
        public string AircraftTypeCode { get; set; }
        public string GpsAltitude { get; set; }
        public string IsAircraftOnGround { get; set; }
    }
}
