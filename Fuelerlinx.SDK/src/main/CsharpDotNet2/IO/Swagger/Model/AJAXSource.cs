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
  public class AJAXSource {
    /// <summary>
    /// Gets or Sets ClientId
    /// </summary>
    [DataMember(Name="clientId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "clientId")]
    public string ClientId { get; set; }

    /// <summary>
    /// Gets or Sets Source
    /// </summary>
    [DataMember(Name="source", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "source")]
    public string Source { get; set; }

    /// <summary>
    /// Gets or Sets Delay
    /// </summary>
    [DataMember(Name="delay", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "delay")]
    public int? Delay { get; set; }

    /// <summary>
    /// Gets or Sets ClearAll
    /// </summary>
    [DataMember(Name="clearAll", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "clearAll")]
    public bool? ClearAll { get; set; }

    /// <summary>
    /// Gets or Sets OnlyOnce
    /// </summary>
    [DataMember(Name="onlyOnce", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "onlyOnce")]
    public bool? OnlyOnce { get; set; }

    /// <summary>
    /// Gets or Sets Shift
    /// </summary>
    [DataMember(Name="shift", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "shift")]
    public bool? Shift { get; set; }

    /// <summary>
    /// Gets or Sets CustomFunction
    /// </summary>
    [DataMember(Name="customFunction", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "customFunction")]
    public string CustomFunction { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AJAXSource {\n");
      sb.Append("  ClientId: ").Append(ClientId).Append("\n");
      sb.Append("  Source: ").Append(Source).Append("\n");
      sb.Append("  Delay: ").Append(Delay).Append("\n");
      sb.Append("  ClearAll: ").Append(ClearAll).Append("\n");
      sb.Append("  OnlyOnce: ").Append(OnlyOnce).Append("\n");
      sb.Append("  Shift: ").Append(Shift).Append("\n");
      sb.Append("  CustomFunction: ").Append(CustomFunction).Append("\n");
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
