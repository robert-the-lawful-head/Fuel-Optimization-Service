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
  public class PlotBand {
    /// <summary>
    /// Gets or Sets Color
    /// </summary>
    [DataMember(Name="color", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "color")]
    public string Color { get; set; }

    /// <summary>
    /// Gets or Sets Events
    /// </summary>
    [DataMember(Name="events", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "events")]
    public PlotEvents Events { get; set; }

    /// <summary>
    /// Gets or Sets From
    /// </summary>
    [DataMember(Name="from", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "from")]
    public double? From { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or Sets Label
    /// </summary>
    [DataMember(Name="label", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "label")]
    public PlotLabel Label { get; set; }

    /// <summary>
    /// Gets or Sets To
    /// </summary>
    [DataMember(Name="to", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "to")]
    public double? To { get; set; }

    /// <summary>
    /// Gets or Sets ZIndex
    /// </summary>
    [DataMember(Name="zIndex", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "zIndex")]
    public int? ZIndex { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PlotBand {\n");
      sb.Append("  Color: ").Append(Color).Append("\n");
      sb.Append("  Events: ").Append(Events).Append("\n");
      sb.Append("  From: ").Append(From).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Label: ").Append(Label).Append("\n");
      sb.Append("  To: ").Append(To).Append("\n");
      sb.Append("  ZIndex: ").Append(ZIndex).Append("\n");
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
