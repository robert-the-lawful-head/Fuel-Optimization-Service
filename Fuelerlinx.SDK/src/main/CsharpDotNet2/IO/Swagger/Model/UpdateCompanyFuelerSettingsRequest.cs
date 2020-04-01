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
  public class UpdateCompanyFuelerSettingsRequest {
    /// <summary>
    /// Gets or Sets CompanyFuelerSettings
    /// </summary>
    [DataMember(Name="companyFuelerSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyFuelerSettings")]
    public CompanyFuelerSettingsDTO CompanyFuelerSettings { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateCompanyFuelerSettingsRequest {\n");
      sb.Append("  CompanyFuelerSettings: ").Append(CompanyFuelerSettings).Append("\n");
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
