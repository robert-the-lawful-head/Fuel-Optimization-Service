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
  public class UpdateServicesAndFeesByCompanyRequest {
    /// <summary>
    /// Gets or Sets ServicesAndFeesByCompany
    /// </summary>
    [DataMember(Name="servicesAndFeesByCompany", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "servicesAndFeesByCompany")]
    public ServicesAndFeesByCompanyDTO ServicesAndFeesByCompany { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateServicesAndFeesByCompanyRequest {\n");
      sb.Append("  ServicesAndFeesByCompany: ").Append(ServicesAndFeesByCompany).Append("\n");
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
