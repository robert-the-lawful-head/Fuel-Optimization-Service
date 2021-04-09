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
  public class InvoiceFileTestValidationResult {
    /// <summary>
    /// Gets or Sets ValidationFailures
    /// </summary>
    [DataMember(Name="validationFailures", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "validationFailures")]
    public List<string> ValidationFailures { get; set; }

    /// <summary>
    /// Gets or Sets IsValid
    /// </summary>
    [DataMember(Name="isValid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isValid")]
    public bool? IsValid { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InvoiceFileTestValidationResult {\n");
      sb.Append("  ValidationFailures: ").Append(ValidationFailures).Append("\n");
      sb.Append("  IsValid: ").Append(IsValid).Append("\n");
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
