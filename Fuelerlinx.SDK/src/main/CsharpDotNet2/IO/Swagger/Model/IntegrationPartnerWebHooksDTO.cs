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
  public class IntegrationPartnerWebHooksDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets IntegrationPartnerId
    /// </summary>
    [DataMember(Name="integrationPartnerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "integrationPartnerId")]
    public int? IntegrationPartnerId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="environment", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "environment")]
    public int? Environment { get; set; }

    /// <summary>
    /// Gets or Sets WebHookEndPoint
    /// </summary>
    [DataMember(Name="webHookEndPoint", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "webHookEndPoint")]
    public string WebHookEndPoint { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="webHookType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "webHookType")]
    public int? WebHookType { get; set; }

    /// <summary>
    /// Gets or Sets CustomSettingsJSON
    /// </summary>
    [DataMember(Name="customSettingsJSON", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "customSettingsJSON")]
    public string CustomSettingsJSON { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class IntegrationPartnerWebHooksDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  IntegrationPartnerId: ").Append(IntegrationPartnerId).Append("\n");
      sb.Append("  Environment: ").Append(Environment).Append("\n");
      sb.Append("  WebHookEndPoint: ").Append(WebHookEndPoint).Append("\n");
      sb.Append("  WebHookType: ").Append(WebHookType).Append("\n");
      sb.Append("  CustomSettingsJSON: ").Append(CustomSettingsJSON).Append("\n");
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
