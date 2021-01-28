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
  public class MostCommonReportedRampFeeResultDTO {
    /// <summary>
    /// Gets or Sets RampFee
    /// </summary>
    [DataMember(Name="rampFee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFee")]
    public double? RampFee { get; set; }

    /// <summary>
    /// Gets or Sets RampFeeWaived
    /// </summary>
    [DataMember(Name="rampFeeWaived", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFeeWaived")]
    public Weight RampFeeWaived { get; set; }

    /// <summary>
    /// Gets or Sets AppliesTo
    /// </summary>
    [DataMember(Name="appliesTo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "appliesTo")]
    public string AppliesTo { get; set; }

    /// <summary>
    /// Gets or Sets LastUpdated
    /// </summary>
    [DataMember(Name="lastUpdated", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lastUpdated")]
    public string LastUpdated { get; set; }

    /// <summary>
    /// Gets or Sets ReportCount
    /// </summary>
    [DataMember(Name="reportCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reportCount")]
    public int? ReportCount { get; set; }

    /// <summary>
    /// Gets or Sets Source
    /// </summary>
    [DataMember(Name="source", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "source")]
    public string Source { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MostCommonReportedRampFeeResultDTO {\n");
      sb.Append("  RampFee: ").Append(RampFee).Append("\n");
      sb.Append("  RampFeeWaived: ").Append(RampFeeWaived).Append("\n");
      sb.Append("  AppliesTo: ").Append(AppliesTo).Append("\n");
      sb.Append("  LastUpdated: ").Append(LastUpdated).Append("\n");
      sb.Append("  ReportCount: ").Append(ReportCount).Append("\n");
      sb.Append("  Source: ").Append(Source).Append("\n");
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
