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
  public class AutoReconciliationSettings {
    /// <summary>
    /// Gets or Sets IsPreviewOnly
    /// </summary>
    [DataMember(Name="isPreviewOnly", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isPreviewOnly")]
    public bool? IsPreviewOnly { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AutoReconciliationSettings {\n");
      sb.Append("  IsPreviewOnly: ").Append(IsPreviewOnly).Append("\n");
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
