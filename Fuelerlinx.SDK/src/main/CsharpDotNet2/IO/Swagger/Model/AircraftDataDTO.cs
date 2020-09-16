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
  public class AircraftDataDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets TailNumber
    /// </summary>
    [DataMember(Name="tailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumber")]
    public string TailNumber { get; set; }

    /// <summary>
    /// Gets or Sets MinLandingRunwayLength
    /// </summary>
    [DataMember(Name="minLandingRunwayLength", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minLandingRunwayLength")]
    public Distance MinLandingRunwayLength { get; set; }

    /// <summary>
    /// Gets or Sets MaintCostPerHour
    /// </summary>
    [DataMember(Name="maintCostPerHour", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maintCostPerHour")]
    public double? MaintCostPerHour { get; set; }

    /// <summary>
    /// Gets or Sets MaxRange
    /// </summary>
    [DataMember(Name="maxRange", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxRange")]
    public Distance MaxRange { get; set; }

    /// <summary>
    /// Aircraft sizes:             0 = Not set             1 = Light Jet             2 = Medium Jet             3 = Heavy Jet             4 = Light Helicopter             5 = Wide Body             6 = Single Turbo Prop             7 = Very Light Jet             8 = Single Engine Piston             9 = Medium Helicopter             10 = Heavy Helicopter             11 = Light Twin             12 = Heavy Twin             13 = Light Turbo Prop             14 = Medium Turbo Prop             15 = Heavy Turbo Prop             16 = Super Heavy Jet    * `NotSet` - Not Set  * `LightJet` - Light Jet  * `MediumJet` - Medium Jet  * `HeavyJet` - Heavy Jet  * `LightHelicopter` - Light Helicopter  * `WideBody` - Wide Body  * `SingleTurboProp` - Single Turbo Prop  * `VeryLightJet` - Very Light Jet  * `SingleEnginePiston` - Single Engine Piston  * `MediumHelicopter` - Medium Helicopter  * `HeavyHelicopter` - Heavy Helicopter  * `LightTwin` - Light Twin  * `HeavyTwin` - Heavy Twin  * `LightTurboProp` - Light Turbo Prop  * `MediumTurboprop` - Medium Turbo Prop  * `HeavyTurboprop` - Heavy Turbo Prop  * `SuperHeavyJet` - Super Heavy Jet  
    /// </summary>
    /// <value>Aircraft sizes:             0 = Not set             1 = Light Jet             2 = Medium Jet             3 = Heavy Jet             4 = Light Helicopter             5 = Wide Body             6 = Single Turbo Prop             7 = Very Light Jet             8 = Single Engine Piston             9 = Medium Helicopter             10 = Heavy Helicopter             11 = Light Twin             12 = Heavy Twin             13 = Light Turbo Prop             14 = Medium Turbo Prop             15 = Heavy Turbo Prop             16 = Super Heavy Jet    * `NotSet` - Not Set  * `LightJet` - Light Jet  * `MediumJet` - Medium Jet  * `HeavyJet` - Heavy Jet  * `LightHelicopter` - Light Helicopter  * `WideBody` - Wide Body  * `SingleTurboProp` - Single Turbo Prop  * `VeryLightJet` - Very Light Jet  * `SingleEnginePiston` - Single Engine Piston  * `MediumHelicopter` - Medium Helicopter  * `HeavyHelicopter` - Heavy Helicopter  * `LightTwin` - Light Twin  * `HeavyTwin` - Heavy Twin  * `LightTurboProp` - Light Turbo Prop  * `MediumTurboprop` - Medium Turbo Prop  * `HeavyTurboprop` - Heavy Turbo Prop  * `SuperHeavyJet` - Super Heavy Jet  </value>
    [DataMember(Name="size", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "size")]
    public int? Size { get; set; }

    /// <summary>
    /// Gets or Sets FuelBurnRate
    /// </summary>
    [DataMember(Name="fuelBurnRate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelBurnRate")]
    public double? FuelBurnRate { get; set; }

    /// <summary>
    /// Gets or Sets FuelCapacity
    /// </summary>
    [DataMember(Name="fuelCapacity", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelCapacity")]
    public Weight FuelCapacity { get; set; }

    /// <summary>
    /// Gets or Sets PayloadBurnRate
    /// </summary>
    [DataMember(Name="payloadBurnRate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "payloadBurnRate")]
    public double? PayloadBurnRate { get; set; }

    /// <summary>
    /// Gets or Sets AircraftId
    /// </summary>
    [DataMember(Name="aircraftId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aircraftId")]
    public int? AircraftId { get; set; }

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
    /// Gets or Sets Qbclass
    /// </summary>
    [DataMember(Name="qbclass", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "qbclass")]
    public string Qbclass { get; set; }

    /// <summary>
    /// Gets or Sets Ifrreseve
    /// </summary>
    [DataMember(Name="ifrreseve", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ifrreseve")]
    public Weight Ifrreseve { get; set; }

    /// <summary>
    /// Gets or Sets Default
    /// </summary>
    [DataMember(Name="default", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "default")]
    public bool? Default { get; set; }

    /// <summary>
    /// Gets or Sets NormalCruiseTas
    /// </summary>
    [DataMember(Name="normalCruiseTas", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "normalCruiseTas")]
    public Speed NormalCruiseTas { get; set; }

    /// <summary>
    /// Gets or Sets Apcode
    /// </summary>
    [DataMember(Name="apcode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "apcode")]
    public string Apcode { get; set; }

    /// <summary>
    /// Gets or Sets MinimumUplift
    /// </summary>
    [DataMember(Name="minimumUplift", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minimumUplift")]
    public Weight MinimumUplift { get; set; }

    /// <summary>
    /// Gets or Sets AircraftTypeEngineName
    /// </summary>
    [DataMember(Name="aircraftTypeEngineName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aircraftTypeEngineName")]
    public string AircraftTypeEngineName { get; set; }

    /// <summary>
    /// Gets or Sets FleetGroup
    /// </summary>
    [DataMember(Name="fleetGroup", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fleetGroup")]
    public string FleetGroup { get; set; }

    /// <summary>
    /// Gets or Sets FactorySpecifications
    /// </summary>
    [DataMember(Name="factorySpecifications", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "factorySpecifications")]
    public AircraftDTO FactorySpecifications { get; set; }

    /// <summary>
    /// Gets or Sets TankeringSettings
    /// </summary>
    [DataMember(Name="tankeringSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tankeringSettings")]
    public UserAircraftTankeringSettingsDTO TankeringSettings { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AircraftDataDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
      sb.Append("  MinLandingRunwayLength: ").Append(MinLandingRunwayLength).Append("\n");
      sb.Append("  MaintCostPerHour: ").Append(MaintCostPerHour).Append("\n");
      sb.Append("  MaxRange: ").Append(MaxRange).Append("\n");
      sb.Append("  Size: ").Append(Size).Append("\n");
      sb.Append("  FuelBurnRate: ").Append(FuelBurnRate).Append("\n");
      sb.Append("  FuelCapacity: ").Append(FuelCapacity).Append("\n");
      sb.Append("  PayloadBurnRate: ").Append(PayloadBurnRate).Append("\n");
      sb.Append("  AircraftId: ").Append(AircraftId).Append("\n");
      sb.Append("  MaxTakeoffWeight: ").Append(MaxTakeoffWeight).Append("\n");
      sb.Append("  MaxLandingWeight: ").Append(MaxLandingWeight).Append("\n");
      sb.Append("  AircraftCeiling: ").Append(AircraftCeiling).Append("\n");
      sb.Append("  MinTakeoffRunwayLengthAtSeaLevel: ").Append(MinTakeoffRunwayLengthAtSeaLevel).Append("\n");
      sb.Append("  MinTakeoffRunwayLength: ").Append(MinTakeoffRunwayLength).Append("\n");
      sb.Append("  ZeroFuelWeight: ").Append(ZeroFuelWeight).Append("\n");
      sb.Append("  BasicOperatingWeight: ").Append(BasicOperatingWeight).Append("\n");
      sb.Append("  Qbclass: ").Append(Qbclass).Append("\n");
      sb.Append("  Ifrreseve: ").Append(Ifrreseve).Append("\n");
      sb.Append("  Default: ").Append(Default).Append("\n");
      sb.Append("  NormalCruiseTas: ").Append(NormalCruiseTas).Append("\n");
      sb.Append("  Apcode: ").Append(Apcode).Append("\n");
      sb.Append("  MinimumUplift: ").Append(MinimumUplift).Append("\n");
      sb.Append("  AircraftTypeEngineName: ").Append(AircraftTypeEngineName).Append("\n");
      sb.Append("  FleetGroup: ").Append(FleetGroup).Append("\n");
      sb.Append("  FactorySpecifications: ").Append(FactorySpecifications).Append("\n");
      sb.Append("  TankeringSettings: ").Append(TankeringSettings).Append("\n");
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
