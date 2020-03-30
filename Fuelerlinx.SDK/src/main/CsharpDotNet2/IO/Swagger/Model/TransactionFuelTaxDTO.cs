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
  public class TransactionFuelTaxDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets TransactionId
    /// </summary>
    [DataMember(Name="transactionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionId")]
    public int? TransactionId { get; set; }

    /// <summary>
    /// Gets or Sets TaxDescription
    /// </summary>
    [DataMember(Name="taxDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "taxDescription")]
    public string TaxDescription { get; set; }

    /// <summary>
    /// Gets or Sets TaxAmountPerGallon
    /// </summary>
    [DataMember(Name="taxAmountPerGallon", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "taxAmountPerGallon")]
    public double? TaxAmountPerGallon { get; set; }

    /// <summary>
    /// Gets or Sets TaxAmountFlat
    /// </summary>
    [DataMember(Name="taxAmountFlat", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "taxAmountFlat")]
    public double? TaxAmountFlat { get; set; }

    /// <summary>
    /// Gets or Sets TaxPercentage
    /// </summary>
    [DataMember(Name="taxPercentage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "taxPercentage")]
    public double? TaxPercentage { get; set; }

    /// <summary>
    /// Gets or Sets Omitted
    /// </summary>
    [DataMember(Name="omitted", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "omitted")]
    public bool? Omitted { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TransactionFuelTaxDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
      sb.Append("  TaxDescription: ").Append(TaxDescription).Append("\n");
      sb.Append("  TaxAmountPerGallon: ").Append(TaxAmountPerGallon).Append("\n");
      sb.Append("  TaxAmountFlat: ").Append(TaxAmountFlat).Append("\n");
      sb.Append("  TaxPercentage: ").Append(TaxPercentage).Append("\n");
      sb.Append("  Omitted: ").Append(Omitted).Append("\n");
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
