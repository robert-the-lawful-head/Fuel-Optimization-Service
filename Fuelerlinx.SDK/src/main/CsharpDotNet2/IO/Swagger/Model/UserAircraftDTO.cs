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
  public class UserAircraftDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets UserId
    /// </summary>
    [DataMember(Name="userId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userId")]
    public int? UserId { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets TailNumberId
    /// </summary>
    [DataMember(Name="tailNumberId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumberId")]
    public int? TailNumberId { get; set; }

    /// <summary>
    /// Gets or Sets AddDate
    /// </summary>
    [DataMember(Name="addDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addDate")]
    public DateTime? AddDate { get; set; }

    /// <summary>
    /// Gets or Sets Default
    /// </summary>
    [DataMember(Name="default", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "default")]
    public bool? Default { get; set; }

    /// <summary>
    /// Gets or Sets ClientName
    /// </summary>
    [DataMember(Name="clientName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "clientName")]
    public string ClientName { get; set; }

    /// <summary>
    /// Gets or Sets AircraftData
    /// </summary>
    [DataMember(Name="aircraftData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aircraftData")]
    public AircraftDataDTO AircraftData { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UserAircraftDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  TailNumberId: ").Append(TailNumberId).Append("\n");
      sb.Append("  AddDate: ").Append(AddDate).Append("\n");
      sb.Append("  Default: ").Append(Default).Append("\n");
      sb.Append("  ClientName: ").Append(ClientName).Append("\n");
      sb.Append("  AircraftData: ").Append(AircraftData).Append("\n");
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
