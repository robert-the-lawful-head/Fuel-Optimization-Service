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
  public class WeeklyPricingDTO {
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
    /// Gets or Sets FuelerId
    /// </summary>
    [DataMember(Name="fuelerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerId")]
    public int? FuelerId { get; set; }

    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets Fbo
    /// </summary>
    [DataMember(Name="fbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fbo")]
    public string Fbo { get; set; }

    /// <summary>
    /// Gets or Sets Price
    /// </summary>
    [DataMember(Name="price", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "price")]
    public double? Price { get; set; }

    /// <summary>
    /// Gets or Sets MinGallons
    /// </summary>
    [DataMember(Name="minGallons", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minGallons")]
    public int? MinGallons { get; set; }

    /// <summary>
    /// Gets or Sets MaxGallons
    /// </summary>
    [DataMember(Name="maxGallons", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxGallons")]
    public int? MaxGallons { get; set; }

    /// <summary>
    /// Gets or Sets Product
    /// </summary>
    [DataMember(Name="product", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "product")]
    public string Product { get; set; }

    /// <summary>
    /// Gets or Sets TailNumber
    /// </summary>
    [DataMember(Name="tailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumber")]
    public string TailNumber { get; set; }

    /// <summary>
    /// Gets or Sets Notes
    /// </summary>
    [DataMember(Name="notes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "notes")]
    public string Notes { get; set; }

    /// <summary>
    /// Gets or Sets DateAdded
    /// </summary>
    [DataMember(Name="dateAdded", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateAdded")]
    public DateTime? DateAdded { get; set; }

    /// <summary>
    /// Gets or Sets EffectiveDate
    /// </summary>
    [DataMember(Name="effectiveDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "effectiveDate")]
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// Gets or Sets ExpirationDate
    /// </summary>
    [DataMember(Name="expirationDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "expirationDate")]
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Gets or Sets PriceCurrency
    /// </summary>
    [DataMember(Name="priceCurrency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceCurrency")]
    public string PriceCurrency { get; set; }

    /// <summary>
    /// Gets or Sets Inclusive
    /// </summary>
    [DataMember(Name="inclusive", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "inclusive")]
    public bool? Inclusive { get; set; }

    /// <summary>
    /// Gets or Sets BasePrice
    /// </summary>
    [DataMember(Name="basePrice", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "basePrice")]
    public double? BasePrice { get; set; }

    /// <summary>
    /// Gets or Sets VolumeSpecificNote
    /// </summary>
    [DataMember(Name="volumeSpecificNote", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "volumeSpecificNote")]
    public string VolumeSpecificNote { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class WeeklyPricingDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  FuelerId: ").Append(FuelerId).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  Fbo: ").Append(Fbo).Append("\n");
      sb.Append("  Price: ").Append(Price).Append("\n");
      sb.Append("  MinGallons: ").Append(MinGallons).Append("\n");
      sb.Append("  MaxGallons: ").Append(MaxGallons).Append("\n");
      sb.Append("  Product: ").Append(Product).Append("\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
      sb.Append("  Notes: ").Append(Notes).Append("\n");
      sb.Append("  DateAdded: ").Append(DateAdded).Append("\n");
      sb.Append("  EffectiveDate: ").Append(EffectiveDate).Append("\n");
      sb.Append("  ExpirationDate: ").Append(ExpirationDate).Append("\n");
      sb.Append("  PriceCurrency: ").Append(PriceCurrency).Append("\n");
      sb.Append("  Inclusive: ").Append(Inclusive).Append("\n");
      sb.Append("  BasePrice: ").Append(BasePrice).Append("\n");
      sb.Append("  VolumeSpecificNote: ").Append(VolumeSpecificNote).Append("\n");
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
