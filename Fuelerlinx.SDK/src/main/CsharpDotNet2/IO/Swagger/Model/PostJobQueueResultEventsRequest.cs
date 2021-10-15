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
  public class PostJobQueueResultEventsRequest {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets JobQueueResultId
    /// </summary>
    [DataMember(Name="jobQueueResultId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobQueueResultId")]
    public int? JobQueueResultId { get; set; }

    /// <summary>
    /// Gets or Sets EventLabel
    /// </summary>
    [DataMember(Name="eventLabel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "eventLabel")]
    public string EventLabel { get; set; }

    /// <summary>
    /// Gets or Sets EventDescription
    /// </summary>
    [DataMember(Name="eventDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "eventDescription")]
    public string EventDescription { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="eventType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "eventType")]
    public int? EventType { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostJobQueueResultEventsRequest {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  JobQueueResultId: ").Append(JobQueueResultId).Append("\n");
      sb.Append("  EventLabel: ").Append(EventLabel).Append("\n");
      sb.Append("  EventDescription: ").Append(EventDescription).Append("\n");
      sb.Append("  EventType: ").Append(EventType).Append("\n");
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
