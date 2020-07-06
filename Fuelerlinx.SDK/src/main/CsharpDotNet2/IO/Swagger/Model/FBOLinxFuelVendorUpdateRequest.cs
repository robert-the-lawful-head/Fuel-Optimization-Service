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
  public class FBOLinxFuelVendorUpdateRequest {
    /// <summary>
    /// Gets or Sets FboId
    /// </summary>
    [DataMember(Name="fboId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboId")]
    public int? FboId { get; set; }

    /// <summary>
    /// Gets or Sets Email
    /// </summary>
    [DataMember(Name="email", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "email")]
    public string Email { get; set; }

    /// <summary>
    /// Gets or Sets EmailToCC
    /// </summary>
    [DataMember(Name="emailToCC", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "emailToCC")]
    public string EmailToCC { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FBOLinxFuelVendorUpdateRequest {\n");
      sb.Append("  FboId: ").Append(FboId).Append("\n");
      sb.Append("  Email: ").Append(Email).Append("\n");
      sb.Append("  EmailToCC: ").Append(EmailToCC).Append("\n");
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
