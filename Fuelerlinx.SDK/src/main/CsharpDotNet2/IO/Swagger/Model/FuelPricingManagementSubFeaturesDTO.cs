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
  public class FuelPricingManagementSubFeaturesDTO {
    /// <summary>
    /// Gets or Sets FuelVendorSetup
    /// </summary>
    [DataMember(Name="FuelVendorSetup", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FuelVendorSetup")]
    public MenuFeatureDTO FuelVendorSetup { get; set; }

    /// <summary>
    /// Gets or Sets ManageDirectPricing
    /// </summary>
    [DataMember(Name="ManageDirectPricing", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ManageDirectPricing")]
    public MenuFeatureDTO ManageDirectPricing { get; set; }

    /// <summary>
    /// Gets or Sets UploadPriceSheets
    /// </summary>
    [DataMember(Name="UploadPriceSheets", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "UploadPriceSheets")]
    public MenuFeatureDTO UploadPriceSheets { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FuelPricingManagementSubFeaturesDTO {\n");
      sb.Append("  FuelVendorSetup: ").Append(FuelVendorSetup).Append("\n");
      sb.Append("  ManageDirectPricing: ").Append(ManageDirectPricing).Append("\n");
      sb.Append("  UploadPriceSheets: ").Append(UploadPriceSheets).Append("\n");
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
