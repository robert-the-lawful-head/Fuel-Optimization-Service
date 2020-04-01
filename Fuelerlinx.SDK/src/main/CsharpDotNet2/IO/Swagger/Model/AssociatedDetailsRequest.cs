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
  public class AssociatedDetailsRequest {
    /// <summary>
    /// Gets or Sets FuelPriceOption
    /// </summary>
    [DataMember(Name="fuelPriceOption", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelPriceOption")]
    public FuelPriceOption FuelPriceOption { get; set; }

    /// <summary>
    /// Gets or Sets TailNumber
    /// </summary>
    [DataMember(Name="tailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumber")]
    public string TailNumber { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AssociatedDetailsRequest {\n");
      sb.Append("  FuelPriceOption: ").Append(FuelPriceOption).Append("\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
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
