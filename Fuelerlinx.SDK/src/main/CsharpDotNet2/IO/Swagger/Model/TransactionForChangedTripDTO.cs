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
  public class TransactionForChangedTripDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets RequestId
    /// </summary>
    [DataMember(Name="requestId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "requestId")]
    public int? RequestId { get; set; }

    /// <summary>
    /// Gets or Sets HasBeenReviewed
    /// </summary>
    [DataMember(Name="hasBeenReviewed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hasBeenReviewed")]
    public bool? HasBeenReviewed { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TransactionForChangedTripDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  RequestId: ").Append(RequestId).Append("\n");
      sb.Append("  HasBeenReviewed: ").Append(HasBeenReviewed).Append("\n");
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
