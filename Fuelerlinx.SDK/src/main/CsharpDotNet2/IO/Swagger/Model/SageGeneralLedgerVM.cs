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
  public class SageGeneralLedgerVM {
    /// <summary>
    /// Gets or Sets GeneralLedgerCode
    /// </summary>
    [DataMember(Name="generalLedgerCode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "generalLedgerCode")]
    public string GeneralLedgerCode { get; set; }

    /// <summary>
    /// Gets or Sets GeneralLedgerItemName
    /// </summary>
    [DataMember(Name="generalLedgerItemName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "generalLedgerItemName")]
    public string GeneralLedgerItemName { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SageGeneralLedgerVM {\n");
      sb.Append("  GeneralLedgerCode: ").Append(GeneralLedgerCode).Append("\n");
      sb.Append("  GeneralLedgerItemName: ").Append(GeneralLedgerItemName).Append("\n");
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
