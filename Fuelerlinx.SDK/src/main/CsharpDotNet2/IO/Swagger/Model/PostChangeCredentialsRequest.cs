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
  public class PostChangeCredentialsRequest {
    /// <summary>
    /// Gets or Sets OldUsername
    /// </summary>
    [DataMember(Name="oldUsername", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "oldUsername")]
    public string OldUsername { get; set; }

    /// <summary>
    /// Gets or Sets NewUsername
    /// </summary>
    [DataMember(Name="newUsername", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "newUsername")]
    public string NewUsername { get; set; }

    /// <summary>
    /// Gets or Sets IsChangingUsername
    /// </summary>
    [DataMember(Name="isChangingUsername", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isChangingUsername")]
    public bool? IsChangingUsername { get; set; }

    /// <summary>
    /// Gets or Sets OldPassword
    /// </summary>
    [DataMember(Name="oldPassword", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "oldPassword")]
    public string OldPassword { get; set; }

    /// <summary>
    /// Gets or Sets NewPassword
    /// </summary>
    [DataMember(Name="newPassword", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "newPassword")]
    public string NewPassword { get; set; }

    /// <summary>
    /// Gets or Sets IsChangingPassword
    /// </summary>
    [DataMember(Name="isChangingPassword", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isChangingPassword")]
    public bool? IsChangingPassword { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostChangeCredentialsRequest {\n");
      sb.Append("  OldUsername: ").Append(OldUsername).Append("\n");
      sb.Append("  NewUsername: ").Append(NewUsername).Append("\n");
      sb.Append("  IsChangingUsername: ").Append(IsChangingUsername).Append("\n");
      sb.Append("  OldPassword: ").Append(OldPassword).Append("\n");
      sb.Append("  NewPassword: ").Append(NewPassword).Append("\n");
      sb.Append("  IsChangingPassword: ").Append(IsChangingPassword).Append("\n");
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
