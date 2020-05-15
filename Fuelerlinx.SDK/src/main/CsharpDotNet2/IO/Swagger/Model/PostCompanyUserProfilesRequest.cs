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
  public class PostCompanyUserProfilesRequest {
    /// <summary>
    /// Gets or Sets ProfileName
    /// </summary>
    [DataMember(Name="profileName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "profileName")]
    public string ProfileName { get; set; }

    /// <summary>
    /// Gets or Sets SettingsJson
    /// </summary>
    [DataMember(Name="settingsJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "settingsJson")]
    public string SettingsJson { get; set; }

    /// <summary>
    /// Gets or Sets DateAdded
    /// </summary>
    [DataMember(Name="dateAdded", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateAdded")]
    public DateTime? DateAdded { get; set; }

    /// <summary>
    /// Gets or Sets Role
    /// </summary>
    [DataMember(Name="role", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "role")]
    public int? Role { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostCompanyUserProfilesRequest {\n");
      sb.Append("  ProfileName: ").Append(ProfileName).Append("\n");
      sb.Append("  SettingsJson: ").Append(SettingsJson).Append("\n");
      sb.Append("  DateAdded: ").Append(DateAdded).Append("\n");
      sb.Append("  Role: ").Append(Role).Append("\n");
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
