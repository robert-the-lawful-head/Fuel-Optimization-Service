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
  public class Trip {
    /// <summary>
    /// Gets or Sets Legs
    /// </summary>
    [DataMember(Name="legs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legs")]
    public List<Leg> Legs { get; set; }

    /// <summary>
    /// Gets or Sets TailNumber
    /// </summary>
    [DataMember(Name="tailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumber")]
    public string TailNumber { get; set; }

    /// <summary>
    /// Gets or Sets StartingFuel
    /// </summary>
    [DataMember(Name="startingFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "startingFuel")]
    public Weight StartingFuel { get; set; }

    /// <summary>
    /// Gets or Sets MaxFuelCapacity
    /// </summary>
    [DataMember(Name="maxFuelCapacity", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxFuelCapacity")]
    public Weight MaxFuelCapacity { get; set; }

    /// <summary>
    /// If true then will treat the trip as returning back to leg 1
    /// </summary>
    /// <value>If true then will treat the trip as returning back to leg 1</value>
    [DataMember(Name="roundTrip", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "roundTrip")]
    public bool? RoundTrip { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Trip {\n");
      sb.Append("  Legs: ").Append(Legs).Append("\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
      sb.Append("  StartingFuel: ").Append(StartingFuel).Append("\n");
      sb.Append("  MaxFuelCapacity: ").Append(MaxFuelCapacity).Append("\n");
      sb.Append("  RoundTrip: ").Append(RoundTrip).Append("\n");
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
