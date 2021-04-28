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
  public class ReportGroupDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

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
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets DateCreatedUtc
    /// </summary>
    [DataMember(Name="dateCreatedUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateCreatedUtc")]
    public DateTime? DateCreatedUtc { get; set; }

    /// <summary>
    /// Gets or Sets SettingsJson
    /// </summary>
    [DataMember(Name="settingsJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "settingsJson")]
    public string SettingsJson { get; set; }

    /// <summary>
    /// Gets or Sets ReportGroupAssociations
    /// </summary>
    [DataMember(Name="reportGroupAssociations", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reportGroupAssociations")]
    public List<ReportGroupAssociationsDTO> ReportGroupAssociations { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ReportGroupDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  DateCreatedUtc: ").Append(DateCreatedUtc).Append("\n");
      sb.Append("  SettingsJson: ").Append(SettingsJson).Append("\n");
      sb.Append("  ReportGroupAssociations: ").Append(ReportGroupAssociations).Append("\n");
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
