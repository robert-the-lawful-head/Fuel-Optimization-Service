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
  public class FuelBurnProfile {
    /// <summary>
    /// Gets or Sets Items
    /// </summary>
    [DataMember(Name="items", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "items")]
    public List<ResultItem> Items { get; set; }

    /// <summary>
    /// Gets or Sets IsTankering
    /// </summary>
    [DataMember(Name="isTankering", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isTankering")]
    public bool? IsTankering { get; set; }

    /// <summary>
    /// Gets or Sets WeightUnit
    /// </summary>
    [DataMember(Name="weightUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "weightUnit")]
    public Weight WeightUnit { get; set; }

    /// <summary>
    /// Gets or Sets FuelUplifted
    /// </summary>
    [DataMember(Name="fuelUplifted", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelUplifted")]
    public double? FuelUplifted { get; set; }

    /// <summary>
    /// Gets or Sets EstFuelBurn
    /// </summary>
    [DataMember(Name="estFuelBurn", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "estFuelBurn")]
    public double? EstFuelBurn { get; set; }

    /// <summary>
    /// Gets or Sets TaxiIncl
    /// </summary>
    [DataMember(Name="taxiIncl", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "taxiIncl")]
    public double? TaxiIncl { get; set; }

    /// <summary>
    /// Gets or Sets ExtraFuelUplifted
    /// </summary>
    [DataMember(Name="extraFuelUplifted", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "extraFuelUplifted")]
    public double? ExtraFuelUplifted { get; set; }

    /// <summary>
    /// Gets or Sets ExtraFuel
    /// </summary>
    [DataMember(Name="extraFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "extraFuel")]
    public double? ExtraFuel { get; set; }

    /// <summary>
    /// Gets or Sets RampFuel
    /// </summary>
    [DataMember(Name="rampFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFuel")]
    public double? RampFuel { get; set; }

    /// <summary>
    /// Gets or Sets MaxAllowed
    /// </summary>
    [DataMember(Name="maxAllowed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxAllowed")]
    public double? MaxAllowed { get; set; }

    /// <summary>
    /// Gets or Sets EstimatedFuelAtDestination
    /// </summary>
    [DataMember(Name="estimatedFuelAtDestination", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "estimatedFuelAtDestination")]
    public double? EstimatedFuelAtDestination { get; set; }

    /// <summary>
    /// Gets or Sets ReserveFuelIncl
    /// </summary>
    [DataMember(Name="reserveFuelIncl", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reserveFuelIncl")]
    public double? ReserveFuelIncl { get; set; }

    /// <summary>
    /// Gets or Sets AlternateFuelIncl
    /// </summary>
    [DataMember(Name="alternateFuelIncl", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "alternateFuelIncl")]
    public double? AlternateFuelIncl { get; set; }

    /// <summary>
    /// Gets or Sets HoldingFuelIncl
    /// </summary>
    [DataMember(Name="holdingFuelIncl", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "holdingFuelIncl")]
    public double? HoldingFuelIncl { get; set; }

    /// <summary>
    /// Gets or Sets LegNumber
    /// </summary>
    [DataMember(Name="legNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legNumber")]
    public int? LegNumber { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FuelBurnProfile {\n");
      sb.Append("  Items: ").Append(Items).Append("\n");
      sb.Append("  IsTankering: ").Append(IsTankering).Append("\n");
      sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
      sb.Append("  FuelUplifted: ").Append(FuelUplifted).Append("\n");
      sb.Append("  EstFuelBurn: ").Append(EstFuelBurn).Append("\n");
      sb.Append("  TaxiIncl: ").Append(TaxiIncl).Append("\n");
      sb.Append("  ExtraFuelUplifted: ").Append(ExtraFuelUplifted).Append("\n");
      sb.Append("  ExtraFuel: ").Append(ExtraFuel).Append("\n");
      sb.Append("  RampFuel: ").Append(RampFuel).Append("\n");
      sb.Append("  MaxAllowed: ").Append(MaxAllowed).Append("\n");
      sb.Append("  EstimatedFuelAtDestination: ").Append(EstimatedFuelAtDestination).Append("\n");
      sb.Append("  ReserveFuelIncl: ").Append(ReserveFuelIncl).Append("\n");
      sb.Append("  AlternateFuelIncl: ").Append(AlternateFuelIncl).Append("\n");
      sb.Append("  HoldingFuelIncl: ").Append(HoldingFuelIncl).Append("\n");
      sb.Append("  LegNumber: ").Append(LegNumber).Append("\n");
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
