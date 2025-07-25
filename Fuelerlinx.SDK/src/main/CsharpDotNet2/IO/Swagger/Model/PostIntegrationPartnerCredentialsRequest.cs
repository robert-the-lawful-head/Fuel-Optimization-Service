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
  public class PostIntegrationPartnerCredentialsRequest {
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="partnerType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "partnerType")]
    public int? PartnerType { get; set; }

    /// <summary>
    /// Gets or Sets Affiliation
    /// </summary>
    [DataMember(Name="affiliation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "affiliation")]
    public int? Affiliation { get; set; }

    /// <summary>
    /// Gets or Sets PartnerCredentialsModel
    /// </summary>
    [DataMember(Name="partnerCredentialsModel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "partnerCredentialsModel")]
    public Object PartnerCredentialsModel { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostIntegrationPartnerCredentialsRequest {\n");
      sb.Append("  PartnerType: ").Append(PartnerType).Append("\n");
      sb.Append("  Affiliation: ").Append(Affiliation).Append("\n");
      sb.Append("  PartnerCredentialsModel: ").Append(PartnerCredentialsModel).Append("\n");
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
