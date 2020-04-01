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
  public class RampFeeByCompanyNoteDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets RampFeeByCompanyId
    /// </summary>
    [DataMember(Name="rampFeeByCompanyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFeeByCompanyId")]
    public int? RampFeeByCompanyId { get; set; }

    /// <summary>
    /// Gets or Sets DateStamp
    /// </summary>
    [DataMember(Name="dateStamp", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateStamp")]
    public string DateStamp { get; set; }

    /// <summary>
    /// Gets or Sets TimeStamp
    /// </summary>
    [DataMember(Name="timeStamp", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timeStamp")]
    public string TimeStamp { get; set; }

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
    /// Gets or Sets Note
    /// </summary>
    [DataMember(Name="note", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "note")]
    public string Note { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="state", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "state")]
    public int? State { get; set; }

    /// <summary>
    /// Gets or Sets AssociatedWithNoteId
    /// </summary>
    [DataMember(Name="associatedWithNoteId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "associatedWithNoteId")]
    public int? AssociatedWithNoteId { get; set; }

    /// <summary>
    /// Gets or Sets TimeZone
    /// </summary>
    [DataMember(Name="timeZone", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timeZone")]
    public string TimeZone { get; set; }

    /// <summary>
    /// Gets or Sets RampFeeByCompany
    /// </summary>
    [DataMember(Name="rampFeeByCompany", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFeeByCompany")]
    public RampFeeByCompanyDTO RampFeeByCompany { get; set; }

    /// <summary>
    /// Gets or Sets StateDescription
    /// </summary>
    [DataMember(Name="stateDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "stateDescription")]
    public string StateDescription { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class RampFeeByCompanyNoteDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  RampFeeByCompanyId: ").Append(RampFeeByCompanyId).Append("\n");
      sb.Append("  DateStamp: ").Append(DateStamp).Append("\n");
      sb.Append("  TimeStamp: ").Append(TimeStamp).Append("\n");
      sb.Append("  AddedByUserId: ").Append(AddedByUserId).Append("\n");
      sb.Append("  AddedByName: ").Append(AddedByName).Append("\n");
      sb.Append("  Note: ").Append(Note).Append("\n");
      sb.Append("  State: ").Append(State).Append("\n");
      sb.Append("  AssociatedWithNoteId: ").Append(AssociatedWithNoteId).Append("\n");
      sb.Append("  TimeZone: ").Append(TimeZone).Append("\n");
      sb.Append("  RampFeeByCompany: ").Append(RampFeeByCompany).Append("\n");
      sb.Append("  StateDescription: ").Append(StateDescription).Append("\n");
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
