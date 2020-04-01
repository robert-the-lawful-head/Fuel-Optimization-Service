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
  public class DispatchEmailOptions {
    /// <summary>
    /// Gets or Sets UserEmailAddresses
    /// </summary>
    [DataMember(Name="userEmailAddresses", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userEmailAddresses")]
    public List<UserEmailDTO> UserEmailAddresses { get; set; }

    /// <summary>
    /// Gets or Sets CopyScheduledCrewEmails
    /// </summary>
    [DataMember(Name="copyScheduledCrewEmails", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "copyScheduledCrewEmails")]
    public bool? CopyScheduledCrewEmails { get; set; }

    /// <summary>
    /// Gets or Sets WebNotificationFailed
    /// </summary>
    [DataMember(Name="webNotificationFailed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "webNotificationFailed")]
    public bool? WebNotificationFailed { get; set; }

    /// <summary>
    /// Gets or Sets WebNotificationFailureMessage
    /// </summary>
    [DataMember(Name="webNotificationFailureMessage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "webNotificationFailureMessage")]
    public string WebNotificationFailureMessage { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DispatchEmailOptions {\n");
      sb.Append("  UserEmailAddresses: ").Append(UserEmailAddresses).Append("\n");
      sb.Append("  CopyScheduledCrewEmails: ").Append(CopyScheduledCrewEmails).Append("\n");
      sb.Append("  WebNotificationFailed: ").Append(WebNotificationFailed).Append("\n");
      sb.Append("  WebNotificationFailureMessage: ").Append(WebNotificationFailureMessage).Append("\n");
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
