using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    public partial class FuelReq
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [Column("CustomerID")]
        public int? CustomerId { get; set; }
        [Column("ICAO")]
        [StringLength(4)]
        public string Icao { get; set; }
        [Column("FBOID")]
        public int? Fboid { get; set; }
        [Column("CustomerAircraftID")]
        public int? CustomerAircraftId { get; set; }
        [Column("ETA", TypeName = "datetime")]
        public DateTime? Eta { get; set; }
        [Column("ETD", TypeName = "datetime")]
        public DateTime? Etd { get; set; }
        [StringLength(1)]
        public string TimeStandard { get; set; }
        public bool? Cancelled { get; set; }
        public double? QuotedVolume { get; set; }
        [Column("QuotedPPG")]
        public double? QuotedPpg { get; set; }
        public string Notes { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        public double? ActualVolume { get; set; }
        [Column("ActualPPG")]
        public double? ActualPpg { get; set; }
        [StringLength(10)]
        public string Source { get; set; }
        [Column("SourceID")]
        public int? SourceId { get; set; }
        public string DispatchNotes { get; set; }
        public bool? Archived { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("FuelReqs")]
        public Customers Customer { get; set; }

        [ForeignKey("CustomerAircraftId")]
        [InverseProperty("FuelReqs")]
        public CustomerAircrafts CustomerAircraft { get; set; }

        [ForeignKey("Fboid")]
        [InverseProperty("FuelReqs")]
        public Fbos Fbo { get; set; }
    }
}
