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
  public class ReportDataOptions {
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="reportType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reportType")]
    public int? ReportType { get; set; }

    /// <summary>
    /// Gets or Sets Columns
    /// </summary>
    [DataMember(Name="columns", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "columns")]
    public List<ReportColumnDTO> Columns { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ReportDataOptions {\n");
      sb.Append("  ReportType: ").Append(ReportType).Append("\n");
      sb.Append("  Columns: ").Append(Columns).Append("\n");
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
