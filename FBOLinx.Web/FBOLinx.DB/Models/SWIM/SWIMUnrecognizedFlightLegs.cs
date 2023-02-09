using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FBOLinx.Core.BaseModels.Entities;
using FBOLinx.Core.Enums;

namespace FBOLinx.DB.Models
{
    [Table("SWIMUnrecognizedFlightLegs")]
    public class SWIMUnrecognizedFlightLeg : FBOLinxBaseEntityModel<int>
    {
        [Column(TypeName = "varchar(50)")]
        public string Gufi { get; set; }

        [Required]
        [StringLength(10)]
        public string AircraftIdentification { get; set; }

        [Required]
        public DateTime MessageTimestamp { get; set; } // PST Time
        
        public string DeparturePoint { get; set; }
        
        public string ArrivalPoint { get; set; }
        
        public DateTime? ATD { get; set; }
        
        public DateTime? ETA { get; set; }

        public double? ActualSpeed { get; set; }
        public double? Altitude { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        
        public string XmlMessage { get; set; }
    }
}
