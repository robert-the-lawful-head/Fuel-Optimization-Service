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
  public class ProcessInvoiceFileResponse {
    /// <summary>
    /// Gets or Sets AutoReconProcess
    /// </summary>
    [DataMember(Name="autoReconProcess", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "autoReconProcess")]
    public AutoReconProcessDTO AutoReconProcess { get; set; }

    /// <summary>
    /// Gets or Sets JobQueueResultEvents
    /// </summary>
    [DataMember(Name="jobQueueResultEvents", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobQueueResultEvents")]
    public List<JobQueueResultEventsDTO> JobQueueResultEvents { get; set; }

    /// <summary>
    /// Gets or Sets Success
    /// </summary>
    [DataMember(Name="success", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "success")]
    public bool? Success { get; set; }

    /// <summary>
    /// Gets or Sets Message
    /// </summary>
    [DataMember(Name="message", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ProcessInvoiceFileResponse {\n");
      sb.Append("  AutoReconProcess: ").Append(AutoReconProcess).Append("\n");
      sb.Append("  JobQueueResultEvents: ").Append(JobQueueResultEvents).Append("\n");
      sb.Append("  Success: ").Append(Success).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
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
