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
  public class IntegrationPartnerDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets PartnerName
    /// </summary>
    [DataMember(Name="partnerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "partnerName")]
    public string PartnerName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="partnerType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "partnerType")]
    public int? PartnerType { get; set; }

    /// <summary>
    /// Gets or Sets PartnerTypeDescription
    /// </summary>
    [DataMember(Name="partnerTypeDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "partnerTypeDescription")]
    public string PartnerTypeDescription { get; set; }

    /// <summary>
    /// Gets or Sets Affiliation
    /// </summary>
    [DataMember(Name="affiliation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "affiliation")]
    public int? Affiliation { get; set; }

    /// <summary>
    /// Gets or Sets Apikey
    /// </summary>
    [DataMember(Name="apikey", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "apikey")]
    public Guid? Apikey { get; set; }

    /// <summary>
    /// Gets or Sets CreationDate
    /// </summary>
    [DataMember(Name="creationDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "creationDate")]
    public DateTime? CreationDate { get; set; }

    /// <summary>
    /// Gets or Sets ModifiedDate
    /// </summary>
    [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "modifiedDate")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or Sets PartnerId
    /// </summary>
    [DataMember(Name="partnerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "partnerId")]
    public Guid? PartnerId { get; set; }

    /// <summary>
    /// Gets or Sets PartnerLogo
    /// </summary>
    [DataMember(Name="partnerLogo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "partnerLogo")]
    public string PartnerLogo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="trustLevel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "trustLevel")]
    public int? TrustLevel { get; set; }

    /// <summary>
    /// Gets or Sets WebHooks
    /// </summary>
    [DataMember(Name="webHooks", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "webHooks")]
    public List<IntegrationPartnerWebHooksDTO> WebHooks { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class IntegrationPartnerDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  PartnerName: ").Append(PartnerName).Append("\n");
      sb.Append("  PartnerType: ").Append(PartnerType).Append("\n");
      sb.Append("  PartnerTypeDescription: ").Append(PartnerTypeDescription).Append("\n");
      sb.Append("  Affiliation: ").Append(Affiliation).Append("\n");
      sb.Append("  Apikey: ").Append(Apikey).Append("\n");
      sb.Append("  CreationDate: ").Append(CreationDate).Append("\n");
      sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
      sb.Append("  PartnerId: ").Append(PartnerId).Append("\n");
      sb.Append("  PartnerLogo: ").Append(PartnerLogo).Append("\n");
      sb.Append("  TrustLevel: ").Append(TrustLevel).Append("\n");
      sb.Append("  WebHooks: ").Append(WebHooks).Append("\n");
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
