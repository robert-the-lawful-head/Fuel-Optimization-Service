using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    [Table("airCrafts")]
    public partial class AirCrafts
    {
        public enum AircraftSizes: short
        {
            [Description("Not Set")]
            NotSet = 0,
            [Description("Light Jet")]
            LightJet = 1,
            [Description("Medium Jet")]
            MediumJet = 2,
            [Description("Heavy Jet")]
            HeavyJet = 3,
            [Description("Light Helicopter")]
            LightHelicopter = 4,
            [Description("Wide Body")]
            WideBody = 5,
            [Description("Single Turboprop")]
            SingleTurboProp = 6,
            [Description("Very Light Jet")]
            VeryLightJet = 7,
            [Description("Single Engine Piston")]
            SingleEnginePiston = 8,
            [Description("Medium Helicopter")]
            MediumHelicopter = 9,
            [Description("Heavy Helicopter")]
            HeavyHelicopter = 10,
            [Description("Light Twin")]
            LightTwin = 11,
            [Description("Heavy Twin")]
            HeavyTwin = 12,
            [Description("Light Turboprop")]
            LightTurboProp = 13,
            [Description("Medium Turboprop")]
            MediumTurboprop = 14,
            [Description("Heavy Turboprop")]
            HeavyTurboprop = 15,
            [Description("Super Heavy Jet")]
            SuperHeavyJet = 16
        }

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
    }
}
