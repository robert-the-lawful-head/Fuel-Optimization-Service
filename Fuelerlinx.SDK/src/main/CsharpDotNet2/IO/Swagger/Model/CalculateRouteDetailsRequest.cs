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
  public class CalculateRouteDetailsRequest {
    /// <summary>
    /// Gets or Sets TailNumber
    /// </summary>
    [DataMember(Name="tailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumber")]
    public string TailNumber { get; set; }

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
    /// Gets or Sets DepartureDateTime
    /// </summary>
    [DataMember(Name="departureDateTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureDateTime")]
    public DateTime? DepartureDateTime { get; set; }

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
    /// Gets or Sets IFlightPlannerCruiseProfileId
    /// </summary>
    [DataMember(Name="iFlightPlannerCruiseProfileId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "iFlightPlannerCruiseProfileId")]
    public int? IFlightPlannerCruiseProfileId { get; set; }

    /// <summary>
    /// Routing Types:             0 = Optimal             1 = Direct             2 = Custom    * `Optimal` - Optimal  * `Direct` - Direct  * `Custom` - Customer  
    /// </summary>
    /// <value>Routing Types:             0 = Optimal             1 = Direct             2 = Custom    * `Optimal` - Optimal  * `Direct` - Direct  * `Custom` - Customer  </value>
    [DataMember(Name="routingType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "routingType")]
    public int? RoutingType { get; set; }

    /// <summary>
    /// Gets or Sets CustomRoute
    /// </summary>
    [DataMember(Name="customRoute", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "customRoute")]
    public string CustomRoute { get; set; }

    /// <summary>
    /// Gets or Sets AlternateAirport
    /// </summary>
    [DataMember(Name="alternateAirport", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "alternateAirport")]
    public string AlternateAirport { get; set; }

    /// <summary>
    /// Gets or Sets AlternateFuel
    /// </summary>
    [DataMember(Name="alternateFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "alternateFuel")]
    public Weight AlternateFuel { get; set; }

    /// <summary>
    /// Gets or Sets ReserveFuel
    /// </summary>
    [DataMember(Name="reserveFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reserveFuel")]
    public Weight ReserveFuel { get; set; }

    /// <summary>
    /// Gets or Sets HoldTime
    /// </summary>
    [DataMember(Name="holdTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "holdTime")]
    public Time HoldTime { get; set; }

    /// <summary>
    /// Gets or Sets CruiseAirSpeed
    /// </summary>
    [DataMember(Name="cruiseAirSpeed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cruiseAirSpeed")]
    public Speed CruiseAirSpeed { get; set; }

    /// <summary>
    /// Gets or Sets MaxAltitudeInFeet
    /// </summary>
    [DataMember(Name="maxAltitudeInFeet", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxAltitudeInFeet")]
    public int? MaxAltitudeInFeet { get; set; }

    /// <summary>
    /// Gets or Sets SpecificAltitudeInFeet
    /// </summary>
    [DataMember(Name="specificAltitudeInFeet", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "specificAltitudeInFeet")]
    public int? SpecificAltitudeInFeet { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CalculateRouteDetailsRequest {\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
      sb.Append("  DepartureAirportIdentifier: ").Append(DepartureAirportIdentifier).Append("\n");
      sb.Append("  ArrivalAirportIdentifier: ").Append(ArrivalAirportIdentifier).Append("\n");
      sb.Append("  DepartureDateTime: ").Append(DepartureDateTime).Append("\n");
      sb.Append("  MaxLandingWeight: ").Append(MaxLandingWeight).Append("\n");
      sb.Append("  MaxTakeoffWeight: ").Append(MaxTakeoffWeight).Append("\n");
      sb.Append("  CargoWeight: ").Append(CargoWeight).Append("\n");
      sb.Append("  NumberOfPassengers: ").Append(NumberOfPassengers).Append("\n");
      sb.Append("  TotalPassengerWeight: ").Append(TotalPassengerWeight).Append("\n");
      sb.Append("  IFlightPlannerCruiseProfileId: ").Append(IFlightPlannerCruiseProfileId).Append("\n");
      sb.Append("  RoutingType: ").Append(RoutingType).Append("\n");
      sb.Append("  CustomRoute: ").Append(CustomRoute).Append("\n");
      sb.Append("  AlternateAirport: ").Append(AlternateAirport).Append("\n");
      sb.Append("  AlternateFuel: ").Append(AlternateFuel).Append("\n");
      sb.Append("  ReserveFuel: ").Append(ReserveFuel).Append("\n");
      sb.Append("  HoldTime: ").Append(HoldTime).Append("\n");
      sb.Append("  CruiseAirSpeed: ").Append(CruiseAirSpeed).Append("\n");
      sb.Append("  MaxAltitudeInFeet: ").Append(MaxAltitudeInFeet).Append("\n");
      sb.Append("  SpecificAltitudeInFeet: ").Append(SpecificAltitudeInFeet).Append("\n");
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
