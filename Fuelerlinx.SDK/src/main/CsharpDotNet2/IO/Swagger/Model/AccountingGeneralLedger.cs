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
  public class AccountingGeneralLedger {
    /// <summary>
    /// Gets or Sets GeneralLedgerCode
    /// </summary>
    [DataMember(Name="generalLedgerCode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "generalLedgerCode")]
    public string GeneralLedgerCode { get; set; }

    /// <summary>
    /// Gets or Sets GeneralLedgerItemName
    /// </summary>
    [DataMember(Name="generalLedgerItemName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "generalLedgerItemName")]
    public string GeneralLedgerItemName { get; set; }

    /// <summary>
    /// Gets or Sets GeneralLedgerItemCategory
    /// </summary>
    [DataMember(Name="generalLedgerItemCategory", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "generalLedgerItemCategory")]
    public string GeneralLedgerItemCategory { get; set; }

    /// <summary>
    /// Gets or Sets GeneralLedgerLabelValue
    /// </summary>
    [DataMember(Name="generalLedgerLabelValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "generalLedgerLabelValue")]
    public string GeneralLedgerLabelValue { get; set; }

    /// <summary>
    /// Gets or Sets Value
    /// </summary>
    [DataMember(Name="value", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "value")]
    public string Value { get; set; }

    /// <summary>
    /// Gets or Sets Label
    /// </summary>
    [DataMember(Name="label", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "label")]
    public string Label { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AccountingGeneralLedger {\n");
      sb.Append("  GeneralLedgerCode: ").Append(GeneralLedgerCode).Append("\n");
      sb.Append("  GeneralLedgerItemName: ").Append(GeneralLedgerItemName).Append("\n");
      sb.Append("  GeneralLedgerItemCategory: ").Append(GeneralLedgerItemCategory).Append("\n");
      sb.Append("  GeneralLedgerLabelValue: ").Append(GeneralLedgerLabelValue).Append("\n");
      sb.Append("  Value: ").Append(Value).Append("\n");
      sb.Append("  Label: ").Append(Label).Append("\n");
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
