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
  public class AnalysisSettingsDTO {
    /// <summary>
    /// Gets or Sets OID
    /// </summary>
    [DataMember(Name="OID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "OID")]
    public int? OID { get; set; }

    /// <summary>
    /// Gets or Sets UserID
    /// </summary>
    [DataMember(Name="UserID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "UserID")]
    public int? UserID { get; set; }

    /// <summary>
    /// Gets or Sets ShowFleetSizeFilter
    /// </summary>
    [DataMember(Name="ShowFleetSizeFilter", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ShowFleetSizeFilter")]
    public bool? ShowFleetSizeFilter { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AnalysisSettingsDTO {\n");
      sb.Append("  OID: ").Append(OID).Append("\n");
      sb.Append("  UserID: ").Append(UserID).Append("\n");
      sb.Append("  ShowFleetSizeFilter: ").Append(ShowFleetSizeFilter).Append("\n");
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
