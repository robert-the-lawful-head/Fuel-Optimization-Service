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
  public class JobQueueFilesDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets JobQueueId
    /// </summary>
    [DataMember(Name="jobQueueId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobQueueId")]
    public int? JobQueueId { get; set; }

    /// <summary>
    /// Gets or Sets JobFileDataId
    /// </summary>
    [DataMember(Name="jobFileDataId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobFileDataId")]
    public int? JobFileDataId { get; set; }

    /// <summary>
    /// Gets or Sets FileName
    /// </summary>
    [DataMember(Name="fileName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fileName")]
    public string FileName { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class JobQueueFilesDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  JobQueueId: ").Append(JobQueueId).Append("\n");
      sb.Append("  JobFileDataId: ").Append(JobFileDataId).Append("\n");
      sb.Append("  FileName: ").Append(FileName).Append("\n");
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
