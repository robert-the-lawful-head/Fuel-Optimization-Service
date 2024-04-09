namespace FBOLinx.ServiceLayer.Dto.Responses
{
    public class AircraftWatchLiveData
    {
        public int? CustomerInfoByGroupId { get; set; }
        public string TailNumber { get; set; }
        public string AtcFlightNumber { get; set; }
        public string AircraftTypeCode { get; set; }
        public bool IsAircraftOnGround { get; set; }
        public string Company { get; set; }
        public string AircraftMakeModel { get; set; }
        public string LastQuote { get; set; }
        public string CurrentPricing { get; set; }
        public string AircraftICAO { get; set; }
    }
}

