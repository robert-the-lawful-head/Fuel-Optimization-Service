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
  public class Appearance {
    /// <summary>
    /// Gets or Sets RenderTo
    /// </summary>
    [DataMember(Name="renderTo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "renderTo")]
    public string RenderTo { get; set; }

    /// <summary>
    /// Gets or Sets DefaultSeriesType
    /// </summary>
    [DataMember(Name="defaultSeriesType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "defaultSeriesType")]
    public string DefaultSeriesType { get; set; }

    /// <summary>
    /// Gets or Sets AlignTicks
    /// </summary>
    [DataMember(Name="alignTicks", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "alignTicks")]
    public bool? AlignTicks { get; set; }

    /// <summary>
    /// Gets or Sets Animation
    /// </summary>
    [DataMember(Name="animation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "animation")]
    public bool? Animation { get; set; }

    /// <summary>
    /// Gets or Sets BackgroundColor
    /// </summary>
    [DataMember(Name="backgroundColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "backgroundColor")]
    public Background BackgroundColor { get; set; }

    /// <summary>
    /// Gets or Sets BorderColor
    /// </summary>
    [DataMember(Name="borderColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "borderColor")]
    public string BorderColor { get; set; }

    /// <summary>
    /// Gets or Sets BorderWidth
    /// </summary>
    [DataMember(Name="borderWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "borderWidth")]
    public int? BorderWidth { get; set; }

    /// <summary>
    /// Gets or Sets BorderRadius
    /// </summary>
    [DataMember(Name="borderRadius", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "borderRadius")]
    public int? BorderRadius { get; set; }

    /// <summary>
    /// Gets or Sets ClassName
    /// </summary>
    [DataMember(Name="className", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "className")]
    public string ClassName { get; set; }

    /// <summary>
    /// Gets or Sets Events
    /// </summary>
    [DataMember(Name="events", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "events")]
    public ChartEvents Events { get; set; }

    /// <summary>
    /// Gets or Sets Style
    /// </summary>
    [DataMember(Name="style", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "style")]
    public CSSObject Style { get; set; }

    /// <summary>
    /// Gets or Sets IgnoreHiddenSeries
    /// </summary>
    [DataMember(Name="ignoreHiddenSeries", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ignoreHiddenSeries")]
    public bool? IgnoreHiddenSeries { get; set; }

    /// <summary>
    /// Gets or Sets Inverted
    /// </summary>
    [DataMember(Name="inverted", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "inverted")]
    public bool? Inverted { get; set; }

    /// <summary>
    /// Gets or Sets Margin
    /// </summary>
    [DataMember(Name="margin", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "margin")]
    public List<int?> Margin { get; set; }

    /// <summary>
    /// Gets or Sets MarginTop
    /// </summary>
    [DataMember(Name="marginTop", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "marginTop")]
    public int? MarginTop { get; set; }

    /// <summary>
    /// Gets or Sets MarginRight
    /// </summary>
    [DataMember(Name="marginRight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "marginRight")]
    public int? MarginRight { get; set; }

    /// <summary>
    /// Gets or Sets MarginBottom
    /// </summary>
    [DataMember(Name="marginBottom", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "marginBottom")]
    public int? MarginBottom { get; set; }

    /// <summary>
    /// Gets or Sets MarginLeft
    /// </summary>
    [DataMember(Name="marginLeft", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "marginLeft")]
    public int? MarginLeft { get; set; }

    /// <summary>
    /// Gets or Sets Shadow
    /// </summary>
    [DataMember(Name="shadow", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "shadow")]
    public bool? Shadow { get; set; }

    /// <summary>
    /// Gets or Sets ShowAxes
    /// </summary>
    [DataMember(Name="showAxes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "showAxes")]
    public bool? ShowAxes { get; set; }

    /// <summary>
    /// Gets or Sets PlotBackgroundImage
    /// </summary>
    [DataMember(Name="plotBackgroundImage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "plotBackgroundImage")]
    public string PlotBackgroundImage { get; set; }

    /// <summary>
    /// Gets or Sets PlotBackgroundColor
    /// </summary>
    [DataMember(Name="plotBackgroundColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "plotBackgroundColor")]
    public Background PlotBackgroundColor { get; set; }

    /// <summary>
    /// Gets or Sets PlotBorderColor
    /// </summary>
    [DataMember(Name="plotBorderColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "plotBorderColor")]
    public string PlotBorderColor { get; set; }

    /// <summary>
    /// Gets or Sets PlotBorderWidth
    /// </summary>
    [DataMember(Name="plotBorderWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "plotBorderWidth")]
    public int? PlotBorderWidth { get; set; }

    /// <summary>
    /// Gets or Sets PlotShadow
    /// </summary>
    [DataMember(Name="plotShadow", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "plotShadow")]
    public bool? PlotShadow { get; set; }

    /// <summary>
    /// Gets or Sets ZoomType
    /// </summary>
    [DataMember(Name="zoomType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "zoomType")]
    public string ZoomType { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Appearance {\n");
      sb.Append("  RenderTo: ").Append(RenderTo).Append("\n");
      sb.Append("  DefaultSeriesType: ").Append(DefaultSeriesType).Append("\n");
      sb.Append("  AlignTicks: ").Append(AlignTicks).Append("\n");
      sb.Append("  Animation: ").Append(Animation).Append("\n");
      sb.Append("  BackgroundColor: ").Append(BackgroundColor).Append("\n");
      sb.Append("  BorderColor: ").Append(BorderColor).Append("\n");
      sb.Append("  BorderWidth: ").Append(BorderWidth).Append("\n");
      sb.Append("  BorderRadius: ").Append(BorderRadius).Append("\n");
      sb.Append("  ClassName: ").Append(ClassName).Append("\n");
      sb.Append("  Events: ").Append(Events).Append("\n");
      sb.Append("  Style: ").Append(Style).Append("\n");
      sb.Append("  IgnoreHiddenSeries: ").Append(IgnoreHiddenSeries).Append("\n");
      sb.Append("  Inverted: ").Append(Inverted).Append("\n");
      sb.Append("  Margin: ").Append(Margin).Append("\n");
      sb.Append("  MarginTop: ").Append(MarginTop).Append("\n");
      sb.Append("  MarginRight: ").Append(MarginRight).Append("\n");
      sb.Append("  MarginBottom: ").Append(MarginBottom).Append("\n");
      sb.Append("  MarginLeft: ").Append(MarginLeft).Append("\n");
      sb.Append("  Shadow: ").Append(Shadow).Append("\n");
      sb.Append("  ShowAxes: ").Append(ShowAxes).Append("\n");
      sb.Append("  PlotBackgroundImage: ").Append(PlotBackgroundImage).Append("\n");
      sb.Append("  PlotBackgroundColor: ").Append(PlotBackgroundColor).Append("\n");
      sb.Append("  PlotBorderColor: ").Append(PlotBorderColor).Append("\n");
      sb.Append("  PlotBorderWidth: ").Append(PlotBorderWidth).Append("\n");
      sb.Append("  PlotShadow: ").Append(PlotShadow).Append("\n");
      sb.Append("  ZoomType: ").Append(ZoomType).Append("\n");
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
