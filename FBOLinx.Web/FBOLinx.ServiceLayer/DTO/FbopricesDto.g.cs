using System;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FbopricesDto
    {
        public int Oid { get; set; }
        public int? Fboid { get; set; }
        public string Product { get; set; }
        public double? Price { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? Timestamp { get; set; }
        public double? SalesTax { get; set; }
        public string Currency { get; set; }
        public bool? Expired { get; set; }
        public FbosDto Fbo { get; set; }
    }
}