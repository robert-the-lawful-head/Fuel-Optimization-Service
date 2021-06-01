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
  public class UpdateSupportedInvoiceImportFileTestsRequest {
    /// <summary>
    /// Gets or Sets SupportedInvoiceImportFileTests
    /// </summary>
    [DataMember(Name="supportedInvoiceImportFileTests", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supportedInvoiceImportFileTests")]
    public SupportedInvoiceImportFileTestsDTO SupportedInvoiceImportFileTests { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateSupportedInvoiceImportFileTestsRequest {\n");
      sb.Append("  SupportedInvoiceImportFileTests: ").Append(SupportedInvoiceImportFileTests).Append("\n");
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
