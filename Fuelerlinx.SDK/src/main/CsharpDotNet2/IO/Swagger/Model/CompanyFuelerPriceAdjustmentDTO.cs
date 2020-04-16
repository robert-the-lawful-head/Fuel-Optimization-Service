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
  public class CompanyFuelerPriceAdjustmentDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets CompanyFuelerId
    /// </summary>
    [DataMember(Name="companyFuelerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyFuelerId")]
    public int? CompanyFuelerId { get; set; }

    /// <summary>
    /// NotSet = 0,             Private = 1,             Commercial = 2    * `NotSet` - Not Set  * `Private` - Private  * `Commercial` - Commercial  
    /// </summary>
    /// <value>NotSet = 0,             Private = 1,             Commercial = 2    * `NotSet` - Not Set  * `Private` - Private  * `Commercial` - Commercial  </value>
    [DataMember(Name="flightTypeClassification", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightTypeClassification")]
    public int? FlightTypeClassification { get; set; }

    /// <summary>
    /// Gets or Sets PriceAdjustment
    /// </summary>
    [DataMember(Name="priceAdjustment", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceAdjustment")]
    public double? PriceAdjustment { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Gets or Sets CreationDateUtc
    /// </summary>
    [DataMember(Name="creationDateUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "creationDateUtc")]
    public DateTime? CreationDateUtc { get; set; }

    /// <summary>
    /// Gets or Sets ExpirationDateUtc
    /// </summary>
    [DataMember(Name="expirationDateUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "expirationDateUtc")]
    public DateTime? ExpirationDateUtc { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CompanyFuelerPriceAdjustmentDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyFuelerId: ").Append(CompanyFuelerId).Append("\n");
      sb.Append("  FlightTypeClassification: ").Append(FlightTypeClassification).Append("\n");
      sb.Append("  PriceAdjustment: ").Append(PriceAdjustment).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      sb.Append("  CreationDateUtc: ").Append(CreationDateUtc).Append("\n");
      sb.Append("  ExpirationDateUtc: ").Append(ExpirationDateUtc).Append("\n");
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
