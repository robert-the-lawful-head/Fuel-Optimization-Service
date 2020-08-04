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
  public class UpdateSchedulingIntegrationDispatchServiceLogRequest {
    /// <summary>
    /// Gets or Sets SchedulingIntegrationDispatchServiceLog
    /// </summary>
    [DataMember(Name="schedulingIntegrationDispatchServiceLog", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "schedulingIntegrationDispatchServiceLog")]
    public SchedulingIntegrationDispatchServiceLogDTO SchedulingIntegrationDispatchServiceLog { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateSchedulingIntegrationDispatchServiceLogRequest {\n");
      sb.Append("  SchedulingIntegrationDispatchServiceLog: ").Append(SchedulingIntegrationDispatchServiceLog).Append("\n");
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
