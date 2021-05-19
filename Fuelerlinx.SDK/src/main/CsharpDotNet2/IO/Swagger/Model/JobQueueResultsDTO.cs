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
  public class JobQueueResultsDTO {
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
    /// Gets or Sets DateRanUtc
    /// </summary>
    [DataMember(Name="dateRanUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateRanUtc")]
    public DateTime? DateRanUtc { get; set; }

    /// <summary>
    /// Gets or Sets ResultsJson
    /// </summary>
    [DataMember(Name="resultsJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "resultsJson")]
    public string ResultsJson { get; set; }

    /// <summary>
    /// Gets or Sets JobQueueResultEvents
    /// </summary>
    [DataMember(Name="jobQueueResultEvents", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobQueueResultEvents")]
    public List<JobQueueResultEventsDTO> JobQueueResultEvents { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class JobQueueResultsDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  JobQueueId: ").Append(JobQueueId).Append("\n");
      sb.Append("  DateRanUtc: ").Append(DateRanUtc).Append("\n");
      sb.Append("  ResultsJson: ").Append(ResultsJson).Append("\n");
      sb.Append("  JobQueueResultEvents: ").Append(JobQueueResultEvents).Append("\n");
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
