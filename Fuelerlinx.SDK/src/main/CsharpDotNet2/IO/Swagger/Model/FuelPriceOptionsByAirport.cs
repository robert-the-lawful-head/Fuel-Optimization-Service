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
  public class FuelPriceOptionsByAirport {
    /// <summary>
    /// Gets or Sets AirportIdentifier
    /// </summary>
    [DataMember(Name="airportIdentifier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airportIdentifier")]
    public string AirportIdentifier { get; set; }

    /// <summary>
    /// Gets or Sets AcukwikAirportId
    /// </summary>
    [DataMember(Name="acukwikAirportId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "acukwikAirportId")]
    public int? AcukwikAirportId { get; set; }

    /// <summary>
    /// Gets or Sets FuelPriceOptions
    /// </summary>
    [DataMember(Name="fuelPriceOptions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelPriceOptions")]
    public List<FuelPriceOption> FuelPriceOptions { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FuelPriceOptionsByAirport {\n");
      sb.Append("  AirportIdentifier: ").Append(AirportIdentifier).Append("\n");
      sb.Append("  AcukwikAirportId: ").Append(AcukwikAirportId).Append("\n");
      sb.Append("  FuelPriceOptions: ").Append(FuelPriceOptions).Append("\n");
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
