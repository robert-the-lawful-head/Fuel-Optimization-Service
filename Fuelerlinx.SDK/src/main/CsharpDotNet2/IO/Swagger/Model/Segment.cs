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
  public class Segment {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets Lat
    /// </summary>
    [DataMember(Name="lat", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lat")]
    public double? Lat { get; set; }

    /// <summary>
    /// Gets or Sets Lon
    /// </summary>
    [DataMember(Name="lon", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lon")]
    public double? Lon { get; set; }

    /// <summary>
    /// Gets or Sets Altitude
    /// </summary>
    [DataMember(Name="altitude", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "altitude")]
    public int? Altitude { get; set; }

    /// <summary>
    /// Gets or Sets Fuel
    /// </summary>
    [DataMember(Name="fuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuel")]
    public double? Fuel { get; set; }

    /// <summary>
    /// Gets or Sets Time
    /// </summary>
    [DataMember(Name="time", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "time")]
    public int? Time { get; set; }

    /// <summary>
    /// Gets or Sets TotalFuel
    /// </summary>
    [DataMember(Name="totalFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "totalFuel")]
    public double? TotalFuel { get; set; }

    /// <summary>
    /// Gets or Sets TotalTime
    /// </summary>
    [DataMember(Name="totalTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "totalTime")]
    public int? TotalTime { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Segment {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Lat: ").Append(Lat).Append("\n");
      sb.Append("  Lon: ").Append(Lon).Append("\n");
      sb.Append("  Altitude: ").Append(Altitude).Append("\n");
      sb.Append("  Fuel: ").Append(Fuel).Append("\n");
      sb.Append("  Time: ").Append(Time).Append("\n");
      sb.Append("  TotalFuel: ").Append(TotalFuel).Append("\n");
      sb.Append("  TotalTime: ").Append(TotalTime).Append("\n");
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
