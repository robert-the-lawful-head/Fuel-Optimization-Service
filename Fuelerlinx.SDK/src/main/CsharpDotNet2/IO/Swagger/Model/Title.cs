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
  public class Title {
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="align", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "align")]
    public int? Align { get; set; }

    /// <summary>
    /// Gets or Sets Margin
    /// </summary>
    [DataMember(Name="margin", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "margin")]
    public int? Margin { get; set; }

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
    /// Gets or Sets Text
    /// </summary>
    [DataMember(Name="text", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "text")]
    public string Text { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Title {\n");
      sb.Append("  Align: ").Append(Align).Append("\n");
      sb.Append("  Margin: ").Append(Margin).Append("\n");
      sb.Append("  Rotation: ").Append(Rotation).Append("\n");
      sb.Append("  Style: ").Append(Style).Append("\n");
      sb.Append("  Text: ").Append(Text).Append("\n");
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
