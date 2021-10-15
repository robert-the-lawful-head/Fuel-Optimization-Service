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
  public class AircraftProfile {
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
    /// Airspeed units:             0 = Unspecified             4 = KMH             8 = Knots             16 = MPH             32 = Mach    * `Unspecified` -   * `KMH` -   * `Knots` -   * `MPH` -   * `Mach` -   
    /// </summary>
    /// <value>Airspeed units:             0 = Unspecified             4 = KMH             8 = Knots             16 = MPH             32 = Mach    * `Unspecified` -   * `KMH` -   * `Knots` -   * `MPH` -   * `Mach` -   </value>
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
    /// Fuel burn units:             1 = Gallons per hour             2 = Pounds per hour             4 = Liters per hour             8 = Kilograms per hour    * `GallonsPerHour` -   * `PoundsPerHour` -   * `LitersPerHour` -   * `KilogramsPerHour` -   
    /// </summary>
    /// <value>Fuel burn units:             1 = Gallons per hour             2 = Pounds per hour             4 = Liters per hour             8 = Kilograms per hour    * `GallonsPerHour` -   * `PoundsPerHour` -   * `LitersPerHour` -   * `KilogramsPerHour` -   </value>
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
    /// Aircraft Performance Profile Types:             0 = None             1 = By Segment             2 = By Hour             4 = By Altitude             8 = SkyPlan    * `None` -   * `BySegment` -   * `ByHour` -   * `ByAltitude` -   * `SkyPlan` -   
    /// </summary>
    /// <value>Aircraft Performance Profile Types:             0 = None             1 = By Segment             2 = By Hour             4 = By Altitude             8 = SkyPlan    * `None` -   * `BySegment` -   * `ByHour` -   * `ByAltitude` -   * `SkyPlan` -   </value>
    [DataMember(Name="performanceProfileType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "performanceProfileType")]
    public int? PerformanceProfileType { get; set; }

    /// <summary>
    /// Volume units:             0 = Gallons             1 = Liters    * `Gallons` -   * `Liters` -   
    /// </summary>
    /// <value>Volume units:             0 = Gallons             1 = Liters    * `Gallons` -   * `Liters` -   </value>
    [DataMember(Name="volumeUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "volumeUnit")]
    public int? VolumeUnit { get; set; }

    /// <summary>
    /// Weight units:             0 = Pounds             1 = Kilograms    * `Pounds` -   * `Kilograms` -   
    /// </summary>
    /// <value>Weight units:             0 = Pounds             1 = Kilograms    * `Pounds` -   * `Kilograms` -   </value>
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
      sb.Append("class AircraftProfile {\n");
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
