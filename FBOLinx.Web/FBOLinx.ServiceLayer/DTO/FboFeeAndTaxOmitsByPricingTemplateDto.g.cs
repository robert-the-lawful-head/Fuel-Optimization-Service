using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FboFeeAndTaxOmitsByPricingTemplateDto
    {
        public int Oid { get; set; }
        public int FboFeeAndTaxId { get; set; }
        public int PricingTemplateId { get; set; }
        public FboFeesAndTaxesDto FboFeeAndTax { get; set; }
    }
}