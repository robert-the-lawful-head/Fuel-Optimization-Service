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
  public class IntegrationFuelOrderDetailsDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets TransactionId
    /// </summary>
    [DataMember(Name="transactionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionId")]
    public int? TransactionId { get; set; }

    /// <summary>
    /// Gets or Sets IntegrationDataIdentifier
    /// </summary>
    [DataMember(Name="integrationDataIdentifier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "integrationDataIdentifier")]
    public string IntegrationDataIdentifier { get; set; }

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
    /// Gets or Sets GeneratedFuelComment
    /// </summary>
    [DataMember(Name="generatedFuelComment", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "generatedFuelComment")]
    public string GeneratedFuelComment { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class IntegrationFuelOrderDetailsDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
      sb.Append("  IntegrationDataIdentifier: ").Append(IntegrationDataIdentifier).Append("\n");
      sb.Append("  IntegrationPartnerType: ").Append(IntegrationPartnerType).Append("\n");
      sb.Append("  IntegrationPartnerAffiliation: ").Append(IntegrationPartnerAffiliation).Append("\n");
      sb.Append("  GeneratedFuelComment: ").Append(GeneratedFuelComment).Append("\n");
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
