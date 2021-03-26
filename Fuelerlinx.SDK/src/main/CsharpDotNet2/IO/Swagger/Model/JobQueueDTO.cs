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
  public class JobQueueDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets JobName
    /// </summary>
    [DataMember(Name="jobName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobName")]
    public string JobName { get; set; }

    /// <summary>
    /// Gets or Sets JobVariablesJson
    /// </summary>
    [DataMember(Name="jobVariablesJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobVariablesJson")]
    public string JobVariablesJson { get; set; }

    /// <summary>
    /// Gets or Sets DateCreatedUtc
    /// </summary>
    [DataMember(Name="dateCreatedUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateCreatedUtc")]
    public DateTime? DateCreatedUtc { get; set; }

    /// <summary>
    /// Gets or Sets LastRanUtc
    /// </summary>
    [DataMember(Name="lastRanUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lastRanUtc")]
    public DateTime? LastRanUtc { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="source", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "source")]
    public int? Source { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="status", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status")]
    public int? Status { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="jobType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobType")]
    public int? JobType { get; set; }

    /// <summary>
    /// Gets or Sets JobQueueResults
    /// </summary>
    [DataMember(Name="jobQueueResults", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobQueueResults")]
    public List<JobQueueResultsDTO> JobQueueResults { get; set; }

    /// <summary>
    /// Gets or Sets JobQueueFiles
    /// </summary>
    [DataMember(Name="jobQueueFiles", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobQueueFiles")]
    public List<JobQueueFilesDTO> JobQueueFiles { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class JobQueueDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  JobName: ").Append(JobName).Append("\n");
      sb.Append("  JobVariablesJson: ").Append(JobVariablesJson).Append("\n");
      sb.Append("  DateCreatedUtc: ").Append(DateCreatedUtc).Append("\n");
      sb.Append("  LastRanUtc: ").Append(LastRanUtc).Append("\n");
      sb.Append("  Source: ").Append(Source).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  JobType: ").Append(JobType).Append("\n");
      sb.Append("  JobQueueResults: ").Append(JobQueueResults).Append("\n");
      sb.Append("  JobQueueFiles: ").Append(JobQueueFiles).Append("\n");
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
