using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class CustomerMarginsDto
    {
        public int PriceTierId { get; set; }
        public int TemplateId { get; set; }
        public double? Amount { get; set; }
        public int Oid { get; set; }
        public PriceTiersDto PriceTier { get; set; }
        public PricingTemplateDto PricingTemplate { get; set; }
    }
}