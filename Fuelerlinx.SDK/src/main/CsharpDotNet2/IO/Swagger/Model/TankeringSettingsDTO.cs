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
  public class TankeringSettingsDTO {
    /// <summary>
    /// Gets or Sets OmitAlternateFuel
    /// </summary>
    [DataMember(Name="OmitAlternateFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "OmitAlternateFuel")]
    public bool? OmitAlternateFuel { get; set; }

    /// <summary>
    /// Price Search Preference:             0 = SelectedOptionOnly             1 = SelectedFBOOnly             2 = AllOptions    * `SelectedOptionOnly` - Selected fuel option only  * `SelectedFBOOnly` - Selected FBO only  * `AllOptions` - All available fuel options  
    /// </summary>
    /// <value>Price Search Preference:             0 = SelectedOptionOnly             1 = SelectedFBOOnly             2 = AllOptions    * `SelectedOptionOnly` - Selected fuel option only  * `SelectedFBOOnly` - Selected FBO only  * `AllOptions` - All available fuel options  </value>
    [DataMember(Name="PriceSearchPreference", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "PriceSearchPreference")]
    public int? PriceSearchPreference { get; set; }

    /// <summary>
    /// Gets or Sets UserID
    /// </summary>
    [DataMember(Name="UserID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "UserID")]
    public int? UserID { get; set; }

    /// <summary>
    /// Gets or Sets OptimizeTankering
    /// </summary>
    [DataMember(Name="OptimizeTankering", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "OptimizeTankering")]
    public bool? OptimizeTankering { get; set; }

    /// <summary>
    /// Flight Data Options:             0 = False             1 = True             2 = Company Default    * `False` - False  * `True` - True  * `CompanyDefault` - CompanyDefault  
    /// </summary>
    /// <value>Flight Data Options:             0 = False             1 = True             2 = Company Default    * `False` - False  * `True` - True  * `CompanyDefault` - CompanyDefault  </value>
    [DataMember(Name="UseFlightData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "UseFlightData")]
    public int? UseFlightData { get; set; }

    /// <summary>
    /// Gets or Sets DefaultPassengerWeight
    /// </summary>
    [DataMember(Name="DefaultPassengerWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "DefaultPassengerWeight")]
    public double? DefaultPassengerWeight { get; set; }

    /// <summary>
    /// Gets or Sets DefaultCargoWeight
    /// </summary>
    [DataMember(Name="DefaultCargoWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "DefaultCargoWeight")]
    public double? DefaultCargoWeight { get; set; }

    /// <summary>
    /// Default Cargo Weight Types:             0 = PerPassenger             1 = Total    * `PerPassenger` - PerPassenger  * `Total` - Total  
    /// </summary>
    /// <value>Default Cargo Weight Types:             0 = PerPassenger             1 = Total    * `PerPassenger` - PerPassenger  * `Total` - Total  </value>
    [DataMember(Name="DefaultCargoWeightType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "DefaultCargoWeightType")]
    public int? DefaultCargoWeightType { get; set; }

    /// <summary>
    /// Gets or Sets ForceMinimumUpliftBias
    /// </summary>
    [DataMember(Name="ForceMinimumUpliftBias", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ForceMinimumUpliftBias")]
    public bool? ForceMinimumUpliftBias { get; set; }

    /// <summary>
    /// Gets or Sets MinimumUpliftBiasFeeAmount
    /// </summary>
    [DataMember(Name="MinimumUpliftBiasFeeAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "MinimumUpliftBiasFeeAmount")]
    public double? MinimumUpliftBiasFeeAmount { get; set; }

    /// <summary>
    /// Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  
    /// </summary>
    /// <value>Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  </value>
    [DataMember(Name="WeightUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "WeightUnit")]
    public int? WeightUnit { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TankeringSettingsDTO {\n");
      sb.Append("  OmitAlternateFuel: ").Append(OmitAlternateFuel).Append("\n");
      sb.Append("  PriceSearchPreference: ").Append(PriceSearchPreference).Append("\n");
      sb.Append("  UserID: ").Append(UserID).Append("\n");
      sb.Append("  OptimizeTankering: ").Append(OptimizeTankering).Append("\n");
      sb.Append("  UseFlightData: ").Append(UseFlightData).Append("\n");
      sb.Append("  DefaultPassengerWeight: ").Append(DefaultPassengerWeight).Append("\n");
      sb.Append("  DefaultCargoWeight: ").Append(DefaultCargoWeight).Append("\n");
      sb.Append("  DefaultCargoWeightType: ").Append(DefaultCargoWeightType).Append("\n");
      sb.Append("  ForceMinimumUpliftBias: ").Append(ForceMinimumUpliftBias).Append("\n");
      sb.Append("  MinimumUpliftBiasFeeAmount: ").Append(MinimumUpliftBiasFeeAmount).Append("\n");
      sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
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
