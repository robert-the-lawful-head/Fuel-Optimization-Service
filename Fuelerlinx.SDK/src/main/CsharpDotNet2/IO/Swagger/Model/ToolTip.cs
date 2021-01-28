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
  public class ToolTip {
    /// <summary>
    /// Gets or Sets BackgroundColor
    /// </summary>
    [DataMember(Name="backgroundColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "backgroundColor")]
    public string BackgroundColor { get; set; }

    /// <summary>
    /// Gets or Sets BorderColor
    /// </summary>
    [DataMember(Name="borderColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "borderColor")]
    public string BorderColor { get; set; }

    /// <summary>
    /// Gets or Sets BorderRadius
    /// </summary>
    [DataMember(Name="borderRadius", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "borderRadius")]
    public int? BorderRadius { get; set; }

    /// <summary>
    /// Gets or Sets BorderWidth
    /// </summary>
    [DataMember(Name="borderWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "borderWidth")]
    public int? BorderWidth { get; set; }

    /// <summary>
    /// Gets or Sets Crosshairs
    /// </summary>
    [DataMember(Name="crosshairs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "crosshairs")]
    public bool? Crosshairs { get; set; }

    /// <summary>
    /// Gets or Sets Enabled
    /// </summary>
    [DataMember(Name="enabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Gets or Sets Formatter
    /// </summary>
    [DataMember(Name="formatter", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "formatter")]
    public string Formatter { get; set; }

    /// <summary>
    /// Gets or Sets Shadow
    /// </summary>
    [DataMember(Name="shadow", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "shadow")]
    public bool? Shadow { get; set; }

    /// <summary>
    /// Gets or Sets Shared
    /// </summary>
    [DataMember(Name="shared", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "shared")]
    public bool? Shared { get; set; }

    /// <summary>
    /// Gets or Sets Snap
    /// </summary>
    [DataMember(Name="snap", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "snap")]
    public int? Snap { get; set; }

    /// <summary>
    /// Gets or Sets Style
    /// </summary>
    [DataMember(Name="style", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "style")]
    public CSSObject Style { get; set; }

    /// <summary>
    /// Gets or Sets HeaderFormat
    /// </summary>
    [DataMember(Name="headerFormat", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "headerFormat")]
    public string HeaderFormat { get; set; }

    /// <summary>
    /// Gets or Sets PointFormat
    /// </summary>
    [DataMember(Name="pointFormat", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pointFormat")]
    public string PointFormat { get; set; }

    /// <summary>
    /// Gets or Sets UseHTML
    /// </summary>
    [DataMember(Name="useHTML", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "useHTML")]
    public bool? UseHTML { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ToolTip {\n");
      sb.Append("  BackgroundColor: ").Append(BackgroundColor).Append("\n");
      sb.Append("  BorderColor: ").Append(BorderColor).Append("\n");
      sb.Append("  BorderRadius: ").Append(BorderRadius).Append("\n");
      sb.Append("  BorderWidth: ").Append(BorderWidth).Append("\n");
      sb.Append("  Crosshairs: ").Append(Crosshairs).Append("\n");
      sb.Append("  Enabled: ").Append(Enabled).Append("\n");
      sb.Append("  Formatter: ").Append(Formatter).Append("\n");
      sb.Append("  Shadow: ").Append(Shadow).Append("\n");
      sb.Append("  Shared: ").Append(Shared).Append("\n");
      sb.Append("  Snap: ").Append(Snap).Append("\n");
      sb.Append("  Style: ").Append(Style).Append("\n");
      sb.Append("  HeaderFormat: ").Append(HeaderFormat).Append("\n");
      sb.Append("  PointFormat: ").Append(PointFormat).Append("\n");
      sb.Append("  UseHTML: ").Append(UseHTML).Append("\n");
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
