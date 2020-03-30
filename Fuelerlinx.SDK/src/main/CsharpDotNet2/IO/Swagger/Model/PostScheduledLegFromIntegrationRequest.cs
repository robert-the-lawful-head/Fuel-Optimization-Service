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
  public class PostScheduledLegFromIntegrationRequest {
    /// <summary>
    /// Gets or Sets ScheduledLegData
    /// </summary>
    [DataMember(Name="scheduledLegData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "scheduledLegData")]
    public Object ScheduledLegData { get; set; }

    /// <summary>
    /// Gets or Sets LegIdentifier
    /// </summary>
    [DataMember(Name="legIdentifier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legIdentifier")]
    public string LegIdentifier { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostScheduledLegFromIntegrationRequest {\n");
      sb.Append("  ScheduledLegData: ").Append(ScheduledLegData).Append("\n");
      sb.Append("  LegIdentifier: ").Append(LegIdentifier).Append("\n");
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
