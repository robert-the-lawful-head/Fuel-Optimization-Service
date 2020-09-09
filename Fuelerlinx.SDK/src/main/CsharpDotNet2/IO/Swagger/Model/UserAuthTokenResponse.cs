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
  public class UserAuthTokenResponse {
    /// <summary>
    /// Gets or Sets Token
    /// </summary>
    [DataMember(Name="token", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "token")]
    public string Token { get; set; }

    /// <summary>
    /// Gets or Sets AuthToken
    /// </summary>
    [DataMember(Name="authToken", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "authToken")]
    public string AuthToken { get; set; }

    /// <summary>
    /// Gets or Sets AuthTokenExpiration
    /// </summary>
    [DataMember(Name="authTokenExpiration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "authTokenExpiration")]
    public DateTime? AuthTokenExpiration { get; set; }

    /// <summary>
    /// Gets or Sets RefreshToken
    /// </summary>
    [DataMember(Name="refreshToken", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "refreshToken")]
    public string RefreshToken { get; set; }

    /// <summary>
    /// Gets or Sets RefreshTokenExpiration
    /// </summary>
    [DataMember(Name="refreshTokenExpiration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "refreshTokenExpiration")]
    public DateTime? RefreshTokenExpiration { get; set; }

    /// <summary>
    /// Gets or Sets Username
    /// </summary>
    [DataMember(Name="username", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "username")]
    public string Username { get; set; }

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
    /// Gets or Sets Success
    /// </summary>
    [DataMember(Name="success", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "success")]
    public bool? Success { get; set; }

    /// <summary>
    /// Gets or Sets Message
    /// </summary>
    [DataMember(Name="message", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UserAuthTokenResponse {\n");
      sb.Append("  Token: ").Append(Token).Append("\n");
      sb.Append("  AuthToken: ").Append(AuthToken).Append("\n");
      sb.Append("  AuthTokenExpiration: ").Append(AuthTokenExpiration).Append("\n");
      sb.Append("  RefreshToken: ").Append(RefreshToken).Append("\n");
      sb.Append("  RefreshTokenExpiration: ").Append(RefreshTokenExpiration).Append("\n");
      sb.Append("  Username: ").Append(Username).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  Success: ").Append(Success).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
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
