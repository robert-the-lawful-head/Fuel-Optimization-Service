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
  public class CompanyFboChangeLogDTO {
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
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets UserId
    /// </summary>
    [DataMember(Name="userId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userId")]
    public int? UserId { get; set; }

    /// <summary>
    /// Gets or Sets Fbo
    /// </summary>
    [DataMember(Name="fbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fbo")]
    public string Fbo { get; set; }

    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets ChangedItem
    /// </summary>
    [DataMember(Name="changedItem", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "changedItem")]
    public int? ChangedItem { get; set; }

    /// <summary>
    /// Gets or Sets FboJson
    /// </summary>
    [DataMember(Name="fboJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboJson")]
    public string FboJson { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CompanyFboChangeLogDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  DateTimeRecorded: ").Append(DateTimeRecorded).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  Fbo: ").Append(Fbo).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  ChangedItem: ").Append(ChangedItem).Append("\n");
      sb.Append("  FboJson: ").Append(FboJson).Append("\n");
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
