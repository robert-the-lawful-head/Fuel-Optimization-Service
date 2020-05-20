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
  public class Series {
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="type", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "type")]
    public int? Type { get; set; }

    /// <summary>
    /// Gets or Sets Center
    /// </summary>
    [DataMember(Name="center", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "center")]
    public List<Object> Center { get; set; }

    /// <summary>
    /// Gets or Sets Size
    /// </summary>
    [DataMember(Name="size", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "size")]
    public int? Size { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="stack", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "stack")]
    public int? Stack { get; set; }

    /// <summary>
    /// Gets or Sets InnerSize
    /// </summary>
    [DataMember(Name="innerSize", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "innerSize")]
    public int? InnerSize { get; set; }

    /// <summary>
    /// Gets or Sets XAxis
    /// </summary>
    [DataMember(Name="xAxis", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "xAxis")]
    public int? XAxis { get; set; }

    /// <summary>
    /// Gets or Sets YAxis
    /// </summary>
    [DataMember(Name="yAxis", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "yAxis")]
    public int? YAxis { get; set; }

    /// <summary>
    /// Gets or Sets Data
    /// </summary>
    [DataMember(Name="data", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "data")]
    public List<Object> Data { get; set; }

    /// <summary>
    /// Gets or Sets Level
    /// </summary>
    [DataMember(Name="level", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "level")]
    public int? Level { get; set; }

    /// <summary>
    /// Gets or Sets Color
    /// </summary>
    [DataMember(Name="color", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "color")]
    public string Color { get; set; }

    /// <summary>
    /// Gets or Sets ShowInLegend
    /// </summary>
    [DataMember(Name="showInLegend", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "showInLegend")]
    public bool? ShowInLegend { get; set; }

    /// <summary>
    /// Gets or Sets Selected
    /// </summary>
    [DataMember(Name="selected", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "selected")]
    public bool? Selected { get; set; }

    /// <summary>
    /// Gets or Sets Visible
    /// </summary>
    [DataMember(Name="visible", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "visible")]
    public bool? Visible { get; set; }

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
      sb.Append("class Series {\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Type: ").Append(Type).Append("\n");
      sb.Append("  Center: ").Append(Center).Append("\n");
      sb.Append("  Size: ").Append(Size).Append("\n");
      sb.Append("  Stack: ").Append(Stack).Append("\n");
      sb.Append("  InnerSize: ").Append(InnerSize).Append("\n");
      sb.Append("  XAxis: ").Append(XAxis).Append("\n");
      sb.Append("  YAxis: ").Append(YAxis).Append("\n");
      sb.Append("  Data: ").Append(Data).Append("\n");
      sb.Append("  Level: ").Append(Level).Append("\n");
      sb.Append("  Color: ").Append(Color).Append("\n");
      sb.Append("  ShowInLegend: ").Append(ShowInLegend).Append("\n");
      sb.Append("  Selected: ").Append(Selected).Append("\n");
      sb.Append("  Visible: ").Append(Visible).Append("\n");
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
