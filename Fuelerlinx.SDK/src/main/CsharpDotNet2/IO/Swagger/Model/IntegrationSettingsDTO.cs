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
  public class IntegrationSettingsDTO {
    /// <summary>
    /// Gets or Sets OID
    /// </summary>
    [DataMember(Name="OID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "OID")]
    public int? OID { get; set; }

    /// <summary>
    /// Gets or Sets IsMLTEnabled
    /// </summary>
    [DataMember(Name="IsMLTEnabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "IsMLTEnabled")]
    public bool? IsMLTEnabled { get; set; }

    /// <summary>
    /// Gets or Sets UserID
    /// </summary>
    [DataMember(Name="UserID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "UserID")]
    public int? UserID { get; set; }

    /// <summary>
    /// Gets or Sets SortType
    /// </summary>
    [DataMember(Name="SortType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "SortType")]
    public int? SortType { get; set; }

    /// <summary>
    /// Gets or Sets OmitPreferredFBOs
    /// </summary>
    [DataMember(Name="OmitPreferredFBOs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "OmitPreferredFBOs")]
    public bool? OmitPreferredFBOs { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingLoginUserName
    /// </summary>
    [DataMember(Name="SchedulingLoginUserName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "SchedulingLoginUserName")]
    public string SchedulingLoginUserName { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingLoginPassword
    /// </summary>
    [DataMember(Name="SchedulingLoginPassword", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "SchedulingLoginPassword")]
    public string SchedulingLoginPassword { get; set; }

    /// <summary>
    /// Gets or Sets PushSkippedLegsToTripSheet
    /// </summary>
    [DataMember(Name="PushSkippedLegsToTripSheet", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "PushSkippedLegsToTripSheet")]
    public bool? PushSkippedLegsToTripSheet { get; set; }

    /// <summary>
    /// Gets or Sets UseScheduledETEDomestic
    /// </summary>
    [DataMember(Name="UseScheduledETEDomestic", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "UseScheduledETEDomestic")]
    public bool? UseScheduledETEDomestic { get; set; }

    /// <summary>
    /// Gets or Sets UseScheduledETEInternational
    /// </summary>
    [DataMember(Name="UseScheduledETEInternational", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "UseScheduledETEInternational")]
    public bool? UseScheduledETEInternational { get; set; }

    /// <summary>
    /// Gets or Sets IsDefaultSchedulingAccount
    /// </summary>
    [DataMember(Name="IsDefaultSchedulingAccount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "IsDefaultSchedulingAccount")]
    public bool? IsDefaultSchedulingAccount { get; set; }

    /// <summary>
    /// Gets or Sets UserPreferredFBO
    /// </summary>
    [DataMember(Name="UserPreferredFBO", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "UserPreferredFBO")]
    public bool? UserPreferredFBO { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class IntegrationSettingsDTO {\n");
      sb.Append("  OID: ").Append(OID).Append("\n");
      sb.Append("  IsMLTEnabled: ").Append(IsMLTEnabled).Append("\n");
      sb.Append("  UserID: ").Append(UserID).Append("\n");
      sb.Append("  SortType: ").Append(SortType).Append("\n");
      sb.Append("  OmitPreferredFBOs: ").Append(OmitPreferredFBOs).Append("\n");
      sb.Append("  SchedulingLoginUserName: ").Append(SchedulingLoginUserName).Append("\n");
      sb.Append("  SchedulingLoginPassword: ").Append(SchedulingLoginPassword).Append("\n");
      sb.Append("  PushSkippedLegsToTripSheet: ").Append(PushSkippedLegsToTripSheet).Append("\n");
      sb.Append("  UseScheduledETEDomestic: ").Append(UseScheduledETEDomestic).Append("\n");
      sb.Append("  UseScheduledETEInternational: ").Append(UseScheduledETEInternational).Append("\n");
      sb.Append("  IsDefaultSchedulingAccount: ").Append(IsDefaultSchedulingAccount).Append("\n");
      sb.Append("  UserPreferredFBO: ").Append(UserPreferredFBO).Append("\n");
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
