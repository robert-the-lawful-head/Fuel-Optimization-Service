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
  public class TransactionsSubFeaturesDTO {
    /// <summary>
    /// Gets or Sets PDFParsing
    /// </summary>
    [DataMember(Name="PDFParsing", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "PDFParsing")]
    public PdfParsingDTO PDFParsing { get; set; }

    /// <summary>
    /// Gets or Sets SyncWithDegaFuelOrders
    /// </summary>
    [DataMember(Name="SyncWithDegaFuelOrders", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "SyncWithDegaFuelOrders")]
    public MenuFeatureDTO SyncWithDegaFuelOrders { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TransactionsSubFeaturesDTO {\n");
      sb.Append("  PDFParsing: ").Append(PDFParsing).Append("\n");
      sb.Append("  SyncWithDegaFuelOrders: ").Append(SyncWithDegaFuelOrders).Append("\n");
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
