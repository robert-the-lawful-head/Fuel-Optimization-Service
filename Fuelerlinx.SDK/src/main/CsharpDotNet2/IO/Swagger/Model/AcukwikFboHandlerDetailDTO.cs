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
  public class AcukwikFboHandlerDetailDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets AirportId
    /// </summary>
    [DataMember(Name="airportId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airportId")]
    public int? AirportId { get; set; }

    /// <summary>
    /// Gets or Sets HandlerId
    /// </summary>
    [DataMember(Name="handlerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlerId")]
    public int? HandlerId { get; set; }

    /// <summary>
    /// Gets or Sets HandlerLongName
    /// </summary>
    [DataMember(Name="handlerLongName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlerLongName")]
    public string HandlerLongName { get; set; }

    /// <summary>
    /// Gets or Sets HandlerType
    /// </summary>
    [DataMember(Name="handlerType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlerType")]
    public string HandlerType { get; set; }

    /// <summary>
    /// Gets or Sets HandlerTelephone
    /// </summary>
    [DataMember(Name="handlerTelephone", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlerTelephone")]
    public string HandlerTelephone { get; set; }

    /// <summary>
    /// Gets or Sets HandlerFax
    /// </summary>
    [DataMember(Name="handlerFax", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlerFax")]
    public string HandlerFax { get; set; }

    /// <summary>
    /// Gets or Sets HandlerTollFree
    /// </summary>
    [DataMember(Name="handlerTollFree", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlerTollFree")]
    public string HandlerTollFree { get; set; }

    /// <summary>
    /// Gets or Sets HandlerFreq
    /// </summary>
    [DataMember(Name="handlerFreq", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlerFreq")]
    public double? HandlerFreq { get; set; }

    /// <summary>
    /// Gets or Sets HandlerFuelBrand
    /// </summary>
    [DataMember(Name="handlerFuelBrand", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlerFuelBrand")]
    public string HandlerFuelBrand { get; set; }

    /// <summary>
    /// Gets or Sets HandlerFuelBrand2
    /// </summary>
    [DataMember(Name="handlerFuelBrand2", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlerFuelBrand2")]
    public string HandlerFuelBrand2 { get; set; }

    /// <summary>
    /// Gets or Sets HandlerFuelSupply
    /// </summary>
    [DataMember(Name="handlerFuelSupply", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlerFuelSupply")]
    public string HandlerFuelSupply { get; set; }

    /// <summary>
    /// Gets or Sets HandlerLocationOnField
    /// </summary>
    [DataMember(Name="handlerLocationOnField", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "handlerLocationOnField")]
    public string HandlerLocationOnField { get; set; }

    /// <summary>
    /// Gets or Sets MultiService
    /// </summary>
    [DataMember(Name="multiService", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "multiService")]
    public string MultiService { get; set; }

    /// <summary>
    /// Gets or Sets Avcard
    /// </summary>
    [DataMember(Name="avcard", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "avcard")]
    public string Avcard { get; set; }

    /// <summary>
    /// Gets or Sets AcukwikId
    /// </summary>
    [DataMember(Name="acukwikId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "acukwikId")]
    public double? AcukwikId { get; set; }

    /// <summary>
    /// Gets or Sets IsFbo
    /// </summary>
    [DataMember(Name="isFbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isFbo")]
    public bool? IsFbo { get; set; }

    /// <summary>
    /// Gets or Sets Airport
    /// </summary>
    [DataMember(Name="airport", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airport")]
    public AcukwikAirportDTO Airport { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AcukwikFboHandlerDetailDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  AirportId: ").Append(AirportId).Append("\n");
      sb.Append("  HandlerId: ").Append(HandlerId).Append("\n");
      sb.Append("  HandlerLongName: ").Append(HandlerLongName).Append("\n");
      sb.Append("  HandlerType: ").Append(HandlerType).Append("\n");
      sb.Append("  HandlerTelephone: ").Append(HandlerTelephone).Append("\n");
      sb.Append("  HandlerFax: ").Append(HandlerFax).Append("\n");
      sb.Append("  HandlerTollFree: ").Append(HandlerTollFree).Append("\n");
      sb.Append("  HandlerFreq: ").Append(HandlerFreq).Append("\n");
      sb.Append("  HandlerFuelBrand: ").Append(HandlerFuelBrand).Append("\n");
      sb.Append("  HandlerFuelBrand2: ").Append(HandlerFuelBrand2).Append("\n");
      sb.Append("  HandlerFuelSupply: ").Append(HandlerFuelSupply).Append("\n");
      sb.Append("  HandlerLocationOnField: ").Append(HandlerLocationOnField).Append("\n");
      sb.Append("  MultiService: ").Append(MultiService).Append("\n");
      sb.Append("  Avcard: ").Append(Avcard).Append("\n");
      sb.Append("  AcukwikId: ").Append(AcukwikId).Append("\n");
      sb.Append("  IsFbo: ").Append(IsFbo).Append("\n");
      sb.Append("  Airport: ").Append(Airport).Append("\n");
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
