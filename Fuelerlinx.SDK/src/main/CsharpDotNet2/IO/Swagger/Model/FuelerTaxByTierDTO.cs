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
  public class FuelerTaxByTierDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets FuelMasterId
    /// </summary>
    [DataMember(Name="fuelMasterId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelMasterId")]
    public int? FuelMasterId { get; set; }

    /// <summary>
    /// Gets or Sets MinVolume
    /// </summary>
    [DataMember(Name="minVolume", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minVolume")]
    public double? MinVolume { get; set; }

    /// <summary>
    /// Gets or Sets MaxVolume
    /// </summary>
    [DataMember(Name="maxVolume", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxVolume")]
    public double? MaxVolume { get; set; }

    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets Price
    /// </summary>
    [DataMember(Name="price", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "price")]
    public double? Price { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Gets or Sets UnitOfMeasure
    /// </summary>
    [DataMember(Name="unitOfMeasure", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "unitOfMeasure")]
    public string UnitOfMeasure { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="priceType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceType")]
    public int? PriceType { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FuelerTaxByTierDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  FuelMasterId: ").Append(FuelMasterId).Append("\n");
      sb.Append("  MinVolume: ").Append(MinVolume).Append("\n");
      sb.Append("  MaxVolume: ").Append(MaxVolume).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  Price: ").Append(Price).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      sb.Append("  UnitOfMeasure: ").Append(UnitOfMeasure).Append("\n");
      sb.Append("  PriceType: ").Append(PriceType).Append("\n");
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
