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
  public class ChartSettings {
    /// <summary>
    /// 0 = NumericValues             1 = ArrayOfNumericXY             2 = ArrayOfDateTimeXY             3 = ArrayOfPointNames    * `NumericValues` - Numerical values  * `ArrayOfNumericXY` - Arrays of numeric x and y  * `ArrayOfDateTimeXY` - Arrays of datetime x and y  * `ArrayOfPointNames` - Arrays of point.name and y  * `ArrayOfPointNamesWithDates` - Arrays of point.name and y with dates for x  
    /// </summary>
    /// <value>0 = NumericValues             1 = ArrayOfNumericXY             2 = ArrayOfDateTimeXY             3 = ArrayOfPointNames    * `NumericValues` - Numerical values  * `ArrayOfNumericXY` - Arrays of numeric x and y  * `ArrayOfDateTimeXY` - Arrays of datetime x and y  * `ArrayOfPointNames` - Arrays of point.name and y  * `ArrayOfPointNamesWithDates` - Arrays of point.name and y with dates for x  </value>
    [DataMember(Name="seriesType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "seriesType")]
    public int? SeriesType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="chartType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "chartType")]
    public int? ChartType { get; set; }

    /// <summary>
    /// Gets or Sets HighChartsChart
    /// </summary>
    [DataMember(Name="highChartsChart", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "highChartsChart")]
    public ChartOptions HighChartsChart { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ChartSettings {\n");
      sb.Append("  SeriesType: ").Append(SeriesType).Append("\n");
      sb.Append("  ChartType: ").Append(ChartType).Append("\n");
      sb.Append("  HighChartsChart: ").Append(HighChartsChart).Append("\n");
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
