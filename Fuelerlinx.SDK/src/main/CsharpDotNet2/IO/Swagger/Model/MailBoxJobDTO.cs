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
  public class MailBoxJobDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets EmailAddress
    /// </summary>
    [DataMember(Name="emailAddress", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "emailAddress")]
    public string EmailAddress { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets JobName
    /// </summary>
    [DataMember(Name="jobName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "jobName")]
    public string JobName { get; set; }

    /// <summary>
    /// Gets or Sets ParametersJson
    /// </summary>
    [DataMember(Name="parametersJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "parametersJson")]
    public string ParametersJson { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MailBoxJobDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  EmailAddress: ").Append(EmailAddress).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  JobName: ").Append(JobName).Append("\n");
      sb.Append("  ParametersJson: ").Append(ParametersJson).Append("\n");
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
