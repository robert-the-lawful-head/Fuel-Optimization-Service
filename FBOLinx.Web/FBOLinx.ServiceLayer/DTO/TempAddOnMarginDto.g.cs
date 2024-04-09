using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class TempAddOnMarginDto
    {
        public int Id { get; set; }
        public int FboId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public double? MarginJet { get; set; }
        public double? MarginAvgas { get; set; }
    }
}