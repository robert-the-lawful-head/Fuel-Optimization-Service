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
  public class FboAirport {
    /// <summary>
    /// Gets or Sets Oid
    /// </summary>
    [DataMember(Name="oid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "oid")]
    public int? Oid { get; set; }

    /// <summary>
    /// Gets or Sets Iata
    /// </summary>
    [DataMember(Name="iata", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "iata")]
    public string Iata { get; set; }

    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets Fboid
    /// </summary>
    [DataMember(Name="fboid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboid")]
    public int? Fboid { get; set; }

    /// <summary>
    /// Gets or Sets DefaultTemplate
    /// </summary>
    [DataMember(Name="defaultTemplate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "defaultTemplate")]
    public bool? DefaultTemplate { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FboAirport {\n");
      sb.Append("  Oid: ").Append(Oid).Append("\n");
      sb.Append("  Iata: ").Append(Iata).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  Fboid: ").Append(Fboid).Append("\n");
      sb.Append("  DefaultTemplate: ").Append(DefaultTemplate).Append("\n");
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
