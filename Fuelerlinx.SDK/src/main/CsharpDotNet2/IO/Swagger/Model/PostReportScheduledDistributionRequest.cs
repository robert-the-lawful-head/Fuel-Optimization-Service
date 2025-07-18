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
  public class PostReportScheduledDistributionRequest {
    /// <summary>
    /// Gets or Sets EmailRecipients
    /// </summary>
    [DataMember(Name="emailRecipients", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "emailRecipients")]
    public List<string> EmailRecipients { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="reportPeriod", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reportPeriod")]
    public int? ReportPeriod { get; set; }

    /// <summary>
    /// Gets or Sets StartDateUtc
    /// </summary>
    [DataMember(Name="startDateUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "startDateUtc")]
    public DateTime? StartDateUtc { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="sendFrequency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sendFrequency")]
    public int? SendFrequency { get; set; }

    /// <summary>
    /// Gets or Sets LastSentUtc
    /// </summary>
    [DataMember(Name="lastSentUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lastSentUtc")]
    public DateTime? LastSentUtc { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets Filter
    /// </summary>
    [DataMember(Name="filter", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "filter")]
    public ReportFilter Filter { get; set; }

    /// <summary>
    /// Gets or Sets EmailSubject
    /// </summary>
    [DataMember(Name="emailSubject", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "emailSubject")]
    public string EmailSubject { get; set; }

    /// <summary>
    /// Gets or Sets IsEnabled
    /// </summary>
    [DataMember(Name="isEnabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isEnabled")]
    public bool? IsEnabled { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostReportScheduledDistributionRequest {\n");
      sb.Append("  EmailRecipients: ").Append(EmailRecipients).Append("\n");
      sb.Append("  ReportPeriod: ").Append(ReportPeriod).Append("\n");
      sb.Append("  StartDateUtc: ").Append(StartDateUtc).Append("\n");
      sb.Append("  SendFrequency: ").Append(SendFrequency).Append("\n");
      sb.Append("  LastSentUtc: ").Append(LastSentUtc).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Filter: ").Append(Filter).Append("\n");
      sb.Append("  EmailSubject: ").Append(EmailSubject).Append("\n");
      sb.Append("  IsEnabled: ").Append(IsEnabled).Append("\n");
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
