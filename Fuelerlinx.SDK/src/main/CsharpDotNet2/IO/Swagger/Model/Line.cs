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
  public class Line {
    /// <summary>
    /// Gets or Sets DataLabels
    /// </summary>
    [DataMember(Name="dataLabels", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dataLabels")]
    public DataLabels DataLabels { get; set; }

    /// <summary>
    /// Gets or Sets EnableMouseTracking
    /// </summary>
    [DataMember(Name="enableMouseTracking", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "enableMouseTracking")]
    public bool? EnableMouseTracking { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Line {\n");
      sb.Append("  DataLabels: ").Append(DataLabels).Append("\n");
      sb.Append("  EnableMouseTracking: ").Append(EnableMouseTracking).Append("\n");
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
