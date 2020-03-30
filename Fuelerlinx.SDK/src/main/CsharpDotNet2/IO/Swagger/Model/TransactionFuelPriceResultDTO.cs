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
  public class TransactionFuelPriceResultDTO {
    /// <summary>
    /// Gets or Sets RequestId
    /// </summary>
    [DataMember(Name="requestId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "requestId")]
    public int? RequestId { get; set; }

    /// <summary>
    /// Gets or Sets FuelerId
    /// </summary>
    [DataMember(Name="fuelerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerId")]
    public int? FuelerId { get; set; }

    /// <summary>
    /// Gets or Sets MinVolume
    /// </summary>
    [DataMember(Name="minVolume", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minVolume")]
    public Weight MinVolume { get; set; }

    /// <summary>
    /// Gets or Sets MaxVolume
    /// </summary>
    [DataMember(Name="maxVolume", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxVolume")]
    public Weight MaxVolume { get; set; }

    /// <summary>
    /// Gets or Sets Price
    /// </summary>
    [DataMember(Name="price", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "price")]
    public double? Price { get; set; }

    /// <summary>
    /// Gets or Sets EstimatedTotal
    /// </summary>
    [DataMember(Name="estimatedTotal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "estimatedTotal")]
    public double? EstimatedTotal { get; set; }

    /// <summary>
    /// Gets or Sets Fbo
    /// </summary>
    [DataMember(Name="fbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fbo")]
    public string Fbo { get; set; }

    /// <summary>
    /// Gets or Sets FuelMasterId
    /// </summary>
    [DataMember(Name="fuelMasterId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelMasterId")]
    public int? FuelMasterId { get; set; }

    /// <summary>
    /// Gets or Sets Product
    /// </summary>
    [DataMember(Name="product", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "product")]
    public string Product { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets FuelerName
    /// </summary>
    [DataMember(Name="fuelerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerName")]
    public string FuelerName { get; set; }

    /// <summary>
    /// Gets or Sets Notes
    /// </summary>
    [DataMember(Name="notes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "notes")]
    public string Notes { get; set; }

    /// <summary>
    /// Gets or Sets BasePrice
    /// </summary>
    [DataMember(Name="basePrice", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "basePrice")]
    public double? BasePrice { get; set; }

    /// <summary>
    /// Gets or Sets PriceCurrency
    /// </summary>
    [DataMember(Name="priceCurrency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceCurrency")]
    public string PriceCurrency { get; set; }

    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TransactionFuelPriceResultDTO {\n");
      sb.Append("  RequestId: ").Append(RequestId).Append("\n");
      sb.Append("  FuelerId: ").Append(FuelerId).Append("\n");
      sb.Append("  MinVolume: ").Append(MinVolume).Append("\n");
      sb.Append("  MaxVolume: ").Append(MaxVolume).Append("\n");
      sb.Append("  Price: ").Append(Price).Append("\n");
      sb.Append("  EstimatedTotal: ").Append(EstimatedTotal).Append("\n");
      sb.Append("  Fbo: ").Append(Fbo).Append("\n");
      sb.Append("  FuelMasterId: ").Append(FuelMasterId).Append("\n");
      sb.Append("  Product: ").Append(Product).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  FuelerName: ").Append(FuelerName).Append("\n");
      sb.Append("  Notes: ").Append(Notes).Append("\n");
      sb.Append("  BasePrice: ").Append(BasePrice).Append("\n");
      sb.Append("  PriceCurrency: ").Append(PriceCurrency).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
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
