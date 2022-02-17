using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class DefaultDiscountDto
    {
        public int Oid { get; set; }
        public double? Margin { get; set; }
        public short? MarginType { get; set; }
        public int Fboid { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string Notes { get; set; }
    }
}