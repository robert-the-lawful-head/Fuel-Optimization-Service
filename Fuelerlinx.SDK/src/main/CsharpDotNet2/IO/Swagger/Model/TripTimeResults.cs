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
  public class TripTimeResults {
    /// <summary>
    /// Gets or Sets Items
    /// </summary>
    [DataMember(Name="items", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "items")]
    public List<ResultItem> Items { get; set; }

    /// <summary>
    /// Gets or Sets TimeUnit
    /// </summary>
    [DataMember(Name="timeUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timeUnit")]
    public Time TimeUnit { get; set; }

    /// <summary>
    /// Gets or Sets EstTotalFlightTime
    /// </summary>
    [DataMember(Name="estTotalFlightTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "estTotalFlightTime")]
    public string EstTotalFlightTime { get; set; }

    /// <summary>
    /// Gets or Sets LegDistance
    /// </summary>
    [DataMember(Name="legDistance", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legDistance")]
    public string LegDistance { get; set; }

    /// <summary>
    /// Gets or Sets TankeringPenalty
    /// </summary>
    [DataMember(Name="tankeringPenalty", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tankeringPenalty")]
    public string TankeringPenalty { get; set; }

    /// <summary>
    /// Gets or Sets LegNumber
    /// </summary>
    [DataMember(Name="legNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legNumber")]
    public int? LegNumber { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TripTimeResults {\n");
      sb.Append("  Items: ").Append(Items).Append("\n");
      sb.Append("  TimeUnit: ").Append(TimeUnit).Append("\n");
      sb.Append("  EstTotalFlightTime: ").Append(EstTotalFlightTime).Append("\n");
      sb.Append("  LegDistance: ").Append(LegDistance).Append("\n");
      sb.Append("  TankeringPenalty: ").Append(TankeringPenalty).Append("\n");
      sb.Append("  LegNumber: ").Append(LegNumber).Append("\n");
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
