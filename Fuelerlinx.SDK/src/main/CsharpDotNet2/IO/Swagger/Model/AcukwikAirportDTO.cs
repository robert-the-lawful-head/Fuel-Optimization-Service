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
  public class AcukwikAirportDTO {
    /// <summary>
    /// Gets or Sets AirportId
    /// </summary>
    [DataMember(Name="airportId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airportId")]
    public int? AirportId { get; set; }

    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets Iata
    /// </summary>
    [DataMember(Name="iata", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "iata")]
    public string Iata { get; set; }

    /// <summary>
    /// Gets or Sets Faa
    /// </summary>
    [DataMember(Name="faa", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "faa")]
    public string Faa { get; set; }

    /// <summary>
    /// Gets or Sets FullAirportName
    /// </summary>
    [DataMember(Name="fullAirportName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fullAirportName")]
    public string FullAirportName { get; set; }

    /// <summary>
    /// Gets or Sets AirportCity
    /// </summary>
    [DataMember(Name="airportCity", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airportCity")]
    public string AirportCity { get; set; }

    /// <summary>
    /// Gets or Sets StateSubdivision
    /// </summary>
    [DataMember(Name="stateSubdivision", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "stateSubdivision")]
    public string StateSubdivision { get; set; }

    /// <summary>
    /// Gets or Sets Country
    /// </summary>
    [DataMember(Name="country", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "country")]
    public string Country { get; set; }

    /// <summary>
    /// Gets or Sets AirportType
    /// </summary>
    [DataMember(Name="airportType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airportType")]
    public string AirportType { get; set; }

    /// <summary>
    /// Gets or Sets DistanceFromCity
    /// </summary>
    [DataMember(Name="distanceFromCity", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "distanceFromCity")]
    public string DistanceFromCity { get; set; }

    /// <summary>
    /// Gets or Sets Latitude
    /// </summary>
    [DataMember(Name="latitude", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "latitude")]
    public string Latitude { get; set; }

    /// <summary>
    /// Gets or Sets Longitude
    /// </summary>
    [DataMember(Name="longitude", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "longitude")]
    public string Longitude { get; set; }

    /// <summary>
    /// Gets or Sets Elevation
    /// </summary>
    [DataMember(Name="elevation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "elevation")]
    public double? Elevation { get; set; }

    /// <summary>
    /// Gets or Sets Variation
    /// </summary>
    [DataMember(Name="variation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "variation")]
    public string Variation { get; set; }

    /// <summary>
    /// Gets or Sets IntlTimeZone
    /// </summary>
    [DataMember(Name="intlTimeZone", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "intlTimeZone")]
    public double? IntlTimeZone { get; set; }

    /// <summary>
    /// Gets or Sets DaylightSavingsYn
    /// </summary>
    [DataMember(Name="daylightSavingsYn", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "daylightSavingsYn")]
    public string DaylightSavingsYn { get; set; }

    /// <summary>
    /// Gets or Sets FuelType
    /// </summary>
    [DataMember(Name="fuelType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelType")]
    public string FuelType { get; set; }

    /// <summary>
    /// Gets or Sets AirportOfEntry
    /// </summary>
    [DataMember(Name="airportOfEntry", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airportOfEntry")]
    public string AirportOfEntry { get; set; }

    /// <summary>
    /// Gets or Sets Customs
    /// </summary>
    [DataMember(Name="customs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "customs")]
    public string Customs { get; set; }

    /// <summary>
    /// Gets or Sets HandlingMandatory
    /// </summary>
    [DataMember(Name="handlingMandatory", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlingMandatory")]
    public string HandlingMandatory { get; set; }

    /// <summary>
    /// Gets or Sets SlotsRequired
    /// </summary>
    [DataMember(Name="slotsRequired", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "slotsRequired")]
    public string SlotsRequired { get; set; }

    /// <summary>
    /// Gets or Sets Open24Hours
    /// </summary>
    [DataMember(Name="open24Hours", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "open24Hours")]
    public string Open24Hours { get; set; }

    /// <summary>
    /// Gets or Sets ControlTowerHours
    /// </summary>
    [DataMember(Name="controlTowerHours", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "controlTowerHours")]
    public string ControlTowerHours { get; set; }

    /// <summary>
    /// Gets or Sets ApproachList
    /// </summary>
    [DataMember(Name="approachList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "approachList")]
    public string ApproachList { get; set; }

    /// <summary>
    /// Gets or Sets PrimaryRunwayId
    /// </summary>
    [DataMember(Name="primaryRunwayId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "primaryRunwayId")]
    public string PrimaryRunwayId { get; set; }

    /// <summary>
    /// Gets or Sets RunwayLength
    /// </summary>
    [DataMember(Name="runwayLength", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "runwayLength")]
    public double? RunwayLength { get; set; }

    /// <summary>
    /// Gets or Sets RunwayWidth
    /// </summary>
    [DataMember(Name="runwayWidth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "runwayWidth")]
    public double? RunwayWidth { get; set; }

    /// <summary>
    /// Gets or Sets Lighting
    /// </summary>
    [DataMember(Name="lighting", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lighting")]
    public string Lighting { get; set; }

    /// <summary>
    /// Gets or Sets AirportNameShort
    /// </summary>
    [DataMember(Name="airportNameShort", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airportNameShort")]
    public string AirportNameShort { get; set; }

    /// <summary>
    /// Gets or Sets IsUnitedStatesAirport
    /// </summary>
    [DataMember(Name="isUnitedStatesAirport", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isUnitedStatesAirport")]
    public bool? IsUnitedStatesAirport { get; set; }

    /// <summary>
    /// Gets or Sets ProperAirportIdentifier
    /// </summary>
    [DataMember(Name="properAirportIdentifier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "properAirportIdentifier")]
    public string ProperAirportIdentifier { get; set; }

    /// <summary>
    /// Gets or Sets FboHandlerDetailsCollection
    /// </summary>
    [DataMember(Name="fboHandlerDetailsCollection", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboHandlerDetailsCollection")]
    public List<AcukwikFboHandlerDetailDTO> FboHandlerDetailsCollection { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AcukwikAirportDTO {\n");
      sb.Append("  AirportId: ").Append(AirportId).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  Iata: ").Append(Iata).Append("\n");
      sb.Append("  Faa: ").Append(Faa).Append("\n");
      sb.Append("  FullAirportName: ").Append(FullAirportName).Append("\n");
      sb.Append("  AirportCity: ").Append(AirportCity).Append("\n");
      sb.Append("  StateSubdivision: ").Append(StateSubdivision).Append("\n");
      sb.Append("  Country: ").Append(Country).Append("\n");
      sb.Append("  AirportType: ").Append(AirportType).Append("\n");
      sb.Append("  DistanceFromCity: ").Append(DistanceFromCity).Append("\n");
      sb.Append("  Latitude: ").Append(Latitude).Append("\n");
      sb.Append("  Longitude: ").Append(Longitude).Append("\n");
      sb.Append("  Elevation: ").Append(Elevation).Append("\n");
      sb.Append("  Variation: ").Append(Variation).Append("\n");
      sb.Append("  IntlTimeZone: ").Append(IntlTimeZone).Append("\n");
      sb.Append("  DaylightSavingsYn: ").Append(DaylightSavingsYn).Append("\n");
      sb.Append("  FuelType: ").Append(FuelType).Append("\n");
      sb.Append("  AirportOfEntry: ").Append(AirportOfEntry).Append("\n");
      sb.Append("  Customs: ").Append(Customs).Append("\n");
      sb.Append("  HandlingMandatory: ").Append(HandlingMandatory).Append("\n");
      sb.Append("  SlotsRequired: ").Append(SlotsRequired).Append("\n");
      sb.Append("  Open24Hours: ").Append(Open24Hours).Append("\n");
      sb.Append("  ControlTowerHours: ").Append(ControlTowerHours).Append("\n");
      sb.Append("  ApproachList: ").Append(ApproachList).Append("\n");
      sb.Append("  PrimaryRunwayId: ").Append(PrimaryRunwayId).Append("\n");
      sb.Append("  RunwayLength: ").Append(RunwayLength).Append("\n");
      sb.Append("  RunwayWidth: ").Append(RunwayWidth).Append("\n");
      sb.Append("  Lighting: ").Append(Lighting).Append("\n");
      sb.Append("  AirportNameShort: ").Append(AirportNameShort).Append("\n");
      sb.Append("  IsUnitedStatesAirport: ").Append(IsUnitedStatesAirport).Append("\n");
      sb.Append("  ProperAirportIdentifier: ").Append(ProperAirportIdentifier).Append("\n");
      sb.Append("  FboHandlerDetailsCollection: ").Append(FboHandlerDetailsCollection).Append("\n");
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
