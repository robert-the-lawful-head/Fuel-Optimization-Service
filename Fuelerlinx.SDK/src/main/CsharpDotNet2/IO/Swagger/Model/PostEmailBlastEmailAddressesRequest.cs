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
  public class PostEmailBlastEmailAddressesRequest {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets EmailBlastId
    /// </summary>
    [DataMember(Name="emailBlastId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "emailBlastId")]
    public int? EmailBlastId { get; set; }

    /// <summary>
    /// Gets or Sets Subscribers
    /// </summary>
    [DataMember(Name="subscribers", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "subscribers")]
    public List<EmailBlastSubscriberDTO> Subscribers { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostEmailBlastEmailAddressesRequest {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  EmailBlastId: ").Append(EmailBlastId).Append("\n");
      sb.Append("  Subscribers: ").Append(Subscribers).Append("\n");
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
