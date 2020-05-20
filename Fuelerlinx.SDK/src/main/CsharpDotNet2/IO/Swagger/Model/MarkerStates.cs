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
  public class MarkerStates {
    /// <summary>
    /// Gets or Sets Hover
    /// </summary>
    [DataMember(Name="hover", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hover")]
    public MarkerStateSettings Hover { get; set; }

    /// <summary>
    /// Gets or Sets Select
    /// </summary>
    [DataMember(Name="select", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "select")]
    public MarkerStateSettings Select { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MarkerStates {\n");
      sb.Append("  Hover: ").Append(Hover).Append("\n");
      sb.Append("  Select: ").Append(Select).Append("\n");
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
