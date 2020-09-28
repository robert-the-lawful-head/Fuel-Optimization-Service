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
  public class UpdateCompanyFuelerChangeLogRequest {
    /// <summary>
    /// Gets or Sets CompanyFuelerChangeLog
    /// </summary>
    [DataMember(Name="companyFuelerChangeLog", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyFuelerChangeLog")]
    public CompanyFuelerChangeLogDTO CompanyFuelerChangeLog { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateCompanyFuelerChangeLogRequest {\n");
      sb.Append("  CompanyFuelerChangeLog: ").Append(CompanyFuelerChangeLog).Append("\n");
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
