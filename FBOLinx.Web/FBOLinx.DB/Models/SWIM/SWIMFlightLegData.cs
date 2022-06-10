using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public class SWIMFlightLegData : FBOLinxBaseEntityModel<int>
    {
        [Required]
        public DateTime ETA { get; set; }
        public double? ActualSpeed { get; set; }
        public double? Altitude { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime MessageTimestamp { get; set; }

        public int SWIMFlightLegId { get; set; }
        public virtual SWIMFlightLegs SWIMFlightLeg { get; set; }
    }
}
