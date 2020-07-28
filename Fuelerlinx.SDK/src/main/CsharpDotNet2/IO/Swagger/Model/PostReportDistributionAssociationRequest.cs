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
  public class PostReportDistributionAssociationRequest {
    /// <summary>
    /// Gets or Sets ReportId
    /// </summary>
    [DataMember(Name="reportId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reportId")]
    public int? ReportId { get; set; }

    /// <summary>
    /// Gets or Sets ScheduledDistributionId
    /// </summary>
    [DataMember(Name="scheduledDistributionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "scheduledDistributionId")]
    public int? ScheduledDistributionId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostReportDistributionAssociationRequest {\n");
      sb.Append("  ReportId: ").Append(ReportId).Append("\n");
      sb.Append("  ScheduledDistributionId: ").Append(ScheduledDistributionId).Append("\n");
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
