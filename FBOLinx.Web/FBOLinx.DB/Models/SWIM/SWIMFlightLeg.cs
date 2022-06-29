using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FBOLinx.Core.BaseModels.Entities;

namespace FBOLinx.DB.Models
{
    [Table("SWIMFlightLegs")]
    public class SWIMFlightLeg : FBOLinxBaseEntityModel<int>
    {
        [Required]
        [StringLength(10)]
        public string AircraftIdentification { get; set; }

        [Required]
        [StringLength(4)]
        public string DepartureICAO { get; set; }

        [Required]
        [StringLength(4)]
        public string ArrivalICAO { get; set; }

        [Required]
        public DateTime ATD { get; set; }

        [Required]
        public DateTime ETA { get; set; }

        public virtual ICollection<SWIMFlightLegData> SWIMFlightLegDataMessages { get; set; }
    }
}
