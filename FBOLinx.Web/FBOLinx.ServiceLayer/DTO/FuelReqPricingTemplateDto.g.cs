using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FuelReqPricingTemplateDto
    {
        public int Oid { get; set; }
        public int FuelReqId { get; set; }
        public int PricingTemplateId { get; set; }
        public string PricingTemplateName { get; set; }
        public string PricingTemplateRaw { get; set; }
        public FuelReqDto FuelReq { get; set; }
    }
}