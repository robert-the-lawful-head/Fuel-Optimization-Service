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
  public class FlightBridgeExternalUserInformation {
    /// <summary>
    /// Gets or Sets UserName
    /// </summary>
    [DataMember(Name="UserName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "UserName")]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or Sets FirstName
    /// </summary>
    [DataMember(Name="FirstName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FirstName")]
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or Sets LastName
    /// </summary>
    [DataMember(Name="LastName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "LastName")]
    public string LastName { get; set; }

    /// <summary>
    /// Gets or Sets CompanyName
    /// </summary>
    [DataMember(Name="CompanyName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "CompanyName")]
    public string CompanyName { get; set; }

    /// <summary>
    /// Gets or Sets CompanyIdentifier
    /// </summary>
    [DataMember(Name="CompanyIdentifier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "CompanyIdentifier")]
    public int? CompanyIdentifier { get; set; }

    /// <summary>
    /// Gets or Sets AllowedToCreateModifyTrips
    /// </summary>
    [DataMember(Name="AllowedToCreateModifyTrips", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "AllowedToCreateModifyTrips")]
    public bool? AllowedToCreateModifyTrips { get; set; }

    /// <summary>
    /// Gets or Sets AllowedToCreateModifyOrders
    /// </summary>
    [DataMember(Name="AllowedToCreateModifyOrders", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "AllowedToCreateModifyOrders")]
    public bool? AllowedToCreateModifyOrders { get; set; }

    /// <summary>
    /// Gets or Sets EmailAddress
    /// </summary>
    [DataMember(Name="EmailAddress", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "EmailAddress")]
    public string EmailAddress { get; set; }

    /// <summary>
    /// Gets or Sets PhoneNumber
    /// </summary>
    [DataMember(Name="PhoneNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "PhoneNumber")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or Sets Alias
    /// </summary>
    [DataMember(Name="Alias", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Alias")]
    public string Alias { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FlightBridgeExternalUserInformation {\n");
      sb.Append("  UserName: ").Append(UserName).Append("\n");
      sb.Append("  FirstName: ").Append(FirstName).Append("\n");
      sb.Append("  LastName: ").Append(LastName).Append("\n");
      sb.Append("  CompanyName: ").Append(CompanyName).Append("\n");
      sb.Append("  CompanyIdentifier: ").Append(CompanyIdentifier).Append("\n");
      sb.Append("  AllowedToCreateModifyTrips: ").Append(AllowedToCreateModifyTrips).Append("\n");
      sb.Append("  AllowedToCreateModifyOrders: ").Append(AllowedToCreateModifyOrders).Append("\n");
      sb.Append("  EmailAddress: ").Append(EmailAddress).Append("\n");
      sb.Append("  PhoneNumber: ").Append(PhoneNumber).Append("\n");
      sb.Append("  Alias: ").Append(Alias).Append("\n");
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
