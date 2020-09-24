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
  public class FbolinxCustomerTransactionsCountAtAirport {
    /// <summary>
    /// Gets or Sets FuelerLinxCustomerId
    /// </summary>
    [DataMember(Name="fuelerLinxCustomerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerLinxCustomerId")]
    public int? FuelerLinxCustomerId { get; set; }

    /// <summary>
    /// Gets or Sets TransactionsCount
    /// </summary>
    [DataMember(Name="transactionsCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionsCount")]
    public int? TransactionsCount { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FbolinxCustomerTransactionsCountAtAirport {\n");
      sb.Append("  FuelerLinxCustomerId: ").Append(FuelerLinxCustomerId).Append("\n");
      sb.Append("  TransactionsCount: ").Append(TransactionsCount).Append("\n");
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
