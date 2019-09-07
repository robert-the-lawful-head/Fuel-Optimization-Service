using System;
using System.Collections.Generic;

namespace FBOLinx.Web.Models
{
    public partial class TempAddOnMargin
    {
        public int Id { get; set; }
        public int FboId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public decimal? MarginJet { get; set; }
        public decimal? MarginAvgas { get; set; }
    }
}
