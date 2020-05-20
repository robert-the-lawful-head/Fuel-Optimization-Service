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
  public class PlotEvents {
    /// <summary>
    /// Gets or Sets Click
    /// </summary>
    [DataMember(Name="click", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "click")]
    public string Click { get; set; }

    /// <summary>
    /// Gets or Sets MouseOver
    /// </summary>
    [DataMember(Name="mouseOver", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mouseOver")]
    public string MouseOver { get; set; }

    /// <summary>
    /// Gets or Sets MouseOut
    /// </summary>
    [DataMember(Name="mouseOut", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mouseOut")]
    public string MouseOut { get; set; }

    /// <summary>
    /// Gets or Sets MouseMove
    /// </summary>
    [DataMember(Name="mouseMove", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mouseMove")]
    public string MouseMove { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PlotEvents {\n");
      sb.Append("  Click: ").Append(Click).Append("\n");
      sb.Append("  MouseOver: ").Append(MouseOver).Append("\n");
      sb.Append("  MouseOut: ").Append(MouseOut).Append("\n");
      sb.Append("  MouseMove: ").Append(MouseMove).Append("\n");
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
