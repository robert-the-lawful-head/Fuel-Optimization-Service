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
  public class FlightPlansByCompanyDTO {
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
    /// Gets or Sets FlightPlanId
    /// </summary>
    [DataMember(Name="flightPlanId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightPlanId")]
    public string FlightPlanId { get; set; }

    /// <summary>
    /// Gets or Sets DepartureDateTimeZulu
    /// </summary>
    [DataMember(Name="departureDateTimeZulu", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureDateTimeZulu")]
    public DateTime? DepartureDateTimeZulu { get; set; }

    /// <summary>
    /// Gets or Sets DepartureDateTimeLocal
    /// </summary>
    [DataMember(Name="departureDateTimeLocal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureDateTimeLocal")]
    public DateTime? DepartureDateTimeLocal { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="flightPlanningProvider", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightPlanningProvider")]
    public int? FlightPlanningProvider { get; set; }

    /// <summary>
    /// Gets or Sets FlightPlanJson
    /// </summary>
    [DataMember(Name="flightPlanJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightPlanJson")]
    public string FlightPlanJson { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FlightPlansByCompanyDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
      sb.Append("  FlightPlanId: ").Append(FlightPlanId).Append("\n");
      sb.Append("  DepartureDateTimeZulu: ").Append(DepartureDateTimeZulu).Append("\n");
      sb.Append("  DepartureDateTimeLocal: ").Append(DepartureDateTimeLocal).Append("\n");
      sb.Append("  FlightPlanningProvider: ").Append(FlightPlanningProvider).Append("\n");
      sb.Append("  FlightPlanJson: ").Append(FlightPlanJson).Append("\n");
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
