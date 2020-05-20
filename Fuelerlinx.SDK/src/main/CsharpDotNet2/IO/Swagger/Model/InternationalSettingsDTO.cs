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
  public class InternationalSettingsDTO {
    /// <summary>
    /// Gets or Sets ShowVAT
    /// </summary>
    [DataMember(Name="ShowVAT", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ShowVAT")]
    public bool? ShowVAT { get; set; }

    /// <summary>
    /// Gets or Sets ShowHandlersOnDispatch
    /// </summary>
    [DataMember(Name="ShowHandlersOnDispatch", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ShowHandlersOnDispatch")]
    public bool? ShowHandlersOnDispatch { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InternationalSettingsDTO {\n");
      sb.Append("  ShowVAT: ").Append(ShowVAT).Append("\n");
      sb.Append("  ShowHandlersOnDispatch: ").Append(ShowHandlersOnDispatch).Append("\n");
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
