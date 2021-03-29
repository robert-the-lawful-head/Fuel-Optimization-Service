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
  public class SupportedInvoiceImportFileTestsDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets SupportedInvoiceImportFileId
    /// </summary>
    [DataMember(Name="supportedInvoiceImportFileId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supportedInvoiceImportFileId")]
    public int? SupportedInvoiceImportFileId { get; set; }

    /// <summary>
    /// Gets or Sets ExpectedLineItemsCount
    /// </summary>
    [DataMember(Name="expectedLineItemsCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "expectedLineItemsCount")]
    public int? ExpectedLineItemsCount { get; set; }

    /// <summary>
    /// Gets or Sets ExpectedInvoiceNumbersCount
    /// </summary>
    [DataMember(Name="expectedInvoiceNumbersCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "expectedInvoiceNumbersCount")]
    public int? ExpectedInvoiceNumbersCount { get; set; }

    /// <summary>
    /// Gets or Sets ExpectedTotal
    /// </summary>
    [DataMember(Name="expectedTotal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "expectedTotal")]
    public double? ExpectedTotal { get; set; }

    /// <summary>
    /// Gets or Sets ExpectedTails
    /// </summary>
    [DataMember(Name="expectedTails", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "expectedTails")]
    public string ExpectedTails { get; set; }

    /// <summary>
    /// Gets or Sets HasFboonAllItems
    /// </summary>
    [DataMember(Name="hasFboonAllItems", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hasFboonAllItems")]
    public bool? HasFboonAllItems { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SupportedInvoiceImportFileTestsDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  SupportedInvoiceImportFileId: ").Append(SupportedInvoiceImportFileId).Append("\n");
      sb.Append("  ExpectedLineItemsCount: ").Append(ExpectedLineItemsCount).Append("\n");
      sb.Append("  ExpectedInvoiceNumbersCount: ").Append(ExpectedInvoiceNumbersCount).Append("\n");
      sb.Append("  ExpectedTotal: ").Append(ExpectedTotal).Append("\n");
      sb.Append("  ExpectedTails: ").Append(ExpectedTails).Append("\n");
      sb.Append("  HasFboonAllItems: ").Append(HasFboonAllItems).Append("\n");
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
