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
  public class VeemCancelPaymentResponse {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets RequestId
    /// </summary>
    [DataMember(Name="requestId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "requestId")]
    public string RequestId { get; set; }

    /// <summary>
    /// Gets or Sets PayeeAmount
    /// </summary>
    [DataMember(Name="payeeAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "payeeAmount")]
    public AmountDetails PayeeAmount { get; set; }

    /// <summary>
    /// Gets or Sets Notes
    /// </summary>
    [DataMember(Name="notes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "notes")]
    public string Notes { get; set; }

    /// <summary>
    /// Gets or Sets Status
    /// </summary>
    [DataMember(Name="status", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; }

    /// <summary>
    /// Gets or Sets Payee
    /// </summary>
    [DataMember(Name="payee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "payee")]
    public PayeeDetail Payee { get; set; }

    /// <summary>
    /// Gets or Sets PurposeOfPayment
    /// </summary>
    [DataMember(Name="purposeOfPayment", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "purposeOfPayment")]
    public string PurposeOfPayment { get; set; }

    /// <summary>
    /// Gets or Sets ClaimLink
    /// </summary>
    [DataMember(Name="claimLink", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "claimLink")]
    public string ClaimLink { get; set; }

    /// <summary>
    /// Gets or Sets TimeCreated
    /// </summary>
    [DataMember(Name="timeCreated", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timeCreated")]
    public DateTime? TimeCreated { get; set; }

    /// <summary>
    /// Gets or Sets TimeUpdated
    /// </summary>
    [DataMember(Name="timeUpdated", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timeUpdated")]
    public DateTime? TimeUpdated { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class VeemCancelPaymentResponse {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  RequestId: ").Append(RequestId).Append("\n");
      sb.Append("  PayeeAmount: ").Append(PayeeAmount).Append("\n");
      sb.Append("  Notes: ").Append(Notes).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  Payee: ").Append(Payee).Append("\n");
      sb.Append("  PurposeOfPayment: ").Append(PurposeOfPayment).Append("\n");
      sb.Append("  ClaimLink: ").Append(ClaimLink).Append("\n");
      sb.Append("  TimeCreated: ").Append(TimeCreated).Append("\n");
      sb.Append("  TimeUpdated: ").Append(TimeUpdated).Append("\n");
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
