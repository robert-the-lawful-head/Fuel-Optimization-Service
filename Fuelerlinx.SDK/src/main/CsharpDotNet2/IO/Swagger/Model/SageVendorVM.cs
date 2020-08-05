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
  public class SageVendorVM {
    /// <summary>
    /// Gets or Sets VendorId
    /// </summary>
    [DataMember(Name="vendorId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "vendorId")]
    public string VendorId { get; set; }

    /// <summary>
    /// Gets or Sets VendorName
    /// </summary>
    [DataMember(Name="vendorName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "vendorName")]
    public string VendorName { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SageVendorVM {\n");
      sb.Append("  VendorId: ").Append(VendorId).Append("\n");
      sb.Append("  VendorName: ").Append(VendorName).Append("\n");
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
