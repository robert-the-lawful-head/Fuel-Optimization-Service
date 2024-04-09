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
  public class PermissionsDTO {
    /// <summary>
    /// Gets or Sets TripPlanning
    /// </summary>
    [DataMember(Name="TripPlanning", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "TripPlanning")]
    public TripPlanningDTO TripPlanning { get; set; }

    /// <summary>
    /// Gets or Sets Menu
    /// </summary>
    [DataMember(Name="Menu", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Menu")]
    public MenuDTO Menu { get; set; }

    /// <summary>
    /// Gets or Sets MobileMenu
    /// </summary>
    [DataMember(Name="MobileMenu", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "MobileMenu")]
    public MobileMenuDTO MobileMenu { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PermissionsDTO {\n");
      sb.Append("  TripPlanning: ").Append(TripPlanning).Append("\n");
      sb.Append("  Menu: ").Append(Menu).Append("\n");
      sb.Append("  MobileMenu: ").Append(MobileMenu).Append("\n");
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
