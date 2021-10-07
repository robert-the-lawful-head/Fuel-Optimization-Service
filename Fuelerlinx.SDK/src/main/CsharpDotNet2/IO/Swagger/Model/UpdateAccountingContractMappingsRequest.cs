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
  public class UpdateAccountingContractMappingsRequest {
    /// <summary>
    /// Gets or Sets AccountingContractMappings
    /// </summary>
    [DataMember(Name="accountingContractMappings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "accountingContractMappings")]
    public AccountingContractMappingsDTO AccountingContractMappings { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateAccountingContractMappingsRequest {\n");
      sb.Append("  AccountingContractMappings: ").Append(AccountingContractMappings).Append("\n");
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
