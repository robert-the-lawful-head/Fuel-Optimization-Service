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
  public class WeightAndBalanceOptions {
    /// <summary>
    /// Gets or Sets MaxLandingWeight
    /// </summary>
    [DataMember(Name="maxLandingWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxLandingWeight")]
    public Weight MaxLandingWeight { get; set; }

    /// <summary>
    /// Gets or Sets MaxTakeoffWeight
    /// </summary>
    [DataMember(Name="maxTakeoffWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxTakeoffWeight")]
    public Weight MaxTakeoffWeight { get; set; }

    /// <summary>
    /// Gets or Sets CargoWeight
    /// </summary>
    [DataMember(Name="cargoWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cargoWeight")]
    public Weight CargoWeight { get; set; }

    /// <summary>
    /// Gets or Sets NumberOfPassengers
    /// </summary>
    [DataMember(Name="numberOfPassengers", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "numberOfPassengers")]
    public int? NumberOfPassengers { get; set; }

    /// <summary>
    /// Gets or Sets TotalPassengerWeight
    /// </summary>
    [DataMember(Name="totalPassengerWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "totalPassengerWeight")]
    public Weight TotalPassengerWeight { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class WeightAndBalanceOptions {\n");
      sb.Append("  MaxLandingWeight: ").Append(MaxLandingWeight).Append("\n");
      sb.Append("  MaxTakeoffWeight: ").Append(MaxTakeoffWeight).Append("\n");
      sb.Append("  CargoWeight: ").Append(CargoWeight).Append("\n");
      sb.Append("  NumberOfPassengers: ").Append(NumberOfPassengers).Append("\n");
      sb.Append("  TotalPassengerWeight: ").Append(TotalPassengerWeight).Append("\n");
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
