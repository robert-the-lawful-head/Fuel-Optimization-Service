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
  public class IFlightPlannerRouteDetailsCalculation {
    /// <summary>
    /// Gets or Sets TotalFuelWhenTankering
    /// </summary>
    [DataMember(Name="totalFuelWhenTankering", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "totalFuelWhenTankering")]
    public double? TotalFuelWhenTankering { get; set; }

    /// <summary>
    /// Gets or Sets DepartureDate
    /// </summary>
    [DataMember(Name="departureDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureDate")]
    public int? DepartureDate { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalDate
    /// </summary>
    [DataMember(Name="arrivalDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalDate")]
    public int? ArrivalDate { get; set; }

    /// <summary>
    /// Gets or Sets From
    /// </summary>
    [DataMember(Name="from", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "from")]
    public string From { get; set; }

    /// <summary>
    /// Gets or Sets To
    /// </summary>
    [DataMember(Name="to", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "to")]
    public string To { get; set; }

    /// <summary>
    /// Gets or Sets Alternate
    /// </summary>
    [DataMember(Name="alternate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "alternate")]
    public string Alternate { get; set; }

    /// <summary>
    /// Gets or Sets FuelReserve
    /// </summary>
    [DataMember(Name="fuelReserve", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelReserve")]
    public double? FuelReserve { get; set; }

    /// <summary>
    /// Gets or Sets Aircraft
    /// </summary>
    [DataMember(Name="aircraft", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aircraft")]
    public RouteDetailsCalculationAircraftData Aircraft { get; set; }

    /// <summary>
    /// Gets or Sets Airspeed
    /// </summary>
    [DataMember(Name="airspeed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airspeed")]
    public string Airspeed { get; set; }

    /// <summary>
    /// Gets or Sets Altitudes
    /// </summary>
    [DataMember(Name="altitudes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "altitudes")]
    public string Altitudes { get; set; }

    /// <summary>
    /// Gets or Sets TotalFuelOnBoard
    /// </summary>
    [DataMember(Name="totalFuelOnBoard", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "totalFuelOnBoard")]
    public double? TotalFuelOnBoard { get; set; }

    /// <summary>
    /// Gets or Sets Distance
    /// </summary>
    [DataMember(Name="distance", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "distance")]
    public double? Distance { get; set; }

    /// <summary>
    /// Gets or Sets Fuel
    /// </summary>
    [DataMember(Name="fuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuel")]
    public double? Fuel { get; set; }

    /// <summary>
    /// Gets or Sets PayloadBurnRate
    /// </summary>
    [DataMember(Name="payloadBurnRate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "payloadBurnRate")]
    public double? PayloadBurnRate { get; set; }

    /// <summary>
    /// Gets or Sets Time
    /// </summary>
    [DataMember(Name="time", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "time")]
    public int? Time { get; set; }

    /// <summary>
    /// Gets or Sets RouteText
    /// </summary>
    [DataMember(Name="routeText", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "routeText")]
    public string RouteText { get; set; }

    /// <summary>
    /// Gets or Sets NavigationLog
    /// </summary>
    [DataMember(Name="navigationLog", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "navigationLog")]
    public NavigationLog NavigationLog { get; set; }

    /// <summary>
    /// Gets or Sets AirspeedKTAS
    /// </summary>
    [DataMember(Name="airspeedKTAS", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airspeedKTAS")]
    public string AirspeedKTAS { get; set; }

    /// <summary>
    /// Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  
    /// </summary>
    /// <value>Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  </value>
    [DataMember(Name="weightUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "weightUnit")]
    public int? WeightUnit { get; set; }

    /// <summary>
    /// Gets or Sets WeightUnitDescription
    /// </summary>
    [DataMember(Name="weightUnitDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "weightUnitDescription")]
    public string WeightUnitDescription { get; set; }

    /// <summary>
    /// Time units:             0 = Seconds             1 = Minutes             2 = Hours    * `Seconds` - Seconds  * `Minutes` - Minutes  * `Hours` - Hours  
    /// </summary>
    /// <value>Time units:             0 = Seconds             1 = Minutes             2 = Hours    * `Seconds` - Seconds  * `Minutes` - Minutes  * `Hours` - Hours  </value>
    [DataMember(Name="timeUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timeUnit")]
    public int? TimeUnit { get; set; }

    /// <summary>
    /// Gets or Sets TimeUnitDescription
    /// </summary>
    [DataMember(Name="timeUnitDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timeUnitDescription")]
    public string TimeUnitDescription { get; set; }

    /// <summary>
    /// Speed units:             0 = Knots             1 = MPH             2 = KMH             3 = Mach    * `Knots` - Knots  * `MPH` - MPH  * `KMH` - KMH  * `Mach` - Mach  
    /// </summary>
    /// <value>Speed units:             0 = Knots             1 = MPH             2 = KMH             3 = Mach    * `Knots` - Knots  * `MPH` - MPH  * `KMH` - KMH  * `Mach` - Mach  </value>
    [DataMember(Name="speedUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "speedUnit")]
    public int? SpeedUnit { get; set; }

    /// <summary>
    /// Gets or Sets SpeedUnitDescription
    /// </summary>
    [DataMember(Name="speedUnitDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "speedUnitDescription")]
    public string SpeedUnitDescription { get; set; }

    /// <summary>
    /// Distance units:             0 = Feet             1 = Kilometer             2 = Meter             3 = Nautical Mile    * `Feet` - Feet  * `Kilometer` - Kilometer  * `Meter` - Meter  * `NauticalMile` - Nautical Mile  
    /// </summary>
    /// <value>Distance units:             0 = Feet             1 = Kilometer             2 = Meter             3 = Nautical Mile    * `Feet` - Feet  * `Kilometer` - Kilometer  * `Meter` - Meter  * `NauticalMile` - Nautical Mile  </value>
    [DataMember(Name="distanceUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "distanceUnit")]
    public int? DistanceUnit { get; set; }

    /// <summary>
    /// Gets or Sets DistanceUnitDescription
    /// </summary>
    [DataMember(Name="distanceUnitDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "distanceUnitDescription")]
    public string DistanceUnitDescription { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class IFlightPlannerRouteDetailsCalculation {\n");
      sb.Append("  TotalFuelWhenTankering: ").Append(TotalFuelWhenTankering).Append("\n");
      sb.Append("  DepartureDate: ").Append(DepartureDate).Append("\n");
      sb.Append("  ArrivalDate: ").Append(ArrivalDate).Append("\n");
      sb.Append("  From: ").Append(From).Append("\n");
      sb.Append("  To: ").Append(To).Append("\n");
      sb.Append("  Alternate: ").Append(Alternate).Append("\n");
      sb.Append("  FuelReserve: ").Append(FuelReserve).Append("\n");
      sb.Append("  Aircraft: ").Append(Aircraft).Append("\n");
      sb.Append("  Airspeed: ").Append(Airspeed).Append("\n");
      sb.Append("  Altitudes: ").Append(Altitudes).Append("\n");
      sb.Append("  TotalFuelOnBoard: ").Append(TotalFuelOnBoard).Append("\n");
      sb.Append("  Distance: ").Append(Distance).Append("\n");
      sb.Append("  Fuel: ").Append(Fuel).Append("\n");
      sb.Append("  PayloadBurnRate: ").Append(PayloadBurnRate).Append("\n");
      sb.Append("  Time: ").Append(Time).Append("\n");
      sb.Append("  RouteText: ").Append(RouteText).Append("\n");
      sb.Append("  NavigationLog: ").Append(NavigationLog).Append("\n");
      sb.Append("  AirspeedKTAS: ").Append(AirspeedKTAS).Append("\n");
      sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
      sb.Append("  WeightUnitDescription: ").Append(WeightUnitDescription).Append("\n");
      sb.Append("  TimeUnit: ").Append(TimeUnit).Append("\n");
      sb.Append("  TimeUnitDescription: ").Append(TimeUnitDescription).Append("\n");
      sb.Append("  SpeedUnit: ").Append(SpeedUnit).Append("\n");
      sb.Append("  SpeedUnitDescription: ").Append(SpeedUnitDescription).Append("\n");
      sb.Append("  DistanceUnit: ").Append(DistanceUnit).Append("\n");
      sb.Append("  DistanceUnitDescription: ").Append(DistanceUnitDescription).Append("\n");
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
