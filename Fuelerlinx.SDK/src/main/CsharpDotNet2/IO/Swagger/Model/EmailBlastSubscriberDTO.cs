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
  public class EmailBlastSubscriberDTO {
    /// <summary>
    /// Gets or Sets EmailAddress
    /// </summary>
    [DataMember(Name="emailAddress", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "emailAddress")]
    public string EmailAddress { get; set; }

    /// <summary>
    /// Gets or Sets IsSubscribed
    /// </summary>
    [DataMember(Name="isSubscribed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isSubscribed")]
    public bool? IsSubscribed { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class EmailBlastSubscriberDTO {\n");
      sb.Append("  EmailAddress: ").Append(EmailAddress).Append("\n");
      sb.Append("  IsSubscribed: ").Append(IsSubscribed).Append("\n");
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
