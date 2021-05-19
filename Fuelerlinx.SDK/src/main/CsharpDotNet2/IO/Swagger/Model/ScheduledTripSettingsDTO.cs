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
  public class ScheduledTripSettingsDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets DepartureDispatchOnly
    /// </summary>
    [DataMember(Name="departureDispatchOnly", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureDispatchOnly")]
    public bool? DepartureDispatchOnly { get; set; }

    /// <summary>
    /// Gets or Sets ShowArrivalOfFinalLeg
    /// </summary>
    [DataMember(Name="showArrivalOfFinalLeg", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "showArrivalOfFinalLeg")]
    public bool? ShowArrivalOfFinalLeg { get; set; }

    /// <summary>
    /// Gets or Sets DispatchMessageFormat
    /// </summary>
    [DataMember(Name="dispatchMessageFormat", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dispatchMessageFormat")]
    public string DispatchMessageFormat { get; set; }

    /// <summary>
    /// Gets or Sets NonDispatchMessageFormat
    /// </summary>
    [DataMember(Name="nonDispatchMessageFormat", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "nonDispatchMessageFormat")]
    public string NonDispatchMessageFormat { get; set; }

    /// <summary>
    /// Gets or Sets AllowRampFeeInTripSheet
    /// </summary>
    [DataMember(Name="allowRampFeeInTripSheet", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "allowRampFeeInTripSheet")]
    public bool? AllowRampFeeInTripSheet { get; set; }

    /// <summary>
    /// Gets or Sets CopyScheduledCrewEmails
    /// </summary>
    [DataMember(Name="copyScheduledCrewEmails", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "copyScheduledCrewEmails")]
    public bool? CopyScheduledCrewEmails { get; set; }

    /// <summary>
    /// Gets or Sets CreateExpenseInFos
    /// </summary>
    [DataMember(Name="createExpenseInFos", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "createExpenseInFos")]
    public bool? CreateExpenseInFos { get; set; }

    /// <summary>
    /// Gets or Sets AddTimeStampToFuelComment
    /// </summary>
    [DataMember(Name="addTimeStampToFuelComment", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addTimeStampToFuelComment")]
    public bool? AddTimeStampToFuelComment { get; set; }

    /// <summary>
    /// Gets or Sets AddFillerLegs
    /// </summary>
    [DataMember(Name="addFillerLegs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addFillerLegs")]
    public bool? AddFillerLegs { get; set; }

    /// <summary>
    /// Gets or Sets IncludeTimeStampOfUserNotes
    /// </summary>
    [DataMember(Name="includeTimeStampOfUserNotes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "includeTimeStampOfUserNotes")]
    public bool? IncludeTimeStampOfUserNotes { get; set; }

    /// <summary>
    /// Gets or Sets PushSkippedLegsToTripSheet
    /// </summary>
    [DataMember(Name="pushSkippedLegsToTripSheet", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pushSkippedLegsToTripSheet")]
    public bool? PushSkippedLegsToTripSheet { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="legSortType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legSortType")]
    public int? LegSortType { get; set; }

    /// <summary>
    /// Gets or Sets UseScheduledEteDomestic
    /// </summary>
    [DataMember(Name="useScheduledEteDomestic", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "useScheduledEteDomestic")]
    public bool? UseScheduledEteDomestic { get; set; }

    /// <summary>
    /// Gets or Sets UseScheduledEteInternational
    /// </summary>
    [DataMember(Name="useScheduledEteInternational", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "useScheduledEteInternational")]
    public bool? UseScheduledEteInternational { get; set; }

    /// <summary>
    /// Gets or Sets UserPreferredFbo
    /// </summary>
    [DataMember(Name="userPreferredFbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userPreferredFbo")]
    public bool? UserPreferredFbo { get; set; }

    /// <summary>
    /// Gets or Sets NonDispatchTripSheetNotesPreText
    /// </summary>
    [DataMember(Name="nonDispatchTripSheetNotesPreText", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "nonDispatchTripSheetNotesPreText")]
    public string NonDispatchTripSheetNotesPreText { get; set; }

    /// <summary>
    /// Gets or Sets WriteToTripSheetOnDispatch
    /// </summary>
    [DataMember(Name="writeToTripSheetOnDispatch", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "writeToTripSheetOnDispatch")]
    public bool? WriteToTripSheetOnDispatch { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ScheduledTripSettingsDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  DepartureDispatchOnly: ").Append(DepartureDispatchOnly).Append("\n");
      sb.Append("  ShowArrivalOfFinalLeg: ").Append(ShowArrivalOfFinalLeg).Append("\n");
      sb.Append("  DispatchMessageFormat: ").Append(DispatchMessageFormat).Append("\n");
      sb.Append("  NonDispatchMessageFormat: ").Append(NonDispatchMessageFormat).Append("\n");
      sb.Append("  AllowRampFeeInTripSheet: ").Append(AllowRampFeeInTripSheet).Append("\n");
      sb.Append("  CopyScheduledCrewEmails: ").Append(CopyScheduledCrewEmails).Append("\n");
      sb.Append("  CreateExpenseInFos: ").Append(CreateExpenseInFos).Append("\n");
      sb.Append("  AddTimeStampToFuelComment: ").Append(AddTimeStampToFuelComment).Append("\n");
      sb.Append("  AddFillerLegs: ").Append(AddFillerLegs).Append("\n");
      sb.Append("  IncludeTimeStampOfUserNotes: ").Append(IncludeTimeStampOfUserNotes).Append("\n");
      sb.Append("  PushSkippedLegsToTripSheet: ").Append(PushSkippedLegsToTripSheet).Append("\n");
      sb.Append("  LegSortType: ").Append(LegSortType).Append("\n");
      sb.Append("  UseScheduledEteDomestic: ").Append(UseScheduledEteDomestic).Append("\n");
      sb.Append("  UseScheduledEteInternational: ").Append(UseScheduledEteInternational).Append("\n");
      sb.Append("  UserPreferredFbo: ").Append(UserPreferredFbo).Append("\n");
      sb.Append("  NonDispatchTripSheetNotesPreText: ").Append(NonDispatchTripSheetNotesPreText).Append("\n");
      sb.Append("  WriteToTripSheetOnDispatch: ").Append(WriteToTripSheetOnDispatch).Append("\n");
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
