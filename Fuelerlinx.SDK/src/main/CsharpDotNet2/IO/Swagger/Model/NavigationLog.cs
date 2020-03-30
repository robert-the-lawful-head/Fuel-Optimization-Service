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
  public class NavigationLog {
    /// <summary>
    /// Gets or Sets FuelData
    /// </summary>
    [DataMember(Name="fuelData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelData")]
    public FuelData FuelData { get; set; }

    /// <summary>
    /// Gets or Sets Segments
    /// </summary>
    [DataMember(Name="segments", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "segments")]
    public List<Segment> Segments { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class NavigationLog {\n");
      sb.Append("  FuelData: ").Append(FuelData).Append("\n");
      sb.Append("  Segments: ").Append(Segments).Append("\n");
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
