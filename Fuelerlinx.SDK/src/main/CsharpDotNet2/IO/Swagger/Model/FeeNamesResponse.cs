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
  public class FeeNamesResponse {
    /// <summary>
    /// Gets or Sets Oid
    /// </summary>
    [DataMember(Name="oid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "oid")]
    public int? Oid { get; set; }

    /// <summary>
    /// Gets or Sets Item
    /// </summary>
    [DataMember(Name="item", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "item")]
    public string Item { get; set; }

    /// <summary>
    /// Gets or Sets AccountingCategory
    /// </summary>
    [DataMember(Name="accountingCategory", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "accountingCategory")]
    public string AccountingCategory { get; set; }

    /// <summary>
    /// Gets or Sets AccountingItemName
    /// </summary>
    [DataMember(Name="accountingItemName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "accountingItemName")]
    public string AccountingItemName { get; set; }

    /// <summary>
    /// Gets or Sets AccountingItemCode
    /// </summary>
    [DataMember(Name="accountingItemCode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "accountingItemCode")]
    public string AccountingItemCode { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FeeNamesResponse {\n");
      sb.Append("  Oid: ").Append(Oid).Append("\n");
      sb.Append("  Item: ").Append(Item).Append("\n");
      sb.Append("  AccountingCategory: ").Append(AccountingCategory).Append("\n");
      sb.Append("  AccountingItemName: ").Append(AccountingItemName).Append("\n");
      sb.Append("  AccountingItemCode: ").Append(AccountingItemCode).Append("\n");
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
