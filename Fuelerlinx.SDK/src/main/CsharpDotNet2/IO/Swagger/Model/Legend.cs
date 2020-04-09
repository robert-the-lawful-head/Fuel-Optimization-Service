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
  public class Legend {
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="align", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "align")]
    public int? Align { get; set; }

    /// <summary>
    /// Gets or Sets BackgroundColor
    /// </summary>
    [DataMember(Name="backgroundColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "backgroundColor")]
    public string BackgroundColor { get; set; }

    /// <summary>
    /// Gets or Sets BorderColor
    /// </summary>
    [DataMember(Name="borderColor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "borderColor")]
    public string BorderColor { get; set; }

    /// <summary>
    /// Gets or Sets BorderRadius
    /// </summary>
    [DataMember(Name="borderRadius", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "borderRadius")]
    public int? BorderRadius { get; set; }

    /// <summary>
    /// Gets or Sets BorderWidth
    /// </summary>
    [DataMember(Name="borderWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "borderWidth")]
    public int? BorderWidth { get; set; }

    /// <summary>
    /// Gets or Sets Enabled
    /// </summary>
    [DataMember(Name="enabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Gets or Sets Floating
    /// </summary>
    [DataMember(Name="floating", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "floating")]
    public bool? Floating { get; set; }

    /// <summary>
    /// Gets or Sets ItemHiddenStyle
    /// </summary>
    [DataMember(Name="itemHiddenStyle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "itemHiddenStyle")]
    public ItemStyle ItemHiddenStyle { get; set; }

    /// <summary>
    /// Gets or Sets ItemHoverStyle
    /// </summary>
    [DataMember(Name="itemHoverStyle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "itemHoverStyle")]
    public ItemStyle ItemHoverStyle { get; set; }

    /// <summary>
    /// Gets or Sets ItemStyle
    /// </summary>
    [DataMember(Name="itemStyle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "itemStyle")]
    public ItemStyle ItemStyle { get; set; }

    /// <summary>
    /// Gets or Sets ItemWidth
    /// </summary>
    [DataMember(Name="itemWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "itemWidth")]
    public int? ItemWidth { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="layout", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "layout")]
    public int? Layout { get; set; }

    /// <summary>
    /// Gets or Sets LabelFormatter
    /// </summary>
    [DataMember(Name="labelFormatter", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "labelFormatter")]
    public string LabelFormatter { get; set; }

    /// <summary>
    /// Gets or Sets LineHeight
    /// </summary>
    [DataMember(Name="lineHeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lineHeight")]
    public int? LineHeight { get; set; }

    /// <summary>
    /// Gets or Sets Margin
    /// </summary>
    [DataMember(Name="margin", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "margin")]
    public int? Margin { get; set; }

    /// <summary>
    /// Gets or Sets Reversed
    /// </summary>
    [DataMember(Name="reversed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reversed")]
    public bool? Reversed { get; set; }

    /// <summary>
    /// Gets or Sets Shadow
    /// </summary>
    [DataMember(Name="shadow", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "shadow")]
    public bool? Shadow { get; set; }

    /// <summary>
    /// Gets or Sets Style
    /// </summary>
    [DataMember(Name="style", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "style")]
    public CSSObject Style { get; set; }

    /// <summary>
    /// Gets or Sets SymbolPadding
    /// </summary>
    [DataMember(Name="symbolPadding", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "symbolPadding")]
    public int? SymbolPadding { get; set; }

    /// <summary>
    /// Gets or Sets SymbolWidth
    /// </summary>
    [DataMember(Name="symbolWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "symbolWidth")]
    public int? SymbolWidth { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="verticalAlign", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "verticalAlign")]
    public int? VerticalAlign { get; set; }

    /// <summary>
    /// Gets or Sets Width
    /// </summary>
    [DataMember(Name="width", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "width")]
    public int? Width { get; set; }

    /// <summary>
    /// Gets or Sets X
    /// </summary>
    [DataMember(Name="x", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "x")]
    public int? X { get; set; }

    /// <summary>
    /// Gets or Sets Y
    /// </summary>
    [DataMember(Name="y", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "y")]
    public int? Y { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Legend {\n");
      sb.Append("  Align: ").Append(Align).Append("\n");
      sb.Append("  BackgroundColor: ").Append(BackgroundColor).Append("\n");
      sb.Append("  BorderColor: ").Append(BorderColor).Append("\n");
      sb.Append("  BorderRadius: ").Append(BorderRadius).Append("\n");
      sb.Append("  BorderWidth: ").Append(BorderWidth).Append("\n");
      sb.Append("  Enabled: ").Append(Enabled).Append("\n");
      sb.Append("  Floating: ").Append(Floating).Append("\n");
      sb.Append("  ItemHiddenStyle: ").Append(ItemHiddenStyle).Append("\n");
      sb.Append("  ItemHoverStyle: ").Append(ItemHoverStyle).Append("\n");
      sb.Append("  ItemStyle: ").Append(ItemStyle).Append("\n");
      sb.Append("  ItemWidth: ").Append(ItemWidth).Append("\n");
      sb.Append("  Layout: ").Append(Layout).Append("\n");
      sb.Append("  LabelFormatter: ").Append(LabelFormatter).Append("\n");
      sb.Append("  LineHeight: ").Append(LineHeight).Append("\n");
      sb.Append("  Margin: ").Append(Margin).Append("\n");
      sb.Append("  Reversed: ").Append(Reversed).Append("\n");
      sb.Append("  Shadow: ").Append(Shadow).Append("\n");
      sb.Append("  Style: ").Append(Style).Append("\n");
      sb.Append("  SymbolPadding: ").Append(SymbolPadding).Append("\n");
      sb.Append("  SymbolWidth: ").Append(SymbolWidth).Append("\n");
      sb.Append("  VerticalAlign: ").Append(VerticalAlign).Append("\n");
      sb.Append("  Width: ").Append(Width).Append("\n");
      sb.Append("  X: ").Append(X).Append("\n");
      sb.Append("  Y: ").Append(Y).Append("\n");
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
