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
  public class RoutesBetweenAirportsExStruct {
    /// <summary>
    /// Gets or Sets Count
    /// </summary>
    [DataMember(Name="count", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "count")]
    public int? Count { get; set; }

    /// <summary>
    /// Gets or Sets Route
    /// </summary>
    [DataMember(Name="route", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "route")]
    public string Route { get; set; }

    /// <summary>
    /// Gets or Sets FiledAltitudeMin
    /// </summary>
    [DataMember(Name="filedAltitude_min", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "filedAltitude_min")]
    public int? FiledAltitudeMin { get; set; }

    /// <summary>
    /// Gets or Sets FiledAltitudeMax
    /// </summary>
    [DataMember(Name="filedAltitude_max", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "filedAltitude_max")]
    public int? FiledAltitudeMax { get; set; }

    /// <summary>
    /// Gets or Sets LastDeparturetime
    /// </summary>
    [DataMember(Name="last_departuretime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "last_departuretime")]
    public int? LastDeparturetime { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class RoutesBetweenAirportsExStruct {\n");
      sb.Append("  Count: ").Append(Count).Append("\n");
      sb.Append("  Route: ").Append(Route).Append("\n");
      sb.Append("  FiledAltitudeMin: ").Append(FiledAltitudeMin).Append("\n");
      sb.Append("  FiledAltitudeMax: ").Append(FiledAltitudeMax).Append("\n");
      sb.Append("  LastDeparturetime: ").Append(LastDeparturetime).Append("\n");
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
