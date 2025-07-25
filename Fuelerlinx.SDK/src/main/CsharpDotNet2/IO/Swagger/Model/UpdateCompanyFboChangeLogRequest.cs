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
  public class UpdateCompanyFboChangeLogRequest {
    /// <summary>
    /// Gets or Sets CompanyFboChangeLog
    /// </summary>
    [DataMember(Name="companyFboChangeLog", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyFboChangeLog")]
    public CompanyFboChangeLogDTO CompanyFboChangeLog { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateCompanyFboChangeLogRequest {\n");
      sb.Append("  CompanyFboChangeLog: ").Append(CompanyFboChangeLog).Append("\n");
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
