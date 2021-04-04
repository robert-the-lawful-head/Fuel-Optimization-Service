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
  public class BytescoutFilesDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets FileName
    /// </summary>
    [DataMember(Name="fileName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fileName")]
    public string FileName { get; set; }

    /// <summary>
    /// Gets or Sets DateAddedUtc
    /// </summary>
    [DataMember(Name="dateAddedUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateAddedUtc")]
    public DateTime? DateAddedUtc { get; set; }

    /// <summary>
    /// Gets or Sets BytescoutFileDataId
    /// </summary>
    [DataMember(Name="bytescoutFileDataId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "bytescoutFileDataId")]
    public int? BytescoutFileDataId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class BytescoutFilesDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  FileName: ").Append(FileName).Append("\n");
      sb.Append("  DateAddedUtc: ").Append(DateAddedUtc).Append("\n");
      sb.Append("  BytescoutFileDataId: ").Append(BytescoutFileDataId).Append("\n");
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
