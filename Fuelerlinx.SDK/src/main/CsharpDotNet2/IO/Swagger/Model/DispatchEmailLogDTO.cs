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
  public class DispatchEmailLogDTO {
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
    /// Gets or Sets DateAndTime
    /// </summary>
    [DataMember(Name="dateAndTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateAndTime")]
    public DateTime? DateAndTime { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets UserId
    /// </summary>
    [DataMember(Name="userId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userId")]
    public int? UserId { get; set; }

    /// <summary>
    /// Gets or Sets TailNumber
    /// </summary>
    [DataMember(Name="tailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumber")]
    public string TailNumber { get; set; }

    /// <summary>
    /// Gets or Sets EmailSent
    /// </summary>
    [DataMember(Name="emailSent", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "emailSent")]
    public bool? EmailSent { get; set; }

    /// <summary>
    /// Gets or Sets EmailAddressesIncluded
    /// </summary>
    [DataMember(Name="emailAddressesIncluded", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "emailAddressesIncluded")]
    public string EmailAddressesIncluded { get; set; }

    /// <summary>
    /// Gets or Sets EmailError
    /// </summary>
    [DataMember(Name="emailError", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "emailError")]
    public string EmailError { get; set; }

    /// <summary>
    /// Gets or Sets DispatchMailType
    /// </summary>
    [DataMember(Name="dispatchMailType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dispatchMailType")]
    public int? DispatchMailType { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DispatchEmailLogDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
      sb.Append("  DateAndTime: ").Append(DateAndTime).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
      sb.Append("  EmailSent: ").Append(EmailSent).Append("\n");
      sb.Append("  EmailAddressesIncluded: ").Append(EmailAddressesIncluded).Append("\n");
      sb.Append("  EmailError: ").Append(EmailError).Append("\n");
      sb.Append("  DispatchMailType: ").Append(DispatchMailType).Append("\n");
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
