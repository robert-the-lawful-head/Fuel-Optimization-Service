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
  public class PdfParsingSubFeaturesDTO {
    /// <summary>
    /// Gets or Sets InvoiceMatching
    /// </summary>
    [DataMember(Name="InvoiceMatching", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "InvoiceMatching")]
    public MenuFeatureDTO InvoiceMatching { get; set; }

    /// <summary>
    /// Gets or Sets InvoiceMatchNotifications
    /// </summary>
    [DataMember(Name="InvoiceMatchNotifications", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "InvoiceMatchNotifications")]
    public MenuFeatureDTO InvoiceMatchNotifications { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PdfParsingSubFeaturesDTO {\n");
      sb.Append("  InvoiceMatching: ").Append(InvoiceMatching).Append("\n");
      sb.Append("  InvoiceMatchNotifications: ").Append(InvoiceMatchNotifications).Append("\n");
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
