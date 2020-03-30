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
  public class TransactionAttachmentDTO {
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
    /// Gets or Sets DateAdded
    /// </summary>
    [DataMember(Name="dateAdded", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateAdded")]
    public DateTime? DateAdded { get; set; }

    /// <summary>
    /// Gets or Sets FileName
    /// </summary>
    [DataMember(Name="fileName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fileName")]
    public string FileName { get; set; }

    /// <summary>
    /// Gets or Sets AttachmentFileDataId
    /// </summary>
    [DataMember(Name="attachmentFileDataId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "attachmentFileDataId")]
    public int? AttachmentFileDataId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="attachmentType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "attachmentType")]
    public int? AttachmentType { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TransactionAttachmentDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
      sb.Append("  DateAdded: ").Append(DateAdded).Append("\n");
      sb.Append("  FileName: ").Append(FileName).Append("\n");
      sb.Append("  AttachmentFileDataId: ").Append(AttachmentFileDataId).Append("\n");
      sb.Append("  AttachmentType: ").Append(AttachmentType).Append("\n");
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
