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
  public class AircraftPerformanceAltitude {
    /// <summary>
    /// Gets or Sets Airspeed
    /// </summary>
    [DataMember(Name="airspeed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airspeed")]
    public string Airspeed { get; set; }

    /// <summary>
    /// Gets or Sets Ktas
    /// </summary>
    [DataMember(Name="ktas", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ktas")]
    public double? Ktas { get; set; }

    /// <summary>
    /// Gets or Sets Altitude
    /// </summary>
    [DataMember(Name="altitude", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "altitude")]
    public int? Altitude { get; set; }

    /// <summary>
    /// Gets or Sets Rate
    /// </summary>
    [DataMember(Name="rate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rate")]
    public double? Rate { get; set; }

    /// <summary>
    /// Gets or Sets FuelBurn
    /// </summary>
    [DataMember(Name="fuelBurn", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelBurn")]
    public double? FuelBurn { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AircraftPerformanceAltitude {\n");
      sb.Append("  Airspeed: ").Append(Airspeed).Append("\n");
      sb.Append("  Ktas: ").Append(Ktas).Append("\n");
      sb.Append("  Altitude: ").Append(Altitude).Append("\n");
      sb.Append("  Rate: ").Append(Rate).Append("\n");
      sb.Append("  FuelBurn: ").Append(FuelBurn).Append("\n");
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
