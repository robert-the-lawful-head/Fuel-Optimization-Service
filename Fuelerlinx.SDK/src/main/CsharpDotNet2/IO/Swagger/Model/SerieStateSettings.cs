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
  public class SerieStateSettings {
    /// <summary>
    /// Gets or Sets Enabled
    /// </summary>
    [DataMember(Name="enabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "enabled")]
    public bool? Enabled { get; set; }

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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SerieStateSettings {\n");
      sb.Append("  Enabled: ").Append(Enabled).Append("\n");
      sb.Append("  LineWidth: ").Append(LineWidth).Append("\n");
      sb.Append("  Marker: ").Append(Marker).Append("\n");
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
