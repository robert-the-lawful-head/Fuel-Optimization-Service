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
  public class UpdateSavedTripLegRequest {
    /// <summary>
    /// Gets or Sets SavedTripLeg
    /// </summary>
    [DataMember(Name="savedTripLeg", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "savedTripLeg")]
    public SavedTripLegDTO SavedTripLeg { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateSavedTripLegRequest {\n");
      sb.Append("  SavedTripLeg: ").Append(SavedTripLeg).Append("\n");
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
