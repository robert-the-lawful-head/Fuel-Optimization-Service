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
  public class UpdateTaxesByCountryRequest {
    /// <summary>
    /// Gets or Sets TaxesByCountry
    /// </summary>
    [DataMember(Name="taxesByCountry", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "taxesByCountry")]
    public TaxesByCountryDTO TaxesByCountry { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UpdateTaxesByCountryRequest {\n");
      sb.Append("  TaxesByCountry: ").Append(TaxesByCountry).Append("\n");
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
