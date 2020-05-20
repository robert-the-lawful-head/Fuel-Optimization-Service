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
  public class ReportDataDTO {
    /// <summary>
    /// Gets or Sets HighChartsChart
    /// </summary>
    [DataMember(Name="highChartsChart", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "highChartsChart")]
    public ChartOptions HighChartsChart { get; set; }

    /// <summary>
    /// Gets or Sets DataJson
    /// </summary>
    [DataMember(Name="dataJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dataJson")]
    public string DataJson { get; set; }

    /// <summary>
    /// Gets or Sets ChartOptionsJson
    /// </summary>
    [DataMember(Name="chartOptionsJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "chartOptionsJson")]
    public string ChartOptionsJson { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Gets or Sets CurrencySymbol
    /// </summary>
    [DataMember(Name="currencySymbol", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currencySymbol")]
    public string CurrencySymbol { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ReportDataDTO {\n");
      sb.Append("  HighChartsChart: ").Append(HighChartsChart).Append("\n");
      sb.Append("  DataJson: ").Append(DataJson).Append("\n");
      sb.Append("  ChartOptionsJson: ").Append(ChartOptionsJson).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      sb.Append("  CurrencySymbol: ").Append(CurrencySymbol).Append("\n");
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
