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
  public class HighChartsChart {
    /// <summary>
    /// Gets or Sets Chart
    /// </summary>
    [DataMember(Name="chart", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "chart")]
    public Chart Chart { get; set; }

    /// <summary>
    /// Gets or Sets Title
    /// </summary>
    [DataMember(Name="title", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "title")]
    public Title Title { get; set; }

    /// <summary>
    /// Gets or Sets Subtitle
    /// </summary>
    [DataMember(Name="subtitle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "subtitle")]
    public Title Subtitle { get; set; }

    /// <summary>
    /// Gets or Sets XAxis
    /// </summary>
    [DataMember(Name="xAxis", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "xAxis")]
    public XAxis XAxis { get; set; }

    /// <summary>
    /// Gets or Sets PlotOptions
    /// </summary>
    [DataMember(Name="plotOptions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "plotOptions")]
    public PlotOptions PlotOptions { get; set; }

    /// <summary>
    /// Gets or Sets Credits
    /// </summary>
    [DataMember(Name="credits", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "credits")]
    public Credits Credits { get; set; }

    /// <summary>
    /// Gets or Sets Legend
    /// </summary>
    [DataMember(Name="legend", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legend")]
    public Legend Legend { get; set; }

    /// <summary>
    /// Gets or Sets Series
    /// </summary>
    [DataMember(Name="series", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "series")]
    public List<Series> Series { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class HighChartsChart {\n");
      sb.Append("  Chart: ").Append(Chart).Append("\n");
      sb.Append("  Title: ").Append(Title).Append("\n");
      sb.Append("  Subtitle: ").Append(Subtitle).Append("\n");
      sb.Append("  XAxis: ").Append(XAxis).Append("\n");
      sb.Append("  PlotOptions: ").Append(PlotOptions).Append("\n");
      sb.Append("  Credits: ").Append(Credits).Append("\n");
      sb.Append("  Legend: ").Append(Legend).Append("\n");
      sb.Append("  Series: ").Append(Series).Append("\n");
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
