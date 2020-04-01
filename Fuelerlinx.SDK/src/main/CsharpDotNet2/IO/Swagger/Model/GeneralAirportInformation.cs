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
  public class GeneralAirportInformation {
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
    /// Gets or Sets ApproachList
    /// </summary>
    [DataMember(Name="approachList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "approachList")]
    public string ApproachList { get; set; }

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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class GeneralAirportInformation {\n");
      sb.Append("  AirportId: ").Append(AirportId).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  Iata: ").Append(Iata).Append("\n");
      sb.Append("  Faa: ").Append(Faa).Append("\n");
      sb.Append("  FullAirportName: ").Append(FullAirportName).Append("\n");
      sb.Append("  AirportCity: ").Append(AirportCity).Append("\n");
      sb.Append("  StateSubdivision: ").Append(StateSubdivision).Append("\n");
      sb.Append("  Country: ").Append(Country).Append("\n");
      sb.Append("  ApproachList: ").Append(ApproachList).Append("\n");
      sb.Append("  RunwayLength: ").Append(RunwayLength).Append("\n");
      sb.Append("  RunwayWidth: ").Append(RunwayWidth).Append("\n");
      sb.Append("  AirportNameShort: ").Append(AirportNameShort).Append("\n");
      sb.Append("  IsUnitedStatesAirport: ").Append(IsUnitedStatesAirport).Append("\n");
      sb.Append("  ProperAirportIdentifier: ").Append(ProperAirportIdentifier).Append("\n");
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
