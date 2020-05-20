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
  public class TankeringDTO {
    /// <summary>
    /// Gets or Sets MinimumExtraReserve
    /// </summary>
    [DataMember(Name="MinimumExtraReserve", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "MinimumExtraReserve")]
    public MenuFeatureDTO MinimumExtraReserve { get; set; }

    /// <summary>
    /// Gets or Sets IsEnabled
    /// </summary>
    [DataMember(Name="IsEnabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "IsEnabled")]
    public bool? IsEnabled { get; set; }

    /// <summary>
    /// Gets or Sets Caption
    /// </summary>
    [DataMember(Name="Caption", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Caption")]
    public string Caption { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TankeringDTO {\n");
      sb.Append("  MinimumExtraReserve: ").Append(MinimumExtraReserve).Append("\n");
      sb.Append("  IsEnabled: ").Append(IsEnabled).Append("\n");
      sb.Append("  Caption: ").Append(Caption).Append("\n");
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
