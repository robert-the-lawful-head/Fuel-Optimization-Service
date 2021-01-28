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
  public class UpdatePriceSheetFileDataRequest {
    /// <summary>
    /// Gets or Sets PriceSheetFileData
    /// </summary>
    [DataMember(Name="priceSheetFileData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceSheetFileData")]
    public PriceSheetFileDataDTO PriceSheetFileData { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdatePriceSheetFileDataRequest {\n");
      sb.Append("  PriceSheetFileData: ").Append(PriceSheetFileData).Append("\n");
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
