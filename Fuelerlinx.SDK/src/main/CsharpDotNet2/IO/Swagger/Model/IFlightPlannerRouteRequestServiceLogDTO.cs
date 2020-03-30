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
  public class IFlightPlannerRouteRequestServiceLogDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets RequestDate
    /// </summary>
    [DataMember(Name="requestDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "requestDate")]
    public DateTime? RequestDate { get; set; }

    /// <summary>
    /// Gets or Sets Apikey
    /// </summary>
    [DataMember(Name="apikey", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "apikey")]
    public string Apikey { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets UserId
    /// </summary>
    [DataMember(Name="userId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userId")]
    public int? UserId { get; set; }

    /// <summary>
    /// Gets or Sets TailNumber
    /// </summary>
    [DataMember(Name="tailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumber")]
    public string TailNumber { get; set; }

    /// <summary>
    /// Gets or Sets DepartureAirport
    /// </summary>
    [DataMember(Name="departureAirport", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureAirport")]
    public string DepartureAirport { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalAirport
    /// </summary>
    [DataMember(Name="arrivalAirport", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalAirport")]
    public string ArrivalAirport { get; set; }

    /// <summary>
    /// Gets or Sets Request
    /// </summary>
    [DataMember(Name="request", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "request")]
    public string Request { get; set; }

    /// <summary>
    /// Gets or Sets Response
    /// </summary>
    [DataMember(Name="response", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "response")]
    public string Response { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class IFlightPlannerRouteRequestServiceLogDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  RequestDate: ").Append(RequestDate).Append("\n");
      sb.Append("  Apikey: ").Append(Apikey).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
      sb.Append("  DepartureAirport: ").Append(DepartureAirport).Append("\n");
      sb.Append("  ArrivalAirport: ").Append(ArrivalAirport).Append("\n");
      sb.Append("  Request: ").Append(Request).Append("\n");
      sb.Append("  Response: ").Append(Response).Append("\n");
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
