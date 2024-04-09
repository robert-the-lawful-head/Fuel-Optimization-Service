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
  public class FbolinxCustomerFuelVendors {
    /// <summary>
    /// Gets or Sets FuelerLinxId
    /// </summary>
    [DataMember(Name="fuelerLinxId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerLinxId")]
    public int? FuelerLinxId { get; set; }

    /// <summary>
    /// Gets or Sets FuelVendors
    /// </summary>
    [DataMember(Name="fuelVendors", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelVendors")]
    public string FuelVendors { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FbolinxCustomerFuelVendors {\n");
      sb.Append("  FuelerLinxId: ").Append(FuelerLinxId).Append("\n");
      sb.Append("  FuelVendors: ").Append(FuelVendors).Append("\n");
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
