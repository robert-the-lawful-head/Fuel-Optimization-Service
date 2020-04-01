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
  public class PostSupplierDetailsRequest {
    /// <summary>
    /// Gets or Sets SupplierName
    /// </summary>
    [DataMember(Name="supplierName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supplierName")]
    public string SupplierName { get; set; }

    /// <summary>
    /// Gets or Sets SupplierSite
    /// </summary>
    [DataMember(Name="supplierSite", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supplierSite")]
    public string SupplierSite { get; set; }

    /// <summary>
    /// Gets or Sets SupplierNumber
    /// </summary>
    [DataMember(Name="supplierNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supplierNumber")]
    public string SupplierNumber { get; set; }

    /// <summary>
    /// Gets or Sets PaymentTerms
    /// </summary>
    [DataMember(Name="paymentTerms", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paymentTerms")]
    public string PaymentTerms { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostSupplierDetailsRequest {\n");
      sb.Append("  SupplierName: ").Append(SupplierName).Append("\n");
      sb.Append("  SupplierSite: ").Append(SupplierSite).Append("\n");
      sb.Append("  SupplierNumber: ").Append(SupplierNumber).Append("\n");
      sb.Append("  PaymentTerms: ").Append(PaymentTerms).Append("\n");
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
