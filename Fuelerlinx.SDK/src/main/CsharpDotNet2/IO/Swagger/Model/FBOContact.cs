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
  public class FBOContact {
    /// <summary>
    /// Gets or Sets Fboid
    /// </summary>
    [DataMember(Name="fboid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboid")]
    public int? Fboid { get; set; }

    /// <summary>
    /// Gets or Sets ContactId
    /// </summary>
    [DataMember(Name="contactId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "contactId")]
    public int? ContactId { get; set; }

    /// <summary>
    /// Gets or Sets Oid
    /// </summary>
    [DataMember(Name="oid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "oid")]
    public int? Oid { get; set; }

    /// <summary>
    /// Gets or Sets Contact
    /// </summary>
    [DataMember(Name="contact", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "contact")]
    public FBOLinxContact Contact { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FBOContact {\n");
      sb.Append("  Fboid: ").Append(Fboid).Append("\n");
      sb.Append("  ContactId: ").Append(ContactId).Append("\n");
      sb.Append("  Oid: ").Append(Oid).Append("\n");
      sb.Append("  Contact: ").Append(Contact).Append("\n");
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
