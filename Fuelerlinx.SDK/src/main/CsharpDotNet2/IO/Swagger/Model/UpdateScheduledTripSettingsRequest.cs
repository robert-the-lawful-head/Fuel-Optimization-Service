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
  public class UpdateScheduledTripSettingsRequest {
    /// <summary>
    /// Gets or Sets ScheduledTripSettings
    /// </summary>
    [DataMember(Name="scheduledTripSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "scheduledTripSettings")]
    public ScheduledTripSettingsDTO ScheduledTripSettings { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateScheduledTripSettingsRequest {\n");
      sb.Append("  ScheduledTripSettings: ").Append(ScheduledTripSettings).Append("\n");
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
