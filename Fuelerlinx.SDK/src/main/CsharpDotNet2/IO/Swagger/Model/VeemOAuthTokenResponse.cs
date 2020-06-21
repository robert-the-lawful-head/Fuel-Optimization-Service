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
  public class VeemOAuthTokenResponse {
    /// <summary>
    /// Gets or Sets AccessToken
    /// </summary>
    [DataMember(Name="access_token", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "access_token")]
    public string AccessToken { get; set; }

    /// <summary>
    /// Gets or Sets TokenType
    /// </summary>
    [DataMember(Name="token_type", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "token_type")]
    public string TokenType { get; set; }

    /// <summary>
    /// Gets or Sets RefreshToken
    /// </summary>
    [DataMember(Name="refresh_token", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "refresh_token")]
    public string RefreshToken { get; set; }

    /// <summary>
    /// Gets or Sets ExpiresIn
    /// </summary>
    [DataMember(Name="expires_in", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "expires_in")]
    public string ExpiresIn { get; set; }

    /// <summary>
    /// Gets or Sets Scope
    /// </summary>
    [DataMember(Name="scope", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "scope")]
    public string Scope { get; set; }

    /// <summary>
    /// Gets or Sets AccountId
    /// </summary>
    [DataMember(Name="account_id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "account_id")]
    public string AccountId { get; set; }

    /// <summary>
    /// Gets or Sets UserId
    /// </summary>
    [DataMember(Name="user_id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "user_id")]
    public string UserId { get; set; }

    /// <summary>
    /// Gets or Sets UserName
    /// </summary>
    [DataMember(Name="user_name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "user_name")]
    public string UserName { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VeemOAuthTokenResponse {\n");
      sb.Append("  AccessToken: ").Append(AccessToken).Append("\n");
      sb.Append("  TokenType: ").Append(TokenType).Append("\n");
      sb.Append("  RefreshToken: ").Append(RefreshToken).Append("\n");
      sb.Append("  ExpiresIn: ").Append(ExpiresIn).Append("\n");
      sb.Append("  Scope: ").Append(Scope).Append("\n");
      sb.Append("  AccountId: ").Append(AccountId).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  UserName: ").Append(UserName).Append("\n");
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
