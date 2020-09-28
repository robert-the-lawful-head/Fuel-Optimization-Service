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
  public class PostUserAircraftTankeringSettingsRequest {
    /// <summary>
    /// Gets or Sets TailNumberId
    /// </summary>
    [DataMember(Name="tailNumberId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumberId")]
    public int? TailNumberId { get; set; }

    /// <summary>
    /// Gets or Sets FuelBias
    /// </summary>
    [DataMember(Name="fuelBias", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelBias")]
    public double? FuelBias { get; set; }

    /// <summary>
    /// Gets or Sets SpeedBias
    /// </summary>
    [DataMember(Name="speedBias", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "speedBias")]
    public double? SpeedBias { get; set; }

    /// <summary>
    /// Gets or Sets DefaultStartingFuel
    /// </summary>
    [DataMember(Name="defaultStartingFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "defaultStartingFuel")]
    public Weight DefaultStartingFuel { get; set; }

    /// <summary>
    /// Gets or Sets AllowMlt
    /// </summary>
    [DataMember(Name="allowMlt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "allowMlt")]
    public bool? AllowMlt { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostUserAircraftTankeringSettingsRequest {\n");
      sb.Append("  TailNumberId: ").Append(TailNumberId).Append("\n");
      sb.Append("  FuelBias: ").Append(FuelBias).Append("\n");
      sb.Append("  SpeedBias: ").Append(SpeedBias).Append("\n");
      sb.Append("  DefaultStartingFuel: ").Append(DefaultStartingFuel).Append("\n");
      sb.Append("  AllowMlt: ").Append(AllowMlt).Append("\n");
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
