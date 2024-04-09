using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class PriceTiersDto
    {
        public int Oid { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public double? MaxEntered { get; set; }
        public CustomerMarginsDto CustomerMargin { get; set; }
    }
}