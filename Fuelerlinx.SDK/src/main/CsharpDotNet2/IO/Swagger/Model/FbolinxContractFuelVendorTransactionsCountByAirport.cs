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
  public class FbolinxContractFuelVendorTransactionsCountByAirport {
    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets ContractFuelVendor
    /// </summary>
    [DataMember(Name="contractFuelVendor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "contractFuelVendor")]
    public string ContractFuelVendor { get; set; }

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
      sb.Append("class FbolinxContractFuelVendorTransactionsCountByAirport {\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  ContractFuelVendor: ").Append(ContractFuelVendor).Append("\n");
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
