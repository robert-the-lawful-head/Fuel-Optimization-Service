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
  public class PostTransactionAccountingTransferRequest {
    /// <summary>
    /// Gets or Sets TransactionId
    /// </summary>
    [DataMember(Name="transactionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionId")]
    public int? TransactionId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="transferStatus", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transferStatus")]
    public int? TransferStatus { get; set; }

    /// <summary>
    /// Gets or Sets ReadyDateUtc
    /// </summary>
    [DataMember(Name="readyDateUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "readyDateUtc")]
    public string ReadyDateUtc { get; set; }

    /// <summary>
    /// Gets or Sets ReadyTimeUtc
    /// </summary>
    [DataMember(Name="readyTimeUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "readyTimeUtc")]
    public string ReadyTimeUtc { get; set; }

    /// <summary>
    /// Gets or Sets TransferDateUtc
    /// </summary>
    [DataMember(Name="transferDateUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transferDateUtc")]
    public string TransferDateUtc { get; set; }

    /// <summary>
    /// Gets or Sets TransferTimeUtc
    /// </summary>
    [DataMember(Name="transferTimeUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transferTimeUtc")]
    public string TransferTimeUtc { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostTransactionAccountingTransferRequest {\n");
      sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
      sb.Append("  TransferStatus: ").Append(TransferStatus).Append("\n");
      sb.Append("  ReadyDateUtc: ").Append(ReadyDateUtc).Append("\n");
      sb.Append("  ReadyTimeUtc: ").Append(ReadyTimeUtc).Append("\n");
      sb.Append("  TransferDateUtc: ").Append(TransferDateUtc).Append("\n");
      sb.Append("  TransferTimeUtc: ").Append(TransferTimeUtc).Append("\n");
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
