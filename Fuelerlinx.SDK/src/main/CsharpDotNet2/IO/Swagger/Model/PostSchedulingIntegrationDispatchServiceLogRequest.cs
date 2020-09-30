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
  public class PostSchedulingIntegrationDispatchServiceLogRequest {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

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
    /// Gets or Sets DateTimeRecorded
    /// </summary>
    [DataMember(Name="dateTimeRecorded", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateTimeRecorded")]
    public DateTime? DateTimeRecorded { get; set; }

    /// <summary>
    /// Gets or Sets RequestSerializedData
    /// </summary>
    [DataMember(Name="requestSerializedData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "requestSerializedData")]
    public string RequestSerializedData { get; set; }

    /// <summary>
    /// Gets or Sets RequestSerializedAs
    /// </summary>
    [DataMember(Name="requestSerializedAs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "requestSerializedAs")]
    public string RequestSerializedAs { get; set; }

    /// <summary>
    /// Gets or Sets ResponseSerializedData
    /// </summary>
    [DataMember(Name="responseSerializedData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "responseSerializedData")]
    public string ResponseSerializedData { get; set; }

    /// <summary>
    /// Gets or Sets ResponseSerializedAs
    /// </summary>
    [DataMember(Name="responseSerializedAs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "responseSerializedAs")]
    public string ResponseSerializedAs { get; set; }

    /// <summary>
    /// Gets or Sets IntegrationType
    /// </summary>
    [DataMember(Name="integrationType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "integrationType")]
    public int? IntegrationType { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostSchedulingIntegrationDispatchServiceLogRequest {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  DateTimeRecorded: ").Append(DateTimeRecorded).Append("\n");
      sb.Append("  RequestSerializedData: ").Append(RequestSerializedData).Append("\n");
      sb.Append("  RequestSerializedAs: ").Append(RequestSerializedAs).Append("\n");
      sb.Append("  ResponseSerializedData: ").Append(ResponseSerializedData).Append("\n");
      sb.Append("  ResponseSerializedAs: ").Append(ResponseSerializedAs).Append("\n");
      sb.Append("  IntegrationType: ").Append(IntegrationType).Append("\n");
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
