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
  public class PlotOptionEvents {
    /// <summary>
    /// Gets or Sets Click
    /// </summary>
    [DataMember(Name="click", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "click")]
    public string Click { get; set; }

    /// <summary>
    /// Gets or Sets Hide
    /// </summary>
    [DataMember(Name="hide", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hide")]
    public string Hide { get; set; }

    /// <summary>
    /// Gets or Sets LegendItemClick
    /// </summary>
    [DataMember(Name="legendItemClick", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legendItemClick")]
    public string LegendItemClick { get; set; }

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
    /// Gets or Sets Show
    /// </summary>
    [DataMember(Name="show", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "show")]
    public string Show { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PlotOptionEvents {\n");
      sb.Append("  Click: ").Append(Click).Append("\n");
      sb.Append("  Hide: ").Append(Hide).Append("\n");
      sb.Append("  LegendItemClick: ").Append(LegendItemClick).Append("\n");
      sb.Append("  MouseOver: ").Append(MouseOver).Append("\n");
      sb.Append("  MouseOut: ").Append(MouseOut).Append("\n");
      sb.Append("  Show: ").Append(Show).Append("\n");
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
