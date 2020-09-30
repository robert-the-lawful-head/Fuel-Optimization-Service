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
  public class PostFuelPriceServiceLogRequest {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

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
    /// Gets or Sets SerializedAs
    /// </summary>
    [DataMember(Name="serializedAs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "serializedAs")]
    public string SerializedAs { get; set; }

    /// <summary>
    /// Gets or Sets SerializedData
    /// </summary>
    [DataMember(Name="serializedData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "serializedData")]
    public string SerializedData { get; set; }

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
    /// Gets or Sets Icaos
    /// </summary>
    [DataMember(Name="icaos", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icaos")]
    public string Icaos { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostFuelPriceServiceLogRequest {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  DateTimeRecorded: ").Append(DateTimeRecorded).Append("\n");
      sb.Append("  SerializedAs: ").Append(SerializedAs).Append("\n");
      sb.Append("  SerializedData: ").Append(SerializedData).Append("\n");
      sb.Append("  FuelerName: ").Append(FuelerName).Append("\n");
      sb.Append("  FuelerId: ").Append(FuelerId).Append("\n");
      sb.Append("  Icaos: ").Append(Icaos).Append("\n");
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
