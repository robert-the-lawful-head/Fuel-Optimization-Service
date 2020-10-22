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
  public class CompanyFuelerDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets FuelerId
    /// </summary>
    [DataMember(Name="fuelerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerId")]
    public int? FuelerId { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets Active
    /// </summary>
    [DataMember(Name="active", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "active")]
    public bool? Active { get; set; }

    /// <summary>
    /// Gets or Sets AddDate
    /// </summary>
    [DataMember(Name="addDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addDate")]
    public DateTime? AddDate { get; set; }

    /// <summary>
    /// Gets or Sets DisplayName
    /// </summary>
    [DataMember(Name="displayName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "displayName")]
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or Sets Settings
    /// </summary>
    [DataMember(Name="settings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "settings")]
    public CompanyFuelerSettingsDTO Settings { get; set; }

    /// <summary>
    /// Gets or Sets FuelVendor
    /// </summary>
    [DataMember(Name="fuelVendor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelVendor")]
    public FuelVendorDTO FuelVendor { get; set; }

    /// <summary>
    /// Gets or Sets CompanyFuelerNotes
    /// </summary>
    [DataMember(Name="companyFuelerNotes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyFuelerNotes")]
    public List<CompanyFuelerNotesDTO> CompanyFuelerNotes { get; set; }

    /// <summary>
    /// Gets or Sets PriceAdjustments
    /// </summary>
    [DataMember(Name="priceAdjustments", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceAdjustments")]
    public List<CompanyFuelerPriceAdjustmentDTO> PriceAdjustments { get; set; }

    /// <summary>
    /// Gets or Sets PriceSheetFileCaptures
    /// </summary>
    [DataMember(Name="priceSheetFileCaptures", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceSheetFileCaptures")]
    public List<CompanyFuelerPriceSheetFileCaptureDTO> PriceSheetFileCaptures { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CompanyFuelerDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  FuelerId: ").Append(FuelerId).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  Active: ").Append(Active).Append("\n");
      sb.Append("  AddDate: ").Append(AddDate).Append("\n");
      sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
      sb.Append("  Settings: ").Append(Settings).Append("\n");
      sb.Append("  FuelVendor: ").Append(FuelVendor).Append("\n");
      sb.Append("  CompanyFuelerNotes: ").Append(CompanyFuelerNotes).Append("\n");
      sb.Append("  PriceAdjustments: ").Append(PriceAdjustments).Append("\n");
      sb.Append("  PriceSheetFileCaptures: ").Append(PriceSheetFileCaptures).Append("\n");
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
