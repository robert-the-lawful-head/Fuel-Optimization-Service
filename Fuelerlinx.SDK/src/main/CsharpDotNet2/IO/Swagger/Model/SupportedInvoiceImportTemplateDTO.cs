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
  public class SupportedInvoiceImportTemplateDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

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
    /// Gets or Sets InvoiceImportTemplatesJson
    /// </summary>
    [DataMember(Name="invoiceImportTemplatesJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoiceImportTemplatesJson")]
    public string InvoiceImportTemplatesJson { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SupportedInvoiceImportTemplateDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  FuelerProcessName: ").Append(FuelerProcessName).Append("\n");
      sb.Append("  InvoiceImportTemplatesJson: ").Append(InvoiceImportTemplatesJson).Append("\n");
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
