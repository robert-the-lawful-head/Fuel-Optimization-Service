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
  public class XAxisItem {
    /// <summary>
    /// Gets or Sets Categories
    /// </summary>
    [DataMember(Name="categories", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categories")]
    public List<string> Categories { get; set; }

    /// <summary>
    /// Gets or Sets TickWidth
    /// </summary>
    [DataMember(Name="tickWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tickWidth")]
    public int? TickWidth { get; set; }

    /// <summary>
    /// Gets or Sets AllowDecimals
    /// </summary>
    [DataMember(Name="allowDecimals", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "allowDecimals")]
    public bool? AllowDecimals { get; set; }

    /// <summary>
    /// Gets or Sets AlternateGridColor
    /// </summary>
    [DataMember(Name="alternateGridColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "alternateGridColor")]
    public string AlternateGridColor { get; set; }

    /// <summary>
    /// Gets or Sets DateTimeLabelFormats
    /// </summary>
    [DataMember(Name="dateTimeLabelFormats", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateTimeLabelFormats")]
    public DateTimeLabelFormats DateTimeLabelFormats { get; set; }

    /// <summary>
    /// Gets or Sets EndOnTick
    /// </summary>
    [DataMember(Name="endOnTick", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "endOnTick")]
    public bool? EndOnTick { get; set; }

    /// <summary>
    /// Gets or Sets GridLineColor
    /// </summary>
    [DataMember(Name="gridLineColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "gridLineColor")]
    public string GridLineColor { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="gridLineDashStyle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "gridLineDashStyle")]
    public int? GridLineDashStyle { get; set; }

    /// <summary>
    /// Gets or Sets GridLineWidth
    /// </summary>
    [DataMember(Name="gridLineWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "gridLineWidth")]
    public int? GridLineWidth { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets Labels
    /// </summary>
    [DataMember(Name="labels", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "labels")]
    public Labels Labels { get; set; }

    /// <summary>
    /// Gets or Sets LineColor
    /// </summary>
    [DataMember(Name="lineColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lineColor")]
    public string LineColor { get; set; }

    /// <summary>
    /// Gets or Sets LineWidth
    /// </summary>
    [DataMember(Name="lineWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lineWidth")]
    public int? LineWidth { get; set; }

    /// <summary>
    /// Gets or Sets LinkedTo
    /// </summary>
    [DataMember(Name="linkedTo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "linkedTo")]
    public int? LinkedTo { get; set; }

    /// <summary>
    /// Gets or Sets Max
    /// </summary>
    [DataMember(Name="max", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "max")]
    public int? Max { get; set; }

    /// <summary>
    /// Gets or Sets MaxPadding
    /// </summary>
    [DataMember(Name="maxPadding", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxPadding")]
    public double? MaxPadding { get; set; }

    /// <summary>
    /// Gets or Sets MaxZoom
    /// </summary>
    [DataMember(Name="maxZoom", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxZoom")]
    public int? MaxZoom { get; set; }

    /// <summary>
    /// Gets or Sets Min
    /// </summary>
    [DataMember(Name="min", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "min")]
    public int? Min { get; set; }

    /// <summary>
    /// Gets or Sets MinorGridLineColor
    /// </summary>
    [DataMember(Name="minorGridLineColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minorGridLineColor")]
    public string MinorGridLineColor { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="minorGridLineDashStyle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minorGridLineDashStyle")]
    public int? MinorGridLineDashStyle { get; set; }

    /// <summary>
    /// Gets or Sets MinorGridLineWidth
    /// </summary>
    [DataMember(Name="minorGridLineWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minorGridLineWidth")]
    public int? MinorGridLineWidth { get; set; }

    /// <summary>
    /// Gets or Sets MinorTickColor
    /// </summary>
    [DataMember(Name="minorTickColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minorTickColor")]
    public string MinorTickColor { get; set; }

    /// <summary>
    /// Gets or Sets MinorTickInterval
    /// </summary>
    [DataMember(Name="minorTickInterval", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minorTickInterval")]
    public int? MinorTickInterval { get; set; }

    /// <summary>
    /// Gets or Sets MinorTickLength
    /// </summary>
    [DataMember(Name="minorTickLength", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minorTickLength")]
    public int? MinorTickLength { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="minorTickPosition", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minorTickPosition")]
    public int? MinorTickPosition { get; set; }

    /// <summary>
    /// Gets or Sets MinorTickWidth
    /// </summary>
    [DataMember(Name="minorTickWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minorTickWidth")]
    public int? MinorTickWidth { get; set; }

    /// <summary>
    /// Gets or Sets MinPadding
    /// </summary>
    [DataMember(Name="minPadding", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minPadding")]
    public double? MinPadding { get; set; }

    /// <summary>
    /// Gets or Sets Offset
    /// </summary>
    [DataMember(Name="offset", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "offset")]
    public int? Offset { get; set; }

    /// <summary>
    /// Gets or Sets Opposite
    /// </summary>
    [DataMember(Name="opposite", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "opposite")]
    public bool? Opposite { get; set; }

    /// <summary>
    /// Gets or Sets PlotBands
    /// </summary>
    [DataMember(Name="plotBands", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "plotBands")]
    public List<PlotBand> PlotBands { get; set; }

    /// <summary>
    /// Gets or Sets PlotLines
    /// </summary>
    [DataMember(Name="plotLines", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "plotLines")]
    public List<PlotLine> PlotLines { get; set; }

    /// <summary>
    /// Gets or Sets Reversed
    /// </summary>
    [DataMember(Name="reversed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reversed")]
    public bool? Reversed { get; set; }

    /// <summary>
    /// Gets or Sets ShowFirstLabel
    /// </summary>
    [DataMember(Name="showFirstLabel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "showFirstLabel")]
    public bool? ShowFirstLabel { get; set; }

    /// <summary>
    /// Gets or Sets ShowLastLabel
    /// </summary>
    [DataMember(Name="showLastLabel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "showLastLabel")]
    public bool? ShowLastLabel { get; set; }

    /// <summary>
    /// Gets or Sets StackLabels
    /// </summary>
    [DataMember(Name="stackLabels", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "stackLabels")]
    public DataLabels StackLabels { get; set; }

    /// <summary>
    /// Gets or Sets StartOfWeek
    /// </summary>
    [DataMember(Name="startOfWeek", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "startOfWeek")]
    public int? StartOfWeek { get; set; }

    /// <summary>
    /// Gets or Sets StartOnTick
    /// </summary>
    [DataMember(Name="startOnTick", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "startOnTick")]
    public bool? StartOnTick { get; set; }

    /// <summary>
    /// Gets or Sets TickColor
    /// </summary>
    [DataMember(Name="tickColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tickColor")]
    public string TickColor { get; set; }

    /// <summary>
    /// Gets or Sets TickInterval
    /// </summary>
    [DataMember(Name="tickInterval", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tickInterval")]
    public long? TickInterval { get; set; }

    /// <summary>
    /// Gets or Sets TickLength
    /// </summary>
    [DataMember(Name="tickLength", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tickLength")]
    public int? TickLength { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="tickmarkPlacement", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tickmarkPlacement")]
    public int? TickmarkPlacement { get; set; }

    /// <summary>
    /// Gets or Sets TickPixelInterval
    /// </summary>
    [DataMember(Name="tickPixelInterval", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tickPixelInterval")]
    public int? TickPixelInterval { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="tickPosition", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tickPosition")]
    public int? TickPosition { get; set; }

    /// <summary>
    /// Gets or Sets Title
    /// </summary>
    [DataMember(Name="title", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "title")]
    public Title Title { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="type", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "type")]
    public int? Type { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class XAxisItem {\n");
      sb.Append("  Categories: ").Append(Categories).Append("\n");
      sb.Append("  TickWidth: ").Append(TickWidth).Append("\n");
      sb.Append("  AllowDecimals: ").Append(AllowDecimals).Append("\n");
      sb.Append("  AlternateGridColor: ").Append(AlternateGridColor).Append("\n");
      sb.Append("  DateTimeLabelFormats: ").Append(DateTimeLabelFormats).Append("\n");
      sb.Append("  EndOnTick: ").Append(EndOnTick).Append("\n");
      sb.Append("  GridLineColor: ").Append(GridLineColor).Append("\n");
      sb.Append("  GridLineDashStyle: ").Append(GridLineDashStyle).Append("\n");
      sb.Append("  GridLineWidth: ").Append(GridLineWidth).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Labels: ").Append(Labels).Append("\n");
      sb.Append("  LineColor: ").Append(LineColor).Append("\n");
      sb.Append("  LineWidth: ").Append(LineWidth).Append("\n");
      sb.Append("  LinkedTo: ").Append(LinkedTo).Append("\n");
      sb.Append("  Max: ").Append(Max).Append("\n");
      sb.Append("  MaxPadding: ").Append(MaxPadding).Append("\n");
      sb.Append("  MaxZoom: ").Append(MaxZoom).Append("\n");
      sb.Append("  Min: ").Append(Min).Append("\n");
      sb.Append("  MinorGridLineColor: ").Append(MinorGridLineColor).Append("\n");
      sb.Append("  MinorGridLineDashStyle: ").Append(MinorGridLineDashStyle).Append("\n");
      sb.Append("  MinorGridLineWidth: ").Append(MinorGridLineWidth).Append("\n");
      sb.Append("  MinorTickColor: ").Append(MinorTickColor).Append("\n");
      sb.Append("  MinorTickInterval: ").Append(MinorTickInterval).Append("\n");
      sb.Append("  MinorTickLength: ").Append(MinorTickLength).Append("\n");
      sb.Append("  MinorTickPosition: ").Append(MinorTickPosition).Append("\n");
      sb.Append("  MinorTickWidth: ").Append(MinorTickWidth).Append("\n");
      sb.Append("  MinPadding: ").Append(MinPadding).Append("\n");
      sb.Append("  Offset: ").Append(Offset).Append("\n");
      sb.Append("  Opposite: ").Append(Opposite).Append("\n");
      sb.Append("  PlotBands: ").Append(PlotBands).Append("\n");
      sb.Append("  PlotLines: ").Append(PlotLines).Append("\n");
      sb.Append("  Reversed: ").Append(Reversed).Append("\n");
      sb.Append("  ShowFirstLabel: ").Append(ShowFirstLabel).Append("\n");
      sb.Append("  ShowLastLabel: ").Append(ShowLastLabel).Append("\n");
      sb.Append("  StackLabels: ").Append(StackLabels).Append("\n");
      sb.Append("  StartOfWeek: ").Append(StartOfWeek).Append("\n");
      sb.Append("  StartOnTick: ").Append(StartOnTick).Append("\n");
      sb.Append("  TickColor: ").Append(TickColor).Append("\n");
      sb.Append("  TickInterval: ").Append(TickInterval).Append("\n");
      sb.Append("  TickLength: ").Append(TickLength).Append("\n");
      sb.Append("  TickmarkPlacement: ").Append(TickmarkPlacement).Append("\n");
      sb.Append("  TickPixelInterval: ").Append(TickPixelInterval).Append("\n");
      sb.Append("  TickPosition: ").Append(TickPosition).Append("\n");
      sb.Append("  Title: ").Append(Title).Append("\n");
      sb.Append("  Type: ").Append(Type).Append("\n");
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
