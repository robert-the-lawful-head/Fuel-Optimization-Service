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
  public class UpdateCompanyFuelerRequest {
    /// <summary>
    /// Gets or Sets CompanyFueler
    /// </summary>
    [DataMember(Name="companyFueler", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyFueler")]
    public CompanyFuelerDTO CompanyFueler { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateCompanyFuelerRequest {\n");
      sb.Append("  CompanyFueler: ").Append(CompanyFueler).Append("\n");
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
