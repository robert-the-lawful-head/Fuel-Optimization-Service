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
  public class AircraftDTO {
    /// <summary>
    /// Gets or Sets AircraftId
    /// </summary>
    [DataMember(Name="aircraftId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aircraftId")]
    public int? AircraftId { get; set; }

    /// <summary>
    /// Gets or Sets Make
    /// </summary>
    [DataMember(Name="make", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "make")]
    public string Make { get; set; }

    /// <summary>
    /// Gets or Sets Model
    /// </summary>
    [DataMember(Name="model", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "model")]
    public string Model { get; set; }

    /// <summary>
    /// Gets or Sets NormalCruiseTas
    /// </summary>
    [DataMember(Name="normalCruiseTas", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "normalCruiseTas")]
    public Speed NormalCruiseTas { get; set; }

    /// <summary>
    /// Gets or Sets FuelCapacity
    /// </summary>
    [DataMember(Name="fuelCapacity", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelCapacity")]
    public Weight FuelCapacity { get; set; }

    /// <summary>
    /// Gets or Sets MinLandingRunwayLength
    /// </summary>
    [DataMember(Name="minLandingRunwayLength", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minLandingRunwayLength")]
    public Distance MinLandingRunwayLength { get; set; }

    /// <summary>
    /// Gets or Sets RangeNm
    /// </summary>
    [DataMember(Name="rangeNm", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rangeNm")]
    public double? RangeNm { get; set; }

    /// <summary>
    /// Gets or Sets RangePerGal
    /// </summary>
    [DataMember(Name="rangePerGal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rangePerGal")]
    public double? RangePerGal { get; set; }

    /// <summary>
    /// Gets or Sets MaxRangeHours
    /// </summary>
    [DataMember(Name="maxRangeHours", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxRangeHours")]
    public double? MaxRangeHours { get; set; }

    /// <summary>
    /// Gets or Sets MaxRangeMinutes
    /// </summary>
    [DataMember(Name="maxRangeMinutes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxRangeMinutes")]
    public double? MaxRangeMinutes { get; set; }

    /// <summary>
    /// Gets or Sets ReserveMinutes
    /// </summary>
    [DataMember(Name="reserveMinutes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reserveMinutes")]
    public double? ReserveMinutes { get; set; }

    /// <summary>
    /// Gets or Sets ReserveNm
    /// </summary>
    [DataMember(Name="reserveNm", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reserveNm")]
    public double? ReserveNm { get; set; }

    /// <summary>
    /// Aircraft sizes:             0 = Not set             1 = Light Jet             2 = Medium Jet             3 = Heavy Jet             4 = Light Helicopter             5 = Wide Body             6 = Single Turbo Prop             7 = Very Light Jet             8 = Single Engine Piston             9 = Medium Helicopter             10 = Heavy Helicopter             11 = Light Twin             12 = Heavy Twin             13 = Light Turbo Prop             14 = Medium Turbo Prop             15 = Heavy Turbo Prop             16 = Super Heavy Jet    * `NotSet` - Not Set  * `LightJet` - Light Jet  * `MediumJet` - Medium Jet  * `HeavyJet` - Heavy Jet  * `LightHelicopter` - Light Helicopter  * `WideBody` - Wide Body  * `SingleTurboProp` - Single Turbo Prop  * `VeryLightJet` - Very Light Jet  * `SingleEnginePiston` - Single Engine Piston  * `MediumHelicopter` - Medium Helicopter  * `HeavyHelicopter` - Heavy Helicopter  * `LightTwin` - Light Twin  * `HeavyTwin` - Heavy Twin  * `LightTurboProp` - Light Turbo Prop  * `MediumTurboprop` - Medium Turbo Prop  * `HeavyTurboprop` - Heavy Turbo Prop  * `SuperHeavyJet` - Super Heavy Jet  
    /// </summary>
    /// <value>Aircraft sizes:             0 = Not set             1 = Light Jet             2 = Medium Jet             3 = Heavy Jet             4 = Light Helicopter             5 = Wide Body             6 = Single Turbo Prop             7 = Very Light Jet             8 = Single Engine Piston             9 = Medium Helicopter             10 = Heavy Helicopter             11 = Light Twin             12 = Heavy Twin             13 = Light Turbo Prop             14 = Medium Turbo Prop             15 = Heavy Turbo Prop             16 = Super Heavy Jet    * `NotSet` - Not Set  * `LightJet` - Light Jet  * `MediumJet` - Medium Jet  * `HeavyJet` - Heavy Jet  * `LightHelicopter` - Light Helicopter  * `WideBody` - Wide Body  * `SingleTurboProp` - Single Turbo Prop  * `VeryLightJet` - Very Light Jet  * `SingleEnginePiston` - Single Engine Piston  * `MediumHelicopter` - Medium Helicopter  * `HeavyHelicopter` - Heavy Helicopter  * `LightTwin` - Light Twin  * `HeavyTwin` - Heavy Twin  * `LightTurboProp` - Light Turbo Prop  * `MediumTurboprop` - Medium Turbo Prop  * `HeavyTurboprop` - Heavy Turbo Prop  * `SuperHeavyJet` - Super Heavy Jet  </value>
    [DataMember(Name="size", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "size")]
    public int? Size { get; set; }

    /// <summary>
    /// Gets or Sets MaxTakeoffWeight
    /// </summary>
    [DataMember(Name="maxTakeoffWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxTakeoffWeight")]
    public Weight MaxTakeoffWeight { get; set; }

    /// <summary>
    /// Gets or Sets MaxLandingWeight
    /// </summary>
    [DataMember(Name="maxLandingWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxLandingWeight")]
    public Weight MaxLandingWeight { get; set; }

    /// <summary>
    /// Gets or Sets AircraftCeiling
    /// </summary>
    [DataMember(Name="aircraftCeiling", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aircraftCeiling")]
    public Distance AircraftCeiling { get; set; }

    /// <summary>
    /// Gets or Sets MinTakeoffRunwayLengthAtSeaLevel
    /// </summary>
    [DataMember(Name="minTakeoffRunwayLengthAtSeaLevel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minTakeoffRunwayLengthAtSeaLevel")]
    public Distance MinTakeoffRunwayLengthAtSeaLevel { get; set; }

    /// <summary>
    /// Gets or Sets MinTakeoffRunwayLength
    /// </summary>
    [DataMember(Name="minTakeoffRunwayLength", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minTakeoffRunwayLength")]
    public Distance MinTakeoffRunwayLength { get; set; }

    /// <summary>
    /// Gets or Sets ZeroFuelWeight
    /// </summary>
    [DataMember(Name="zeroFuelWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "zeroFuelWeight")]
    public Weight ZeroFuelWeight { get; set; }

    /// <summary>
    /// Gets or Sets BasicOperatingWeight
    /// </summary>
    [DataMember(Name="basicOperatingWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "basicOperatingWeight")]
    public Weight BasicOperatingWeight { get; set; }

    /// <summary>
    /// Gets or Sets FuelType
    /// </summary>
    [DataMember(Name="fuelType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelType")]
    public string FuelType { get; set; }

    /// <summary>
    /// Gets or Sets Ifrreseve
    /// </summary>
    [DataMember(Name="ifrreseve", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ifrreseve")]
    public Weight Ifrreseve { get; set; }

    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets AdditionalSpecifications
    /// </summary>
    [DataMember(Name="additionalSpecifications", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "additionalSpecifications")]
    public AircraftSpecificationsDTO AdditionalSpecifications { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AircraftDTO {\n");
      sb.Append("  AircraftId: ").Append(AircraftId).Append("\n");
      sb.Append("  Make: ").Append(Make).Append("\n");
      sb.Append("  Model: ").Append(Model).Append("\n");
      sb.Append("  NormalCruiseTas: ").Append(NormalCruiseTas).Append("\n");
      sb.Append("  FuelCapacity: ").Append(FuelCapacity).Append("\n");
      sb.Append("  MinLandingRunwayLength: ").Append(MinLandingRunwayLength).Append("\n");
      sb.Append("  RangeNm: ").Append(RangeNm).Append("\n");
      sb.Append("  RangePerGal: ").Append(RangePerGal).Append("\n");
      sb.Append("  MaxRangeHours: ").Append(MaxRangeHours).Append("\n");
      sb.Append("  MaxRangeMinutes: ").Append(MaxRangeMinutes).Append("\n");
      sb.Append("  ReserveMinutes: ").Append(ReserveMinutes).Append("\n");
      sb.Append("  ReserveNm: ").Append(ReserveNm).Append("\n");
      sb.Append("  Size: ").Append(Size).Append("\n");
      sb.Append("  MaxTakeoffWeight: ").Append(MaxTakeoffWeight).Append("\n");
      sb.Append("  MaxLandingWeight: ").Append(MaxLandingWeight).Append("\n");
      sb.Append("  AircraftCeiling: ").Append(AircraftCeiling).Append("\n");
      sb.Append("  MinTakeoffRunwayLengthAtSeaLevel: ").Append(MinTakeoffRunwayLengthAtSeaLevel).Append("\n");
      sb.Append("  MinTakeoffRunwayLength: ").Append(MinTakeoffRunwayLength).Append("\n");
      sb.Append("  ZeroFuelWeight: ").Append(ZeroFuelWeight).Append("\n");
      sb.Append("  BasicOperatingWeight: ").Append(BasicOperatingWeight).Append("\n");
      sb.Append("  FuelType: ").Append(FuelType).Append("\n");
      sb.Append("  Ifrreseve: ").Append(Ifrreseve).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  AdditionalSpecifications: ").Append(AdditionalSpecifications).Append("\n");
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
