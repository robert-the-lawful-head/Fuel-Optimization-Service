namespace FBOLinx.Core.Constants
{
    public static class FlightWatchConstants
    {
        public static class CoordinatesSource
        {
            public static readonly string Antenna = "Antenna";
            public static readonly string Swim = "SWIM";
            public static readonly string None = "None";
        }
        public static class PositionDateTimeSource
        {
            public static readonly string CreatedDateTime = "CreatedDateTime";
            public static readonly string AircraftPositionDateTimeUtc = "AircraftPositionDateTime";
            public static readonly string BoxTransmissionDateTimeUtc = "BoxTransmissionDateTime";
            public static readonly string SwimLastUpdate = "SwimLastUpdate";
        }
        public static class TransactionStatus
        {
            public static readonly string Live = "LIVE";
        }
        
    }
}
