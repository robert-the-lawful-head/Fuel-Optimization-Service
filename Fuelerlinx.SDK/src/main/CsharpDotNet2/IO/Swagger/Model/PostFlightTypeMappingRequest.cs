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
  public class PostFlightTypeMappingRequest {
    /// <summary>
    /// Gets or Sets FlightTypeName
    /// </summary>
    [DataMember(Name="flightTypeName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightTypeName")]
    public string FlightTypeName { get; set; }

    /// <summary>
    /// NotSet = 0,             Private = 1,             Commercial = 2    * `NotSet` - Not Set  * `Private` - Private  * `Commercial` - Commercial  * `All` - All  
    /// </summary>
    /// <value>NotSet = 0,             Private = 1,             Commercial = 2    * `NotSet` - Not Set  * `Private` - Private  * `Commercial` - Commercial  * `All` - All  </value>
    [DataMember(Name="flightTypeClassification", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightTypeClassification")]
    public int? FlightTypeClassification { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostFlightTypeMappingRequest {\n");
      sb.Append("  FlightTypeName: ").Append(FlightTypeName).Append("\n");
      sb.Append("  FlightTypeClassification: ").Append(FlightTypeClassification).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
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
