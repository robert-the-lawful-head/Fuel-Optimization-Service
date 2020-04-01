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
  public class MultiLegOption {
    /// <summary>
    /// Gets or Sets Total
    /// </summary>
    [DataMember(Name="total", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "total")]
    public double? Total { get; set; }

    /// <summary>
    /// Gets or Sets Volume
    /// </summary>
    [DataMember(Name="volume", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "volume")]
    public double? Volume { get; set; }

    /// <summary>
    /// Gets or Sets FuelCapacity
    /// </summary>
    [DataMember(Name="fuelCapacity", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelCapacity")]
    public double? FuelCapacity { get; set; }

    /// <summary>
    /// Gets or Sets Price
    /// </summary>
    [DataMember(Name="price", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "price")]
    public double? Price { get; set; }

    /// <summary>
    /// Gets or Sets Reserve
    /// </summary>
    [DataMember(Name="reserve", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reserve")]
    public double? Reserve { get; set; }

    /// <summary>
    /// Gets or Sets VolumeAlreadyInTank
    /// </summary>
    [DataMember(Name="volumeAlreadyInTank", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "volumeAlreadyInTank")]
    public double? VolumeAlreadyInTank { get; set; }

    /// <summary>
    /// Gets or Sets LegID
    /// </summary>
    [DataMember(Name="legID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legID")]
    public int? LegID { get; set; }

    /// <summary>
    /// Gets or Sets Note
    /// </summary>
    [DataMember(Name="note", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "note")]
    public string Note { get; set; }

    /// <summary>
    /// Gets or Sets FuelMasterID
    /// </summary>
    [DataMember(Name="fuelMasterID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelMasterID")]
    public int? FuelMasterID { get; set; }

    /// <summary>
    /// Gets or Sets LegNumber
    /// </summary>
    [DataMember(Name="legNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legNumber")]
    public int? LegNumber { get; set; }

    /// <summary>
    /// Gets or Sets SelectedFuelOption
    /// </summary>
    [DataMember(Name="selectedFuelOption", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "selectedFuelOption")]
    public FuelPriceOption SelectedFuelOption { get; set; }

    /// <summary>
    /// Gets or Sets RecommendationID
    /// </summary>
    [DataMember(Name="recommendationID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "recommendationID")]
    public int? RecommendationID { get; set; }

    /// <summary>
    /// Gets or Sets RecommendationLegID
    /// </summary>
    [DataMember(Name="recommendationLegID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "recommendationLegID")]
    public long? RecommendationLegID { get; set; }

    /// <summary>
    /// Gets or Sets IsMaxUplift
    /// </summary>
    [DataMember(Name="isMaxUplift", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isMaxUplift")]
    public bool? IsMaxUplift { get; set; }

    /// <summary>
    /// Gets or Sets WeightUnit
    /// </summary>
    [DataMember(Name="weightUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "weightUnit")]
    public Weight WeightUnit { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MultiLegOption {\n");
      sb.Append("  Total: ").Append(Total).Append("\n");
      sb.Append("  Volume: ").Append(Volume).Append("\n");
      sb.Append("  FuelCapacity: ").Append(FuelCapacity).Append("\n");
      sb.Append("  Price: ").Append(Price).Append("\n");
      sb.Append("  Reserve: ").Append(Reserve).Append("\n");
      sb.Append("  VolumeAlreadyInTank: ").Append(VolumeAlreadyInTank).Append("\n");
      sb.Append("  LegID: ").Append(LegID).Append("\n");
      sb.Append("  Note: ").Append(Note).Append("\n");
      sb.Append("  FuelMasterID: ").Append(FuelMasterID).Append("\n");
      sb.Append("  LegNumber: ").Append(LegNumber).Append("\n");
      sb.Append("  SelectedFuelOption: ").Append(SelectedFuelOption).Append("\n");
      sb.Append("  RecommendationID: ").Append(RecommendationID).Append("\n");
      sb.Append("  RecommendationLegID: ").Append(RecommendationLegID).Append("\n");
      sb.Append("  IsMaxUplift: ").Append(IsMaxUplift).Append("\n");
      sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
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
