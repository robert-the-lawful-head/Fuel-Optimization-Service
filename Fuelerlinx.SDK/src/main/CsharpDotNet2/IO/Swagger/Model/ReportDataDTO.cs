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
    /// Gets or Sets ChartDataCollection
    /// </summary>
    [DataMember(Name="chartDataCollection", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "chartDataCollection")]
    public List<ChartDataDTO> ChartDataCollection { get; set; }

    /// <summary>
    /// Gets or Sets DataJson
    /// </summary>
    [DataMember(Name="dataJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dataJson")]
    public string DataJson { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ReportDataDTO {\n");
      sb.Append("  ChartDataCollection: ").Append(ChartDataCollection).Append("\n");
      sb.Append("  DataJson: ").Append(DataJson).Append("\n");
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
