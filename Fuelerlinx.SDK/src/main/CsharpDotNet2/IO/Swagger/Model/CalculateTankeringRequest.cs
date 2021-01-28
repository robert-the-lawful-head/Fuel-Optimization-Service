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
  public class CalculateTankeringRequest {
    /// <summary>
    /// Gets or Sets Trip
    /// </summary>
    [DataMember(Name="trip", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "trip")]
    public Trip Trip { get; set; }

    /// <summary>
    /// Gets or Sets SummaryOptions
    /// </summary>
    [DataMember(Name="summaryOptions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "summaryOptions")]
    public SummaryOptions SummaryOptions { get; set; }

    /// <summary>
    /// Gets or Sets TankeringOptions
    /// </summary>
    [DataMember(Name="tankeringOptions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tankeringOptions")]
    public TripTankeringOptions TankeringOptions { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CalculateTankeringRequest {\n");
      sb.Append("  Trip: ").Append(Trip).Append("\n");
      sb.Append("  SummaryOptions: ").Append(SummaryOptions).Append("\n");
      sb.Append("  TankeringOptions: ").Append(TankeringOptions).Append("\n");
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
