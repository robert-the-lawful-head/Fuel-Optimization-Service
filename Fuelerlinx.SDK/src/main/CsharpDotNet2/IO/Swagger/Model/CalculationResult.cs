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
  public class CalculationResult {
    /// <summary>
    /// Gets or Sets DefaultOptions
    /// </summary>
    [DataMember(Name="defaultOptions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "defaultOptions")]
    public List<MultiLegOption> DefaultOptions { get; set; }

    /// <summary>
    /// Gets or Sets BestOptions
    /// </summary>
    [DataMember(Name="bestOptions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "bestOptions")]
    public List<MultiLegOption> BestOptions { get; set; }

    /// <summary>
    /// Gets or Sets Summary
    /// </summary>
    [DataMember(Name="summary", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "summary")]
    public Summary Summary { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CalculationResult {\n");
      sb.Append("  DefaultOptions: ").Append(DefaultOptions).Append("\n");
      sb.Append("  BestOptions: ").Append(BestOptions).Append("\n");
      sb.Append("  Summary: ").Append(Summary).Append("\n");
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
