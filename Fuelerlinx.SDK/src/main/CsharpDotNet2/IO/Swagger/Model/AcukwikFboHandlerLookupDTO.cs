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
  public class AcukwikFboHandlerLookupDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets AcukwikFboHandlerId
    /// </summary>
    [DataMember(Name="acukwikFboHandlerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "acukwikFboHandlerId")]
    public int? AcukwikFboHandlerId { get; set; }

    /// <summary>
    /// Gets or Sets AcukwikFboHandlerName
    /// </summary>
    [DataMember(Name="acukwikFboHandlerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "acukwikFboHandlerName")]
    public string AcukwikFboHandlerName { get; set; }

    /// <summary>
    /// Gets or Sets FboHandlerName
    /// </summary>
    [DataMember(Name="fboHandlerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboHandlerName")]
    public string FboHandlerName { get; set; }

    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets AcukwikAirportId
    /// </summary>
    [DataMember(Name="acukwikAirportId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "acukwikAirportId")]
    public int? AcukwikAirportId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AcukwikFboHandlerLookupDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  AcukwikFboHandlerId: ").Append(AcukwikFboHandlerId).Append("\n");
      sb.Append("  AcukwikFboHandlerName: ").Append(AcukwikFboHandlerName).Append("\n");
      sb.Append("  FboHandlerName: ").Append(FboHandlerName).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  AcukwikAirportId: ").Append(AcukwikAirportId).Append("\n");
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
