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
  public class ReportColumnDTO {
    /// <summary>
    /// Gets or Sets Heading
    /// </summary>
    [DataMember(Name="heading", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "heading")]
    public string Heading { get; set; }

    /// <summary>
    /// Gets or Sets Width
    /// </summary>
    [DataMember(Name="width", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "width")]
    public int? Width { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="columnFormat", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "columnFormat")]
    public int? ColumnFormat { get; set; }

    /// <summary>
    /// Gets or Sets IsHidden
    /// </summary>
    [DataMember(Name="isHidden", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isHidden")]
    public bool? IsHidden { get; set; }

    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets PropertyName
    /// </summary>
    [DataMember(Name="propertyName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "propertyName")]
    public string PropertyName { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ReportColumnDTO {\n");
      sb.Append("  Heading: ").Append(Heading).Append("\n");
      sb.Append("  Width: ").Append(Width).Append("\n");
      sb.Append("  ColumnFormat: ").Append(ColumnFormat).Append("\n");
      sb.Append("  IsHidden: ").Append(IsHidden).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  PropertyName: ").Append(PropertyName).Append("\n");
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
