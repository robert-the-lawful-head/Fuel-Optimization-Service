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
  public class IntegrationAuthorizationDTO {
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
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="integrationPartnerType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "integrationPartnerType")]
    public int? IntegrationPartnerType { get; set; }

    /// <summary>
    /// Gets or Sets IntegrationPartnerAffiliation
    /// </summary>
    [DataMember(Name="integrationPartnerAffiliation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "integrationPartnerAffiliation")]
    public int? IntegrationPartnerAffiliation { get; set; }

    /// <summary>
    /// Gets or Sets IntegrationAuthorizationJson
    /// </summary>
    [DataMember(Name="integrationAuthorizationJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "integrationAuthorizationJson")]
    public string IntegrationAuthorizationJson { get; set; }

    /// <summary>
    /// Gets or Sets CreationDateUtc
    /// </summary>
    [DataMember(Name="creationDateUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "creationDateUtc")]
    public DateTime? CreationDateUtc { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class IntegrationAuthorizationDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  IntegrationPartnerType: ").Append(IntegrationPartnerType).Append("\n");
      sb.Append("  IntegrationPartnerAffiliation: ").Append(IntegrationPartnerAffiliation).Append("\n");
      sb.Append("  IntegrationAuthorizationJson: ").Append(IntegrationAuthorizationJson).Append("\n");
      sb.Append("  CreationDateUtc: ").Append(CreationDateUtc).Append("\n");
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
