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
  public class PostTransactionAccountingDataRequest {
    /// <summary>
    /// Gets or Sets TransactionId
    /// </summary>
    [DataMember(Name="transactionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionId")]
    public int? TransactionId { get; set; }

    /// <summary>
    /// Gets or Sets AccountingData
    /// </summary>
    [DataMember(Name="accountingData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "accountingData")]
    public Object AccountingData { get; set; }

    /// <summary>
    /// Gets or Sets InvoiceDate
    /// </summary>
    [DataMember(Name="invoiceDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoiceDate")]
    public DateTime? InvoiceDate { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostTransactionAccountingDataRequest {\n");
      sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
      sb.Append("  AccountingData: ").Append(AccountingData).Append("\n");
      sb.Append("  InvoiceDate: ").Append(InvoiceDate).Append("\n");
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
