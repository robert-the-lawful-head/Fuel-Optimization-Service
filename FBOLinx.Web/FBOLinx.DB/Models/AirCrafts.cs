using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FBOLinx.Core.Enums;

namespace FBOLinx.DB.Models
{
    [Table("airCrafts")]
    public partial class AirCrafts
    {
        [Key]
        [Column("AircraftID")]
        public int AircraftId { get; set; }
        [Column("MAKE")]
        [StringLength(100)]
        public string Make { get; set; }
        [Column("MODEL")]
        [StringLength(100)]
        public string Model { get; set; }
        [Column("NORMAL CRUISE TAS")]
        public double? NormalCruiseTas { get; set; }
        [Column("FUEL CAPACITY\u00A0(gal)")]
        public double? FuelCapacityGal { get; set; }
        [Column("LANDING PERF LENGTH")]
        public double? LandingPerfLength { get; set; }
        [Column("RANGE (nm)")]
        public double? RangeNm { get; set; }
        public double? RangePerGal { get; set; }
        public double? MaxRangeHours { get; set; }
        public double? MaxRangeMinutes { get; set; }
        public double? ReserveMinutes { get; set; }
        [Column("ReserveNM")]
        public double? ReserveNm { get; set; }
        public AircraftSizes? Size { get; set; }
        public double? MaxTakeoffWeight { get; set; }
        public double? MaxLandingWeight { get; set; }
        public double? AircraftCeiling { get; set; }
        [Column("TakeoffPerfSL")]
        public double? TakeoffPerfSl { get; set; }
        public double? TakeoffPerf { get; set; }
        public double? ZeroFuelWeight { get; set; }
        public double? BasicOperatingWeight { get; set; }
        [StringLength(25)]
        public string FuelType { get; set; }
        public short? TempSize { get; set; }

        [InverseProperty("AirCrafts")]
        public ICollection<AFSAircraft> AFSAircraft { get; set; }

        [InverseProperty("AirCrafts")]
        public AircraftSpecifications AircraftSpecifications { get; set; }
    }
}
