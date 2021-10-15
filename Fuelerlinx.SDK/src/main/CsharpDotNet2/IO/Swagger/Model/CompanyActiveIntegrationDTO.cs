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
  public class CompanyActiveIntegrationDTO {
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
    [DataMember(Name="integrationType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "integrationType")]
    public int? IntegrationType { get; set; }

    /// <summary>
    /// Gets or Sets IntegrationTypeDescription
    /// </summary>
    [DataMember(Name="integrationTypeDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "integrationTypeDescription")]
    public string IntegrationTypeDescription { get; set; }

    /// <summary>
    /// Gets or Sets Affiliation
    /// </summary>
    [DataMember(Name="affiliation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "affiliation")]
    public int? Affiliation { get; set; }

    /// <summary>
    /// Gets or Sets AffiliationDescription
    /// </summary>
    [DataMember(Name="affiliationDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "affiliationDescription")]
    public string AffiliationDescription { get; set; }

    /// <summary>
    /// Gets or Sets ActivationDateUtc
    /// </summary>
    [DataMember(Name="activationDateUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "activationDateUtc")]
    public DateTime? ActivationDateUtc { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CompanyActiveIntegrationDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  IntegrationType: ").Append(IntegrationType).Append("\n");
      sb.Append("  IntegrationTypeDescription: ").Append(IntegrationTypeDescription).Append("\n");
      sb.Append("  Affiliation: ").Append(Affiliation).Append("\n");
      sb.Append("  AffiliationDescription: ").Append(AffiliationDescription).Append("\n");
      sb.Append("  ActivationDateUtc: ").Append(ActivationDateUtc).Append("\n");
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
