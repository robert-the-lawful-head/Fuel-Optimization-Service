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
  public class PostCompanyFuelerChangeLogRequest {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets DateTimeRecorded
    /// </summary>
    [DataMember(Name="dateTimeRecorded", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateTimeRecorded")]
    public DateTime? DateTimeRecorded { get; set; }

    /// <summary>
    /// Gets or Sets UserId
    /// </summary>
    [DataMember(Name="userId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userId")]
    public int? UserId { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets FuelerId
    /// </summary>
    [DataMember(Name="fuelerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerId")]
    public int? FuelerId { get; set; }

    /// <summary>
    /// Gets or Sets Action
    /// </summary>
    [DataMember(Name="action", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "action")]
    public int? Action { get; set; }

    /// <summary>
    /// Gets or Sets Source
    /// </summary>
    [DataMember(Name="source", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "source")]
    public int? Source { get; set; }

    /// <summary>
    /// Gets or Sets FuelerJson
    /// </summary>
    [DataMember(Name="fuelerJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerJson")]
    public string FuelerJson { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostCompanyFuelerChangeLogRequest {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  DateTimeRecorded: ").Append(DateTimeRecorded).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  FuelerId: ").Append(FuelerId).Append("\n");
      sb.Append("  Action: ").Append(Action).Append("\n");
      sb.Append("  Source: ").Append(Source).Append("\n");
      sb.Append("  FuelerJson: ").Append(FuelerJson).Append("\n");
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
