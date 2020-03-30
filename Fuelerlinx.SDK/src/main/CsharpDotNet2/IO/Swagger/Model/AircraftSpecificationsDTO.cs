using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class AircraftSpecificationsDTO {
    /// <summary>
    /// Gets or Sets Oid
    /// </summary>
    [DataMember(Name="oid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "oid")]
    public int? Oid { get; set; }

    /// <summary>
    /// Gets or Sets AircraftId
    /// </summary>
    [DataMember(Name="aircraftId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aircraftId")]
    public int? AircraftId { get; set; }

    /// <summary>
    /// Gets or Sets ModelName
    /// </summary>
    [DataMember(Name="modelName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "modelName")]
    public string ModelName { get; set; }

    /// <summary>
    /// Gets or Sets FuselageDimensionsLengthFt
    /// </summary>
    [DataMember(Name="fuselageDimensionsLengthFt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuselageDimensionsLengthFt")]
    public double? FuselageDimensionsLengthFt { get; set; }

    /// <summary>
    /// Gets or Sets FuselageDimensionsHeightFt
    /// </summary>
    [DataMember(Name="fuselageDimensionsHeightFt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuselageDimensionsHeightFt")]
    public double? FuselageDimensionsHeightFt { get; set; }

    /// <summary>
    /// Gets or Sets FuselageDimensionsWingSpanFt
    /// </summary>
    [DataMember(Name="fuselageDimensionsWingSpanFt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuselageDimensionsWingSpanFt")]
    public double? FuselageDimensionsWingSpanFt { get; set; }

    /// <summary>
    /// Gets or Sets CabinDimensionsLengthFtInches
    /// </summary>
    [DataMember(Name="cabinDimensionsLengthFtInches", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cabinDimensionsLengthFtInches")]
    public string CabinDimensionsLengthFtInches { get; set; }

    /// <summary>
    /// Gets or Sets CabinDimensionsHeightFtInches
    /// </summary>
    [DataMember(Name="cabinDimensionsHeightFtInches", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cabinDimensionsHeightFtInches")]
    public string CabinDimensionsHeightFtInches { get; set; }

    /// <summary>
    /// Gets or Sets CabinDimensionsWidthFtInches
    /// </summary>
    [DataMember(Name="cabinDimensionsWidthFtInches", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cabinDimensionsWidthFtInches")]
    public string CabinDimensionsWidthFtInches { get; set; }

    /// <summary>
    /// Gets or Sets CrewConfiguration
    /// </summary>
    [DataMember(Name="crewConfiguration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "crewConfiguration")]
    public double? CrewConfiguration { get; set; }

    /// <summary>
    /// Gets or Sets PassengerConfiguration
    /// </summary>
    [DataMember(Name="passengerConfiguration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "passengerConfiguration")]
    public double? PassengerConfiguration { get; set; }

    /// <summary>
    /// Gets or Sets PressurizationPsi
    /// </summary>
    [DataMember(Name="pressurizationPsi", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pressurizationPsi")]
    public double? PressurizationPsi { get; set; }

    /// <summary>
    /// Gets or Sets FuelCapacityStandardLbs
    /// </summary>
    [DataMember(Name="fuelCapacityStandardLbs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelCapacityStandardLbs")]
    public double? FuelCapacityStandardLbs { get; set; }

    /// <summary>
    /// Gets or Sets FuelCapacityStandardGal
    /// </summary>
    [DataMember(Name="fuelCapacityStandardGal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelCapacityStandardGal")]
    public double? FuelCapacityStandardGal { get; set; }

    /// <summary>
    /// Gets or Sets FuelCapacityOptionalLbs
    /// </summary>
    [DataMember(Name="fuelCapacityOptionalLbs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelCapacityOptionalLbs")]
    public double? FuelCapacityOptionalLbs { get; set; }

    /// <summary>
    /// Gets or Sets FuelCapacityOptionalGal
    /// </summary>
    [DataMember(Name="fuelCapacityOptionalGal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelCapacityOptionalGal")]
    public double? FuelCapacityOptionalGal { get; set; }

    /// <summary>
    /// Gets or Sets MaxRampWeightLbs
    /// </summary>
    [DataMember(Name="maxRampWeightLbs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxRampWeightLbs")]
    public double? MaxRampWeightLbs { get; set; }

    /// <summary>
    /// Gets or Sets MaxTakeoffWeightLbs
    /// </summary>
    [DataMember(Name="maxTakeoffWeightLbs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxTakeoffWeightLbs")]
    public double? MaxTakeoffWeightLbs { get; set; }

    /// <summary>
    /// Gets or Sets ZeroFuelWeightLbs
    /// </summary>
    [DataMember(Name="zeroFuelWeightLbs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "zeroFuelWeightLbs")]
    public double? ZeroFuelWeightLbs { get; set; }

    /// <summary>
    /// Gets or Sets BasicOperatingWeightLbs
    /// </summary>
    [DataMember(Name="basicOperatingWeightLbs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "basicOperatingWeightLbs")]
    public double? BasicOperatingWeightLbs { get; set; }

    /// <summary>
    /// Gets or Sets MaxLandingWeightLbs
    /// </summary>
    [DataMember(Name="maxLandingWeightLbs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxLandingWeightLbs")]
    public double? MaxLandingWeightLbs { get; set; }

    /// <summary>
    /// Gets or Sets VsCleanSpeedKnots
    /// </summary>
    [DataMember(Name="vsCleanSpeedKnots", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "vsCleanSpeedKnots")]
    public double? VsCleanSpeedKnots { get; set; }

    /// <summary>
    /// Gets or Sets VsoLandingSpeedKnots
    /// </summary>
    [DataMember(Name="vsoLandingSpeedKnots", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "vsoLandingSpeedKnots")]
    public double? VsoLandingSpeedKnots { get; set; }

    /// <summary>
    /// Gets or Sets NormalCruiseTasSpeedKnots
    /// </summary>
    [DataMember(Name="normalCruiseTasSpeedKnots", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "normalCruiseTasSpeedKnots")]
    public double? NormalCruiseTasSpeedKnots { get; set; }

    /// <summary>
    /// Gets or Sets VmoMaxOpIasSpeedKnots
    /// </summary>
    [DataMember(Name="vmoMaxOpIasSpeedKnots", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "vmoMaxOpIasSpeedKnots")]
    public double? VmoMaxOpIasSpeedKnots { get; set; }

    /// <summary>
    /// Gets or Sets NormalClimbFpm
    /// </summary>
    [DataMember(Name="normalClimbFpm", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "normalClimbFpm")]
    public double? NormalClimbFpm { get; set; }

    /// <summary>
    /// Gets or Sets EngineOutClimbFpm
    /// </summary>
    [DataMember(Name="engineOutClimbFpm", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "engineOutClimbFpm")]
    public double? EngineOutClimbFpm { get; set; }

    /// <summary>
    /// Gets or Sets CeilingFt
    /// </summary>
    [DataMember(Name="ceilingFt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ceilingFt")]
    public double? CeilingFt { get; set; }

    /// <summary>
    /// Gets or Sets LandingPerfFaaFieldLengthFt
    /// </summary>
    [DataMember(Name="landingPerfFaaFieldLengthFt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "landingPerfFaaFieldLengthFt")]
    public double? LandingPerfFaaFieldLengthFt { get; set; }

    /// <summary>
    /// Gets or Sets TakeoffPerfSlIsaBfl
    /// </summary>
    [DataMember(Name="takeoffPerfSlIsaBfl", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "takeoffPerfSlIsaBfl")]
    public double? TakeoffPerfSlIsaBfl { get; set; }

    /// <summary>
    /// Gets or Sets TakeoffPerf500020cBfl
    /// </summary>
    [DataMember(Name="takeoffPerf500020cBfl", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "takeoffPerf500020cBfl")]
    public double? TakeoffPerf500020cBfl { get; set; }

    /// <summary>
    /// Gets or Sets RangeNm
    /// </summary>
    [DataMember(Name="rangeNm", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rangeNm")]
    public double? RangeNm { get; set; }

    /// <summary>
    /// Gets or Sets NumberOfEngines
    /// </summary>
    [DataMember(Name="numberOfEngines", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "numberOfEngines")]
    public double? NumberOfEngines { get; set; }

    /// <summary>
    /// Gets or Sets EngineModelS
    /// </summary>
    [DataMember(Name="engineModelS", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "engineModelS")]
    public string EngineModelS { get; set; }

    /// <summary>
    /// Gets or Sets EngineThrustLbsPerEngine
    /// </summary>
    [DataMember(Name="engineThrustLbsPerEngine", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "engineThrustLbsPerEngine")]
    public double? EngineThrustLbsPerEngine { get; set; }

    /// <summary>
    /// Gets or Sets EngineShaftHpPerEngine
    /// </summary>
    [DataMember(Name="engineShaftHpPerEngine", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "engineShaftHpPerEngine")]
    public double? EngineShaftHpPerEngine { get; set; }

    /// <summary>
    /// Gets or Sets EngineCommonTboHours
    /// </summary>
    [DataMember(Name="engineCommonTboHours", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "engineCommonTboHours")]
    public double? EngineCommonTboHours { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AircraftSpecificationsDTO {\n");
      sb.Append("  Oid: ").Append(Oid).Append("\n");
      sb.Append("  AircraftId: ").Append(AircraftId).Append("\n");
      sb.Append("  ModelName: ").Append(ModelName).Append("\n");
      sb.Append("  FuselageDimensionsLengthFt: ").Append(FuselageDimensionsLengthFt).Append("\n");
      sb.Append("  FuselageDimensionsHeightFt: ").Append(FuselageDimensionsHeightFt).Append("\n");
      sb.Append("  FuselageDimensionsWingSpanFt: ").Append(FuselageDimensionsWingSpanFt).Append("\n");
      sb.Append("  CabinDimensionsLengthFtInches: ").Append(CabinDimensionsLengthFtInches).Append("\n");
      sb.Append("  CabinDimensionsHeightFtInches: ").Append(CabinDimensionsHeightFtInches).Append("\n");
      sb.Append("  CabinDimensionsWidthFtInches: ").Append(CabinDimensionsWidthFtInches).Append("\n");
      sb.Append("  CrewConfiguration: ").Append(CrewConfiguration).Append("\n");
      sb.Append("  PassengerConfiguration: ").Append(PassengerConfiguration).Append("\n");
      sb.Append("  PressurizationPsi: ").Append(PressurizationPsi).Append("\n");
      sb.Append("  FuelCapacityStandardLbs: ").Append(FuelCapacityStandardLbs).Append("\n");
      sb.Append("  FuelCapacityStandardGal: ").Append(FuelCapacityStandardGal).Append("\n");
      sb.Append("  FuelCapacityOptionalLbs: ").Append(FuelCapacityOptionalLbs).Append("\n");
      sb.Append("  FuelCapacityOptionalGal: ").Append(FuelCapacityOptionalGal).Append("\n");
      sb.Append("  MaxRampWeightLbs: ").Append(MaxRampWeightLbs).Append("\n");
      sb.Append("  MaxTakeoffWeightLbs: ").Append(MaxTakeoffWeightLbs).Append("\n");
      sb.Append("  ZeroFuelWeightLbs: ").Append(ZeroFuelWeightLbs).Append("\n");
      sb.Append("  BasicOperatingWeightLbs: ").Append(BasicOperatingWeightLbs).Append("\n");
      sb.Append("  MaxLandingWeightLbs: ").Append(MaxLandingWeightLbs).Append("\n");
      sb.Append("  VsCleanSpeedKnots: ").Append(VsCleanSpeedKnots).Append("\n");
      sb.Append("  VsoLandingSpeedKnots: ").Append(VsoLandingSpeedKnots).Append("\n");
      sb.Append("  NormalCruiseTasSpeedKnots: ").Append(NormalCruiseTasSpeedKnots).Append("\n");
      sb.Append("  VmoMaxOpIasSpeedKnots: ").Append(VmoMaxOpIasSpeedKnots).Append("\n");
      sb.Append("  NormalClimbFpm: ").Append(NormalClimbFpm).Append("\n");
      sb.Append("  EngineOutClimbFpm: ").Append(EngineOutClimbFpm).Append("\n");
      sb.Append("  CeilingFt: ").Append(CeilingFt).Append("\n");
      sb.Append("  LandingPerfFaaFieldLengthFt: ").Append(LandingPerfFaaFieldLengthFt).Append("\n");
      sb.Append("  TakeoffPerfSlIsaBfl: ").Append(TakeoffPerfSlIsaBfl).Append("\n");
      sb.Append("  TakeoffPerf500020cBfl: ").Append(TakeoffPerf500020cBfl).Append("\n");
      sb.Append("  RangeNm: ").Append(RangeNm).Append("\n");
      sb.Append("  NumberOfEngines: ").Append(NumberOfEngines).Append("\n");
      sb.Append("  EngineModelS: ").Append(EngineModelS).Append("\n");
      sb.Append("  EngineThrustLbsPerEngine: ").Append(EngineThrustLbsPerEngine).Append("\n");
      sb.Append("  EngineShaftHpPerEngine: ").Append(EngineShaftHpPerEngine).Append("\n");
      sb.Append("  EngineCommonTboHours: ").Append(EngineCommonTboHours).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
