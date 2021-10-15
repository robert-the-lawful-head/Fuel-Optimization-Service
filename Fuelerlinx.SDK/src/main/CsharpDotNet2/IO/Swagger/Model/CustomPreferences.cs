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
  public class CustomPreferences {
    /// <summary>
    /// Gets or Sets Dispatching
    /// </summary>
    [DataMember(Name="dispatching", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dispatching")]
    public DispatchingPreferences Dispatching { get; set; }

    /// <summary>
    /// Gets or Sets Quoting
    /// </summary>
    [DataMember(Name="quoting", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "quoting")]
    public QuotingPreferences Quoting { get; set; }

    /// <summary>
    /// Gets or Sets Navigation
    /// </summary>
    [DataMember(Name="navigation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "navigation")]
    public NavigationPreferences Navigation { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CustomPreferences {\n");
      sb.Append("  Dispatching: ").Append(Dispatching).Append("\n");
      sb.Append("  Quoting: ").Append(Quoting).Append("\n");
      sb.Append("  Navigation: ").Append(Navigation).Append("\n");
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
