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
  public class Leg {
    /// <summary>
    /// Gets or Sets LegNumber
    /// </summary>
    [DataMember(Name="legNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legNumber")]
    public int? LegNumber { get; set; }

    /// <summary>
    /// Gets or Sets LegId
    /// </summary>
    [DataMember(Name="legId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legId")]
    public int? LegId { get; set; }

    /// <summary>
    /// Gets or Sets DepartureAirportIdentifier
    /// </summary>
    [DataMember(Name="departureAirportIdentifier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureAirportIdentifier")]
    public string DepartureAirportIdentifier { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalAirportIdentifier
    /// </summary>
    [DataMember(Name="arrivalAirportIdentifier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalAirportIdentifier")]
    public string ArrivalAirportIdentifier { get; set; }

    /// <summary>
    /// Gets or Sets DepartureDate
    /// </summary>
    [DataMember(Name="departureDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureDate")]
    public string DepartureDate { get; set; }

    /// <summary>
    /// Gets or Sets DepartureTime
    /// </summary>
    [DataMember(Name="departureTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureTime")]
    public string DepartureTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="departureType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureType")]
    public int? DepartureType { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingInformation
    /// </summary>
    [DataMember(Name="schedulingInformation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "schedulingInformation")]
    public ScheduledTripDTO SchedulingInformation { get; set; }

    /// <summary>
    /// Gets or Sets LegPricingData
    /// </summary>
    [DataMember(Name="legPricingData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legPricingData")]
    public PricingData LegPricingData { get; set; }

    /// <summary>
    /// Gets or Sets LegTankeringOptions
    /// </summary>
    [DataMember(Name="legTankeringOptions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legTankeringOptions")]
    public LegTankeringOptions LegTankeringOptions { get; set; }

    /// <summary>
    /// Gets or Sets FlightDataWithoutTankeredFuel
    /// </summary>
    [DataMember(Name="flightDataWithoutTankeredFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightDataWithoutTankeredFuel")]
    public RouteDetailsCalculationWithNavLog FlightDataWithoutTankeredFuel { get; set; }

    /// <summary>
    /// Gets or Sets FlightDataWithTankeredFuel
    /// </summary>
    [DataMember(Name="flightDataWithTankeredFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightDataWithTankeredFuel")]
    public RouteDetailsCalculationWithNavLog FlightDataWithTankeredFuel { get; set; }

    /// <summary>
    /// Gets or Sets WeightAndBalanceOptions
    /// </summary>
    [DataMember(Name="weightAndBalanceOptions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "weightAndBalanceOptions")]
    public WeightAndBalanceOptions WeightAndBalanceOptions { get; set; }

    /// <summary>
    /// Gets or Sets LegFlightPlanningRequest
    /// </summary>
    [DataMember(Name="legFlightPlanningRequest", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legFlightPlanningRequest")]
    public LegFlightPlanningRequest LegFlightPlanningRequest { get; set; }

    /// <summary>
    /// This property is to only be used if FuelerLinx has approved your custom flight data in JSON form.  Please notify techsupport@fuelerlinx.com if you'd like to provide your own flight data.
    /// </summary>
    /// <value>This property is to only be used if FuelerLinx has approved your custom flight data in JSON form.  Please notify techsupport@fuelerlinx.com if you'd like to provide your own flight data.</value>
    [DataMember(Name="flightDataWithoutTankeredFuelJSON", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightDataWithoutTankeredFuelJSON")]
    public string FlightDataWithoutTankeredFuelJSON { get; set; }

    /// <summary>
    /// This property is to only be used if FuelerLinx has approved your custom flight data in JSON form.  Please notify techsupport@fuelerlinx.com if you'd like to provide your own flight data.
    /// </summary>
    /// <value>This property is to only be used if FuelerLinx has approved your custom flight data in JSON form.  Please notify techsupport@fuelerlinx.com if you'd like to provide your own flight data.</value>
    [DataMember(Name="flightDataWithTankeredFuelJSON", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightDataWithTankeredFuelJSON")]
    public string FlightDataWithTankeredFuelJSON { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Leg {\n");
      sb.Append("  LegNumber: ").Append(LegNumber).Append("\n");
      sb.Append("  LegId: ").Append(LegId).Append("\n");
      sb.Append("  DepartureAirportIdentifier: ").Append(DepartureAirportIdentifier).Append("\n");
      sb.Append("  ArrivalAirportIdentifier: ").Append(ArrivalAirportIdentifier).Append("\n");
      sb.Append("  DepartureDate: ").Append(DepartureDate).Append("\n");
      sb.Append("  DepartureTime: ").Append(DepartureTime).Append("\n");
      sb.Append("  DepartureType: ").Append(DepartureType).Append("\n");
      sb.Append("  SchedulingInformation: ").Append(SchedulingInformation).Append("\n");
      sb.Append("  LegPricingData: ").Append(LegPricingData).Append("\n");
      sb.Append("  LegTankeringOptions: ").Append(LegTankeringOptions).Append("\n");
      sb.Append("  FlightDataWithoutTankeredFuel: ").Append(FlightDataWithoutTankeredFuel).Append("\n");
      sb.Append("  FlightDataWithTankeredFuel: ").Append(FlightDataWithTankeredFuel).Append("\n");
      sb.Append("  WeightAndBalanceOptions: ").Append(WeightAndBalanceOptions).Append("\n");
      sb.Append("  LegFlightPlanningRequest: ").Append(LegFlightPlanningRequest).Append("\n");
      sb.Append("  FlightDataWithoutTankeredFuelJSON: ").Append(FlightDataWithoutTankeredFuelJSON).Append("\n");
      sb.Append("  FlightDataWithTankeredFuelJSON: ").Append(FlightDataWithTankeredFuelJSON).Append("\n");
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
