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
  public class PostSupportedInvoiceImportTemplateRequest {
    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets FuelerProcessName
    /// </summary>
    [DataMember(Name="fuelerProcessName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerProcessName")]
    public string FuelerProcessName { get; set; }

    /// <summary>
    /// Gets or Sets InvoiceFileTemplatesJson
    /// </summary>
    [DataMember(Name="invoiceFileTemplatesJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoiceFileTemplatesJson")]
    public string InvoiceFileTemplatesJson { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostSupportedInvoiceImportTemplateRequest {\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  FuelerProcessName: ").Append(FuelerProcessName).Append("\n");
      sb.Append("  InvoiceFileTemplatesJson: ").Append(InvoiceFileTemplatesJson).Append("\n");
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
