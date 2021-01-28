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
  public class ChartOptions {
    /// <summary>
    /// Gets or Sets ClientId
    /// </summary>
    [DataMember(Name="clientId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "clientId")]
    public string ClientId { get; set; }

    /// <summary>
    /// Gets or Sets Lang
    /// </summary>
    [DataMember(Name="lang", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lang")]
    public Localization Lang { get; set; }

    /// <summary>
    /// Gets or Sets Appearance
    /// </summary>
    [DataMember(Name="appearance", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "appearance")]
    public Appearance Appearance { get; set; }

    /// <summary>
    /// Gets or Sets Colors
    /// </summary>
    [DataMember(Name="colors", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "colors")]
    public ColorSet Colors { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="renderType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "renderType")]
    public int? RenderType { get; set; }

    /// <summary>
    /// Gets or Sets Legend
    /// </summary>
    [DataMember(Name="legend", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legend")]
    public Legend Legend { get; set; }

    /// <summary>
    /// Gets or Sets Exporting
    /// </summary>
    [DataMember(Name="exporting", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "exporting")]
    public Exporting Exporting { get; set; }

    /// <summary>
    /// Gets or Sets ShowCredits
    /// </summary>
    [DataMember(Name="showCredits", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "showCredits")]
    public bool? ShowCredits { get; set; }

    /// <summary>
    /// Gets or Sets Title
    /// </summary>
    [DataMember(Name="title", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "title")]
    public Title Title { get; set; }

    /// <summary>
    /// Gets or Sets SubTitle
    /// </summary>
    [DataMember(Name="subTitle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "subTitle")]
    public SubTitle SubTitle { get; set; }

    /// <summary>
    /// Gets or Sets Tooltip
    /// </summary>
    [DataMember(Name="tooltip", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tooltip")]
    public ToolTip Tooltip { get; set; }

    /// <summary>
    /// Gets or Sets YAxis
    /// </summary>
    [DataMember(Name="yAxis", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "yAxis")]
    public List<YAxisItem> YAxis { get; set; }

    /// <summary>
    /// Gets or Sets XAxis
    /// </summary>
    [DataMember(Name="xAxis", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "xAxis")]
    public List<XAxisItem> XAxis { get; set; }

    /// <summary>
    /// Gets or Sets Series
    /// </summary>
    [DataMember(Name="series", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "series")]
    public List<Series> Series { get; set; }

    /// <summary>
    /// Gets or Sets Theme
    /// </summary>
    [DataMember(Name="theme", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "theme")]
    public string Theme { get; set; }

    /// <summary>
    /// Gets or Sets AjaxDataSource
    /// </summary>
    [DataMember(Name="ajaxDataSource", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ajaxDataSource")]
    public AJAXSource AjaxDataSource { get; set; }

    /// <summary>
    /// Gets or Sets PlotOptions
    /// </summary>
    [DataMember(Name="plotOptions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "plotOptions")]
    public PlotOptionsSeries PlotOptions { get; set; }

    /// <summary>
    /// Gets or Sets MapNavigation
    /// </summary>
    [DataMember(Name="mapNavigation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mapNavigation")]
    public MapNavigation MapNavigation { get; set; }

    /// <summary>
    /// Gets or Sets ColorAxis
    /// </summary>
    [DataMember(Name="colorAxis", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "colorAxis")]
    public ColorAxis ColorAxis { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ChartOptions {\n");
      sb.Append("  ClientId: ").Append(ClientId).Append("\n");
      sb.Append("  Lang: ").Append(Lang).Append("\n");
      sb.Append("  Appearance: ").Append(Appearance).Append("\n");
      sb.Append("  Colors: ").Append(Colors).Append("\n");
      sb.Append("  RenderType: ").Append(RenderType).Append("\n");
      sb.Append("  Legend: ").Append(Legend).Append("\n");
      sb.Append("  Exporting: ").Append(Exporting).Append("\n");
      sb.Append("  ShowCredits: ").Append(ShowCredits).Append("\n");
      sb.Append("  Title: ").Append(Title).Append("\n");
      sb.Append("  SubTitle: ").Append(SubTitle).Append("\n");
      sb.Append("  Tooltip: ").Append(Tooltip).Append("\n");
      sb.Append("  YAxis: ").Append(YAxis).Append("\n");
      sb.Append("  XAxis: ").Append(XAxis).Append("\n");
      sb.Append("  Series: ").Append(Series).Append("\n");
      sb.Append("  Theme: ").Append(Theme).Append("\n");
      sb.Append("  AjaxDataSource: ").Append(AjaxDataSource).Append("\n");
      sb.Append("  PlotOptions: ").Append(PlotOptions).Append("\n");
      sb.Append("  MapNavigation: ").Append(MapNavigation).Append("\n");
      sb.Append("  ColorAxis: ").Append(ColorAxis).Append("\n");
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
