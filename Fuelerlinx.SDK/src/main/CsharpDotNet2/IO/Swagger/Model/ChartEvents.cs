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
  public class ChartEvents {
    /// <summary>
    /// Gets or Sets AddSeries
    /// </summary>
    [DataMember(Name="addSeries", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addSeries")]
    public string AddSeries { get; set; }

    /// <summary>
    /// Gets or Sets Click
    /// </summary>
    [DataMember(Name="click", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "click")]
    public string Click { get; set; }

    /// <summary>
    /// Gets or Sets Load
    /// </summary>
    [DataMember(Name="load", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "load")]
    public string Load { get; set; }

    /// <summary>
    /// Gets or Sets Redraw
    /// </summary>
    [DataMember(Name="redraw", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "redraw")]
    public string Redraw { get; set; }

    /// <summary>
    /// Gets or Sets Selection
    /// </summary>
    [DataMember(Name="selection", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "selection")]
    public string Selection { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ChartEvents {\n");
      sb.Append("  AddSeries: ").Append(AddSeries).Append("\n");
      sb.Append("  Click: ").Append(Click).Append("\n");
      sb.Append("  Load: ").Append(Load).Append("\n");
      sb.Append("  Redraw: ").Append(Redraw).Append("\n");
      sb.Append("  Selection: ").Append(Selection).Append("\n");
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
