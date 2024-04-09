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
  public class MobileMainMenuSubFeaturesDTO {
    /// <summary>
    /// Gets or Sets QuotingAndTankering
    /// </summary>
    [DataMember(Name="QuotingAndTankering", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "QuotingAndTankering")]
    public MenuFeatureDTO QuotingAndTankering { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingIntegration
    /// </summary>
    [DataMember(Name="SchedulingIntegration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "SchedulingIntegration")]
    public MenuFeatureDTO SchedulingIntegration { get; set; }

    /// <summary>
    /// Gets or Sets Transactions
    /// </summary>
    [DataMember(Name="Transactions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Transactions")]
    public MenuFeatureDTO Transactions { get; set; }

    /// <summary>
    /// Gets or Sets FeesAndNotes
    /// </summary>
    [DataMember(Name="FeesAndNotes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FeesAndNotes")]
    public MenuFeatureDTO FeesAndNotes { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MobileMainMenuSubFeaturesDTO {\n");
      sb.Append("  QuotingAndTankering: ").Append(QuotingAndTankering).Append("\n");
      sb.Append("  SchedulingIntegration: ").Append(SchedulingIntegration).Append("\n");
      sb.Append("  Transactions: ").Append(Transactions).Append("\n");
      sb.Append("  FeesAndNotes: ").Append(FeesAndNotes).Append("\n");
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
