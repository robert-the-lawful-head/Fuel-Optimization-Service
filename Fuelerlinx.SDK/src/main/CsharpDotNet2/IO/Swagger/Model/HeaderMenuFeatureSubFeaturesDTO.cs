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
  public class HeaderMenuFeatureSubFeaturesDTO {
    /// <summary>
    /// Gets or Sets Settings
    /// </summary>
    [DataMember(Name="Settings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Settings")]
    public SettingsDTO Settings { get; set; }

    /// <summary>
    /// Gets or Sets TeamManager
    /// </summary>
    [DataMember(Name="TeamManager", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "TeamManager")]
    public MenuFeatureDTO TeamManager { get; set; }

    /// <summary>
    /// Gets or Sets PaymentMethods
    /// </summary>
    [DataMember(Name="PaymentMethods", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "PaymentMethods")]
    public MenuFeatureDTO PaymentMethods { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class HeaderMenuFeatureSubFeaturesDTO {\n");
      sb.Append("  Settings: ").Append(Settings).Append("\n");
      sb.Append("  TeamManager: ").Append(TeamManager).Append("\n");
      sb.Append("  PaymentMethods: ").Append(PaymentMethods).Append("\n");
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
