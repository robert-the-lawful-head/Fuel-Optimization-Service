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
  public class AmstatAircraftDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Make
    /// </summary>
    [DataMember(Name="make", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "make")]
    public string Make { get; set; }

    /// <summary>
    /// Gets or Sets Model
    /// </summary>
    [DataMember(Name="model", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "model")]
    public string Model { get; set; }

    /// <summary>
    /// Gets or Sets Serial
    /// </summary>
    [DataMember(Name="serial", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "serial")]
    public double? Serial { get; set; }

    /// <summary>
    /// Gets or Sets Owner
    /// </summary>
    [DataMember(Name="owner", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "owner")]
    public string Owner { get; set; }

    /// <summary>
    /// Gets or Sets Operator
    /// </summary>
    [DataMember(Name="operator", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "operator")]
    public string Operator { get; set; }

    /// <summary>
    /// Gets or Sets ManagementCompany
    /// </summary>
    [DataMember(Name="managementCompany", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "managementCompany")]
    public string ManagementCompany { get; set; }

    /// <summary>
    /// Gets or Sets Registration
    /// </summary>
    [DataMember(Name="registration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "registration")]
    public string Registration { get; set; }

    /// <summary>
    /// Gets or Sets Certification
    /// </summary>
    [DataMember(Name="certification", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "certification")]
    public string Certification { get; set; }

    /// <summary>
    /// Gets or Sets BaseAirport
    /// </summary>
    [DataMember(Name="baseAirport", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "baseAirport")]
    public string BaseAirport { get; set; }

    /// <summary>
    /// Gets or Sets OperatorDefaultToOwner
    /// </summary>
    [DataMember(Name="operatorDefaultToOwner", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "operatorDefaultToOwner")]
    public string OperatorDefaultToOwner { get; set; }

    /// <summary>
    /// Gets or Sets AircraftGroup
    /// </summary>
    [DataMember(Name="aircraftGroup", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aircraftGroup")]
    public string AircraftGroup { get; set; }

    /// <summary>
    /// Gets or Sets Region
    /// </summary>
    [DataMember(Name="region", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "region")]
    public string Region { get; set; }

    /// <summary>
    /// Gets or Sets AircraftId
    /// </summary>
    [DataMember(Name="aircraftId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aircraftId")]
    public int? AircraftId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AmstatAircraftDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Make: ").Append(Make).Append("\n");
      sb.Append("  Model: ").Append(Model).Append("\n");
      sb.Append("  Serial: ").Append(Serial).Append("\n");
      sb.Append("  Owner: ").Append(Owner).Append("\n");
      sb.Append("  Operator: ").Append(Operator).Append("\n");
      sb.Append("  ManagementCompany: ").Append(ManagementCompany).Append("\n");
      sb.Append("  Registration: ").Append(Registration).Append("\n");
      sb.Append("  Certification: ").Append(Certification).Append("\n");
      sb.Append("  BaseAirport: ").Append(BaseAirport).Append("\n");
      sb.Append("  OperatorDefaultToOwner: ").Append(OperatorDefaultToOwner).Append("\n");
      sb.Append("  AircraftGroup: ").Append(AircraftGroup).Append("\n");
      sb.Append("  Region: ").Append(Region).Append("\n");
      sb.Append("  AircraftId: ").Append(AircraftId).Append("\n");
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
