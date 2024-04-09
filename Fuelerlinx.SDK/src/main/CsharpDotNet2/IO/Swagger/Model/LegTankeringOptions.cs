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
  public class LegTankeringOptions {
    /// <summary>
    /// Maximizes the fuel uplift on this leg, landing with as much fuel as possible.
    /// </summary>
    /// <value>Maximizes the fuel uplift on this leg, landing with as much fuel as possible.</value>
    [DataMember(Name="landWithMaxFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "landWithMaxFuel")]
    public bool? LandWithMaxFuel { get; set; }

    /// <summary>
    /// Omits the leg from consideration of a fuel uplift.  Will skip fueling in all scenarios.
    /// </summary>
    /// <value>Omits the leg from consideration of a fuel uplift.  Will skip fueling in all scenarios.</value>
    [DataMember(Name="omitFromFuelConsideration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "omitFromFuelConsideration")]
    public bool? OmitFromFuelConsideration { get; set; }

    /// <summary>
    /// Gets or Sets LockedFuelAmount
    /// </summary>
    [DataMember(Name="lockedFuelAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lockedFuelAmount")]
    public Weight LockedFuelAmount { get; set; }

    /// <summary>
    /// Gets or Sets MinimumRequiredRampFuel
    /// </summary>
    [DataMember(Name="minimumRequiredRampFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minimumRequiredRampFuel")]
    public Weight MinimumRequiredRampFuel { get; set; }

    /// <summary>
    /// Gets or Sets MaximumRampFuel
    /// </summary>
    [DataMember(Name="maximumRampFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maximumRampFuel")]
    public Weight MaximumRampFuel { get; set; }

    /// <summary>
    /// Overrides the trip-wide price search preference.  Set to 0 - Selected option only if you want to lock it to the selected option.
    /// </summary>
    /// <value>Overrides the trip-wide price search preference.  Set to 0 - Selected option only if you want to lock it to the selected option.</value>
    [DataMember(Name="priceSearchPreference", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceSearchPreference")]
    public int? PriceSearchPreference { get; set; }

    /// <summary>
    /// Gets or Sets MinExtraReserve
    /// </summary>
    [DataMember(Name="minExtraReserve", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minExtraReserve")]
    public Weight MinExtraReserve { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class LegTankeringOptions {\n");
      sb.Append("  LandWithMaxFuel: ").Append(LandWithMaxFuel).Append("\n");
      sb.Append("  OmitFromFuelConsideration: ").Append(OmitFromFuelConsideration).Append("\n");
      sb.Append("  LockedFuelAmount: ").Append(LockedFuelAmount).Append("\n");
      sb.Append("  MinimumRequiredRampFuel: ").Append(MinimumRequiredRampFuel).Append("\n");
      sb.Append("  MaximumRampFuel: ").Append(MaximumRampFuel).Append("\n");
      sb.Append("  PriceSearchPreference: ").Append(PriceSearchPreference).Append("\n");
      sb.Append("  MinExtraReserve: ").Append(MinExtraReserve).Append("\n");
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
