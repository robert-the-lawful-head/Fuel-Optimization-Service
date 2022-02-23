using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class VolumeScaleDiscountDto
    {
        public int Oid { get; set; }
        public int? CustomerId { get; set; }
        public int Fboid { get; set; }
        public double? Margin { get; set; }
        public short? MarginType { get; set; }
        public DateTime? LastUpdated { get; set; }
        public double? Margin100Ll { get; set; }
        public short? MarginType100Ll { get; set; }
        public int? TemplateId { get; set; }
        public short? JetAvolumeDiscount { get; set; }
        public bool? GroupMargin { get; set; }
        public bool? DefaultSettings { get; set; }
        public bool? DefaultSettings100Ll { get; set; }
    }
}