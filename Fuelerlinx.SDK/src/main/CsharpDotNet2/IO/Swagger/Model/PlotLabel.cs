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
  public class PlotLabel {
    /// <summary>
    /// Gets or Sets Text
    /// </summary>
    [DataMember(Name="text", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "text")]
    public string Text { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="align", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "align")]
    public int? Align { get; set; }

    /// <summary>
    /// Gets or Sets Enabled
    /// </summary>
    [DataMember(Name="enabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Gets or Sets Rotation
    /// </summary>
    [DataMember(Name="rotation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rotation")]
    public int? Rotation { get; set; }

    /// <summary>
    /// Gets or Sets Style
    /// </summary>
    [DataMember(Name="style", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "style")]
    public CSSObject Style { get; set; }

    /// <summary>
    /// Gets or Sets Formatter
    /// </summary>
    [DataMember(Name="formatter", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "formatter")]
    public string Formatter { get; set; }

    /// <summary>
    /// Gets or Sets StaggerLines
    /// </summary>
    [DataMember(Name="staggerLines", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "staggerLines")]
    public int? StaggerLines { get; set; }

    /// <summary>
    /// Gets or Sets Step
    /// </summary>
    [DataMember(Name="step", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "step")]
    public int? Step { get; set; }

    /// <summary>
    /// Gets or Sets X
    /// </summary>
    [DataMember(Name="x", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "x")]
    public int? X { get; set; }

    /// <summary>
    /// Gets or Sets Y
    /// </summary>
    [DataMember(Name="y", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "y")]
    public int? Y { get; set; }

    /// <summary>
    /// Gets or Sets Format
    /// </summary>
    [DataMember(Name="format", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "format")]
    public string Format { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="verticalAlign", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "verticalAlign")]
    public int? VerticalAlign { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PlotLabel {\n");
      sb.Append("  Text: ").Append(Text).Append("\n");
      sb.Append("  Align: ").Append(Align).Append("\n");
      sb.Append("  Enabled: ").Append(Enabled).Append("\n");
      sb.Append("  Rotation: ").Append(Rotation).Append("\n");
      sb.Append("  Style: ").Append(Style).Append("\n");
      sb.Append("  Formatter: ").Append(Formatter).Append("\n");
      sb.Append("  StaggerLines: ").Append(StaggerLines).Append("\n");
      sb.Append("  Step: ").Append(Step).Append("\n");
      sb.Append("  X: ").Append(X).Append("\n");
      sb.Append("  Y: ").Append(Y).Append("\n");
      sb.Append("  Format: ").Append(Format).Append("\n");
      sb.Append("  VerticalAlign: ").Append(VerticalAlign).Append("\n");
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
