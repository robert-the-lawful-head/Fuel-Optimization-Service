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
  public class IndustryAveragePriceResultDTO {
    /// <summary>
    /// Gets or Sets AveragePrice
    /// </summary>
    [DataMember(Name="averagePrice", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "averagePrice")]
    public double? AveragePrice { get; set; }

    /// <summary>
    /// Gets or Sets Interval
    /// </summary>
    [DataMember(Name="interval", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "interval")]
    public int? Interval { get; set; }

    /// <summary>
    /// Gets or Sets Year
    /// </summary>
    [DataMember(Name="year", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "year")]
    public int? Year { get; set; }

    /// <summary>
    /// Subscription levels:             0 = Bronze (1-3 Aircraft)             1 = Silver (4-6 Aircraft)             2 = Gold (7-9 Aircraft)             3 = Platinum (10-25 Aircraft)             4 = Enterprise (26+ Aircraft)    * `Bronze` -   * `Silver` -   * `Gold` -   * `Platinum` -   * `Enterprise` -   
    /// </summary>
    /// <value>Subscription levels:             0 = Bronze (1-3 Aircraft)             1 = Silver (4-6 Aircraft)             2 = Gold (7-9 Aircraft)             3 = Platinum (10-25 Aircraft)             4 = Enterprise (26+ Aircraft)    * `Bronze` -   * `Silver` -   * `Gold` -   * `Platinum` -   * `Enterprise` -   </value>
    [DataMember(Name="subscriptionLevel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "subscriptionLevel")]
    public int? SubscriptionLevel { get; set; }

    /// <summary>
    /// Gets or Sets NumberOfFlightDepartments
    /// </summary>
    [DataMember(Name="numberOfFlightDepartments", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "numberOfFlightDepartments")]
    public int? NumberOfFlightDepartments { get; set; }

    /// <summary>
    /// Gets or Sets SubscriptionLevelDescription
    /// </summary>
    [DataMember(Name="subscriptionLevelDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "subscriptionLevelDescription")]
    public string SubscriptionLevelDescription { get; set; }

    /// <summary>
    /// Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  
    /// </summary>
    /// <value>Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  </value>
    [DataMember(Name="weightUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "weightUnit")]
    public int? WeightUnit { get; set; }

    /// <summary>
    /// Gets or Sets WeightUnitDescription
    /// </summary>
    [DataMember(Name="weightUnitDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "weightUnitDescription")]
    public string WeightUnitDescription { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class IndustryAveragePriceResultDTO {\n");
      sb.Append("  AveragePrice: ").Append(AveragePrice).Append("\n");
      sb.Append("  Interval: ").Append(Interval).Append("\n");
      sb.Append("  Year: ").Append(Year).Append("\n");
      sb.Append("  SubscriptionLevel: ").Append(SubscriptionLevel).Append("\n");
      sb.Append("  NumberOfFlightDepartments: ").Append(NumberOfFlightDepartments).Append("\n");
      sb.Append("  SubscriptionLevelDescription: ").Append(SubscriptionLevelDescription).Append("\n");
      sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
      sb.Append("  WeightUnitDescription: ").Append(WeightUnitDescription).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
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
