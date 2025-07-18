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
  public class PostRampFeeByCompanyRequest {
    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets FboHandlerName
    /// </summary>
    [DataMember(Name="fboHandlerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboHandlerName")]
    public string FboHandlerName { get; set; }

    /// <summary>
    /// Gets or Sets RampFee
    /// </summary>
    [DataMember(Name="rampFee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFee")]
    public double? RampFee { get; set; }

    /// <summary>
    /// Gets or Sets RampFeeWaived
    /// </summary>
    [DataMember(Name="rampFeeWaived", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFeeWaived")]
    public Weight RampFeeWaived { get; set; }

    /// <summary>
    /// 0 = Not Specified             1 = By Size             2 = By Aircraft Type             3 = By Weight Range (Lbs)             4 = By Wingspan (feet)             5 = By Tail Number    * `NotSpecified` - Not Specified  * `BySize` - By Size  * `ByAircraftType` - By Aircraft Type  * `ByWeightRange` - By Weight Range (Lbs)  * `ByWingSpan` - By Wingspan (feet)  * `ByTailNumberList` - By Tailnumber  
    /// </summary>
    /// <value>0 = Not Specified             1 = By Size             2 = By Aircraft Type             3 = By Weight Range (Lbs)             4 = By Wingspan (feet)             5 = By Tail Number    * `NotSpecified` - Not Specified  * `BySize` - By Size  * `ByAircraftType` - By Aircraft Type  * `ByWeightRange` - By Weight Range (Lbs)  * `ByWingSpan` - By Wingspan (feet)  * `ByTailNumberList` - By Tailnumber  </value>
    [DataMember(Name="categoryType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categoryType")]
    public int? CategoryType { get; set; }

    /// <summary>
    /// Gets or Sets CategoryMinValue
    /// </summary>
    [DataMember(Name="categoryMinValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categoryMinValue")]
    public int? CategoryMinValue { get; set; }

    /// <summary>
    /// Gets or Sets CategoryMaxValue
    /// </summary>
    [DataMember(Name="categoryMaxValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categoryMaxValue")]
    public int? CategoryMaxValue { get; set; }

    /// <summary>
    /// Gets or Sets CategoryStringValue
    /// </summary>
    [DataMember(Name="categoryStringValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categoryStringValue")]
    public string CategoryStringValue { get; set; }

    /// <summary>
    /// Gets or Sets Notes
    /// </summary>
    [DataMember(Name="notes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "notes")]
    public string Notes { get; set; }

    /// <summary>
    /// Gets or Sets FacilityFee
    /// </summary>
    [DataMember(Name="facilityFee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "facilityFee")]
    public double? FacilityFee { get; set; }

    /// <summary>
    /// Gets or Sets ExpirationDate
    /// </summary>
    [DataMember(Name="expirationDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "expirationDate")]
    public string ExpirationDate { get; set; }

    /// <summary>
    /// Gets or Sets Applicable
    /// </summary>
    [DataMember(Name="applicable", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applicable")]
    public bool? Applicable { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostRampFeeByCompanyRequest {\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  FboHandlerName: ").Append(FboHandlerName).Append("\n");
      sb.Append("  RampFee: ").Append(RampFee).Append("\n");
      sb.Append("  RampFeeWaived: ").Append(RampFeeWaived).Append("\n");
      sb.Append("  CategoryType: ").Append(CategoryType).Append("\n");
      sb.Append("  CategoryMinValue: ").Append(CategoryMinValue).Append("\n");
      sb.Append("  CategoryMaxValue: ").Append(CategoryMaxValue).Append("\n");
      sb.Append("  CategoryStringValue: ").Append(CategoryStringValue).Append("\n");
      sb.Append("  Notes: ").Append(Notes).Append("\n");
      sb.Append("  FacilityFee: ").Append(FacilityFee).Append("\n");
      sb.Append("  ExpirationDate: ").Append(ExpirationDate).Append("\n");
      sb.Append("  Applicable: ").Append(Applicable).Append("\n");
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
