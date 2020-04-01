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
  public class AircraftPerformanceProfile {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets IsDefault
    /// </summary>
    [DataMember(Name="isDefault", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isDefault")]
    public bool? IsDefault { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="profileType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "profileType")]
    public int? ProfileType { get; set; }

    /// <summary>
    /// Gets or Sets ClimbAirspeed
    /// </summary>
    [DataMember(Name="climbAirspeed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "climbAirspeed")]
    public string ClimbAirspeed { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="climbAirspeedType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "climbAirspeedType")]
    public int? ClimbAirspeedType { get; set; }

    /// <summary>
    /// Gets or Sets ClimbFuelBurn
    /// </summary>
    [DataMember(Name="climbFuelBurn", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "climbFuelBurn")]
    public double? ClimbFuelBurn { get; set; }

    /// <summary>
    /// Gets or Sets ClimbKTAS
    /// </summary>
    [DataMember(Name="climbKTAS", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "climbKTAS")]
    public double? ClimbKTAS { get; set; }

    /// <summary>
    /// Gets or Sets ClimbRate
    /// </summary>
    [DataMember(Name="climbRate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "climbRate")]
    public double? ClimbRate { get; set; }

    /// <summary>
    /// Gets or Sets CruiseAirspeed
    /// </summary>
    [DataMember(Name="cruiseAirspeed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cruiseAirspeed")]
    public string CruiseAirspeed { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="descentAirspeedType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "descentAirspeedType")]
    public int? DescentAirspeedType { get; set; }

    /// <summary>
    /// Gets or Sets CruiseFuelBurn
    /// </summary>
    [DataMember(Name="cruiseFuelBurn", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cruiseFuelBurn")]
    public double? CruiseFuelBurn { get; set; }

    /// <summary>
    /// Gets or Sets CruiseKTAS
    /// </summary>
    [DataMember(Name="cruiseKTAS", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cruiseKTAS")]
    public double? CruiseKTAS { get; set; }

    /// <summary>
    /// Gets or Sets DescentAirspeed
    /// </summary>
    [DataMember(Name="descentAirspeed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "descentAirspeed")]
    public string DescentAirspeed { get; set; }

    /// <summary>
    /// Gets or Sets DescentFuelBurn
    /// </summary>
    [DataMember(Name="descentFuelBurn", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "descentFuelBurn")]
    public double? DescentFuelBurn { get; set; }

    /// <summary>
    /// Gets or Sets DescentKTAS
    /// </summary>
    [DataMember(Name="descentKTAS", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "descentKTAS")]
    public double? DescentKTAS { get; set; }

    /// <summary>
    /// Gets or Sets DescentRate
    /// </summary>
    [DataMember(Name="descentRate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "descentRate")]
    public double? DescentRate { get; set; }

    /// <summary>
    /// Gets or Sets FuelBurnByHour
    /// </summary>
    [DataMember(Name="fuelBurnByHour", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelBurnByHour")]
    public string FuelBurnByHour { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="airspeedType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airspeedType")]
    public int? AirspeedType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="flightPhase", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightPhase")]
    public int? FlightPhase { get; set; }

    /// <summary>
    /// Gets or Sets Altitudes
    /// </summary>
    [DataMember(Name="altitudes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "altitudes")]
    public List<AircraftPerformanceAltitude> Altitudes { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AircraftPerformanceProfile {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  IsDefault: ").Append(IsDefault).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  ProfileType: ").Append(ProfileType).Append("\n");
      sb.Append("  ClimbAirspeed: ").Append(ClimbAirspeed).Append("\n");
      sb.Append("  ClimbAirspeedType: ").Append(ClimbAirspeedType).Append("\n");
      sb.Append("  ClimbFuelBurn: ").Append(ClimbFuelBurn).Append("\n");
      sb.Append("  ClimbKTAS: ").Append(ClimbKTAS).Append("\n");
      sb.Append("  ClimbRate: ").Append(ClimbRate).Append("\n");
      sb.Append("  CruiseAirspeed: ").Append(CruiseAirspeed).Append("\n");
      sb.Append("  DescentAirspeedType: ").Append(DescentAirspeedType).Append("\n");
      sb.Append("  CruiseFuelBurn: ").Append(CruiseFuelBurn).Append("\n");
      sb.Append("  CruiseKTAS: ").Append(CruiseKTAS).Append("\n");
      sb.Append("  DescentAirspeed: ").Append(DescentAirspeed).Append("\n");
      sb.Append("  DescentFuelBurn: ").Append(DescentFuelBurn).Append("\n");
      sb.Append("  DescentKTAS: ").Append(DescentKTAS).Append("\n");
      sb.Append("  DescentRate: ").Append(DescentRate).Append("\n");
      sb.Append("  FuelBurnByHour: ").Append(FuelBurnByHour).Append("\n");
      sb.Append("  AirspeedType: ").Append(AirspeedType).Append("\n");
      sb.Append("  FlightPhase: ").Append(FlightPhase).Append("\n");
      sb.Append("  Altitudes: ").Append(Altitudes).Append("\n");
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
