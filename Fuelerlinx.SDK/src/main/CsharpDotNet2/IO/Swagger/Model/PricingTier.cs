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
  public class PricingTier {
    /// <summary>
    /// Gets or Sets MinGal
    /// </summary>
    [DataMember(Name="minGal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minGal")]
    public int? MinGal { get; set; }

    /// <summary>
    /// Gets or Sets MaxGal
    /// </summary>
    [DataMember(Name="maxGal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxGal")]
    public int? MaxGal { get; set; }

    /// <summary>
    /// Gets or Sets PriceGal
    /// </summary>
    [DataMember(Name="priceGal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceGal")]
    public string PriceGal { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets MotRate
    /// </summary>
    [DataMember(Name="motRate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "motRate")]
    public string MotRate { get; set; }

    /// <summary>
    /// Gets or Sets EstTaxesGal
    /// </summary>
    [DataMember(Name="estTaxesGal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "estTaxesGal")]
    public string EstTaxesGal { get; set; }

    /// <summary>
    /// Gets or Sets BasePriceGal
    /// </summary>
    [DataMember(Name="basePriceGal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "basePriceGal")]
    public string BasePriceGal { get; set; }

    /// <summary>
    /// Gets or Sets TotalPriceGal
    /// </summary>
    [DataMember(Name="totalPriceGal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "totalPriceGal")]
    public string TotalPriceGal { get; set; }

    /// <summary>
    /// Gets or Sets Notes
    /// </summary>
    [DataMember(Name="notes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "notes")]
    public string Notes { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PricingTier {\n");
      sb.Append("  MinGal: ").Append(MinGal).Append("\n");
      sb.Append("  MaxGal: ").Append(MaxGal).Append("\n");
      sb.Append("  PriceGal: ").Append(PriceGal).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  MotRate: ").Append(MotRate).Append("\n");
      sb.Append("  EstTaxesGal: ").Append(EstTaxesGal).Append("\n");
      sb.Append("  BasePriceGal: ").Append(BasePriceGal).Append("\n");
      sb.Append("  TotalPriceGal: ").Append(TotalPriceGal).Append("\n");
      sb.Append("  Notes: ").Append(Notes).Append("\n");
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
