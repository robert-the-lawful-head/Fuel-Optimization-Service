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
  public class FuelBurnProfile {
    /// <summary>
    /// Gets or Sets Items
    /// </summary>
    [DataMember(Name="items", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "items")]
    public List<ResultItem> Items { get; set; }

    /// <summary>
    /// Gets or Sets IsTankering
    /// </summary>
    [DataMember(Name="isTankering", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isTankering")]
    public bool? IsTankering { get; set; }

    /// <summary>
    /// Gets or Sets WeightUnit
    /// </summary>
    [DataMember(Name="weightUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "weightUnit")]
    public Weight WeightUnit { get; set; }

    /// <summary>
    /// Gets or Sets LegNumber
    /// </summary>
    [DataMember(Name="legNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legNumber")]
    public int? LegNumber { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FuelBurnProfile {\n");
      sb.Append("  Items: ").Append(Items).Append("\n");
      sb.Append("  IsTankering: ").Append(IsTankering).Append("\n");
      sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
      sb.Append("  LegNumber: ").Append(LegNumber).Append("\n");
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
