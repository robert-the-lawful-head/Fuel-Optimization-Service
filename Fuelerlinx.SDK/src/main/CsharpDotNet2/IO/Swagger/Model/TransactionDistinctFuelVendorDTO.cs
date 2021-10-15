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
  public class TransactionDistinctFuelVendorDTO {
    /// <summary>
    /// Gets or Sets FuelVendorName
    /// </summary>
    [DataMember(Name="fuelVendorName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelVendorName")]
    public string FuelVendorName { get; set; }

    /// <summary>
    /// Gets or Sets FuelVendorId
    /// </summary>
    [DataMember(Name="fuelVendorId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelVendorId")]
    public int? FuelVendorId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TransactionDistinctFuelVendorDTO {\n");
      sb.Append("  FuelVendorName: ").Append(FuelVendorName).Append("\n");
      sb.Append("  FuelVendorId: ").Append(FuelVendorId).Append("\n");
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
