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
  public class RecommendationSummary {
    /// <summary>
    /// Gets or Sets TripID
    /// </summary>
    [DataMember(Name="tripID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripID")]
    public string TripID { get; set; }

    /// <summary>
    /// Gets or Sets LegNumber
    /// </summary>
    [DataMember(Name="legNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legNumber")]
    public int? LegNumber { get; set; }

    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets Overview
    /// </summary>
    [DataMember(Name="overview", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "overview")]
    public string Overview { get; set; }

    /// <summary>
    /// Gets or Sets AdditionalInstructions
    /// </summary>
    [DataMember(Name="additionalInstructions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "additionalInstructions")]
    public List<string> AdditionalInstructions { get; set; }

    /// <summary>
    /// Gets or Sets ThingsToNote
    /// </summary>
    [DataMember(Name="thingsToNote", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "thingsToNote")]
    public List<string> ThingsToNote { get; set; }

    /// <summary>
    /// Gets or Sets MultiLegOption
    /// </summary>
    [DataMember(Name="multiLegOption", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "multiLegOption")]
    public MultiLegOption MultiLegOption { get; set; }

    /// <summary>
    /// Gets or Sets AdditionalInstructionsDisplayText
    /// </summary>
    [DataMember(Name="additionalInstructionsDisplayText", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "additionalInstructionsDisplayText")]
    public string AdditionalInstructionsDisplayText { get; set; }

    /// <summary>
    /// Gets or Sets ThingsToNoteDisplayText
    /// </summary>
    [DataMember(Name="thingsToNoteDisplayText", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "thingsToNoteDisplayText")]
    public string ThingsToNoteDisplayText { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class RecommendationSummary {\n");
      sb.Append("  TripID: ").Append(TripID).Append("\n");
      sb.Append("  LegNumber: ").Append(LegNumber).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  Overview: ").Append(Overview).Append("\n");
      sb.Append("  AdditionalInstructions: ").Append(AdditionalInstructions).Append("\n");
      sb.Append("  ThingsToNote: ").Append(ThingsToNote).Append("\n");
      sb.Append("  MultiLegOption: ").Append(MultiLegOption).Append("\n");
      sb.Append("  AdditionalInstructionsDisplayText: ").Append(AdditionalInstructionsDisplayText).Append("\n");
      sb.Append("  ThingsToNoteDisplayText: ").Append(ThingsToNoteDisplayText).Append("\n");
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
