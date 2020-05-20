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
  public class UserDTO {
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
    /// Gets or Sets FirstName
    /// </summary>
    [DataMember(Name="firstName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "firstName")]
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or Sets LastName
    /// </summary>
    [DataMember(Name="lastName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lastName")]
    public string LastName { get; set; }

    /// <summary>
    /// Gets or Sets Username
    /// </summary>
    [DataMember(Name="username", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "username")]
    public string Username { get; set; }

    /// <summary>
    /// Gets or Sets Token
    /// </summary>
    [DataMember(Name="token", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "token")]
    public string Token { get; set; }

    /// <summary>
    /// Gets or Sets PrimaryAccount
    /// </summary>
    [DataMember(Name="primaryAccount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "primaryAccount")]
    public bool? PrimaryAccount { get; set; }

    /// <summary>
    /// Gets or Sets DemoMode
    /// </summary>
    [DataMember(Name="demoMode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "demoMode")]
    public bool? DemoMode { get; set; }

    /// <summary>
    /// Gets or Sets PhoneNumber
    /// </summary>
    [DataMember(Name="phoneNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "phoneNumber")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or Sets FullName
    /// </summary>
    [DataMember(Name="fullName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fullName")]
    public string FullName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="role", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "role")]
    public int? Role { get; set; }

    /// <summary>
    /// Gets or Sets CompanyUserProfileId
    /// </summary>
    [DataMember(Name="companyUserProfileId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyUserProfileId")]
    public int? CompanyUserProfileId { get; set; }

    /// <summary>
    /// Gets or Sets Company
    /// </summary>
    [DataMember(Name="company", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "company")]
    public CompanyDTO Company { get; set; }

    /// <summary>
    /// Gets or Sets Preferences
    /// </summary>
    [DataMember(Name="preferences", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "preferences")]
    public PreferencesDTO Preferences { get; set; }

    /// <summary>
    /// Gets or Sets EmailAddresses
    /// </summary>
    [DataMember(Name="emailAddresses", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "emailAddresses")]
    public List<UserEmailDTO> EmailAddresses { get; set; }

    /// <summary>
    /// Gets or Sets CompanyUserProfile
    /// </summary>
    [DataMember(Name="companyUserProfile", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyUserProfile")]
    public CompanyUserProfileDTO CompanyUserProfile { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UserDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  FirstName: ").Append(FirstName).Append("\n");
      sb.Append("  LastName: ").Append(LastName).Append("\n");
      sb.Append("  Username: ").Append(Username).Append("\n");
      sb.Append("  Token: ").Append(Token).Append("\n");
      sb.Append("  PrimaryAccount: ").Append(PrimaryAccount).Append("\n");
      sb.Append("  DemoMode: ").Append(DemoMode).Append("\n");
      sb.Append("  PhoneNumber: ").Append(PhoneNumber).Append("\n");
      sb.Append("  FullName: ").Append(FullName).Append("\n");
      sb.Append("  Role: ").Append(Role).Append("\n");
      sb.Append("  CompanyUserProfileId: ").Append(CompanyUserProfileId).Append("\n");
      sb.Append("  Company: ").Append(Company).Append("\n");
      sb.Append("  Preferences: ").Append(Preferences).Append("\n");
      sb.Append("  EmailAddresses: ").Append(EmailAddresses).Append("\n");
      sb.Append("  CompanyUserProfile: ").Append(CompanyUserProfile).Append("\n");
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
