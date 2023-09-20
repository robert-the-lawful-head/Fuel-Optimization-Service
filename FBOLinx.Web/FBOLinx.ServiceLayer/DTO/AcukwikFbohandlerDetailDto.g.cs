using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class AcukwikFbohandlerDetailDto
    {
        public int? AirportId { get; set; }
        public int HandlerId { get; set; }
        public string HandlerLongName { get; set; }
        public string HandlerType { get; set; }
        public string HandlerTelephone { get; set; }
        public string HandlerFax { get; set; }
        public string HandlerTollFree { get; set; }
        public double? HandlerFreq { get; set; }
        public string HandlerFuelBrand { get; set; }
        public string HandlerFuelBrand2 { get; set; }
        public string HandlerFuelSupply { get; set; }
        public string HandlerLocationOnField { get; set; }
        public string MultiService { get; set; }
        public string Avcard { get; set; }
        public double? AcukwikId { get; set; }
        public AcukwikAirportDTO AcukwikAirport { get; set; }
        public string Email { get; set; }
    }
}