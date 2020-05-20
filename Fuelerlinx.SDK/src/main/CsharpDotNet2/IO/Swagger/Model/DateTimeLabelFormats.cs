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
  public class DateTimeLabelFormats {
    /// <summary>
    /// Gets or Sets Second
    /// </summary>
    [DataMember(Name="second", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "second")]
    public string Second { get; set; }

    /// <summary>
    /// Gets or Sets Minute
    /// </summary>
    [DataMember(Name="minute", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minute")]
    public string Minute { get; set; }

    /// <summary>
    /// Gets or Sets Hour
    /// </summary>
    [DataMember(Name="hour", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hour")]
    public string Hour { get; set; }

    /// <summary>
    /// Gets or Sets Day
    /// </summary>
    [DataMember(Name="day", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "day")]
    public string Day { get; set; }

    /// <summary>
    /// Gets or Sets Week
    /// </summary>
    [DataMember(Name="week", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "week")]
    public string Week { get; set; }

    /// <summary>
    /// Gets or Sets Month
    /// </summary>
    [DataMember(Name="month", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "month")]
    public string Month { get; set; }

    /// <summary>
    /// Gets or Sets Year
    /// </summary>
    [DataMember(Name="year", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "year")]
    public string Year { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DateTimeLabelFormats {\n");
      sb.Append("  Second: ").Append(Second).Append("\n");
      sb.Append("  Minute: ").Append(Minute).Append("\n");
      sb.Append("  Hour: ").Append(Hour).Append("\n");
      sb.Append("  Day: ").Append(Day).Append("\n");
      sb.Append("  Week: ").Append(Week).Append("\n");
      sb.Append("  Month: ").Append(Month).Append("\n");
      sb.Append("  Year: ").Append(Year).Append("\n");
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
