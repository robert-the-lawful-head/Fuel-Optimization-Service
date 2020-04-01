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
  public class TransactionLegSettingsDTO {
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
    /// Gets or Sets MinReserve
    /// </summary>
    [DataMember(Name="minReserve", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minReserve")]
    public Weight MinReserve { get; set; }

    /// <summary>
    /// Gets or Sets MaxTakeoffWeight
    /// </summary>
    [DataMember(Name="maxTakeoffWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxTakeoffWeight")]
    public Weight MaxTakeoffWeight { get; set; }

    /// <summary>
    /// Gets or Sets MaxLandingWeight
    /// </summary>
    [DataMember(Name="maxLandingWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxLandingWeight")]
    public Weight MaxLandingWeight { get; set; }

    /// <summary>
    /// Gets or Sets FlightLevel
    /// </summary>
    [DataMember(Name="flightLevel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightLevel")]
    public int? FlightLevel { get; set; }

    /// <summary>
    /// Gets or Sets NumberOfPassengers
    /// </summary>
    [DataMember(Name="numberOfPassengers", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "numberOfPassengers")]
    public int? NumberOfPassengers { get; set; }

    /// <summary>
    /// Gets or Sets CargoWeight
    /// </summary>
    [DataMember(Name="cargoWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cargoWeight")]
    public Weight CargoWeight { get; set; }

    /// <summary>
    /// Gets or Sets PassengerWeight
    /// </summary>
    [DataMember(Name="passengerWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "passengerWeight")]
    public Weight PassengerWeight { get; set; }

    /// <summary>
    /// Gets or Sets UseDirectRoute
    /// </summary>
    [DataMember(Name="useDirectRoute", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "useDirectRoute")]
    public bool? UseDirectRoute { get; set; }

    /// <summary>
    /// Gets or Sets IsEtopsflight
    /// </summary>
    [DataMember(Name="isEtopsflight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isEtopsflight")]
    public bool? IsEtopsflight { get; set; }

    /// <summary>
    /// Gets or Sets FuelBias
    /// </summary>
    [DataMember(Name="fuelBias", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelBias")]
    public double? FuelBias { get; set; }

    /// <summary>
    /// Gets or Sets SpeedBias
    /// </summary>
    [DataMember(Name="speedBias", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "speedBias")]
    public double? SpeedBias { get; set; }

    /// <summary>
    /// Gets or Sets AlternateAirport
    /// </summary>
    [DataMember(Name="alternateAirport", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "alternateAirport")]
    public string AlternateAirport { get; set; }

    /// <summary>
    /// Gets or Sets FlightType
    /// </summary>
    [DataMember(Name="flightType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightType")]
    public string FlightType { get; set; }

    /// <summary>
    /// Gets or Sets NextDestination
    /// </summary>
    [DataMember(Name="nextDestination", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "nextDestination")]
    public string NextDestination { get; set; }

    /// <summary>
    /// Gets or Sets LandWithMaxFuel
    /// </summary>
    [DataMember(Name="landWithMaxFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "landWithMaxFuel")]
    public bool? LandWithMaxFuel { get; set; }

    /// <summary>
    /// Gets or Sets UseFlightData
    /// </summary>
    [DataMember(Name="useFlightData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "useFlightData")]
    public bool? UseFlightData { get; set; }

    /// <summary>
    /// Gets or Sets LockFuelAmount
    /// </summary>
    [DataMember(Name="lockFuelAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lockFuelAmount")]
    public bool? LockFuelAmount { get; set; }

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
    /// Gets or Sets CustomOrderNotes
    /// </summary>
    [DataMember(Name="customOrderNotes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "customOrderNotes")]
    public string CustomOrderNotes { get; set; }

    /// <summary>
    /// Gets or Sets CompanyAlias
    /// </summary>
    [DataMember(Name="companyAlias", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyAlias")]
    public string CompanyAlias { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TransactionLegSettingsDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  RequestId: ").Append(RequestId).Append("\n");
      sb.Append("  MinReserve: ").Append(MinReserve).Append("\n");
      sb.Append("  MaxTakeoffWeight: ").Append(MaxTakeoffWeight).Append("\n");
      sb.Append("  MaxLandingWeight: ").Append(MaxLandingWeight).Append("\n");
      sb.Append("  FlightLevel: ").Append(FlightLevel).Append("\n");
      sb.Append("  NumberOfPassengers: ").Append(NumberOfPassengers).Append("\n");
      sb.Append("  CargoWeight: ").Append(CargoWeight).Append("\n");
      sb.Append("  PassengerWeight: ").Append(PassengerWeight).Append("\n");
      sb.Append("  UseDirectRoute: ").Append(UseDirectRoute).Append("\n");
      sb.Append("  IsEtopsflight: ").Append(IsEtopsflight).Append("\n");
      sb.Append("  FuelBias: ").Append(FuelBias).Append("\n");
      sb.Append("  SpeedBias: ").Append(SpeedBias).Append("\n");
      sb.Append("  AlternateAirport: ").Append(AlternateAirport).Append("\n");
      sb.Append("  FlightType: ").Append(FlightType).Append("\n");
      sb.Append("  NextDestination: ").Append(NextDestination).Append("\n");
      sb.Append("  LandWithMaxFuel: ").Append(LandWithMaxFuel).Append("\n");
      sb.Append("  UseFlightData: ").Append(UseFlightData).Append("\n");
      sb.Append("  LockFuelAmount: ").Append(LockFuelAmount).Append("\n");
      sb.Append("  HandlerName: ").Append(HandlerName).Append("\n");
      sb.Append("  HandlerEmail: ").Append(HandlerEmail).Append("\n");
      sb.Append("  CustomOrderNotes: ").Append(CustomOrderNotes).Append("\n");
      sb.Append("  CompanyAlias: ").Append(CompanyAlias).Append("\n");
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
