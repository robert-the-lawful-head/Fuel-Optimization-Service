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
  public class PostServicesAndFeesByCompanyRequest {
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
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets Price
    /// </summary>
    [DataMember(Name="price", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "price")]
    public double? Price { get; set; }

    /// <summary>
    /// Gets or Sets VolumeToAvoidFee
    /// </summary>
    [DataMember(Name="volumeToAvoidFee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "volumeToAvoidFee")]
    public Weight VolumeToAvoidFee { get; set; }

    /// <summary>
    /// Gets or Sets PriceCurrency
    /// </summary>
    [DataMember(Name="priceCurrency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceCurrency")]
    public string PriceCurrency { get; set; }

    /// <summary>
    /// Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  
    /// </summary>
    /// <value>Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  </value>
    [DataMember(Name="preferredWeightUnitFormat", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "preferredWeightUnitFormat")]
    public int? PreferredWeightUnitFormat { get; set; }

    /// <summary>
    /// 0 = Not Specified             1 = By Size             2 = By Aircraft Type             3 = By Weight Range (Lbs)             4 = By Wingspan (feet)             5 = By Tail Number    * `NotSpecified` - Not Specified  * `BySize` - By Size  * `ByAircraftType` - By Aircraft Type  * `ByWeightRange` - By Weight Range (Lbs)  * `ByWingSpan` - By Wingspan (feet)  * `ByTailNumberList` - By Tailnumber  
    /// </summary>
    /// <value>0 = Not Specified             1 = By Size             2 = By Aircraft Type             3 = By Weight Range (Lbs)             4 = By Wingspan (feet)             5 = By Tail Number    * `NotSpecified` - Not Specified  * `BySize` - By Size  * `ByAircraftType` - By Aircraft Type  * `ByWeightRange` - By Weight Range (Lbs)  * `ByWingSpan` - By Wingspan (feet)  * `ByTailNumberList` - By Tailnumber  </value>
    [DataMember(Name="categoryType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categoryType")]
    public int? CategoryType { get; set; }

    /// <summary>
    /// Provide the minimum range here for wingspan or weight.  Provide the aircraftId here if by aircraft type. Provide the size enum here if by size.
    /// </summary>
    /// <value>Provide the minimum range here for wingspan or weight.  Provide the aircraftId here if by aircraft type. Provide the size enum here if by size.</value>
    [DataMember(Name="categoryMinValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categoryMinValue")]
    public int? CategoryMinValue { get; set; }

    /// <summary>
    /// Provide the maximum range here for wingspan or weight.
    /// </summary>
    /// <value>Provide the maximum range here for wingspan or weight.</value>
    [DataMember(Name="categoryMaxValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categoryMaxValue")]
    public int? CategoryMaxValue { get; set; }

    /// <summary>
    /// Provide a comma delimited list of tails here if the categoryType is by tail number (5)
    /// </summary>
    /// <value>Provide a comma delimited list of tails here if the categoryType is by tail number (5)</value>
    [DataMember(Name="categoryStringValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categoryStringValue")]
    public string CategoryStringValue { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostServicesAndFeesByCompanyRequest {\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  FboHandlerName: ").Append(FboHandlerName).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Price: ").Append(Price).Append("\n");
      sb.Append("  VolumeToAvoidFee: ").Append(VolumeToAvoidFee).Append("\n");
      sb.Append("  PriceCurrency: ").Append(PriceCurrency).Append("\n");
      sb.Append("  PreferredWeightUnitFormat: ").Append(PreferredWeightUnitFormat).Append("\n");
      sb.Append("  CategoryType: ").Append(CategoryType).Append("\n");
      sb.Append("  CategoryMinValue: ").Append(CategoryMinValue).Append("\n");
      sb.Append("  CategoryMaxValue: ").Append(CategoryMaxValue).Append("\n");
      sb.Append("  CategoryStringValue: ").Append(CategoryStringValue).Append("\n");
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
