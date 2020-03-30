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
  public class FuelOrderCancellationOptions {
    /// <summary>
    /// Gets or Sets NotifyFuelVendorViaEmail
    /// </summary>
    [DataMember(Name="notifyFuelVendorViaEmail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "notifyFuelVendorViaEmail")]
    public bool? NotifyFuelVendorViaEmail { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FuelOrderCancellationOptions {\n");
      sb.Append("  NotifyFuelVendorViaEmail: ").Append(NotifyFuelVendorViaEmail).Append("\n");
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
