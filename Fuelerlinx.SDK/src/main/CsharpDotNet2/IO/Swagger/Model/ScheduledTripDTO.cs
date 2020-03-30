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
  public class ScheduledTripDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets TailNumber
    /// </summary>
    [DataMember(Name="tailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumber")]
    public string TailNumber { get; set; }

    /// <summary>
    /// Gets or Sets DepartureIcao
    /// </summary>
    [DataMember(Name="departureIcao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureIcao")]
    public string DepartureIcao { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalIcao
    /// </summary>
    [DataMember(Name="arrivalIcao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalIcao")]
    public string ArrivalIcao { get; set; }

    /// <summary>
    /// Gets or Sets TripId
    /// </summary>
    [DataMember(Name="tripId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripId")]
    public string TripId { get; set; }

    /// <summary>
    /// Gets or Sets LegNumber
    /// </summary>
    [DataMember(Name="legNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legNumber")]
    public int? LegNumber { get; set; }

    /// <summary>
    /// Gets or Sets DepartureDateTimeZulu
    /// </summary>
    [DataMember(Name="departureDateTimeZulu", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureDateTimeZulu")]
    public DateTime? DepartureDateTimeZulu { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalDateTimeZulu
    /// </summary>
    [DataMember(Name="arrivalDateTimeZulu", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalDateTimeZulu")]
    public DateTime? ArrivalDateTimeZulu { get; set; }

    /// <summary>
    /// Gets or Sets DepartureDateTimeLocal
    /// </summary>
    [DataMember(Name="departureDateTimeLocal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureDateTimeLocal")]
    public DateTime? DepartureDateTimeLocal { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalDateTimeLocal
    /// </summary>
    [DataMember(Name="arrivalDateTimeLocal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalDateTimeLocal")]
    public DateTime? ArrivalDateTimeLocal { get; set; }

    /// <summary>
    /// Gets or Sets PaxCount
    /// </summary>
    [DataMember(Name="paxCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paxCount")]
    public int? PaxCount { get; set; }

    /// <summary>
    /// Gets or Sets CargoLbs
    /// </summary>
    [DataMember(Name="cargoLbs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cargoLbs")]
    public int? CargoLbs { get; set; }

    /// <summary>
    /// Gets or Sets PaxWeight
    /// </summary>
    [DataMember(Name="paxWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paxWeight")]
    public int? PaxWeight { get; set; }

    /// <summary>
    /// Gets or Sets DeparturePreferredFbo
    /// </summary>
    [DataMember(Name="departurePreferredFbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departurePreferredFbo")]
    public string DeparturePreferredFbo { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalPreferredFbo
    /// </summary>
    [DataMember(Name="arrivalPreferredFbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalPreferredFbo")]
    public string ArrivalPreferredFbo { get; set; }

    /// <summary>
    /// Gets or Sets TripType
    /// </summary>
    [DataMember(Name="tripType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripType")]
    public string TripType { get; set; }

    /// <summary>
    /// Gets or Sets FlightId
    /// </summary>
    [DataMember(Name="flightId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightId")]
    public string FlightId { get; set; }

    /// <summary>
    /// Gets or Sets CustomerName
    /// </summary>
    [DataMember(Name="customerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "customerName")]
    public string CustomerName { get; set; }

    /// <summary>
    /// Gets or Sets Mtow
    /// </summary>
    [DataMember(Name="mtow", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mtow")]
    public int? Mtow { get; set; }

    /// <summary>
    /// Gets or Sets FuelOut
    /// </summary>
    [DataMember(Name="fuelOut", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelOut")]
    public int? FuelOut { get; set; }

    /// <summary>
    /// Gets or Sets FuelIn
    /// </summary>
    [DataMember(Name="fuelIn", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelIn")]
    public int? FuelIn { get; set; }

    /// <summary>
    /// Gets or Sets FemalePaxCount
    /// </summary>
    [DataMember(Name="femalePaxCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "femalePaxCount")]
    public int? FemalePaxCount { get; set; }

    /// <summary>
    /// Gets or Sets MalePaxCount
    /// </summary>
    [DataMember(Name="malePaxCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "malePaxCount")]
    public int? MalePaxCount { get; set; }

    /// <summary>
    /// Gets or Sets DepartureRemarks
    /// </summary>
    [DataMember(Name="departureRemarks", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureRemarks")]
    public string DepartureRemarks { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalRemarks
    /// </summary>
    [DataMember(Name="arrivalRemarks", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalRemarks")]
    public string ArrivalRemarks { get; set; }

    /// <summary>
    /// Gets or Sets FlightPlanId
    /// </summary>
    [DataMember(Name="flightPlanId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightPlanId")]
    public string FlightPlanId { get; set; }

    /// <summary>
    /// Gets or Sets TripKey
    /// </summary>
    [DataMember(Name="tripKey", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripKey")]
    public string TripKey { get; set; }

    /// <summary>
    /// Gets or Sets Company
    /// </summary>
    [DataMember(Name="company", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "company")]
    public CompanyDTO Company { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ScheduledTripDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
      sb.Append("  DepartureIcao: ").Append(DepartureIcao).Append("\n");
      sb.Append("  ArrivalIcao: ").Append(ArrivalIcao).Append("\n");
      sb.Append("  TripId: ").Append(TripId).Append("\n");
      sb.Append("  LegNumber: ").Append(LegNumber).Append("\n");
      sb.Append("  DepartureDateTimeZulu: ").Append(DepartureDateTimeZulu).Append("\n");
      sb.Append("  ArrivalDateTimeZulu: ").Append(ArrivalDateTimeZulu).Append("\n");
      sb.Append("  DepartureDateTimeLocal: ").Append(DepartureDateTimeLocal).Append("\n");
      sb.Append("  ArrivalDateTimeLocal: ").Append(ArrivalDateTimeLocal).Append("\n");
      sb.Append("  PaxCount: ").Append(PaxCount).Append("\n");
      sb.Append("  CargoLbs: ").Append(CargoLbs).Append("\n");
      sb.Append("  PaxWeight: ").Append(PaxWeight).Append("\n");
      sb.Append("  DeparturePreferredFbo: ").Append(DeparturePreferredFbo).Append("\n");
      sb.Append("  ArrivalPreferredFbo: ").Append(ArrivalPreferredFbo).Append("\n");
      sb.Append("  TripType: ").Append(TripType).Append("\n");
      sb.Append("  FlightId: ").Append(FlightId).Append("\n");
      sb.Append("  CustomerName: ").Append(CustomerName).Append("\n");
      sb.Append("  Mtow: ").Append(Mtow).Append("\n");
      sb.Append("  FuelOut: ").Append(FuelOut).Append("\n");
      sb.Append("  FuelIn: ").Append(FuelIn).Append("\n");
      sb.Append("  FemalePaxCount: ").Append(FemalePaxCount).Append("\n");
      sb.Append("  MalePaxCount: ").Append(MalePaxCount).Append("\n");
      sb.Append("  DepartureRemarks: ").Append(DepartureRemarks).Append("\n");
      sb.Append("  ArrivalRemarks: ").Append(ArrivalRemarks).Append("\n");
      sb.Append("  FlightPlanId: ").Append(FlightPlanId).Append("\n");
      sb.Append("  TripKey: ").Append(TripKey).Append("\n");
      sb.Append("  Company: ").Append(Company).Append("\n");
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
