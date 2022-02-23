using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class AircraftSpecificationsDto
    {
        public int Oid { get; set; }
        public int? AircraftId { get; set; }
        public string ModelName { get; set; }
        public double? FuselageDimensionsLengthFt { get; set; }
        public double? FuselageDimensionsHeightFt { get; set; }
        public double? FuselageDimensionsWingSpanFt { get; set; }
        public string CabinDimensionsLengthFtInches { get; set; }
        public string CabinDimensionsHeightFtInches { get; set; }
        public string CabinDimensionsWidthFtInches { get; set; }
        public double? CrewConfiguration { get; set; }
        public double? PassengerConfiguration { get; set; }
        public double? PressurizationPsi { get; set; }
        public double? FuelCapacityStandardLbs { get; set; }
        public double? FuelCapacityStandardGal { get; set; }
        public double? FuelCapacityOptionalLbs { get; set; }
        public double? FuelCapacityOptionalGal { get; set; }
        public double? MaxRampWeightLbs { get; set; }
        public double? MaxTakeoffWeightLbs { get; set; }
        public double? ZeroFuelWeightLbs { get; set; }
        public double? BasicOperatingWeightLbs { get; set; }
        public double? MaxLandingWeightLbs { get; set; }
        public double? VsCleanSpeedKnots { get; set; }
        public double? VsoLandingSpeedKnots { get; set; }
        public double? NormalCruiseTasSpeedKnots { get; set; }
        public double? VmoMaxOpIasSpeedKnots { get; set; }
        public double? NormalClimbFpm { get; set; }
        public double? EngineOutClimbFpm { get; set; }
        public double? CeilingFt { get; set; }
        public double? LandingPerfFaaFieldLengthFt { get; set; }
        public double? TakeoffPerfSlIsaBfl { get; set; }
        public double? TakeoffPerf500020cBfl { get; set; }
        public double? RangeNm { get; set; }
        public double? NumberOfEngines { get; set; }
        public string EngineModelS { get; set; }
        public double? EngineThrustLbsPerEngine { get; set; }
        public double? EngineShaftHpPerEngine { get; set; }
        public double? EngineCommonTboHours { get; set; }
        public AirCraftsDto AirCrafts { get; set; }
    }
}