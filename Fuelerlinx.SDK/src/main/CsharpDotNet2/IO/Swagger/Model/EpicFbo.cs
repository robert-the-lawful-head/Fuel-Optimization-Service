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
  public class EpicFbo {
    /// <summary>
    /// Gets or Sets Fuel
    /// </summary>
    [DataMember(Name="fuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuel")]
    public List<Fuel> Fuel { get; set; }

    /// <summary>
    /// Gets or Sets FboName
    /// </summary>
    [DataMember(Name="fboName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboName")]
    public string FboName { get; set; }

    /// <summary>
    /// Gets or Sets Supplier
    /// </summary>
    [DataMember(Name="supplier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supplier")]
    public string Supplier { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class EpicFbo {\n");
      sb.Append("  Fuel: ").Append(Fuel).Append("\n");
      sb.Append("  FboName: ").Append(FboName).Append("\n");
      sb.Append("  Supplier: ").Append(Supplier).Append("\n");
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
