using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class CustomCustomerTypesDto
    {
        public int CustomerId { get; set; }
        public int Fboid { get; set; }
        public int CustomerType { get; set; }
        public int Oid { get; set; }
        public CustomersDto Customer { get; set; }
        public PricingTemplateDto PricingTemplate { get; set; }
    }
}