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
  public class UpdateReportScheduledDistributionRequest {
    /// <summary>
    /// Gets or Sets ReportScheduledDistribution
    /// </summary>
    [DataMember(Name="reportScheduledDistribution", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reportScheduledDistribution")]
    public ReportScheduledDistributionDTO ReportScheduledDistribution { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateReportScheduledDistributionRequest {\n");
      sb.Append("  ReportScheduledDistribution: ").Append(ReportScheduledDistribution).Append("\n");
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
