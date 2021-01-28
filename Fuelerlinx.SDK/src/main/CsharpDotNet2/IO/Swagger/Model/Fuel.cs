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
  public class Fuel {
    /// <summary>
    /// Gets or Sets PricingTier
    /// </summary>
    [DataMember(Name="pricingTier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pricingTier")]
    public List<PricingTier> PricingTier { get; set; }

    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name="type", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets Group
    /// </summary>
    [DataMember(Name="group", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "group")]
    public string Group { get; set; }

    /// <summary>
    /// Gets or Sets FuelPAPValue
    /// </summary>
    [DataMember(Name="fuelPAPValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelPAPValue")]
    public string FuelPAPValue { get; set; }

    /// <summary>
    /// Gets or Sets EffectiveDate
    /// </summary>
    [DataMember(Name="effectiveDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "effectiveDate")]
    public string EffectiveDate { get; set; }

    /// <summary>
    /// Gets or Sets ExpirationDate
    /// </summary>
    [DataMember(Name="expirationDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "expirationDate")]
    public string ExpirationDate { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Fuel {\n");
      sb.Append("  PricingTier: ").Append(PricingTier).Append("\n");
      sb.Append("  Type: ").Append(Type).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Group: ").Append(Group).Append("\n");
      sb.Append("  FuelPAPValue: ").Append(FuelPAPValue).Append("\n");
      sb.Append("  EffectiveDate: ").Append(EffectiveDate).Append("\n");
      sb.Append("  ExpirationDate: ").Append(ExpirationDate).Append("\n");
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
