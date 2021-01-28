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
  public class FuelRequestDetails {
    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets Iata
    /// </summary>
    [DataMember(Name="iata", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "iata")]
    public string Iata { get; set; }

    /// <summary>
    /// Gets or Sets Fbo
    /// </summary>
    [DataMember(Name="fbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fbo")]
    public string Fbo { get; set; }

    /// <summary>
    /// Gets or Sets AirportId
    /// </summary>
    [DataMember(Name="airportId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airportId")]
    public int? AirportId { get; set; }

    /// <summary>
    /// Gets or Sets FlightType
    /// </summary>
    [DataMember(Name="flightType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightType")]
    public string FlightType { get; set; }

    /// <summary>
    /// Valid values are \"Arrival\" and \"Departure\"
    /// </summary>
    /// <value>Valid values are \"Arrival\" and \"Departure\"</value>
    [DataMember(Name="fuelOn", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelOn")]
    public string FuelOn { get; set; }

    /// <summary>
    /// Gets or Sets NoFuel
    /// </summary>
    [DataMember(Name="noFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "noFuel")]
    public bool? NoFuel { get; set; }

    /// <summary>
    /// Gets or Sets PaymentMethod
    /// </summary>
    [DataMember(Name="paymentMethod", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paymentMethod")]
    public string PaymentMethod { get; set; }

    /// <summary>
    /// Gets or Sets FuelAmount
    /// </summary>
    [DataMember(Name="fuelAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelAmount")]
    public Weight FuelAmount { get; set; }

    /// <summary>
    /// Gets or Sets ReportedRampFee
    /// </summary>
    [DataMember(Name="reportedRampFee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reportedRampFee")]
    public double? ReportedRampFee { get; set; }

    /// <summary>
    /// Gets or Sets ReportedRampFeeWaivedAt
    /// </summary>
    [DataMember(Name="reportedRampFeeWaivedAt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reportedRampFeeWaivedAt")]
    public Weight ReportedRampFeeWaivedAt { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalDate
    /// </summary>
    [DataMember(Name="arrivalDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalDate")]
    public string ArrivalDate { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalTime
    /// </summary>
    [DataMember(Name="arrivalTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalTime")]
    public string ArrivalTime { get; set; }

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
    [DataMember(Name="timeStandard", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timeStandard")]
    public int? TimeStandard { get; set; }

    /// <summary>
    /// ICAO of the next destination
    /// </summary>
    /// <value>ICAO of the next destination</value>
    [DataMember(Name="nextDestination", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "nextDestination")]
    public string NextDestination { get; set; }

    /// <summary>
    /// Gets or Sets HandlerName
    /// </summary>
    [DataMember(Name="handlerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlerName")]
    public string HandlerName { get; set; }

    /// <summary>
    /// Gets or Sets HandlerEmail
    /// </summary>
    [DataMember(Name="handlerEmail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlerEmail")]
    public string HandlerEmail { get; set; }

    /// <summary>
    /// Gets or Sets CustomerOrderNotes
    /// </summary>
    [DataMember(Name="customerOrderNotes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "customerOrderNotes")]
    public List<string> CustomerOrderNotes { get; set; }

    /// <summary>
    /// Gets or Sets TailNumber
    /// </summary>
    [DataMember(Name="tailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumber")]
    public string TailNumber { get; set; }

    /// <summary>
    /// Gets or Sets UserCurrentTimeZone
    /// </summary>
    [DataMember(Name="userCurrentTimeZone", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userCurrentTimeZone")]
    public string UserCurrentTimeZone { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FuelRequestDetails {\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  Iata: ").Append(Iata).Append("\n");
      sb.Append("  Fbo: ").Append(Fbo).Append("\n");
      sb.Append("  AirportId: ").Append(AirportId).Append("\n");
      sb.Append("  FlightType: ").Append(FlightType).Append("\n");
      sb.Append("  FuelOn: ").Append(FuelOn).Append("\n");
      sb.Append("  NoFuel: ").Append(NoFuel).Append("\n");
      sb.Append("  PaymentMethod: ").Append(PaymentMethod).Append("\n");
      sb.Append("  FuelAmount: ").Append(FuelAmount).Append("\n");
      sb.Append("  ReportedRampFee: ").Append(ReportedRampFee).Append("\n");
      sb.Append("  ReportedRampFeeWaivedAt: ").Append(ReportedRampFeeWaivedAt).Append("\n");
      sb.Append("  ArrivalDate: ").Append(ArrivalDate).Append("\n");
      sb.Append("  ArrivalTime: ").Append(ArrivalTime).Append("\n");
      sb.Append("  DepartureDate: ").Append(DepartureDate).Append("\n");
      sb.Append("  DepartureTime: ").Append(DepartureTime).Append("\n");
      sb.Append("  TimeStandard: ").Append(TimeStandard).Append("\n");
      sb.Append("  NextDestination: ").Append(NextDestination).Append("\n");
      sb.Append("  HandlerName: ").Append(HandlerName).Append("\n");
      sb.Append("  HandlerEmail: ").Append(HandlerEmail).Append("\n");
      sb.Append("  CustomerOrderNotes: ").Append(CustomerOrderNotes).Append("\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
      sb.Append("  UserCurrentTimeZone: ").Append(UserCurrentTimeZone).Append("\n");
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
