using System.Collections.Generic;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class AirCraftsDto
    {
        public int AircraftId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public double? NormalCruiseTas { get; set; }
        public double? FuelCapacityGal { get; set; }
        public double? LandingPerfLength { get; set; }
        public double? RangeNm { get; set; }
        public double? RangePerGal { get; set; }
        public double? MaxRangeHours { get; set; }
        public double? MaxRangeMinutes { get; set; }
        public double? ReserveMinutes { get; set; }
        public double? ReserveNm { get; set; }
        public AircraftSizes? Size { get; set; }
        public double? MaxTakeoffWeight { get; set; }
        public double? MaxLandingWeight { get; set; }
        public double? AircraftCeiling { get; set; }
        public double? TakeoffPerfSl { get; set; }
        public double? TakeoffPerf { get; set; }
        public double? ZeroFuelWeight { get; set; }
        public double? BasicOperatingWeight { get; set; }
        public string FuelType { get; set; }
        public short? TempSize { get; set; }
        public ICollection<AFSAircraftDto> AFSAircraft { get; set; }
        public AircraftSpecificationsDto AircraftSpecifications { get; set; }
        public FboFavoriteAircraft? FavoriteAircraft { get; set; }
    }
}