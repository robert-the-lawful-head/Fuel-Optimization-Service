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
  public class EntityDataDTO {
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
    /// Gets or Sets AccessibleFeaturesJSON
    /// </summary>
    [DataMember(Name="AccessibleFeaturesJSON", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "AccessibleFeaturesJSON")]
    public string AccessibleFeaturesJSON { get; set; }

    /// <summary>
    /// Gets or Sets HasBeenSyncedWithOldFeatures
    /// </summary>
    [DataMember(Name="HasBeenSyncedWithOldFeatures", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "HasBeenSyncedWithOldFeatures")]
    public bool? HasBeenSyncedWithOldFeatures { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class EntityDataDTO {\n");
      sb.Append("  OID: ").Append(OID).Append("\n");
      sb.Append("  UserID: ").Append(UserID).Append("\n");
      sb.Append("  AccessibleFeaturesJSON: ").Append(AccessibleFeaturesJSON).Append("\n");
      sb.Append("  HasBeenSyncedWithOldFeatures: ").Append(HasBeenSyncedWithOldFeatures).Append("\n");
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
