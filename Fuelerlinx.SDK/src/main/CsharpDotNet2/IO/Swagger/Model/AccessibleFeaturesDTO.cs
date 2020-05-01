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
  public class AccessibleFeaturesDTO {
    /// <summary>
    /// Gets or Sets EntityData
    /// </summary>
    [DataMember(Name="EntityData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "EntityData")]
    public EntityDataDTO EntityData { get; set; }

    /// <summary>
    /// Gets or Sets Permissions
    /// </summary>
    [DataMember(Name="Permissions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Permissions")]
    public PermissionsDTO Permissions { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AccessibleFeaturesDTO {\n");
      sb.Append("  EntityData: ").Append(EntityData).Append("\n");
      sb.Append("  Permissions: ").Append(Permissions).Append("\n");
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
