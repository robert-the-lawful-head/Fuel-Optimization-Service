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
  public class ReportGroupAssociationsDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets ReportGroupId
    /// </summary>
    [DataMember(Name="reportGroupId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reportGroupId")]
    public int? ReportGroupId { get; set; }

    /// <summary>
    /// Gets or Sets ReportId
    /// </summary>
    [DataMember(Name="reportId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reportId")]
    public int? ReportId { get; set; }

    /// <summary>
    /// Gets or Sets Report
    /// </summary>
    [DataMember(Name="report", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "report")]
    public ReportDTO Report { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ReportGroupAssociationsDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  ReportGroupId: ").Append(ReportGroupId).Append("\n");
      sb.Append("  ReportId: ").Append(ReportId).Append("\n");
      sb.Append("  Report: ").Append(Report).Append("\n");
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
