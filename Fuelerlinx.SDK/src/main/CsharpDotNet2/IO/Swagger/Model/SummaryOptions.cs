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
  public class SummaryOptions {
    /// <summary>
    /// Gets or Sets SendSummaryUponCompletion
    /// </summary>
    [DataMember(Name="sendSummaryUponCompletion", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sendSummaryUponCompletion")]
    public bool? SendSummaryUponCompletion { get; set; }

    /// <summary>
    /// Gets or Sets RecipientEmailAddresses
    /// </summary>
    [DataMember(Name="recipientEmailAddresses", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "recipientEmailAddresses")]
    public List<string> RecipientEmailAddresses { get; set; }

    /// <summary>
    /// Gets or Sets SummaryToUse
    /// </summary>
    [DataMember(Name="summaryToUse", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "summaryToUse")]
    public SummaryCustomizations SummaryToUse { get; set; }

    /// <summary>
    /// Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  
    /// </summary>
    /// <value>Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  </value>
    [DataMember(Name="weightUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "weightUnit")]
    public int? WeightUnit { get; set; }

    /// <summary>
    /// Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  
    /// </summary>
    /// <value>Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  </value>
    [DataMember(Name="fuelOptionsWeightUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelOptionsWeightUnit")]
    public int? FuelOptionsWeightUnit { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SummaryOptions {\n");
      sb.Append("  SendSummaryUponCompletion: ").Append(SendSummaryUponCompletion).Append("\n");
      sb.Append("  RecipientEmailAddresses: ").Append(RecipientEmailAddresses).Append("\n");
      sb.Append("  SummaryToUse: ").Append(SummaryToUse).Append("\n");
      sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
      sb.Append("  FuelOptionsWeightUnit: ").Append(FuelOptionsWeightUnit).Append("\n");
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
