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
  public class GroupedTransactionCountByFboAndAirport {
    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets Fbo
    /// </summary>
    [DataMember(Name="fbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fbo")]
    public string Fbo { get; set; }

    /// <summary>
    /// Gets or Sets AirportOrders
    /// </summary>
    [DataMember(Name="airportOrders", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airportOrders")]
    public int? AirportOrders { get; set; }

    /// <summary>
    /// Gets or Sets FboOrders
    /// </summary>
    [DataMember(Name="fboOrders", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboOrders")]
    public int? FboOrders { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class GroupedTransactionCountByFboAndAirport {\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  Fbo: ").Append(Fbo).Append("\n");
      sb.Append("  AirportOrders: ").Append(AirportOrders).Append("\n");
      sb.Append("  FboOrders: ").Append(FboOrders).Append("\n");
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
