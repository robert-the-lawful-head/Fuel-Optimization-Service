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
  public class UserSettingsDTO {
    /// <summary>
    /// Gets or Sets Preference
    /// </summary>
    [DataMember(Name="Preference", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Preference")]
    public PreferenceDTO Preference { get; set; }

    /// <summary>
    /// Gets or Sets TankeringSettings
    /// </summary>
    [DataMember(Name="TankeringSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "TankeringSettings")]
    public TankeringSettingsDTO TankeringSettings { get; set; }

    /// <summary>
    /// Gets or Sets AnalysisSettings
    /// </summary>
    [DataMember(Name="AnalysisSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "AnalysisSettings")]
    public AnalysisSettingsDTO AnalysisSettings { get; set; }

    /// <summary>
    /// Gets or Sets IntegrationSettings
    /// </summary>
    [DataMember(Name="IntegrationSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "IntegrationSettings")]
    public IntegrationSettingsDTO IntegrationSettings { get; set; }

    /// <summary>
    /// Gets or Sets TransactionSettings
    /// </summary>
    [DataMember(Name="TransactionSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "TransactionSettings")]
    public TransactionSettingsDTO TransactionSettings { get; set; }

    /// <summary>
    /// Gets or Sets FlightPlanningSettings
    /// </summary>
    [DataMember(Name="FlightPlanningSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FlightPlanningSettings")]
    public FlightPlanningSettingsDTO FlightPlanningSettings { get; set; }

    /// <summary>
    /// Gets or Sets AccessibleFeatures
    /// </summary>
    [DataMember(Name="AccessibleFeatures", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "AccessibleFeatures")]
    public AccessibleFeaturesDTO AccessibleFeatures { get; set; }

    /// <summary>
    /// Gets or Sets InternationalSettings
    /// </summary>
    [DataMember(Name="InternationalSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "InternationalSettings")]
    public InternationalSettingsDTO InternationalSettings { get; set; }

    /// <summary>
    /// Gets or Sets WeightConversions
    /// </summary>
    [DataMember(Name="WeightConversions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "WeightConversions")]
    public List<WeightConversionDTO> WeightConversions { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UserSettingsDTO {\n");
      sb.Append("  Preference: ").Append(Preference).Append("\n");
      sb.Append("  TankeringSettings: ").Append(TankeringSettings).Append("\n");
      sb.Append("  AnalysisSettings: ").Append(AnalysisSettings).Append("\n");
      sb.Append("  IntegrationSettings: ").Append(IntegrationSettings).Append("\n");
      sb.Append("  TransactionSettings: ").Append(TransactionSettings).Append("\n");
      sb.Append("  FlightPlanningSettings: ").Append(FlightPlanningSettings).Append("\n");
      sb.Append("  AccessibleFeatures: ").Append(AccessibleFeatures).Append("\n");
      sb.Append("  InternationalSettings: ").Append(InternationalSettings).Append("\n");
      sb.Append("  WeightConversions: ").Append(WeightConversions).Append("\n");
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
