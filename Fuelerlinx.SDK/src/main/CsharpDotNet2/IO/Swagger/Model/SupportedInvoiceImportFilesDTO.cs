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
  public class SupportedInvoiceImportFilesDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets SupportedInvoiceImportFileDataId
    /// </summary>
    [DataMember(Name="supportedInvoiceImportFileDataId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supportedInvoiceImportFileDataId")]
    public int? SupportedInvoiceImportFileDataId { get; set; }

    /// <summary>
    /// Gets or Sets FileName
    /// </summary>
    [DataMember(Name="fileName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fileName")]
    public string FileName { get; set; }

    /// <summary>
    /// Gets or Sets DateAddedUtc
    /// </summary>
    [DataMember(Name="dateAddedUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateAddedUtc")]
    public DateTime? DateAddedUtc { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets FuelerId
    /// </summary>
    [DataMember(Name="fuelerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerId")]
    public int? FuelerId { get; set; }

    /// <summary>
    /// Gets or Sets SupportedInvoiceImportFileTests
    /// </summary>
    [DataMember(Name="supportedInvoiceImportFileTests", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supportedInvoiceImportFileTests")]
    public SupportedInvoiceImportFileTestsDTO SupportedInvoiceImportFileTests { get; set; }

    /// <summary>
    /// Gets or Sets Company
    /// </summary>
    [DataMember(Name="company", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "company")]
    public CompanyDTO Company { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SupportedInvoiceImportFilesDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  SupportedInvoiceImportFileDataId: ").Append(SupportedInvoiceImportFileDataId).Append("\n");
      sb.Append("  FileName: ").Append(FileName).Append("\n");
      sb.Append("  DateAddedUtc: ").Append(DateAddedUtc).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  FuelerId: ").Append(FuelerId).Append("\n");
      sb.Append("  SupportedInvoiceImportFileTests: ").Append(SupportedInvoiceImportFileTests).Append("\n");
      sb.Append("  Company: ").Append(Company).Append("\n");
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
