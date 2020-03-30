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
  public class UpdateFuelPurchaseInfoRequest {
    /// <summary>
    /// Gets or Sets FuelPurchaseInfo
    /// </summary>
    [DataMember(Name="fuelPurchaseInfo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelPurchaseInfo")]
    public FuelPriceOption FuelPurchaseInfo { get; set; }

    /// <summary>
    /// Gets or Sets FlightPlanTripId
    /// </summary>
    [DataMember(Name="flightPlanTripId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightPlanTripId")]
    public string FlightPlanTripId { get; set; }

    /// <summary>
    /// Gets or Sets RouteSegmentFlightId
    /// </summary>
    [DataMember(Name="routeSegmentFlightId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "routeSegmentFlightId")]
    public string RouteSegmentFlightId { get; set; }

    /// <summary>
    /// Gets or Sets TransactionId
    /// </summary>
    [DataMember(Name="transactionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionId")]
    public int? TransactionId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="flightPlanningProvider", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightPlanningProvider")]
    public int? FlightPlanningProvider { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateFuelPurchaseInfoRequest {\n");
      sb.Append("  FuelPurchaseInfo: ").Append(FuelPurchaseInfo).Append("\n");
      sb.Append("  FlightPlanTripId: ").Append(FlightPlanTripId).Append("\n");
      sb.Append("  RouteSegmentFlightId: ").Append(RouteSegmentFlightId).Append("\n");
      sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
      sb.Append("  FlightPlanningProvider: ").Append(FlightPlanningProvider).Append("\n");
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
