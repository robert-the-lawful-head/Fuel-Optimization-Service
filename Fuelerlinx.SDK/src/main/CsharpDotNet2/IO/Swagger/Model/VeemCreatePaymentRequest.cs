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
  public class VeemCreatePaymentRequest {
    /// <summary>
    /// Gets or Sets TransactionID
    /// </summary>
    [DataMember(Name="transactionID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionID")]
    public int? TransactionID { get; set; }

    /// <summary>
    /// Gets or Sets Amount
    /// </summary>
    [DataMember(Name="amount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "amount")]
    public double? Amount { get; set; }

    /// <summary>
    /// Gets or Sets PaymentDate
    /// </summary>
    [DataMember(Name="paymentDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paymentDate")]
    public DateTime? PaymentDate { get; set; }

    /// <summary>
    /// Gets or Sets PaymentEmail
    /// </summary>
    [DataMember(Name="paymentEmail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paymentEmail")]
    public string PaymentEmail { get; set; }

    /// <summary>
    /// Gets or Sets Invoice
    /// </summary>
    [DataMember(Name="invoice", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoice")]
    public bool? Invoice { get; set; }

    /// <summary>
    /// Gets or Sets Note
    /// </summary>
    [DataMember(Name="note", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "note")]
    public string Note { get; set; }

    /// <summary>
    /// Gets or Sets UserID
    /// </summary>
    [DataMember(Name="userID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userID")]
    public int? UserID { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VeemCreatePaymentRequest {\n");
      sb.Append("  TransactionID: ").Append(TransactionID).Append("\n");
      sb.Append("  Amount: ").Append(Amount).Append("\n");
      sb.Append("  PaymentDate: ").Append(PaymentDate).Append("\n");
      sb.Append("  PaymentEmail: ").Append(PaymentEmail).Append("\n");
      sb.Append("  Invoice: ").Append(Invoice).Append("\n");
      sb.Append("  Note: ").Append(Note).Append("\n");
      sb.Append("  UserID: ").Append(UserID).Append("\n");
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
