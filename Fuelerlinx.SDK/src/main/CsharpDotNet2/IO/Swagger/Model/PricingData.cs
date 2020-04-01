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
  public class PricingData {
    /// <summary>
    /// Gets or Sets SelectedFuelOption
    /// </summary>
    [DataMember(Name="selectedFuelOption", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "selectedFuelOption")]
    public FuelPriceOption SelectedFuelOption { get; set; }

    /// <summary>
    /// Gets or Sets RampFeeInDollars
    /// </summary>
    [DataMember(Name="rampFeeInDollars", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFeeInDollars")]
    public double? RampFeeInDollars { get; set; }

    /// <summary>
    /// Gets or Sets RampFeeWaivedAtGallonAmount
    /// </summary>
    [DataMember(Name="rampFeeWaivedAtGallonAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFeeWaivedAtGallonAmount")]
    public double? RampFeeWaivedAtGallonAmount { get; set; }

    /// <summary>
    /// Gets or Sets OverrideExistingRampFees
    /// </summary>
    [DataMember(Name="overrideExistingRampFees", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "overrideExistingRampFees")]
    public bool? OverrideExistingRampFees { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PricingData {\n");
      sb.Append("  SelectedFuelOption: ").Append(SelectedFuelOption).Append("\n");
      sb.Append("  RampFeeInDollars: ").Append(RampFeeInDollars).Append("\n");
      sb.Append("  RampFeeWaivedAtGallonAmount: ").Append(RampFeeWaivedAtGallonAmount).Append("\n");
      sb.Append("  OverrideExistingRampFees: ").Append(OverrideExistingRampFees).Append("\n");
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
