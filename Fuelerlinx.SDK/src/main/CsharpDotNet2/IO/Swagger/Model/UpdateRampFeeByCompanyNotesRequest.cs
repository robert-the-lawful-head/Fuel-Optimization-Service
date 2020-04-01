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
  public class UpdateRampFeeByCompanyNotesRequest {
    /// <summary>
    /// Gets or Sets RampFeeByCompanyNote
    /// </summary>
    [DataMember(Name="rampFeeByCompanyNote", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFeeByCompanyNote")]
    public RampFeeByCompanyNoteDTO RampFeeByCompanyNote { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateRampFeeByCompanyNotesRequest {\n");
      sb.Append("  RampFeeByCompanyNote: ").Append(RampFeeByCompanyNote).Append("\n");
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
