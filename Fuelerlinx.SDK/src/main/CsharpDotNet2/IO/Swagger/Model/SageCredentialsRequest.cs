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
  public class SageCredentialsRequest {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets SenderId
    /// </summary>
    [DataMember(Name="senderId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "senderId")]
    public string SenderId { get; set; }

    /// <summary>
    /// Gets or Sets SenderPassword
    /// </summary>
    [DataMember(Name="senderPassword", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "senderPassword")]
    public string SenderPassword { get; set; }

    /// <summary>
    /// Gets or Sets UserId
    /// </summary>
    [DataMember(Name="userId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userId")]
    public string UserId { get; set; }

    /// <summary>
    /// Gets or Sets UserPassword
    /// </summary>
    [DataMember(Name="userPassword", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userPassword")]
    public string UserPassword { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public string CompanyId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SageCredentialsRequest {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  SenderId: ").Append(SenderId).Append("\n");
      sb.Append("  SenderPassword: ").Append(SenderPassword).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  UserPassword: ").Append(UserPassword).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
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
