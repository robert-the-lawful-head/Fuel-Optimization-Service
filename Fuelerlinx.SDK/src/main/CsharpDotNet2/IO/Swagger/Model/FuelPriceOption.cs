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
  public class FuelPriceOption {
    /// <summary>
    /// Gets or Sets Price
    /// </summary>
    [DataMember(Name="price", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "price")]
    public double? Price { get; set; }

    /// <summary>
    /// Gets or Sets Notes
    /// </summary>
    [DataMember(Name="notes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "notes")]
    public string Notes { get; set; }

    /// <summary>
    /// Gets or Sets Fbo
    /// </summary>
    [DataMember(Name="fbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fbo")]
    public string Fbo { get; set; }

    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets Fueler
    /// </summary>
    [DataMember(Name="fueler", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fueler")]
    public string Fueler { get; set; }

    /// <summary>
    /// Gets or Sets Product
    /// </summary>
    [DataMember(Name="product", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "product")]
    public string Product { get; set; }

    /// <summary>
    /// Gets or Sets FboEmail
    /// </summary>
    [DataMember(Name="fboEmail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboEmail")]
    public string FboEmail { get; set; }

    /// <summary>
    /// Gets or Sets IsPreferredFBO
    /// </summary>
    [DataMember(Name="isPreferredFBO", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isPreferredFBO")]
    public bool? IsPreferredFBO { get; set; }

    /// <summary>
    /// Gets or Sets AllInclusive
    /// </summary>
    [DataMember(Name="allInclusive", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "allInclusive")]
    public bool? AllInclusive { get; set; }

    /// <summary>
    /// Gets or Sets FuelMasterID
    /// </summary>
    [DataMember(Name="fuelMasterID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelMasterID")]
    public int? FuelMasterID { get; set; }

    /// <summary>
    /// Gets or Sets IsPostedRetail
    /// </summary>
    [DataMember(Name="isPostedRetail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isPostedRetail")]
    public bool? IsPostedRetail { get; set; }

    /// <summary>
    /// Gets or Sets FuelerDisplayName
    /// </summary>
    [DataMember(Name="fuelerDisplayName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerDisplayName")]
    public string FuelerDisplayName { get; set; }

    /// <summary>
    /// Gets or Sets MinVolume
    /// </summary>
    [DataMember(Name="minVolume", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minVolume")]
    public double? MinVolume { get; set; }

    /// <summary>
    /// Gets or Sets FboComparisonName
    /// </summary>
    [DataMember(Name="fboComparisonName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboComparisonName")]
    public string FboComparisonName { get; set; }

    /// <summary>
    /// Gets or Sets FuelerText
    /// </summary>
    [DataMember(Name="fuelerText", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerText")]
    public string FuelerText { get; set; }

    /// <summary>
    /// Gets or Sets BasePrice
    /// </summary>
    [DataMember(Name="basePrice", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "basePrice")]
    public double? BasePrice { get; set; }

    /// <summary>
    /// Gets or Sets IntoPlaneAgent
    /// </summary>
    [DataMember(Name="intoPlaneAgent", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "intoPlaneAgent")]
    public string IntoPlaneAgent { get; set; }

    /// <summary>
    /// Gets or Sets IsPreferredFueler
    /// </summary>
    [DataMember(Name="isPreferredFueler", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isPreferredFueler")]
    public bool? IsPreferredFueler { get; set; }

    /// <summary>
    /// Gets or Sets IsPreferredProduct
    /// </summary>
    [DataMember(Name="isPreferredProduct", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isPreferredProduct")]
    public bool? IsPreferredProduct { get; set; }

    /// <summary>
    /// Gets or Sets EstimatedTotal
    /// </summary>
    [DataMember(Name="estimatedTotal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "estimatedTotal")]
    public double? EstimatedTotal { get; set; }

    /// <summary>
    /// Gets or Sets FuelerLinxTotal
    /// </summary>
    [DataMember(Name="fuelerLinxTotal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerLinxTotal")]
    public double? FuelerLinxTotal { get; set; }

    /// <summary>
    /// Gets or Sets IsReportedAllInclusive
    /// </summary>
    [DataMember(Name="isReportedAllInclusive", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isReportedAllInclusive")]
    public bool? IsReportedAllInclusive { get; set; }

    /// <summary>
    /// Gets or Sets IsSuspicious
    /// </summary>
    [DataMember(Name="isSuspicious", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isSuspicious")]
    public bool? IsSuspicious { get; set; }

    /// <summary>
    /// Gets or Sets IsMasked
    /// </summary>
    [DataMember(Name="isMasked", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isMasked")]
    public bool? IsMasked { get; set; }

    /// <summary>
    /// Gets or Sets ContainsMinUpliftFeeInNotes
    /// </summary>
    [DataMember(Name="containsMinUpliftFeeInNotes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "containsMinUpliftFeeInNotes")]
    public bool? ContainsMinUpliftFeeInNotes { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets AcukwikFboHandlerName
    /// </summary>
    [DataMember(Name="acukwikFboHandlerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "acukwikFboHandlerName")]
    public string AcukwikFboHandlerName { get; set; }

    /// <summary>
    /// Gets or Sets AcukwikFboHandlerId
    /// </summary>
    [DataMember(Name="acukwikFboHandlerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "acukwikFboHandlerId")]
    public int? AcukwikFboHandlerId { get; set; }

    /// <summary>
    /// Gets or Sets AcukwikAirportId
    /// </summary>
    [DataMember(Name="acukwikAirportId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "acukwikAirportId")]
    public int? AcukwikAirportId { get; set; }

    /// <summary>
    /// Gets or Sets FuelVendorId
    /// </summary>
    [DataMember(Name="fuelVendorId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelVendorId")]
    public int? FuelVendorId { get; set; }

    /// <summary>
    /// Gets or Sets ProductText
    /// </summary>
    [DataMember(Name="productText", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "productText")]
    public string ProductText { get; set; }

    /// <summary>
    /// Gets or Sets PreferredFBOText
    /// </summary>
    [DataMember(Name="preferredFBOText", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "preferredFBOText")]
    public string PreferredFBOText { get; set; }

    /// <summary>
    /// Gets or Sets WeightUnit
    /// </summary>
    [DataMember(Name="weightUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "weightUnit")]
    public Weight WeightUnit { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }

    /// <summary>
    /// An adjusted price specified by the flight department for this particular fuel vendor.  The adjusted price is the [EstimatedTotal] +- the adjusted rate setup by the flight department.
    /// </summary>
    /// <value>An adjusted price specified by the flight department for this particular fuel vendor.  The adjusted price is the [EstimatedTotal] +- the adjusted rate setup by the flight department.</value>
    [DataMember(Name="companyAdjustedTotal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyAdjustedTotal")]
    public double? CompanyAdjustedTotal { get; set; }

    /// <summary>
    /// If the SpecificTailNumbers list is empty/null then the price applies to all tails.
    /// </summary>
    /// <value>If the SpecificTailNumbers list is empty/null then the price applies to all tails.</value>
    [DataMember(Name="specificTailNumbers", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "specificTailNumbers")]
    public List<string> SpecificTailNumbers { get; set; }

    /// <summary>
    /// Gets or Sets Taxes
    /// </summary>
    [DataMember(Name="taxes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "taxes")]
    public List<FuelerTaxByTierDTO> Taxes { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FuelPriceOption {\n");
      sb.Append("  Price: ").Append(Price).Append("\n");
      sb.Append("  Notes: ").Append(Notes).Append("\n");
      sb.Append("  Fbo: ").Append(Fbo).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  Fueler: ").Append(Fueler).Append("\n");
      sb.Append("  Product: ").Append(Product).Append("\n");
      sb.Append("  FboEmail: ").Append(FboEmail).Append("\n");
      sb.Append("  IsPreferredFBO: ").Append(IsPreferredFBO).Append("\n");
      sb.Append("  AllInclusive: ").Append(AllInclusive).Append("\n");
      sb.Append("  FuelMasterID: ").Append(FuelMasterID).Append("\n");
      sb.Append("  IsPostedRetail: ").Append(IsPostedRetail).Append("\n");
      sb.Append("  FuelerDisplayName: ").Append(FuelerDisplayName).Append("\n");
      sb.Append("  MinVolume: ").Append(MinVolume).Append("\n");
      sb.Append("  FboComparisonName: ").Append(FboComparisonName).Append("\n");
      sb.Append("  FuelerText: ").Append(FuelerText).Append("\n");
      sb.Append("  BasePrice: ").Append(BasePrice).Append("\n");
      sb.Append("  IntoPlaneAgent: ").Append(IntoPlaneAgent).Append("\n");
      sb.Append("  IsPreferredFueler: ").Append(IsPreferredFueler).Append("\n");
      sb.Append("  IsPreferredProduct: ").Append(IsPreferredProduct).Append("\n");
      sb.Append("  EstimatedTotal: ").Append(EstimatedTotal).Append("\n");
      sb.Append("  FuelerLinxTotal: ").Append(FuelerLinxTotal).Append("\n");
      sb.Append("  IsReportedAllInclusive: ").Append(IsReportedAllInclusive).Append("\n");
      sb.Append("  IsSuspicious: ").Append(IsSuspicious).Append("\n");
      sb.Append("  IsMasked: ").Append(IsMasked).Append("\n");
      sb.Append("  ContainsMinUpliftFeeInNotes: ").Append(ContainsMinUpliftFeeInNotes).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  AcukwikFboHandlerName: ").Append(AcukwikFboHandlerName).Append("\n");
      sb.Append("  AcukwikFboHandlerId: ").Append(AcukwikFboHandlerId).Append("\n");
      sb.Append("  AcukwikAirportId: ").Append(AcukwikAirportId).Append("\n");
      sb.Append("  FuelVendorId: ").Append(FuelVendorId).Append("\n");
      sb.Append("  ProductText: ").Append(ProductText).Append("\n");
      sb.Append("  PreferredFBOText: ").Append(PreferredFBOText).Append("\n");
      sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      sb.Append("  CompanyAdjustedTotal: ").Append(CompanyAdjustedTotal).Append("\n");
      sb.Append("  SpecificTailNumbers: ").Append(SpecificTailNumbers).Append("\n");
      sb.Append("  Taxes: ").Append(Taxes).Append("\n");
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
