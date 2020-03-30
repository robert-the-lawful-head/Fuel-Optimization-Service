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
  public class DispatchFuelRequest {
    /// <summary>
    /// Gets or Sets FuelRequestDetails
    /// </summary>
    [DataMember(Name="fuelRequestDetails", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelRequestDetails")]
    public FuelRequestDetails FuelRequestDetails { get; set; }

    /// <summary>
    /// Gets or Sets FuelPriceOption
    /// </summary>
    [DataMember(Name="fuelPriceOption", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelPriceOption")]
    public FuelPriceOption FuelPriceOption { get; set; }

    /// <summary>
    /// Gets or Sets DispatchEmailOptions
    /// </summary>
    [DataMember(Name="dispatchEmailOptions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dispatchEmailOptions")]
    public DispatchEmailOptions DispatchEmailOptions { get; set; }

    /// <summary>
    /// Gets or Sets ScheduledTrip
    /// </summary>
    [DataMember(Name="scheduledTrip", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "scheduledTrip")]
    public ScheduledTripDTO ScheduledTrip { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DispatchFuelRequest {\n");
      sb.Append("  FuelRequestDetails: ").Append(FuelRequestDetails).Append("\n");
      sb.Append("  FuelPriceOption: ").Append(FuelPriceOption).Append("\n");
      sb.Append("  DispatchEmailOptions: ").Append(DispatchEmailOptions).Append("\n");
      sb.Append("  ScheduledTrip: ").Append(ScheduledTrip).Append("\n");
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
