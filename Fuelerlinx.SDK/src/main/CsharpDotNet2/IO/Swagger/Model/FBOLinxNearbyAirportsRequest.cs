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
  public class FBOLinxNearbyAirportsRequest {
    /// <summary>
    /// Gets or Sets StartDateTime
    /// </summary>
    [DataMember(Name="startDateTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "startDateTime")]
    public DateTime? StartDateTime { get; set; }

    /// <summary>
    /// Gets or Sets EndDateTime
    /// </summary>
    [DataMember(Name="endDateTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "endDateTime")]
    public DateTime? EndDateTime { get; set; }

    /// <summary>
    /// Gets or Sets DistanceMile
    /// </summary>
    [DataMember(Name="distanceMile", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "distanceMile")]
    public int? DistanceMile { get; set; }

    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FBOLinxNearbyAirportsRequest {\n");
      sb.Append("  StartDateTime: ").Append(StartDateTime).Append("\n");
      sb.Append("  EndDateTime: ").Append(EndDateTime).Append("\n");
      sb.Append("  DistanceMile: ").Append(DistanceMile).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
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
