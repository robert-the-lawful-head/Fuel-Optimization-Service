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
  public class FlightPlanningSettingsDTO {
    /// <summary>
    /// Gets or Sets CompanyID
    /// </summary>
    [DataMember(Name="CompanyID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "CompanyID")]
    public int? CompanyID { get; set; }

    /// <summary>
    /// Gets or Sets AFSAccountID
    /// </summary>
    [DataMember(Name="AFSAccountID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "AFSAccountID")]
    public int? AFSAccountID { get; set; }

    /// <summary>
    /// Gets or Sets IFlightPlannerGUID
    /// </summary>
    [DataMember(Name="IFlightPlannerGUID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "IFlightPlannerGUID")]
    public string IFlightPlannerGUID { get; set; }

    /// <summary>
    /// Gets or Sets UserID
    /// </summary>
    [DataMember(Name="UserID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "UserID")]
    public int? UserID { get; set; }

    /// <summary>
    /// Gets or Sets DefaultRouting
    /// </summary>
    [DataMember(Name="DefaultRouting", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "DefaultRouting")]
    public string DefaultRouting { get; set; }

    /// <summary>
    /// Gets or Sets FleetGroup
    /// </summary>
    [DataMember(Name="FleetGroup", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FleetGroup")]
    public string FleetGroup { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="FlightPlanningProvider", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FlightPlanningProvider")]
    public int? FlightPlanningProvider { get; set; }

    /// <summary>
    /// Gets or Sets FlightPlanningProviderUsername
    /// </summary>
    [DataMember(Name="FlightPlanningProviderUsername", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FlightPlanningProviderUsername")]
    public string FlightPlanningProviderUsername { get; set; }

    /// <summary>
    /// Gets or Sets FlightPlanningProviderPassword
    /// </summary>
    [DataMember(Name="FlightPlanningProviderPassword", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FlightPlanningProviderPassword")]
    public string FlightPlanningProviderPassword { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FlightPlanningSettingsDTO {\n");
      sb.Append("  CompanyID: ").Append(CompanyID).Append("\n");
      sb.Append("  AFSAccountID: ").Append(AFSAccountID).Append("\n");
      sb.Append("  IFlightPlannerGUID: ").Append(IFlightPlannerGUID).Append("\n");
      sb.Append("  UserID: ").Append(UserID).Append("\n");
      sb.Append("  DefaultRouting: ").Append(DefaultRouting).Append("\n");
      sb.Append("  FleetGroup: ").Append(FleetGroup).Append("\n");
      sb.Append("  FlightPlanningProvider: ").Append(FlightPlanningProvider).Append("\n");
      sb.Append("  FlightPlanningProviderUsername: ").Append(FlightPlanningProviderUsername).Append("\n");
      sb.Append("  FlightPlanningProviderPassword: ").Append(FlightPlanningProviderPassword).Append("\n");
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
