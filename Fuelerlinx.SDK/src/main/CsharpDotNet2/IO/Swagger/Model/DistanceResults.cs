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
  public class DistanceResults {
    /// <summary>
    /// Gets or Sets Items
    /// </summary>
    [DataMember(Name="items", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "items")]
    public List<ResultItem> Items { get; set; }

    /// <summary>
    /// Gets or Sets TotalDistanceNM
    /// </summary>
    [DataMember(Name="totalDistanceNM", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "totalDistanceNM")]
    public double? TotalDistanceNM { get; set; }

    /// <summary>
    /// Gets or Sets Airways
    /// </summary>
    [DataMember(Name="airways", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airways")]
    public string Airways { get; set; }

    /// <summary>
    /// Gets or Sets DistanceUnit
    /// </summary>
    [DataMember(Name="distanceUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "distanceUnit")]
    public Distance DistanceUnit { get; set; }

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
      sb.Append("class DistanceResults {\n");
      sb.Append("  Items: ").Append(Items).Append("\n");
      sb.Append("  TotalDistanceNM: ").Append(TotalDistanceNM).Append("\n");
      sb.Append("  Airways: ").Append(Airways).Append("\n");
      sb.Append("  DistanceUnit: ").Append(DistanceUnit).Append("\n");
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
