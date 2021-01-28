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
  public class TaxesByCountryDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Country
    /// </summary>
    [DataMember(Name="country", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "country")]
    public string Country { get; set; }

    /// <summary>
    /// Gets or Sets CurrencyConversionId
    /// </summary>
    [DataMember(Name="currencyConversionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currencyConversionId")]
    public int? CurrencyConversionId { get; set; }

    /// <summary>
    /// Gets or Sets VatPercentage
    /// </summary>
    [DataMember(Name="vatPercentage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "vatPercentage")]
    public double? VatPercentage { get; set; }

    /// <summary>
    /// Gets or Sets MotRate
    /// </summary>
    [DataMember(Name="motRate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "motRate")]
    public double? MotRate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="applyVatToCommercialFlight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applyVatToCommercialFlight")]
    public int? ApplyVatToCommercialFlight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="applyVatToPrivateFlight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applyVatToPrivateFlight")]
    public int? ApplyVatToPrivateFlight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="applyMotToCommercialFlight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applyMotToCommercialFlight")]
    public int? ApplyMotToCommercialFlight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="applyMotToPrivateFlight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applyMotToPrivateFlight")]
    public int? ApplyMotToPrivateFlight { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets MotRateInUsd
    /// </summary>
    [DataMember(Name="motRateInUsd", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "motRateInUsd")]
    public double? MotRateInUsd { get; set; }

    /// <summary>
    /// Gets or Sets CurrencyConversion
    /// </summary>
    [DataMember(Name="currencyConversion", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currencyConversion")]
    public CurrencyConversionDTO CurrencyConversion { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TaxesByCountryDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Country: ").Append(Country).Append("\n");
      sb.Append("  CurrencyConversionId: ").Append(CurrencyConversionId).Append("\n");
      sb.Append("  VatPercentage: ").Append(VatPercentage).Append("\n");
      sb.Append("  MotRate: ").Append(MotRate).Append("\n");
      sb.Append("  ApplyVatToCommercialFlight: ").Append(ApplyVatToCommercialFlight).Append("\n");
      sb.Append("  ApplyVatToPrivateFlight: ").Append(ApplyVatToPrivateFlight).Append("\n");
      sb.Append("  ApplyMotToCommercialFlight: ").Append(ApplyMotToCommercialFlight).Append("\n");
      sb.Append("  ApplyMotToPrivateFlight: ").Append(ApplyMotToPrivateFlight).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  MotRateInUsd: ").Append(MotRateInUsd).Append("\n");
      sb.Append("  CurrencyConversion: ").Append(CurrencyConversion).Append("\n");
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
