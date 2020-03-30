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
  public class TripTankeringOptions {
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="priceSearchPreference", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceSearchPreference")]
    public int? PriceSearchPreference { get; set; }

    /// <summary>
    /// Gets or Sets ForceMinimumUpliftBias
    /// </summary>
    [DataMember(Name="forceMinimumUpliftBias", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "forceMinimumUpliftBias")]
    public bool? ForceMinimumUpliftBias { get; set; }

    /// <summary>
    /// Gets or Sets MinimumUpliftBiasFeeAmount
    /// </summary>
    [DataMember(Name="minimumUpliftBiasFeeAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minimumUpliftBiasFeeAmount")]
    public double? MinimumUpliftBiasFeeAmount { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TripTankeringOptions {\n");
      sb.Append("  PriceSearchPreference: ").Append(PriceSearchPreference).Append("\n");
      sb.Append("  ForceMinimumUpliftBias: ").Append(ForceMinimumUpliftBias).Append("\n");
      sb.Append("  MinimumUpliftBiasFeeAmount: ").Append(MinimumUpliftBiasFeeAmount).Append("\n");
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
