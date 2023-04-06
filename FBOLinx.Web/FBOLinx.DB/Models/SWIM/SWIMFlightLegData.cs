using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.DB.Models
{
    public class SWIMFlightLegData : FBOLinxBaseEntityModel<long>
    {
        [Required]
        public DateTime ETA { get; set; }
        public double? ActualSpeed { get; set; }
        public double? Altitude { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime MessageTimestamp { get; set; } // PST Time
        
        public string RawXmlMessage { get; set; }

        public long SWIMFlightLegId { get; set; }
        public virtual SWIMFlightLeg SWIMFlightLeg { get; set; }

        public virtual ICollection<SWIMFlightLegDataError> SWIMFlightLegDataErrors { get; set; }
    }
}
