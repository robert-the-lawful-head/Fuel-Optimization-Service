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
  public class UpdateSupportedPriceSheetFilesRequest {
    /// <summary>
    /// Gets or Sets SupportedPriceSheetFiles
    /// </summary>
    [DataMember(Name="supportedPriceSheetFiles", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supportedPriceSheetFiles")]
    public SupportedPriceSheetFilesDTO SupportedPriceSheetFiles { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateSupportedPriceSheetFilesRequest {\n");
      sb.Append("  SupportedPriceSheetFiles: ").Append(SupportedPriceSheetFiles).Append("\n");
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
