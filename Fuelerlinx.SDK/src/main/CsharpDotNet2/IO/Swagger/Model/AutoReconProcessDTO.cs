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
  public class AutoReconProcessDTO {
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
    /// Gets or Sets FuelerId
    /// </summary>
    [DataMember(Name="fuelerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerId")]
    public int? FuelerId { get; set; }

    /// <summary>
    /// Gets or Sets Uploaded
    /// </summary>
    [DataMember(Name="uploaded", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "uploaded")]
    public DateTime? Uploaded { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="processState", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "processState")]
    public int? ProcessState { get; set; }

    /// <summary>
    /// Gets or Sets AutoReconciledFiles
    /// </summary>
    [DataMember(Name="autoReconciledFiles", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "autoReconciledFiles")]
    public List<AutoReconciledFileDTO> AutoReconciledFiles { get; set; }

    /// <summary>
    /// Gets or Sets AutoReconciledLineItems
    /// </summary>
    [DataMember(Name="autoReconciledLineItems", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "autoReconciledLineItems")]
    public List<AutoReconciledLineItemDTO> AutoReconciledLineItems { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AutoReconProcessDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  FuelerId: ").Append(FuelerId).Append("\n");
      sb.Append("  Uploaded: ").Append(Uploaded).Append("\n");
      sb.Append("  ProcessState: ").Append(ProcessState).Append("\n");
      sb.Append("  AutoReconciledFiles: ").Append(AutoReconciledFiles).Append("\n");
      sb.Append("  AutoReconciledLineItems: ").Append(AutoReconciledLineItems).Append("\n");
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
