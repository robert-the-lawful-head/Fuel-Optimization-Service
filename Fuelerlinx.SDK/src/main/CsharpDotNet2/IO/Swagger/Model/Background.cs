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
  public class Background {
    /// <summary>
    /// Gets or Sets LinearGradient
    /// </summary>
    [DataMember(Name="linearGradient", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "linearGradient")]
    public List<int?> LinearGradient { get; set; }

    /// <summary>
    /// Gets or Sets Stops
    /// </summary>
    [DataMember(Name="stops", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "stops")]
    public List<List<Object>> Stops { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Background {\n");
      sb.Append("  LinearGradient: ").Append(LinearGradient).Append("\n");
      sb.Append("  Stops: ").Append(Stops).Append("\n");
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
