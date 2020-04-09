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
  public class CSSObject {
    /// <summary>
    /// Gets or Sets Color
    /// </summary>
    [DataMember(Name="color", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "color")]
    public string Color { get; set; }

    /// <summary>
    /// Gets or Sets Font
    /// </summary>
    [DataMember(Name="font", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "font")]
    public string Font { get; set; }

    /// <summary>
    /// Gets or Sets FontWeight
    /// </summary>
    [DataMember(Name="fontWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fontWeight")]
    public string FontWeight { get; set; }

    /// <summary>
    /// Gets or Sets FontSize
    /// </summary>
    [DataMember(Name="fontSize", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fontSize")]
    public string FontSize { get; set; }

    /// <summary>
    /// Gets or Sets FontFamily
    /// </summary>
    [DataMember(Name="fontFamily", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fontFamily")]
    public string FontFamily { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CSSObject {\n");
      sb.Append("  Color: ").Append(Color).Append("\n");
      sb.Append("  Font: ").Append(Font).Append("\n");
      sb.Append("  FontWeight: ").Append(FontWeight).Append("\n");
      sb.Append("  FontSize: ").Append(FontSize).Append("\n");
      sb.Append("  FontFamily: ").Append(FontFamily).Append("\n");
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
