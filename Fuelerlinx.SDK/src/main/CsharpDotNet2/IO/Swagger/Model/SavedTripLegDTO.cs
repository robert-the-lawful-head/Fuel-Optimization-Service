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
  public class SavedTripLegDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets FuelAmount
    /// </summary>
    [DataMember(Name="fuelAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelAmount")]
    public double? FuelAmount { get; set; }

    /// <summary>
    /// Gets or Sets CaptRequest
    /// </summary>
    [DataMember(Name="captRequest", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "captRequest")]
    public bool? CaptRequest { get; set; }

    /// <summary>
    /// Gets or Sets IncludeInDispatch
    /// </summary>
    [DataMember(Name="includeInDispatch", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "includeInDispatch")]
    public bool? IncludeInDispatch { get; set; }

    /// <summary>
    /// Gets or Sets Aog
    /// </summary>
    [DataMember(Name="aog", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aog")]
    public bool? Aog { get; set; }

    /// <summary>
    /// Gets or Sets Eta
    /// </summary>
    [DataMember(Name="eta", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "eta")]
    public DateTime? Eta { get; set; }

    /// <summary>
    /// Gets or Sets Etd
    /// </summary>
    [DataMember(Name="etd", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "etd")]
    public DateTime? Etd { get; set; }

    /// <summary>
    /// Gets or Sets TripId
    /// </summary>
    [DataMember(Name="tripId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripId")]
    public int? TripId { get; set; }

    /// <summary>
    /// Gets or Sets LegStatusJson
    /// </summary>
    [DataMember(Name="legStatusJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legStatusJson")]
    public string LegStatusJson { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SavedTripLegDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  FuelAmount: ").Append(FuelAmount).Append("\n");
      sb.Append("  CaptRequest: ").Append(CaptRequest).Append("\n");
      sb.Append("  IncludeInDispatch: ").Append(IncludeInDispatch).Append("\n");
      sb.Append("  Aog: ").Append(Aog).Append("\n");
      sb.Append("  Eta: ").Append(Eta).Append("\n");
      sb.Append("  Etd: ").Append(Etd).Append("\n");
      sb.Append("  TripId: ").Append(TripId).Append("\n");
      sb.Append("  LegStatusJson: ").Append(LegStatusJson).Append("\n");
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
