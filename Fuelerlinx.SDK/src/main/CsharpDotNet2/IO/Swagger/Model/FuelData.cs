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
  public class FuelData {
    /// <summary>
    /// Gets or Sets AddlFuel
    /// </summary>
    [DataMember(Name="addlFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addlFuel")]
    public SegmentItem AddlFuel { get; set; }

    /// <summary>
    /// Gets or Sets AlternateFuel
    /// </summary>
    [DataMember(Name="alternateFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "alternateFuel")]
    public SegmentItem AlternateFuel { get; set; }

    /// <summary>
    /// Gets or Sets ExtraFuel
    /// </summary>
    [DataMember(Name="extraFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "extraFuel")]
    public SegmentItem ExtraFuel { get; set; }

    /// <summary>
    /// Gets or Sets HoldFuel
    /// </summary>
    [DataMember(Name="holdFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "holdFuel")]
    public SegmentItem HoldFuel { get; set; }

    /// <summary>
    /// Gets or Sets RequiredFuel
    /// </summary>
    [DataMember(Name="requiredFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "requiredFuel")]
    public SegmentItem RequiredFuel { get; set; }

    /// <summary>
    /// Gets or Sets ReserveFuel
    /// </summary>
    [DataMember(Name="reserveFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reserveFuel")]
    public SegmentItem ReserveFuel { get; set; }

    /// <summary>
    /// Gets or Sets RouteFuel
    /// </summary>
    [DataMember(Name="routeFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "routeFuel")]
    public SegmentItem RouteFuel { get; set; }

    /// <summary>
    /// Gets or Sets TankeredFuel
    /// </summary>
    [DataMember(Name="tankeredFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tankeredFuel")]
    public SegmentItem TankeredFuel { get; set; }

    /// <summary>
    /// Gets or Sets TaxiFuel
    /// </summary>
    [DataMember(Name="taxiFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "taxiFuel")]
    public SegmentItem TaxiFuel { get; set; }

    /// <summary>
    /// Gets or Sets TaxiFuel2
    /// </summary>
    [DataMember(Name="taxiFuel2", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "taxiFuel2")]
    public SegmentItem TaxiFuel2 { get; set; }

    /// <summary>
    /// Gets or Sets TotalFuel
    /// </summary>
    [DataMember(Name="totalFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "totalFuel")]
    public SegmentItem TotalFuel { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FuelData {\n");
      sb.Append("  AddlFuel: ").Append(AddlFuel).Append("\n");
      sb.Append("  AlternateFuel: ").Append(AlternateFuel).Append("\n");
      sb.Append("  ExtraFuel: ").Append(ExtraFuel).Append("\n");
      sb.Append("  HoldFuel: ").Append(HoldFuel).Append("\n");
      sb.Append("  RequiredFuel: ").Append(RequiredFuel).Append("\n");
      sb.Append("  ReserveFuel: ").Append(ReserveFuel).Append("\n");
      sb.Append("  RouteFuel: ").Append(RouteFuel).Append("\n");
      sb.Append("  TankeredFuel: ").Append(TankeredFuel).Append("\n");
      sb.Append("  TaxiFuel: ").Append(TaxiFuel).Append("\n");
      sb.Append("  TaxiFuel2: ").Append(TaxiFuel2).Append("\n");
      sb.Append("  TotalFuel: ").Append(TotalFuel).Append("\n");
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
