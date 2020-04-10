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
  public class PlotOptionsSeries {
    /// <summary>
    /// Gets or Sets AllowPointSelect
    /// </summary>
    [DataMember(Name="allowPointSelect", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "allowPointSelect")]
    public bool? AllowPointSelect { get; set; }

    /// <summary>
    /// Gets or Sets Animation
    /// </summary>
    [DataMember(Name="animation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "animation")]
    public bool? Animation { get; set; }

    /// <summary>
    /// Gets or Sets Color
    /// </summary>
    [DataMember(Name="color", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "color")]
    public string Color { get; set; }

    /// <summary>
    /// Gets or Sets ConnectNulls
    /// </summary>
    [DataMember(Name="connectNulls", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "connectNulls")]
    public bool? ConnectNulls { get; set; }

    /// <summary>
    /// Gets or Sets Cursor
    /// </summary>
    [DataMember(Name="cursor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cursor")]
    public string Cursor { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="dashStyle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dashStyle")]
    public int? DashStyle { get; set; }

    /// <summary>
    /// Gets or Sets DataLabels
    /// </summary>
    [DataMember(Name="dataLabels", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dataLabels")]
    public DataLabels DataLabels { get; set; }

    /// <summary>
    /// Gets or Sets EnableMouseTracking
    /// </summary>
    [DataMember(Name="enableMouseTracking", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "enableMouseTracking")]
    public bool? EnableMouseTracking { get; set; }

    /// <summary>
    /// Gets or Sets Events
    /// </summary>
    [DataMember(Name="events", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "events")]
    public PlotOptionEvents Events { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets LineWidth
    /// </summary>
    [DataMember(Name="lineWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lineWidth")]
    public int? LineWidth { get; set; }

    /// <summary>
    /// Gets or Sets Marker
    /// </summary>
    [DataMember(Name="marker", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "marker")]
    public Marker Marker { get; set; }

    /// <summary>
    /// Gets or Sets Point
    /// </summary>
    [DataMember(Name="point", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "point")]
    public PlotPointEvents Point { get; set; }

    /// <summary>
    /// Gets or Sets PointStart
    /// </summary>
    [DataMember(Name="pointStart", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pointStart")]
    public int? PointStart { get; set; }

    /// <summary>
    /// Gets or Sets PointInterval
    /// </summary>
    [DataMember(Name="pointInterval", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pointInterval")]
    public int? PointInterval { get; set; }

    /// <summary>
    /// Gets or Sets Shadow
    /// </summary>
    [DataMember(Name="shadow", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "shadow")]
    public bool? Shadow { get; set; }

    /// <summary>
    /// Gets or Sets ShowInLegend
    /// </summary>
    [DataMember(Name="showInLegend", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "showInLegend")]
    public bool? ShowInLegend { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="stacking", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "stacking")]
    public int? Stacking { get; set; }

    /// <summary>
    /// Gets or Sets States
    /// </summary>
    [DataMember(Name="states", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "states")]
    public SerieStates States { get; set; }

    /// <summary>
    /// Gets or Sets StickyTracking
    /// </summary>
    [DataMember(Name="stickyTracking", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "stickyTracking")]
    public bool? StickyTracking { get; set; }

    /// <summary>
    /// Gets or Sets Visible
    /// </summary>
    [DataMember(Name="visible", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "visible")]
    public bool? Visible { get; set; }

    /// <summary>
    /// Gets or Sets ZIndex
    /// </summary>
    [DataMember(Name="zIndex", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "zIndex")]
    public int? ZIndex { get; set; }

    /// <summary>
    /// Gets or Sets ChartType
    /// </summary>
    [DataMember(Name="chartType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "chartType")]
    public string ChartType { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PlotOptionsSeries {\n");
      sb.Append("  AllowPointSelect: ").Append(AllowPointSelect).Append("\n");
      sb.Append("  Animation: ").Append(Animation).Append("\n");
      sb.Append("  Color: ").Append(Color).Append("\n");
      sb.Append("  ConnectNulls: ").Append(ConnectNulls).Append("\n");
      sb.Append("  Cursor: ").Append(Cursor).Append("\n");
      sb.Append("  DashStyle: ").Append(DashStyle).Append("\n");
      sb.Append("  DataLabels: ").Append(DataLabels).Append("\n");
      sb.Append("  EnableMouseTracking: ").Append(EnableMouseTracking).Append("\n");
      sb.Append("  Events: ").Append(Events).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  LineWidth: ").Append(LineWidth).Append("\n");
      sb.Append("  Marker: ").Append(Marker).Append("\n");
      sb.Append("  Point: ").Append(Point).Append("\n");
      sb.Append("  PointStart: ").Append(PointStart).Append("\n");
      sb.Append("  PointInterval: ").Append(PointInterval).Append("\n");
      sb.Append("  Shadow: ").Append(Shadow).Append("\n");
      sb.Append("  ShowInLegend: ").Append(ShowInLegend).Append("\n");
      sb.Append("  Stacking: ").Append(Stacking).Append("\n");
      sb.Append("  States: ").Append(States).Append("\n");
      sb.Append("  StickyTracking: ").Append(StickyTracking).Append("\n");
      sb.Append("  Visible: ").Append(Visible).Append("\n");
      sb.Append("  ZIndex: ").Append(ZIndex).Append("\n");
      sb.Append("  ChartType: ").Append(ChartType).Append("\n");
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
