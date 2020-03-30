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
  public class SavedRoute {
    /// <summary>
    /// Gets or Sets Alternate
    /// </summary>
    [DataMember(Name="alternate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "alternate")]
    public string Alternate { get; set; }

    /// <summary>
    /// Gets or Sets Altitude
    /// </summary>
    [DataMember(Name="altitude", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "altitude")]
    public long? Altitude { get; set; }

    /// <summary>
    /// Gets or Sets Cdr
    /// </summary>
    [DataMember(Name="cdr", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cdr")]
    public string Cdr { get; set; }

    /// <summary>
    /// Gets or Sets CompleteRouteText
    /// </summary>
    [DataMember(Name="completeRouteText", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "completeRouteText")]
    public string CompleteRouteText { get; set; }

    /// <summary>
    /// Gets or Sets Created
    /// </summary>
    [DataMember(Name="created", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "created")]
    public long? Created { get; set; }

    /// <summary>
    /// Gets or Sets DisplayOrder
    /// </summary>
    [DataMember(Name="displayOrder", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "displayOrder")]
    public long? DisplayOrder { get; set; }

    /// <summary>
    /// Gets or Sets From
    /// </summary>
    [DataMember(Name="from", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "from")]
    public string From { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public long? Id { get; set; }

    /// <summary>
    /// Gets or Sets Modified
    /// </summary>
    [DataMember(Name="modified", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "modified")]
    public long? Modified { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets Route
    /// </summary>
    [DataMember(Name="route", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "route")]
    public string Route { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="routingType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "routingType")]
    public int? RoutingType { get; set; }

    /// <summary>
    /// Gets or Sets Sid
    /// </summary>
    [DataMember(Name="sid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sid")]
    public string Sid { get; set; }

    /// <summary>
    /// Gets or Sets Star
    /// </summary>
    [DataMember(Name="star", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "star")]
    public string Star { get; set; }

    /// <summary>
    /// Gets or Sets To
    /// </summary>
    [DataMember(Name="to", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "to")]
    public string To { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SavedRoute {\n");
      sb.Append("  Alternate: ").Append(Alternate).Append("\n");
      sb.Append("  Altitude: ").Append(Altitude).Append("\n");
      sb.Append("  Cdr: ").Append(Cdr).Append("\n");
      sb.Append("  CompleteRouteText: ").Append(CompleteRouteText).Append("\n");
      sb.Append("  Created: ").Append(Created).Append("\n");
      sb.Append("  DisplayOrder: ").Append(DisplayOrder).Append("\n");
      sb.Append("  From: ").Append(From).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Modified: ").Append(Modified).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Route: ").Append(Route).Append("\n");
      sb.Append("  RoutingType: ").Append(RoutingType).Append("\n");
      sb.Append("  Sid: ").Append(Sid).Append("\n");
      sb.Append("  Star: ").Append(Star).Append("\n");
      sb.Append("  To: ").Append(To).Append("\n");
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
