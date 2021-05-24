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
  public class FBOLinxGroupOrdersRequest {
    /// <summary>
    /// Gets or Sets StartDateTime
    /// </summary>
    [DataMember(Name="startDateTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "startDateTime")]
    public DateTime? StartDateTime { get; set; }

    /// <summary>
    /// Gets or Sets EndDateTime
    /// </summary>
    [DataMember(Name="endDateTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "endDateTime")]
    public DateTime? EndDateTime { get; set; }

    /// <summary>
    /// Gets or Sets Icaos
    /// </summary>
    [DataMember(Name="icaos", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icaos")]
    public List<string> Icaos { get; set; }

    /// <summary>
    /// Gets or Sets Fbos
    /// </summary>
    [DataMember(Name="fbos", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fbos")]
    public List<string> Fbos { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FBOLinxGroupOrdersRequest {\n");
      sb.Append("  StartDateTime: ").Append(StartDateTime).Append("\n");
      sb.Append("  EndDateTime: ").Append(EndDateTime).Append("\n");
      sb.Append("  Icaos: ").Append(Icaos).Append("\n");
      sb.Append("  Fbos: ").Append(Fbos).Append("\n");
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
