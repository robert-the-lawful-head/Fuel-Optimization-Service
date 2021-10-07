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
  public class MainMenuFeatureSubFeaturesDTO {
    /// <summary>
    /// Gets or Sets Dashboard
    /// </summary>
    [DataMember(Name="Dashboard", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Dashboard")]
    public MenuFeatureDTO Dashboard { get; set; }

    /// <summary>
    /// Gets or Sets Integration
    /// </summary>
    [DataMember(Name="Integration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Integration")]
    public MenuFeatureDTO Integration { get; set; }

    /// <summary>
    /// Gets or Sets Transactions
    /// </summary>
    [DataMember(Name="Transactions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Transactions")]
    public MenuFeatureDTO Transactions { get; set; }

    /// <summary>
    /// Gets or Sets Analysis
    /// </summary>
    [DataMember(Name="Analysis", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Analysis")]
    public MenuFeatureDTO Analysis { get; set; }

    /// <summary>
    /// Gets or Sets GroundControl
    /// </summary>
    [DataMember(Name="GroundControl", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "GroundControl")]
    public MenuFeatureDTO GroundControl { get; set; }

    /// <summary>
    /// Gets or Sets Calendar
    /// </summary>
    [DataMember(Name="Calendar", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Calendar")]
    public MenuFeatureDTO Calendar { get; set; }

    /// <summary>
    /// Gets or Sets FlightPlan
    /// </summary>
    [DataMember(Name="FlightPlan", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FlightPlan")]
    public MenuFeatureDTO FlightPlan { get; set; }

    /// <summary>
    /// Gets or Sets FuelPlanner
    /// </summary>
    [DataMember(Name="FuelPlanner", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FuelPlanner")]
    public MenuFeatureDTO FuelPlanner { get; set; }

    /// <summary>
    /// Gets or Sets VendorManagement
    /// </summary>
    [DataMember(Name="VendorManagement", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "VendorManagement")]
    public MenuFeatureDTO VendorManagement { get; set; }

    /// <summary>
    /// Gets or Sets PricesAndFees
    /// </summary>
    [DataMember(Name="PricesAndFees", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "PricesAndFees")]
    public MenuFeatureDTO PricesAndFees { get; set; }

    /// <summary>
    /// Gets or Sets AirProxima
    /// </summary>
    [DataMember(Name="AirProxima", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "AirProxima")]
    public MenuFeatureDTO AirProxima { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MainMenuFeatureSubFeaturesDTO {\n");
      sb.Append("  Dashboard: ").Append(Dashboard).Append("\n");
      sb.Append("  Integration: ").Append(Integration).Append("\n");
      sb.Append("  Transactions: ").Append(Transactions).Append("\n");
      sb.Append("  Analysis: ").Append(Analysis).Append("\n");
      sb.Append("  GroundControl: ").Append(GroundControl).Append("\n");
      sb.Append("  Calendar: ").Append(Calendar).Append("\n");
      sb.Append("  FlightPlan: ").Append(FlightPlan).Append("\n");
      sb.Append("  FuelPlanner: ").Append(FuelPlanner).Append("\n");
      sb.Append("  VendorManagement: ").Append(VendorManagement).Append("\n");
      sb.Append("  PricesAndFees: ").Append(PricesAndFees).Append("\n");
      sb.Append("  AirProxima: ").Append(AirProxima).Append("\n");
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
