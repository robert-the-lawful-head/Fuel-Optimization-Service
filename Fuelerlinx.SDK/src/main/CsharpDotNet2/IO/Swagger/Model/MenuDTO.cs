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
  public class MenuDTO {
    /// <summary>
    /// Gets or Sets MainMenuFeature
    /// </summary>
    [DataMember(Name="MainMenuFeature", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "MainMenuFeature")]
    public MainMenuFeatureDTO MainMenuFeature { get; set; }

    /// <summary>
    /// Gets or Sets HeaderMenuFeature
    /// </summary>
    [DataMember(Name="HeaderMenuFeature", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "HeaderMenuFeature")]
    public HeaderMenuFeatureDTO HeaderMenuFeature { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MenuDTO {\n");
      sb.Append("  MainMenuFeature: ").Append(MainMenuFeature).Append("\n");
      sb.Append("  HeaderMenuFeature: ").Append(HeaderMenuFeature).Append("\n");
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
