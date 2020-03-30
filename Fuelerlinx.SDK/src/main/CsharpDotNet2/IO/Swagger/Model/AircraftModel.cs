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
  public class AircraftModel {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Make
    /// </summary>
    [DataMember(Name="make", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "make")]
    public string Make { get; set; }

    /// <summary>
    /// Gets or Sets SerialNumbers
    /// </summary>
    [DataMember(Name="serialNumbers", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "serialNumbers")]
    public string SerialNumbers { get; set; }

    /// <summary>
    /// Gets or Sets DisplayName
    /// </summary>
    [DataMember(Name="displayName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "displayName")]
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or Sets AircraftID
    /// </summary>
    [DataMember(Name="aircraftID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aircraftID")]
    public int? AircraftID { get; set; }

    /// <summary>
    /// Gets or Sets AircraftType
    /// </summary>
    [DataMember(Name="aircraftType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aircraftType")]
    public string AircraftType { get; set; }

    /// <summary>
    /// Gets or Sets Maker
    /// </summary>
    [DataMember(Name="maker", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maker")]
    public string Maker { get; set; }

    /// <summary>
    /// Gets or Sets Model
    /// </summary>
    [DataMember(Name="model", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "model")]
    public string Model { get; set; }

    /// <summary>
    /// Gets or Sets ModelID
    /// </summary>
    [DataMember(Name="modelID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "modelID")]
    public int? ModelID { get; set; }

    /// <summary>
    /// Gets or Sets TailNumber
    /// </summary>
    [DataMember(Name="tailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumber")]
    public string TailNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="airspeedUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airspeedUnit")]
    public int? AirspeedUnit { get; set; }

    /// <summary>
    /// Gets or Sets EmptyWeight
    /// </summary>
    [DataMember(Name="emptyWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "emptyWeight")]
    public double? EmptyWeight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="fuelBurnUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelBurnUnit")]
    public int? FuelBurnUnit { get; set; }

    /// <summary>
    /// Gets or Sets FuelCapacity
    /// </summary>
    [DataMember(Name="fuelCapacity", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelCapacity")]
    public double? FuelCapacity { get; set; }

    /// <summary>
    /// Gets or Sets FuelReserve
    /// </summary>
    [DataMember(Name="fuelReserve", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelReserve")]
    public double? FuelReserve { get; set; }

    /// <summary>
    /// Gets or Sets FuelWeight
    /// </summary>
    [DataMember(Name="fuelWeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelWeight")]
    public double? FuelWeight { get; set; }

    /// <summary>
    /// Gets or Sets MaxAltitude
    /// </summary>
    [DataMember(Name="maxAltitude", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxAltitude")]
    public int? MaxAltitude { get; set; }

    /// <summary>
    /// Gets or Sets MinRunwayLength
    /// </summary>
    [DataMember(Name="minRunwayLength", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minRunwayLength")]
    public int? MinRunwayLength { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="performanceProfileType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "performanceProfileType")]
    public int? PerformanceProfileType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="volumeUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "volumeUnit")]
    public int? VolumeUnit { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="weightUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "weightUnit")]
    public int? WeightUnit { get; set; }

    /// <summary>
    /// Gets or Sets SkyPlanMachEnabled
    /// </summary>
    [DataMember(Name="skyPlanMachEnabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "skyPlanMachEnabled")]
    public bool? SkyPlanMachEnabled { get; set; }

    /// <summary>
    /// Gets or Sets SkyPlanMachMin
    /// </summary>
    [DataMember(Name="skyPlanMachMin", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "skyPlanMachMin")]
    public int? SkyPlanMachMin { get; set; }

    /// <summary>
    /// Gets or Sets SkyPlanMachMax
    /// </summary>
    [DataMember(Name="skyPlanMachMax", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "skyPlanMachMax")]
    public int? SkyPlanMachMax { get; set; }

    /// <summary>
    /// Gets or Sets Performance
    /// </summary>
    [DataMember(Name="performance", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "performance")]
    public List<AircraftPerformanceProfile> Performance { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AircraftModel {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Make: ").Append(Make).Append("\n");
      sb.Append("  SerialNumbers: ").Append(SerialNumbers).Append("\n");
      sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
      sb.Append("  AircraftID: ").Append(AircraftID).Append("\n");
      sb.Append("  AircraftType: ").Append(AircraftType).Append("\n");
      sb.Append("  Maker: ").Append(Maker).Append("\n");
      sb.Append("  Model: ").Append(Model).Append("\n");
      sb.Append("  ModelID: ").Append(ModelID).Append("\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
      sb.Append("  AirspeedUnit: ").Append(AirspeedUnit).Append("\n");
      sb.Append("  EmptyWeight: ").Append(EmptyWeight).Append("\n");
      sb.Append("  FuelBurnUnit: ").Append(FuelBurnUnit).Append("\n");
      sb.Append("  FuelCapacity: ").Append(FuelCapacity).Append("\n");
      sb.Append("  FuelReserve: ").Append(FuelReserve).Append("\n");
      sb.Append("  FuelWeight: ").Append(FuelWeight).Append("\n");
      sb.Append("  MaxAltitude: ").Append(MaxAltitude).Append("\n");
      sb.Append("  MinRunwayLength: ").Append(MinRunwayLength).Append("\n");
      sb.Append("  PerformanceProfileType: ").Append(PerformanceProfileType).Append("\n");
      sb.Append("  VolumeUnit: ").Append(VolumeUnit).Append("\n");
      sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
      sb.Append("  SkyPlanMachEnabled: ").Append(SkyPlanMachEnabled).Append("\n");
      sb.Append("  SkyPlanMachMin: ").Append(SkyPlanMachMin).Append("\n");
      sb.Append("  SkyPlanMachMax: ").Append(SkyPlanMachMax).Append("\n");
      sb.Append("  Performance: ").Append(Performance).Append("\n");
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
