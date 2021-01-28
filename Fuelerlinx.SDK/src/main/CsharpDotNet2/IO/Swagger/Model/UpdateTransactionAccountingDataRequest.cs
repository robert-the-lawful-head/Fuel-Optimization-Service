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
  public class UpdateTransactionAccountingDataRequest {
    /// <summary>
    /// Gets or Sets TransactionAccountingData
    /// </summary>
    [DataMember(Name="transactionAccountingData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionAccountingData")]
    public TransactionAccountingDataDTO TransactionAccountingData { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateTransactionAccountingDataRequest {\n");
      sb.Append("  TransactionAccountingData: ").Append(TransactionAccountingData).Append("\n");
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
