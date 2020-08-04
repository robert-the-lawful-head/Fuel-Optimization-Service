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
  public class AutoReconciledFileDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets ProcessId
    /// </summary>
    [DataMember(Name="processId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "processId")]
    public int? ProcessId { get; set; }

    /// <summary>
    /// Gets or Sets ImportFileCaptureId
    /// </summary>
    [DataMember(Name="importFileCaptureId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "importFileCaptureId")]
    public int? ImportFileCaptureId { get; set; }

    /// <summary>
    /// Gets or Sets DateCapturedUtc
    /// </summary>
    [DataMember(Name="dateCapturedUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateCapturedUtc")]
    public DateTime? DateCapturedUtc { get; set; }

    /// <summary>
    /// Gets or Sets FileName
    /// </summary>
    [DataMember(Name="fileName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fileName")]
    public string FileName { get; set; }

    /// <summary>
    /// Gets or Sets PageCount
    /// </summary>
    [DataMember(Name="pageCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pageCount")]
    public int? PageCount { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AutoReconciledFileDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  ProcessId: ").Append(ProcessId).Append("\n");
      sb.Append("  ImportFileCaptureId: ").Append(ImportFileCaptureId).Append("\n");
      sb.Append("  DateCapturedUtc: ").Append(DateCapturedUtc).Append("\n");
      sb.Append("  FileName: ").Append(FileName).Append("\n");
      sb.Append("  PageCount: ").Append(PageCount).Append("\n");
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
