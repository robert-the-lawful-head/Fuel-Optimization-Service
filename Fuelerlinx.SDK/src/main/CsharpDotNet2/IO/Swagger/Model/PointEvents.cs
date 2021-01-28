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
  public class PointEvents {
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
    /// Gets or Sets Remove
    /// </summary>
    [DataMember(Name="remove", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "remove")]
    public string Remove { get; set; }

    /// <summary>
    /// Gets or Sets Select
    /// </summary>
    [DataMember(Name="select", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "select")]
    public string Select { get; set; }

    /// <summary>
    /// Gets or Sets Unselect
    /// </summary>
    [DataMember(Name="unselect", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "unselect")]
    public string Unselect { get; set; }

    /// <summary>
    /// Gets or Sets Update
    /// </summary>
    [DataMember(Name="update", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "update")]
    public string Update { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PointEvents {\n");
      sb.Append("  Click: ").Append(Click).Append("\n");
      sb.Append("  MouseOver: ").Append(MouseOver).Append("\n");
      sb.Append("  MouseOut: ").Append(MouseOut).Append("\n");
      sb.Append("  Remove: ").Append(Remove).Append("\n");
      sb.Append("  Select: ").Append(Select).Append("\n");
      sb.Append("  Unselect: ").Append(Unselect).Append("\n");
      sb.Append("  Update: ").Append(Update).Append("\n");
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
