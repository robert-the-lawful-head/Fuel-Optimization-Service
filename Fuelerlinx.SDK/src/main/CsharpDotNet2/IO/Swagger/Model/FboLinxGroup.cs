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
  public class FboLinxGroup {
    /// <summary>
    /// Gets or Sets Oid
    /// </summary>
    [DataMember(Name="oid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "oid")]
    public int? Oid { get; set; }

    /// <summary>
    /// Gets or Sets GroupName
    /// </summary>
    [DataMember(Name="groupName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "groupName")]
    public string GroupName { get; set; }

    /// <summary>
    /// Gets or Sets Username
    /// </summary>
    [DataMember(Name="username", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "username")]
    public string Username { get; set; }

    /// <summary>
    /// Gets or Sets Password
    /// </summary>
    [DataMember(Name="password", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "password")]
    public string Password { get; set; }

    /// <summary>
    /// Gets or Sets Isfbonetwork
    /// </summary>
    [DataMember(Name="isfbonetwork", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isfbonetwork")]
    public bool? Isfbonetwork { get; set; }

    /// <summary>
    /// Gets or Sets Domain
    /// </summary>
    [DataMember(Name="domain", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "domain")]
    public string Domain { get; set; }

    /// <summary>
    /// Gets or Sets LoggedInHomePage
    /// </summary>
    [DataMember(Name="loggedInHomePage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "loggedInHomePage")]
    public string LoggedInHomePage { get; set; }

    /// <summary>
    /// Gets or Sets Active
    /// </summary>
    [DataMember(Name="active", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "active")]
    public bool? Active { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FboLinxGroup {\n");
      sb.Append("  Oid: ").Append(Oid).Append("\n");
      sb.Append("  GroupName: ").Append(GroupName).Append("\n");
      sb.Append("  Username: ").Append(Username).Append("\n");
      sb.Append("  Password: ").Append(Password).Append("\n");
      sb.Append("  Isfbonetwork: ").Append(Isfbonetwork).Append("\n");
      sb.Append("  Domain: ").Append(Domain).Append("\n");
      sb.Append("  LoggedInHomePage: ").Append(LoggedInHomePage).Append("\n");
      sb.Append("  Active: ").Append(Active).Append("\n");
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
