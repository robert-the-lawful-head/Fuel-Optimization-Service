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
  public class ProcessInvoiceFileRequest {
    /// <summary>
    /// Gets or Sets FuelerId
    /// </summary>
    [DataMember(Name="fuelerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerId")]
    public int? FuelerId { get; set; }

    /// <summary>
    /// Gets or Sets ContentType
    /// </summary>
    [DataMember(Name="contentType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "contentType")]
    public string ContentType { get; set; }

    /// <summary>
    /// Gets or Sets FileDataAsBase64String
    /// </summary>
    [DataMember(Name="fileDataAsBase64String", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fileDataAsBase64String")]
    public string FileDataAsBase64String { get; set; }

    /// <summary>
    /// Gets or Sets FileName
    /// </summary>
    [DataMember(Name="fileName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fileName")]
    public string FileName { get; set; }

    /// <summary>
    /// Gets or Sets AutoReconciliationSettings
    /// </summary>
    [DataMember(Name="autoReconciliationSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "autoReconciliationSettings")]
    public AutoReconciliationSettings AutoReconciliationSettings { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ProcessInvoiceFileRequest {\n");
      sb.Append("  FuelerId: ").Append(FuelerId).Append("\n");
      sb.Append("  ContentType: ").Append(ContentType).Append("\n");
      sb.Append("  FileDataAsBase64String: ").Append(FileDataAsBase64String).Append("\n");
      sb.Append("  FileName: ").Append(FileName).Append("\n");
      sb.Append("  AutoReconciliationSettings: ").Append(AutoReconciliationSettings).Append("\n");
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
