using System.Collections.Generic;
using FBOLinx.Core.Enums;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class PricingTemplateDto
    {
        public int Oid { get; set; }
        public string Name { get; set; }
        public int Fboid { get; set; }
        public int? CustomerId { get; set; }
        public bool? Default { get; set; }
        public string Notes { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public short? Type { get; set; }
        public DiscountTypes? DiscountType { get; set; }
        public MarginTypes? MarginType { get; set; }
        public int? EmailContentId { get; set; }
        public List<string> TailNumbers { get; set; }
        public string MarginTypeProduct { get; set; }
        public ICollection<CustomerMarginsDto> CustomerMargins { get; set; }
        public ICollection<AircraftPricesDto> AircraftPrices { get; set; }
        public FbosDto Fbo { get; set; }
        public ICollection<CustomCustomerTypesDto> CustomCustomerTypes { get; set; }
    }
}