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
  public class DeploymentNotesDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets MinimumBuildVersion
    /// </summary>
    [DataMember(Name="minimumBuildVersion", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minimumBuildVersion")]
    public int? MinimumBuildVersion { get; set; }

    /// <summary>
    /// Gets or Sets MaximumBuildVersion
    /// </summary>
    [DataMember(Name="maximumBuildVersion", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maximumBuildVersion")]
    public int? MaximumBuildVersion { get; set; }

    /// <summary>
    /// Gets or Sets Notes
    /// </summary>
    [DataMember(Name="notes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "notes")]
    public string Notes { get; set; }

    /// <summary>
    /// Gets or Sets LastUpdated
    /// </summary>
    [DataMember(Name="lastUpdated", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lastUpdated")]
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="applicationType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applicationType")]
    public int? ApplicationType { get; set; }

    /// <summary>
    /// Gets or Sets ApplicationTypeDescription
    /// </summary>
    [DataMember(Name="applicationTypeDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applicationTypeDescription")]
    public string ApplicationTypeDescription { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DeploymentNotesDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  MinimumBuildVersion: ").Append(MinimumBuildVersion).Append("\n");
      sb.Append("  MaximumBuildVersion: ").Append(MaximumBuildVersion).Append("\n");
      sb.Append("  Notes: ").Append(Notes).Append("\n");
      sb.Append("  LastUpdated: ").Append(LastUpdated).Append("\n");
      sb.Append("  ApplicationType: ").Append(ApplicationType).Append("\n");
      sb.Append("  ApplicationTypeDescription: ").Append(ApplicationTypeDescription).Append("\n");
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
