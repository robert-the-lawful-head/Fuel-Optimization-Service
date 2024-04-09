using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FbocustomerPricingDto
    {
        public int CustomerId { get; set; }
        public int Fboid { get; set; }
        public DateTime? LastUpdated { get; set; }
        public double? PriceMargin { get; set; }
        public int Oid { get; set; }
    }
}