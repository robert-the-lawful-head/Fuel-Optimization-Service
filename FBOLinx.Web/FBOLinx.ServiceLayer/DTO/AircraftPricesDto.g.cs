using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class AircraftPricesDto
    {
        public int Oid { get; set; }
        public int CustomerAircraftId { get; set; }
        public int? PriceTemplateId { get; set; }
        public bool? CustomTemplate { get; set; }
        public CustomerAircraftsDto CustomerAircraft { get; set; }
        public PricingTemplateDto PricingTemplate { get; set; }
    }
}