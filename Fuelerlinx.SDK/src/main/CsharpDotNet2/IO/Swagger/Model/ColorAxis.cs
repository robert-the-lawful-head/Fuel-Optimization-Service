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
  public class ColorAxis {
    /// <summary>
    /// Gets or Sets Min
    /// </summary>
    [DataMember(Name="min", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "min")]
    public int? Min { get; set; }

    /// <summary>
    /// Gets or Sets MinColor
    /// </summary>
    [DataMember(Name="minColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minColor")]
    public string MinColor { get; set; }

    /// <summary>
    /// Gets or Sets MaxColor
    /// </summary>
    [DataMember(Name="maxColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxColor")]
    public string MaxColor { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ColorAxis {\n");
      sb.Append("  Min: ").Append(Min).Append("\n");
      sb.Append("  MinColor: ").Append(MinColor).Append("\n");
      sb.Append("  MaxColor: ").Append(MaxColor).Append("\n");
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
