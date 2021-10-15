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
  public class QuotingPreferences {
    /// <summary>
    /// Gets or Sets BlackListedAirports
    /// </summary>
    [DataMember(Name="blackListedAirports", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "blackListedAirports")]
    public List<BlackListedAirport> BlackListedAirports { get; set; }

    /// <summary>
    /// Gets or Sets AllowEstimatingAllIn
    /// </summary>
    [DataMember(Name="allowEstimatingAllIn", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "allowEstimatingAllIn")]
    public bool? AllowEstimatingAllIn { get; set; }

    /// <summary>
    /// Gets or Sets ShowAllInEstimateImmediately
    /// </summary>
    [DataMember(Name="showAllInEstimateImmediately", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "showAllInEstimateImmediately")]
    public bool? ShowAllInEstimateImmediately { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class QuotingPreferences {\n");
      sb.Append("  BlackListedAirports: ").Append(BlackListedAirports).Append("\n");
      sb.Append("  AllowEstimatingAllIn: ").Append(AllowEstimatingAllIn).Append("\n");
      sb.Append("  ShowAllInEstimateImmediately: ").Append(ShowAllInEstimateImmediately).Append("\n");
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
