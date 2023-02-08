using System;
using System.ComponentModel;

namespace FBOLinx.ServiceLayer.DTO.Requests
{
    public enum TimeStandards : short
    {
        /// <summary>
        /// Not Set
        /// </summary>
        [Description("NotSet")]
        NotSet = 0,
        /// <summary>
        /// Zulu
        /// </summary>
        [Description("Zulu")]
        Zulu = 1,
        /// <summary>
        /// Local
        /// </summary>
        [Description("Local")]
        Local = 2
    }

    public class PricingUpdateRequest
    {
        public double? Retail { get; set; }
        public double? Cost { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public TimeStandards TimeStandard { get; set; } = 0;
    }
}
