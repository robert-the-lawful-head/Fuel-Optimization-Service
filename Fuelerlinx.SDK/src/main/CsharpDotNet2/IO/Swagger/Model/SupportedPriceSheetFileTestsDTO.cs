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
  public class SupportedPriceSheetFileTestsDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets SupportedPriceSheetFileId
    /// </summary>
    [DataMember(Name="supportedPriceSheetFileId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supportedPriceSheetFileId")]
    public int? SupportedPriceSheetFileId { get; set; }

    /// <summary>
    /// Gets or Sets ExpectedPriceTiersCount
    /// </summary>
    [DataMember(Name="expectedPriceTiersCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "expectedPriceTiersCount")]
    public int? ExpectedPriceTiersCount { get; set; }

    /// <summary>
    /// Gets or Sets ExpectedLocations
    /// </summary>
    [DataMember(Name="expectedLocations", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "expectedLocations")]
    public string ExpectedLocations { get; set; }

    /// <summary>
    /// Gets or Sets MinimumPriceInSheet
    /// </summary>
    [DataMember(Name="minimumPriceInSheet", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minimumPriceInSheet")]
    public double? MinimumPriceInSheet { get; set; }

    /// <summary>
    /// Gets or Sets MaximumPriceInSheet
    /// </summary>
    [DataMember(Name="maximumPriceInSheet", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maximumPriceInSheet")]
    public double? MaximumPriceInSheet { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SupportedPriceSheetFileTestsDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  SupportedPriceSheetFileId: ").Append(SupportedPriceSheetFileId).Append("\n");
      sb.Append("  ExpectedPriceTiersCount: ").Append(ExpectedPriceTiersCount).Append("\n");
      sb.Append("  ExpectedLocations: ").Append(ExpectedLocations).Append("\n");
      sb.Append("  MinimumPriceInSheet: ").Append(MinimumPriceInSheet).Append("\n");
      sb.Append("  MaximumPriceInSheet: ").Append(MaximumPriceInSheet).Append("\n");
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
