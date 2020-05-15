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
    /// Gets or Sets LegDistance
    /// </summary>
    [DataMember(Name="legDistance", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legDistance")]
    public string LegDistance { get; set; }

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
    /// Gets or Sets CalculatedCruise
    /// </summary>
    [DataMember(Name="calculatedCruise", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "calculatedCruise")]
    public string CalculatedCruise { get; set; }

    /// <summary>
    /// Gets or Sets CalculatedAltitude
    /// </summary>
    [DataMember(Name="calculatedAltitude", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "calculatedAltitude")]
    public string CalculatedAltitude { get; set; }

    /// <summary>
    /// Gets or Sets Route
    /// </summary>
    [DataMember(Name="route", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "route")]
    public string Route { get; set; }

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
      sb.Append("  LegDistance: ").Append(LegDistance).Append("\n");
      sb.Append("  TotalDistanceNM: ").Append(TotalDistanceNM).Append("\n");
      sb.Append("  Airways: ").Append(Airways).Append("\n");
      sb.Append("  CalculatedCruise: ").Append(CalculatedCruise).Append("\n");
      sb.Append("  CalculatedAltitude: ").Append(CalculatedAltitude).Append("\n");
      sb.Append("  Route: ").Append(Route).Append("\n");
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
