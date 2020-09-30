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
  public class PostFuelOrderServiceLogRequest {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets TransactionId
    /// </summary>
    [DataMember(Name="transactionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionId")]
    public int? TransactionId { get; set; }

    /// <summary>
    /// Gets or Sets DateTimeRecorded
    /// </summary>
    [DataMember(Name="dateTimeRecorded", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateTimeRecorded")]
    public DateTime? DateTimeRecorded { get; set; }

    /// <summary>
    /// Gets or Sets RequestType
    /// </summary>
    [DataMember(Name="requestType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "requestType")]
    public int? RequestType { get; set; }

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
    /// Gets or Sets FuelerName
    /// </summary>
    [DataMember(Name="fuelerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerName")]
    public string FuelerName { get; set; }

    /// <summary>
    /// Gets or Sets FuelerId
    /// </summary>
    [DataMember(Name="fuelerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerId")]
    public int? FuelerId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostFuelOrderServiceLogRequest {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
      sb.Append("  DateTimeRecorded: ").Append(DateTimeRecorded).Append("\n");
      sb.Append("  RequestType: ").Append(RequestType).Append("\n");
      sb.Append("  RequestSerializedData: ").Append(RequestSerializedData).Append("\n");
      sb.Append("  RequestSerializedAs: ").Append(RequestSerializedAs).Append("\n");
      sb.Append("  ResponseSerializedData: ").Append(ResponseSerializedData).Append("\n");
      sb.Append("  ResponseSerializedAs: ").Append(ResponseSerializedAs).Append("\n");
      sb.Append("  FuelerName: ").Append(FuelerName).Append("\n");
      sb.Append("  FuelerId: ").Append(FuelerId).Append("\n");
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
