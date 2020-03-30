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
  public class PostTransactionNoteRequest {
    /// <summary>
    /// Gets or Sets TransactionId
    /// </summary>
    [DataMember(Name="transactionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionId")]
    public int? TransactionId { get; set; }

    /// <summary>
    /// Gets or Sets Note
    /// </summary>
    [DataMember(Name="note", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "note")]
    public string Note { get; set; }

    /// <summary>
    /// Gets or Sets DateAdded
    /// </summary>
    [DataMember(Name="dateAdded", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateAdded")]
    public string DateAdded { get; set; }

    /// <summary>
    /// Gets or Sets TimeAdded
    /// </summary>
    [DataMember(Name="timeAdded", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timeAdded")]
    public string TimeAdded { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="noteType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "noteType")]
    public int? NoteType { get; set; }

    /// <summary>
    /// Gets or Sets AddedByUserId
    /// </summary>
    [DataMember(Name="addedByUserId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addedByUserId")]
    public int? AddedByUserId { get; set; }

    /// <summary>
    /// Gets or Sets AddedByName
    /// </summary>
    [DataMember(Name="addedByName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addedByName")]
    public string AddedByName { get; set; }

    /// <summary>
    /// Gets or Sets AssociatedWithNoteId
    /// </summary>
    [DataMember(Name="associatedWithNoteId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "associatedWithNoteId")]
    public int? AssociatedWithNoteId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="state", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "state")]
    public int? State { get; set; }

    /// <summary>
    /// Gets or Sets TimeZone
    /// </summary>
    [DataMember(Name="timeZone", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timeZone")]
    public string TimeZone { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostTransactionNoteRequest {\n");
      sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
      sb.Append("  Note: ").Append(Note).Append("\n");
      sb.Append("  DateAdded: ").Append(DateAdded).Append("\n");
      sb.Append("  TimeAdded: ").Append(TimeAdded).Append("\n");
      sb.Append("  NoteType: ").Append(NoteType).Append("\n");
      sb.Append("  AddedByUserId: ").Append(AddedByUserId).Append("\n");
      sb.Append("  AddedByName: ").Append(AddedByName).Append("\n");
      sb.Append("  AssociatedWithNoteId: ").Append(AssociatedWithNoteId).Append("\n");
      sb.Append("  State: ").Append(State).Append("\n");
      sb.Append("  TimeZone: ").Append(TimeZone).Append("\n");
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
