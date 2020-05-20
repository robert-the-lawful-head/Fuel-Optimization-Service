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
  public class AccountInfoAndSettingsSubFeaturesDTO {
    /// <summary>
    /// Gets or Sets ContactInfo
    /// </summary>
    [DataMember(Name="ContactInfo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ContactInfo")]
    public MenuFeatureDTO ContactInfo { get; set; }

    /// <summary>
    /// Gets or Sets Password
    /// </summary>
    [DataMember(Name="Password", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Password")]
    public MenuFeatureDTO Password { get; set; }

    /// <summary>
    /// Gets or Sets DispatchPreferences
    /// </summary>
    [DataMember(Name="DispatchPreferences", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "DispatchPreferences")]
    public MenuFeatureDTO DispatchPreferences { get; set; }

    /// <summary>
    /// Gets or Sets TankeringPreferences
    /// </summary>
    [DataMember(Name="TankeringPreferences", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "TankeringPreferences")]
    public MenuFeatureDTO TankeringPreferences { get; set; }

    /// <summary>
    /// Gets or Sets TransactionSettings
    /// </summary>
    [DataMember(Name="TransactionSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "TransactionSettings")]
    public MenuFeatureDTO TransactionSettings { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingSettings
    /// </summary>
    [DataMember(Name="SchedulingSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "SchedulingSettings")]
    public MenuFeatureDTO SchedulingSettings { get; set; }

    /// <summary>
    /// Gets or Sets InternationalSettings
    /// </summary>
    [DataMember(Name="InternationalSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "InternationalSettings")]
    public MenuFeatureDTO InternationalSettings { get; set; }

    /// <summary>
    /// Gets or Sets SummaryCustomizations
    /// </summary>
    [DataMember(Name="SummaryCustomizations", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "SummaryCustomizations")]
    public MenuFeatureDTO SummaryCustomizations { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AccountInfoAndSettingsSubFeaturesDTO {\n");
      sb.Append("  ContactInfo: ").Append(ContactInfo).Append("\n");
      sb.Append("  Password: ").Append(Password).Append("\n");
      sb.Append("  DispatchPreferences: ").Append(DispatchPreferences).Append("\n");
      sb.Append("  TankeringPreferences: ").Append(TankeringPreferences).Append("\n");
      sb.Append("  TransactionSettings: ").Append(TransactionSettings).Append("\n");
      sb.Append("  SchedulingSettings: ").Append(SchedulingSettings).Append("\n");
      sb.Append("  InternationalSettings: ").Append(InternationalSettings).Append("\n");
      sb.Append("  SummaryCustomizations: ").Append(SummaryCustomizations).Append("\n");
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
