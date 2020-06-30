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
  public class RampFeeByCompanyDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets FboHandlerName
    /// </summary>
    [DataMember(Name="fboHandlerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboHandlerName")]
    public string FboHandlerName { get; set; }

    /// <summary>
    /// Gets or Sets RampFee
    /// </summary>
    [DataMember(Name="rampFee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFee")]
    public double? RampFee { get; set; }

    /// <summary>
    /// Gets or Sets RampFeeWaived
    /// </summary>
    [DataMember(Name="rampFeeWaived", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFeeWaived")]
    public Weight RampFeeWaived { get; set; }

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
    /// Gets or Sets Updated
    /// </summary>
    [DataMember(Name="updated", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "updated")]
    public DateTime? Updated { get; set; }

    /// <summary>
    /// 0 = Not Specified  1 = By Size  2 = By Aircraft Type  3 = By Weight Range (Lbs)  4 = By Wingspan (feet)  5 = By Tail Number
    /// </summary>
    /// <value>0 = Not Specified  1 = By Size  2 = By Aircraft Type  3 = By Weight Range (Lbs)  4 = By Wingspan (feet)  5 = By Tail Number</value>
    [DataMember(Name="categoryType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categoryType")]
    public int? CategoryType { get; set; }

    /// <summary>
    /// Gets or Sets CategoryMinValue
    /// </summary>
    [DataMember(Name="categoryMinValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categoryMinValue")]
    public int? CategoryMinValue { get; set; }

    /// <summary>
    /// Gets or Sets CategoryMaxValue
    /// </summary>
    [DataMember(Name="categoryMaxValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categoryMaxValue")]
    public int? CategoryMaxValue { get; set; }

    /// <summary>
    /// Gets or Sets LandingFee
    /// </summary>
    [DataMember(Name="landingFee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "landingFee")]
    public double? LandingFee { get; set; }

    /// <summary>
    /// Gets or Sets FacilityFee
    /// </summary>
    [DataMember(Name="facilityFee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "facilityFee")]
    public double? FacilityFee { get; set; }

    /// <summary>
    /// Gets or Sets ExpirationDate
    /// </summary>
    [DataMember(Name="expirationDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "expirationDate")]
    public string ExpirationDate { get; set; }

    /// <summary>
    /// Gets or Sets Applicable
    /// </summary>
    [DataMember(Name="applicable", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applicable")]
    public bool? Applicable { get; set; }

    /// <summary>
    /// Gets or Sets CategoryStringValue
    /// </summary>
    [DataMember(Name="categoryStringValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categoryStringValue")]
    public string CategoryStringValue { get; set; }

    /// <summary>
    /// Gets or Sets RampFeeByCompanyNotes
    /// </summary>
    [DataMember(Name="rampFeeByCompanyNotes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFeeByCompanyNotes")]
    public List<RampFeeByCompanyNoteDTO> RampFeeByCompanyNotes { get; set; }

    /// <summary>
    /// Gets or Sets ApplicableTailNumbers
    /// </summary>
    [DataMember(Name="applicableTailNumbers", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applicableTailNumbers")]
    public List<string> ApplicableTailNumbers { get; set; }

    /// <summary>
    /// Gets or Sets CategorizationDescription
    /// </summary>
    [DataMember(Name="categorizationDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categorizationDescription")]
    public string CategorizationDescription { get; set; }

    /// <summary>
    /// Gets or Sets AddedByName
    /// </summary>
    [DataMember(Name="addedByName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addedByName")]
    public string AddedByName { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class RampFeeByCompanyDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  FboHandlerName: ").Append(FboHandlerName).Append("\n");
      sb.Append("  RampFee: ").Append(RampFee).Append("\n");
      sb.Append("  RampFeeWaived: ").Append(RampFeeWaived).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  Updated: ").Append(Updated).Append("\n");
      sb.Append("  CategoryType: ").Append(CategoryType).Append("\n");
      sb.Append("  CategoryMinValue: ").Append(CategoryMinValue).Append("\n");
      sb.Append("  CategoryMaxValue: ").Append(CategoryMaxValue).Append("\n");
      sb.Append("  LandingFee: ").Append(LandingFee).Append("\n");
      sb.Append("  FacilityFee: ").Append(FacilityFee).Append("\n");
      sb.Append("  ExpirationDate: ").Append(ExpirationDate).Append("\n");
      sb.Append("  Applicable: ").Append(Applicable).Append("\n");
      sb.Append("  CategoryStringValue: ").Append(CategoryStringValue).Append("\n");
      sb.Append("  RampFeeByCompanyNotes: ").Append(RampFeeByCompanyNotes).Append("\n");
      sb.Append("  ApplicableTailNumbers: ").Append(ApplicableTailNumbers).Append("\n");
      sb.Append("  CategorizationDescription: ").Append(CategorizationDescription).Append("\n");
      sb.Append("  AddedByName: ").Append(AddedByName).Append("\n");
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
