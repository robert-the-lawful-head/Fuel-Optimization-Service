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
  public class IntegrationDispatchDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets RequestId
    /// </summary>
    [DataMember(Name="requestId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "requestId")]
    public int? RequestId { get; set; }

    /// <summary>
    /// Gets or Sets IntegrationType
    /// </summary>
    [DataMember(Name="integrationType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "integrationType")]
    public int? IntegrationType { get; set; }

    /// <summary>
    /// Gets or Sets TripsAircraftId
    /// </summary>
    [DataMember(Name="tripsAircraftId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripsAircraftId")]
    public string TripsAircraftId { get; set; }

    /// <summary>
    /// Gets or Sets TripsTripNumber
    /// </summary>
    [DataMember(Name="tripsTripNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripsTripNumber")]
    public int? TripsTripNumber { get; set; }

    /// <summary>
    /// Gets or Sets LegsKiddate
    /// </summary>
    [DataMember(Name="legsKiddate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legsKiddate")]
    public int? LegsKiddate { get; set; }

    /// <summary>
    /// Gets or Sets LegsKidtime
    /// </summary>
    [DataMember(Name="legsKidtime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legsKidtime")]
    public int? LegsKidtime { get; set; }

    /// <summary>
    /// Gets or Sets LegsKiduser
    /// </summary>
    [DataMember(Name="legsKiduser", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legsKiduser")]
    public string LegsKiduser { get; set; }

    /// <summary>
    /// Gets or Sets LegsKidmult
    /// </summary>
    [DataMember(Name="legsKidmult", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legsKidmult")]
    public int? LegsKidmult { get; set; }

    /// <summary>
    /// Gets or Sets LegsKidcomm
    /// </summary>
    [DataMember(Name="legsKidcomm", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legsKidcomm")]
    public int? LegsKidcomm { get; set; }

    /// <summary>
    /// Gets or Sets TripsTripStatusClosed
    /// </summary>
    [DataMember(Name="tripsTripStatusClosed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripsTripStatusClosed")]
    public bool? TripsTripStatusClosed { get; set; }

    /// <summary>
    /// Gets or Sets TripsAircraftTypeId
    /// </summary>
    [DataMember(Name="tripsAircraftTypeId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripsAircraftTypeId")]
    public string TripsAircraftTypeId { get; set; }

    /// <summary>
    /// Gets or Sets TripsDepartDateZulu
    /// </summary>
    [DataMember(Name="tripsDepartDateZulu", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripsDepartDateZulu")]
    public int? TripsDepartDateZulu { get; set; }

    /// <summary>
    /// Gets or Sets TripsAircraftTailNumber
    /// </summary>
    [DataMember(Name="tripsAircraftTailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripsAircraftTailNumber")]
    public string TripsAircraftTailNumber { get; set; }

    /// <summary>
    /// Gets or Sets LegsAircraftId
    /// </summary>
    [DataMember(Name="legsAircraftId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legsAircraftId")]
    public string LegsAircraftId { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalIcao
    /// </summary>
    [DataMember(Name="arrivalIcao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalIcao")]
    public string ArrivalIcao { get; set; }

    /// <summary>
    /// Gets or Sets Etdzulu
    /// </summary>
    [DataMember(Name="etdzulu", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "etdzulu")]
    public int? Etdzulu { get; set; }

    /// <summary>
    /// Gets or Sets Etazulu
    /// </summary>
    [DataMember(Name="etazulu", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "etazulu")]
    public int? Etazulu { get; set; }

    /// <summary>
    /// Gets or Sets DepartDateZulu
    /// </summary>
    [DataMember(Name="departDateZulu", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departDateZulu")]
    public int? DepartDateZulu { get; set; }

    /// <summary>
    /// Gets or Sets ArriveDateZulu
    /// </summary>
    [DataMember(Name="arriveDateZulu", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arriveDateZulu")]
    public int? ArriveDateZulu { get; set; }

    /// <summary>
    /// Gets or Sets Etdlocal
    /// </summary>
    [DataMember(Name="etdlocal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "etdlocal")]
    public int? Etdlocal { get; set; }

    /// <summary>
    /// Gets or Sets Etalocal
    /// </summary>
    [DataMember(Name="etalocal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "etalocal")]
    public int? Etalocal { get; set; }

    /// <summary>
    /// Gets or Sets DepartDateLocal
    /// </summary>
    [DataMember(Name="departDateLocal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departDateLocal")]
    public int? DepartDateLocal { get; set; }

    /// <summary>
    /// Gets or Sets ArriveDateLocal
    /// </summary>
    [DataMember(Name="arriveDateLocal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arriveDateLocal")]
    public int? ArriveDateLocal { get; set; }

    /// <summary>
    /// Gets or Sets LegNumber
    /// </summary>
    [DataMember(Name="legNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legNumber")]
    public int? LegNumber { get; set; }

    /// <summary>
    /// Gets or Sets Fuelburn
    /// </summary>
    [DataMember(Name="fuelburn", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelburn")]
    public int? Fuelburn { get; set; }

    /// <summary>
    /// Gets or Sets DepartIcao
    /// </summary>
    [DataMember(Name="departIcao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departIcao")]
    public string DepartIcao { get; set; }

    /// <summary>
    /// Gets or Sets Status
    /// </summary>
    [DataMember(Name="status", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status")]
    public int? Status { get; set; }

    /// <summary>
    /// Gets or Sets Cancelled
    /// </summary>
    [DataMember(Name="cancelled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cancelled")]
    public bool? Cancelled { get; set; }

    /// <summary>
    /// Gets or Sets CancelCode
    /// </summary>
    [DataMember(Name="cancelCode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cancelCode")]
    public int? CancelCode { get; set; }

    /// <summary>
    /// Gets or Sets PreferFbo
    /// </summary>
    [DataMember(Name="preferFbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "preferFbo")]
    public string PreferFbo { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalRequestId
    /// </summary>
    [DataMember(Name="arrivalRequestId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalRequestId")]
    public int? ArrivalRequestId { get; set; }

    /// <summary>
    /// Gets or Sets DepartureRequestId
    /// </summary>
    [DataMember(Name="departureRequestId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureRequestId")]
    public int? DepartureRequestId { get; set; }

    /// <summary>
    /// Gets or Sets Viewed
    /// </summary>
    [DataMember(Name="viewed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "viewed")]
    public bool? Viewed { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalPrefFbo
    /// </summary>
    [DataMember(Name="arrivalPrefFbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalPrefFbo")]
    public string ArrivalPrefFbo { get; set; }

    /// <summary>
    /// Gets or Sets DeparturePrefFbo
    /// </summary>
    [DataMember(Name="departurePrefFbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departurePrefFbo")]
    public string DeparturePrefFbo { get; set; }

    /// <summary>
    /// Gets or Sets FoscustomerName
    /// </summary>
    [DataMember(Name="foscustomerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "foscustomerName")]
    public string FoscustomerName { get; set; }

    /// <summary>
    /// Gets or Sets TripType
    /// </summary>
    [DataMember(Name="tripType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripType")]
    public string TripType { get; set; }

    /// <summary>
    /// Gets or Sets CargoLbs
    /// </summary>
    [DataMember(Name="cargoLbs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cargoLbs")]
    public int? CargoLbs { get; set; }

    /// <summary>
    /// Gets or Sets Mtow
    /// </summary>
    [DataMember(Name="mtow", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mtow")]
    public int? Mtow { get; set; }

    /// <summary>
    /// Gets or Sets PaxCount
    /// </summary>
    [DataMember(Name="paxCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paxCount")]
    public int? PaxCount { get; set; }

    /// <summary>
    /// Gets or Sets Wind
    /// </summary>
    [DataMember(Name="wind", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "wind")]
    public int? Wind { get; set; }

    /// <summary>
    /// Gets or Sets Tas
    /// </summary>
    [DataMember(Name="tas", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tas")]
    public int? Tas { get; set; }

    /// <summary>
    /// Gets or Sets Altitude
    /// </summary>
    [DataMember(Name="altitude", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "altitude")]
    public int? Altitude { get; set; }

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
    /// Gets or Sets PaxWeight
    /// </summary>
    [DataMember(Name="paxWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paxWeight")]
    public int? PaxWeight { get; set; }

    /// <summary>
    /// Gets or Sets FlightPlanId
    /// </summary>
    [DataMember(Name="flightPlanId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightPlanId")]
    public string FlightPlanId { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingLegIdentifier
    /// </summary>
    [DataMember(Name="schedulingLegIdentifier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "schedulingLegIdentifier")]
    public string SchedulingLegIdentifier { get; set; }

    /// <summary>
    /// Gets or Sets TripId
    /// </summary>
    [DataMember(Name="tripId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripId")]
    public string TripId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class IntegrationDispatchDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  RequestId: ").Append(RequestId).Append("\n");
      sb.Append("  IntegrationType: ").Append(IntegrationType).Append("\n");
      sb.Append("  TripsAircraftId: ").Append(TripsAircraftId).Append("\n");
      sb.Append("  TripsTripNumber: ").Append(TripsTripNumber).Append("\n");
      sb.Append("  LegsKiddate: ").Append(LegsKiddate).Append("\n");
      sb.Append("  LegsKidtime: ").Append(LegsKidtime).Append("\n");
      sb.Append("  LegsKiduser: ").Append(LegsKiduser).Append("\n");
      sb.Append("  LegsKidmult: ").Append(LegsKidmult).Append("\n");
      sb.Append("  LegsKidcomm: ").Append(LegsKidcomm).Append("\n");
      sb.Append("  TripsTripStatusClosed: ").Append(TripsTripStatusClosed).Append("\n");
      sb.Append("  TripsAircraftTypeId: ").Append(TripsAircraftTypeId).Append("\n");
      sb.Append("  TripsDepartDateZulu: ").Append(TripsDepartDateZulu).Append("\n");
      sb.Append("  TripsAircraftTailNumber: ").Append(TripsAircraftTailNumber).Append("\n");
      sb.Append("  LegsAircraftId: ").Append(LegsAircraftId).Append("\n");
      sb.Append("  ArrivalIcao: ").Append(ArrivalIcao).Append("\n");
      sb.Append("  Etdzulu: ").Append(Etdzulu).Append("\n");
      sb.Append("  Etazulu: ").Append(Etazulu).Append("\n");
      sb.Append("  DepartDateZulu: ").Append(DepartDateZulu).Append("\n");
      sb.Append("  ArriveDateZulu: ").Append(ArriveDateZulu).Append("\n");
      sb.Append("  Etdlocal: ").Append(Etdlocal).Append("\n");
      sb.Append("  Etalocal: ").Append(Etalocal).Append("\n");
      sb.Append("  DepartDateLocal: ").Append(DepartDateLocal).Append("\n");
      sb.Append("  ArriveDateLocal: ").Append(ArriveDateLocal).Append("\n");
      sb.Append("  LegNumber: ").Append(LegNumber).Append("\n");
      sb.Append("  Fuelburn: ").Append(Fuelburn).Append("\n");
      sb.Append("  DepartIcao: ").Append(DepartIcao).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  Cancelled: ").Append(Cancelled).Append("\n");
      sb.Append("  CancelCode: ").Append(CancelCode).Append("\n");
      sb.Append("  PreferFbo: ").Append(PreferFbo).Append("\n");
      sb.Append("  ArrivalRequestId: ").Append(ArrivalRequestId).Append("\n");
      sb.Append("  DepartureRequestId: ").Append(DepartureRequestId).Append("\n");
      sb.Append("  Viewed: ").Append(Viewed).Append("\n");
      sb.Append("  ArrivalPrefFbo: ").Append(ArrivalPrefFbo).Append("\n");
      sb.Append("  DeparturePrefFbo: ").Append(DeparturePrefFbo).Append("\n");
      sb.Append("  FoscustomerName: ").Append(FoscustomerName).Append("\n");
      sb.Append("  TripType: ").Append(TripType).Append("\n");
      sb.Append("  CargoLbs: ").Append(CargoLbs).Append("\n");
      sb.Append("  Mtow: ").Append(Mtow).Append("\n");
      sb.Append("  PaxCount: ").Append(PaxCount).Append("\n");
      sb.Append("  Wind: ").Append(Wind).Append("\n");
      sb.Append("  Tas: ").Append(Tas).Append("\n");
      sb.Append("  Altitude: ").Append(Altitude).Append("\n");
      sb.Append("  FuelOut: ").Append(FuelOut).Append("\n");
      sb.Append("  FuelIn: ").Append(FuelIn).Append("\n");
      sb.Append("  FemalePaxCount: ").Append(FemalePaxCount).Append("\n");
      sb.Append("  MalePaxCount: ").Append(MalePaxCount).Append("\n");
      sb.Append("  PaxWeight: ").Append(PaxWeight).Append("\n");
      sb.Append("  FlightPlanId: ").Append(FlightPlanId).Append("\n");
      sb.Append("  SchedulingLegIdentifier: ").Append(SchedulingLegIdentifier).Append("\n");
      sb.Append("  TripId: ").Append(TripId).Append("\n");
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
