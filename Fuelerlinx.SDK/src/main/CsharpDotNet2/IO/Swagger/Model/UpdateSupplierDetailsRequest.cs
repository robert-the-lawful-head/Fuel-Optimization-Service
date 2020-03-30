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
  public class UpdateSupplierDetailsRequest {
    /// <summary>
    /// Gets or Sets SupplierDetails
    /// </summary>
    [DataMember(Name="supplierDetails", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supplierDetails")]
    public SupplierDetailsDTO SupplierDetails { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateSupplierDetailsRequest {\n");
      sb.Append("  SupplierDetails: ").Append(SupplierDetails).Append("\n");
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
