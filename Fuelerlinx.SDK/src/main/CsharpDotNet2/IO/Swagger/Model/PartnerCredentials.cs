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
  public class PartnerCredentials {
    /// <summary>
    /// Gets or Sets Model
    /// </summary>
    [DataMember(Name="model", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "model")]
    public Object Model { get; set; }

    /// <summary>
    /// Gets or Sets InputFields
    /// </summary>
    [DataMember(Name="inputFields", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "inputFields")]
    public List<PartnerInputFieldDTO> InputFields { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PartnerCredentials {\n");
      sb.Append("  Model: ").Append(Model).Append("\n");
      sb.Append("  InputFields: ").Append(InputFields).Append("\n");
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
