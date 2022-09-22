using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FBOLinx.Core.BaseModels.Entities;
using FBOLinx.Core.Enums;

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
        public string DepartureCity { get; set; }

        //[Required]
        [StringLength(4)]
        public string ArrivalICAO { get; set; }
        public string ArrivalCity { get; set; }

        [Required]
        public DateTime ATD { get; set; }
        public DateTime? ATDLocal { get; set; }
        
        //[Required]
        public DateTime? ETA { get; set; }
        public DateTime? ETALocal { get; set; }

        public bool IsPlaceholder { get; set; }
        public bool IsAircraftOnGround { get; set; }
        public FlightLegStatus? Status { get; set; }
        public double? ActualSpeed { get; set; }
        public double? Altitude { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string FlightDepartment { get; set; }
        public string Make { get; set; }
        public string FAAMake { get; set; }
        public string Model { get; set; }
        public string FAAModel { get; set; }
        public double? FuelCapacityGal { get; set; }
        public string Phone { get; set; }
        public string ICAOAircraftCode { get; set; }
        public bool IsProcessed { get; set; }

        public virtual ICollection<SWIMFlightLegData> SWIMFlightLegDataMessages { get; set; }
    }
}
