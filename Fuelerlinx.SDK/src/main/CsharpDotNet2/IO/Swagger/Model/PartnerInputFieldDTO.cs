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
  public class PartnerInputFieldDTO {
    /// <summary>
    /// Gets or Sets Label
    /// </summary>
    [DataMember(Name="label", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "label")]
    public string Label { get; set; }

    /// <summary>
    /// Gets or Sets IsDescriptionOnly
    /// </summary>
    [DataMember(Name="isDescriptionOnly", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isDescriptionOnly")]
    public bool? IsDescriptionOnly { get; set; }

    /// <summary>
    /// Gets or Sets PropertyName
    /// </summary>
    [DataMember(Name="propertyName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "propertyName")]
    public string PropertyName { get; set; }

    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets DefaultValue
    /// </summary>
    [DataMember(Name="defaultValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "defaultValue")]
    public string DefaultValue { get; set; }

    /// <summary>
    /// Gets or Sets PlaceHolder
    /// </summary>
    [DataMember(Name="placeHolder", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "placeHolder")]
    public string PlaceHolder { get; set; }

    /// <summary>
    /// Gets or Sets IsHidden
    /// </summary>
    [DataMember(Name="isHidden", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isHidden")]
    public bool? IsHidden { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="inputFieldType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "inputFieldType")]
    public int? InputFieldType { get; set; }

    /// <summary>
    /// Gets or Sets ImagePath
    /// </summary>
    [DataMember(Name="imagePath", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "imagePath")]
    public string ImagePath { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PartnerInputFieldDTO {\n");
      sb.Append("  Label: ").Append(Label).Append("\n");
      sb.Append("  IsDescriptionOnly: ").Append(IsDescriptionOnly).Append("\n");
      sb.Append("  PropertyName: ").Append(PropertyName).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  DefaultValue: ").Append(DefaultValue).Append("\n");
      sb.Append("  PlaceHolder: ").Append(PlaceHolder).Append("\n");
      sb.Append("  IsHidden: ").Append(IsHidden).Append("\n");
      sb.Append("  InputFieldType: ").Append(InputFieldType).Append("\n");
      sb.Append("  ImagePath: ").Append(ImagePath).Append("\n");
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
