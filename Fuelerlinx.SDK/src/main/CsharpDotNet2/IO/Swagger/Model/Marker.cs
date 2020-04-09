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
  public class Marker {
    /// <summary>
    /// Gets or Sets States
    /// </summary>
    [DataMember(Name="states", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "states")]
    public MarkerStates States { get; set; }

    /// <summary>
    /// Gets or Sets Symbol
    /// </summary>
    [DataMember(Name="symbol", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "symbol")]
    public string Symbol { get; set; }

    /// <summary>
    /// Gets or Sets Enabled
    /// </summary>
    [DataMember(Name="enabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Gets or Sets FillColor
    /// </summary>
    [DataMember(Name="fillColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fillColor")]
    public string FillColor { get; set; }

    /// <summary>
    /// Gets or Sets LineColor
    /// </summary>
    [DataMember(Name="lineColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lineColor")]
    public string LineColor { get; set; }

    /// <summary>
    /// Gets or Sets LineWidth
    /// </summary>
    [DataMember(Name="lineWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lineWidth")]
    public int? LineWidth { get; set; }

    /// <summary>
    /// Gets or Sets Radius
    /// </summary>
    [DataMember(Name="radius", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "radius")]
    public int? Radius { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Marker {\n");
      sb.Append("  States: ").Append(States).Append("\n");
      sb.Append("  Symbol: ").Append(Symbol).Append("\n");
      sb.Append("  Enabled: ").Append(Enabled).Append("\n");
      sb.Append("  FillColor: ").Append(FillColor).Append("\n");
      sb.Append("  LineColor: ").Append(LineColor).Append("\n");
      sb.Append("  LineWidth: ").Append(LineWidth).Append("\n");
      sb.Append("  Radius: ").Append(Radius).Append("\n");
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
