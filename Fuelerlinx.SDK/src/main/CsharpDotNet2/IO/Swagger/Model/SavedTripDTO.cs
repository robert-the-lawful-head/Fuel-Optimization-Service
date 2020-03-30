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
  public class SavedTripDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets UserId
    /// </summary>
    [DataMember(Name="userId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userId")]
    public int? UserId { get; set; }

    /// <summary>
    /// Gets or Sets UserAircraftId
    /// </summary>
    [DataMember(Name="userAircraftId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userAircraftId")]
    public int? UserAircraftId { get; set; }

    /// <summary>
    /// Gets or Sets FuelUnitType
    /// </summary>
    [DataMember(Name="fuelUnitType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelUnitType")]
    public string FuelUnitType { get; set; }

    /// <summary>
    /// Gets or Sets StartingFuel
    /// </summary>
    [DataMember(Name="startingFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "startingFuel")]
    public double? StartingFuel { get; set; }

    /// <summary>
    /// Gets or Sets MltmodelPropertiesJson
    /// </summary>
    [DataMember(Name="mltmodelPropertiesJson", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mltmodelPropertiesJson")]
    public string MltmodelPropertiesJson { get; set; }

    /// <summary>
    /// Gets or Sets SavedTripLegs
    /// </summary>
    [DataMember(Name="savedTripLegs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "savedTripLegs")]
    public List<SavedTripLegDTO> SavedTripLegs { get; set; }

    /// <summary>
    /// Gets or Sets AircraftData
    /// </summary>
    [DataMember(Name="aircraftData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aircraftData")]
    public AircraftDataDTO AircraftData { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SavedTripDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  UserAircraftId: ").Append(UserAircraftId).Append("\n");
      sb.Append("  FuelUnitType: ").Append(FuelUnitType).Append("\n");
      sb.Append("  StartingFuel: ").Append(StartingFuel).Append("\n");
      sb.Append("  MltmodelPropertiesJson: ").Append(MltmodelPropertiesJson).Append("\n");
      sb.Append("  SavedTripLegs: ").Append(SavedTripLegs).Append("\n");
      sb.Append("  AircraftData: ").Append(AircraftData).Append("\n");
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
