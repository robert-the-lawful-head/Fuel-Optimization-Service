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
    /// Gets or Sets VerticalAlign
    /// </summary>
    [DataMember(Name="verticalAlign", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "verticalAlign")]
    public string VerticalAlign { get; set; }

    /// <summary>
    /// Gets or Sets ItemMarginTop
    /// </summary>
    [DataMember(Name="itemMarginTop", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "itemMarginTop")]
    public int? ItemMarginTop { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Legend {\n");
      sb.Append("  VerticalAlign: ").Append(VerticalAlign).Append("\n");
      sb.Append("  ItemMarginTop: ").Append(ItemMarginTop).Append("\n");
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
