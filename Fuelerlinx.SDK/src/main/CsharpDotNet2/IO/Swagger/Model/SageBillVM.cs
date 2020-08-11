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
  public class SageBillVM {
    /// <summary>
    /// Gets or Sets SageId
    /// </summary>
    [DataMember(Name="sageId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sageId")]
    public int? SageId { get; set; }

    /// <summary>
    /// Gets or Sets AmountDue
    /// </summary>
    [DataMember(Name="amountDue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "amountDue")]
    public double? AmountDue { get; set; }

    /// <summary>
    /// Gets or Sets BaseAmountDue
    /// </summary>
    [DataMember(Name="baseAmountDue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "baseAmountDue")]
    public double? BaseAmountDue { get; set; }

    /// <summary>
    /// Gets or Sets BaseBillAmount
    /// </summary>
    [DataMember(Name="baseBillAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "baseBillAmount")]
    public double? BaseBillAmount { get; set; }

    /// <summary>
    /// Gets or Sets BillAmount
    /// </summary>
    [DataMember(Name="billAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "billAmount")]
    public double? BillAmount { get; set; }

    /// <summary>
    /// Gets or Sets MegaEntityId
    /// </summary>
    [DataMember(Name="megaEntityId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "megaEntityId")]
    public string MegaEntityId { get; set; }

    /// <summary>
    /// Gets or Sets MegaEntityName
    /// </summary>
    [DataMember(Name="megaEntityName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "megaEntityName")]
    public string MegaEntityName { get; set; }

    /// <summary>
    /// Gets or Sets DateWhenCreated
    /// </summary>
    [DataMember(Name="dateWhenCreated", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateWhenCreated")]
    public DateTime? DateWhenCreated { get; set; }

    /// <summary>
    /// Gets or Sets DateWhenDue
    /// </summary>
    [DataMember(Name="dateWhenDue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateWhenDue")]
    public DateTime? DateWhenDue { get; set; }

    /// <summary>
    /// Gets or Sets DocumentNo
    /// </summary>
    [DataMember(Name="documentNo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "documentNo")]
    public string DocumentNo { get; set; }

    /// <summary>
    /// Gets or Sets DateWhenPosted
    /// </summary>
    [DataMember(Name="dateWhenPosted", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateWhenPosted")]
    public DateTime? DateWhenPosted { get; set; }

    /// <summary>
    /// Gets or Sets Message
    /// </summary>
    [DataMember(Name="message", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }

    /// <summary>
    /// Gets or Sets Ponumber
    /// </summary>
    [DataMember(Name="ponumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ponumber")]
    public string Ponumber { get; set; }

    /// <summary>
    /// Gets or Sets State
    /// </summary>
    [DataMember(Name="state", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "state")]
    public string State { get; set; }

    /// <summary>
    /// Gets or Sets Subtotal
    /// </summary>
    [DataMember(Name="subtotal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "subtotal")]
    public double? Subtotal { get; set; }

    /// <summary>
    /// Gets or Sets Total
    /// </summary>
    [DataMember(Name="total", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "total")]
    public double? Total { get; set; }

    /// <summary>
    /// Gets or Sets TransactionSubtotal
    /// </summary>
    [DataMember(Name="transactionSubtotal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionSubtotal")]
    public double? TransactionSubtotal { get; set; }

    /// <summary>
    /// Gets or Sets TransactionTotal
    /// </summary>
    [DataMember(Name="transactionTotal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionTotal")]
    public double? TransactionTotal { get; set; }

    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name="type", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    /// <summary>
    /// Gets or Sets Vendor
    /// </summary>
    [DataMember(Name="vendor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "vendor")]
    public string Vendor { get; set; }

    /// <summary>
    /// Gets or Sets VendorDocNo
    /// </summary>
    [DataMember(Name="vendorDocNo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "vendorDocNo")]
    public string VendorDocNo { get; set; }

    /// <summary>
    /// Gets or Sets CustVendName
    /// </summary>
    [DataMember(Name="custVendName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "custVendName")]
    public string CustVendName { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SageBillVM {\n");
      sb.Append("  SageId: ").Append(SageId).Append("\n");
      sb.Append("  AmountDue: ").Append(AmountDue).Append("\n");
      sb.Append("  BaseAmountDue: ").Append(BaseAmountDue).Append("\n");
      sb.Append("  BaseBillAmount: ").Append(BaseBillAmount).Append("\n");
      sb.Append("  BillAmount: ").Append(BillAmount).Append("\n");
      sb.Append("  MegaEntityId: ").Append(MegaEntityId).Append("\n");
      sb.Append("  MegaEntityName: ").Append(MegaEntityName).Append("\n");
      sb.Append("  DateWhenCreated: ").Append(DateWhenCreated).Append("\n");
      sb.Append("  DateWhenDue: ").Append(DateWhenDue).Append("\n");
      sb.Append("  DocumentNo: ").Append(DocumentNo).Append("\n");
      sb.Append("  DateWhenPosted: ").Append(DateWhenPosted).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
      sb.Append("  Ponumber: ").Append(Ponumber).Append("\n");
      sb.Append("  State: ").Append(State).Append("\n");
      sb.Append("  Subtotal: ").Append(Subtotal).Append("\n");
      sb.Append("  Total: ").Append(Total).Append("\n");
      sb.Append("  TransactionSubtotal: ").Append(TransactionSubtotal).Append("\n");
      sb.Append("  TransactionTotal: ").Append(TransactionTotal).Append("\n");
      sb.Append("  Type: ").Append(Type).Append("\n");
      sb.Append("  Vendor: ").Append(Vendor).Append("\n");
      sb.Append("  VendorDocNo: ").Append(VendorDocNo).Append("\n");
      sb.Append("  CustVendName: ").Append(CustVendName).Append("\n");
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
