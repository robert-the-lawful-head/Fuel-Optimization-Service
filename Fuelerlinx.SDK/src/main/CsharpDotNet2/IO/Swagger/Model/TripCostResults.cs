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
  public class TripCostResults {
    /// <summary>
    /// Gets or Sets Items
    /// </summary>
    [DataMember(Name="items", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "items")]
    public List<ResultItem> Items { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Gets or Sets TotalFlightCost
    /// </summary>
    [DataMember(Name="totalFlightCost", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "totalFlightCost")]
    public double? TotalFlightCost { get; set; }

    /// <summary>
    /// Gets or Sets ExtraFuelCost
    /// </summary>
    [DataMember(Name="extraFuelCost", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "extraFuelCost")]
    public double? ExtraFuelCost { get; set; }

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
      sb.Append("class TripCostResults {\n");
      sb.Append("  Items: ").Append(Items).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      sb.Append("  TotalFlightCost: ").Append(TotalFlightCost).Append("\n");
      sb.Append("  ExtraFuelCost: ").Append(ExtraFuelCost).Append("\n");
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
