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
  public class UpdateImportFileCaptureRequest {
    /// <summary>
    /// Gets or Sets ImportFileCapture
    /// </summary>
    [DataMember(Name="importFileCapture", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "importFileCapture")]
    public ImportFileCaptureDTO ImportFileCapture { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateImportFileCaptureRequest {\n");
      sb.Append("  ImportFileCapture: ").Append(ImportFileCapture).Append("\n");
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
