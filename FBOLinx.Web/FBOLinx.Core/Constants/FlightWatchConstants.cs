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
            public  const string CreatedDateTime = "CreatedDateTime";
            public  const string AircraftPositionDateTimeUtc = "AircraftPositionDateTime";
            public  const string BoxTransmissionDateTimeUtc = "BoxTransmissionDateTime";
            public  const string SwimLastUpdate = "SwimLastUpdate";
        }
        public static class TransactionStatus
        {
            public static readonly string Live = "LIVE";
        }
        
    }
}
