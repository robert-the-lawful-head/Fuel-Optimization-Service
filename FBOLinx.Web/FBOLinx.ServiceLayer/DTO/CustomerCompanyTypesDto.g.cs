namespace FBOLinx.Service.Mapping.Dto
{
    public partial class CustomerCompanyTypesDto
    {
        public int Oid { get; set; }
        public string Name { get; set; }
        public int Fboid { get; set; }
        public int GroupId { get; set; }
        public bool? AllowMultiplePricingTemplates { get; set; }
    }
}