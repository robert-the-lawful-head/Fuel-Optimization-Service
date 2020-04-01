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
  public class Distance {
    /// <summary>
    /// Distance units:             0 = Feet             1 = Kilometer             2 = Meter             3 = Nautical Mile    * `Feet` - Feet  * `Kilometer` - Kilometer  * `Meter` - Meter  * `NauticalMile` - Nautical Mile  
    /// </summary>
    /// <value>Distance units:             0 = Feet             1 = Kilometer             2 = Meter             3 = Nautical Mile    * `Feet` - Feet  * `Kilometer` - Kilometer  * `Meter` - Meter  * `NauticalMile` - Nautical Mile  </value>
    [DataMember(Name="unit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "unit")]
    public int? Unit { get; set; }

    /// <summary>
    /// Gets or Sets Amount
    /// </summary>
    [DataMember(Name="amount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "amount")]
    public double? Amount { get; set; }

    /// <summary>
    /// Gets or Sets UnitDescription
    /// </summary>
    [DataMember(Name="unitDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "unitDescription")]
    public string UnitDescription { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Distance {\n");
      sb.Append("  Unit: ").Append(Unit).Append("\n");
      sb.Append("  Amount: ").Append(Amount).Append("\n");
      sb.Append("  UnitDescription: ").Append(UnitDescription).Append("\n");
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
