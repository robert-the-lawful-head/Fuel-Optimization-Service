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
  public class WeightConversionDTO {
    /// <summary>
    /// Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  
    /// </summary>
    /// <value>Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  </value>
    [DataMember(Name="fromUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fromUnit")]
    public int? FromUnit { get; set; }

    /// <summary>
    /// Gets or Sets FromUnitDescription
    /// </summary>
    [DataMember(Name="fromUnitDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fromUnitDescription")]
    public string FromUnitDescription { get; set; }

    /// <summary>
    /// Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  
    /// </summary>
    /// <value>Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  </value>
    [DataMember(Name="toUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "toUnit")]
    public int? ToUnit { get; set; }

    /// <summary>
    /// Gets or Sets ToUnitDescription
    /// </summary>
    [DataMember(Name="toUnitDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "toUnitDescription")]
    public string ToUnitDescription { get; set; }

    /// <summary>
    /// Gets or Sets Multiplier
    /// </summary>
    [DataMember(Name="multiplier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "multiplier")]
    public double? Multiplier { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class WeightConversionDTO {\n");
      sb.Append("  FromUnit: ").Append(FromUnit).Append("\n");
      sb.Append("  FromUnitDescription: ").Append(FromUnitDescription).Append("\n");
      sb.Append("  ToUnit: ").Append(ToUnit).Append("\n");
      sb.Append("  ToUnitDescription: ").Append(ToUnitDescription).Append("\n");
      sb.Append("  Multiplier: ").Append(Multiplier).Append("\n");
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
