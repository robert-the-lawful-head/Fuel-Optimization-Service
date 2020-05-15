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
  public class SettingsSubFeaturesDTO {
    /// <summary>
    /// Gets or Sets ContactUs
    /// </summary>
    [DataMember(Name="ContactUs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ContactUs")]
    public MenuFeatureDTO ContactUs { get; set; }

    /// <summary>
    /// Gets or Sets AccountInfoAndSettings
    /// </summary>
    [DataMember(Name="AccountInfoAndSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "AccountInfoAndSettings")]
    public AccountInfoAndSettingsDTO AccountInfoAndSettings { get; set; }

    /// <summary>
    /// Gets or Sets AircraftListAndPerformance
    /// </summary>
    [DataMember(Name="AircraftListAndPerformance", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "AircraftListAndPerformance")]
    public MenuFeatureDTO AircraftListAndPerformance { get; set; }

    /// <summary>
    /// Gets or Sets FuelPricingManagement
    /// </summary>
    [DataMember(Name="FuelPricingManagement", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FuelPricingManagement")]
    public FuelPricingManagementDTO FuelPricingManagement { get; set; }

    /// <summary>
    /// Gets or Sets ManageRampFees
    /// </summary>
    [DataMember(Name="ManageRampFees", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ManageRampFees")]
    public MenuFeatureDTO ManageRampFees { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SettingsSubFeaturesDTO {\n");
      sb.Append("  ContactUs: ").Append(ContactUs).Append("\n");
      sb.Append("  AccountInfoAndSettings: ").Append(AccountInfoAndSettings).Append("\n");
      sb.Append("  AircraftListAndPerformance: ").Append(AircraftListAndPerformance).Append("\n");
      sb.Append("  FuelPricingManagement: ").Append(FuelPricingManagement).Append("\n");
      sb.Append("  ManageRampFees: ").Append(ManageRampFees).Append("\n");
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
