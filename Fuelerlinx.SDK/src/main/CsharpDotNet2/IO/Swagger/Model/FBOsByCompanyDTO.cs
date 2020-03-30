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
  public class FBOsByCompanyDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

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
    /// Gets or Sets IsBlackListed
    /// </summary>
    [DataMember(Name="isBlackListed", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isBlackListed")]
    public bool? IsBlackListed { get; set; }

    /// <summary>
    /// Gets or Sets IsPreferred
    /// </summary>
    [DataMember(Name="isPreferred", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isPreferred")]
    public bool? IsPreferred { get; set; }

    /// <summary>
    /// Gets or Sets PreferredFuelerId
    /// </summary>
    [DataMember(Name="preferredFuelerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "preferredFuelerId")]
    public int? PreferredFuelerId { get; set; }

    /// <summary>
    /// Gets or Sets Email
    /// </summary>
    [DataMember(Name="email", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "email")]
    public string Email { get; set; }

    /// <summary>
    /// Gets or Sets CopyOnDispatch
    /// </summary>
    [DataMember(Name="copyOnDispatch", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "copyOnDispatch")]
    public bool? CopyOnDispatch { get; set; }

    /// <summary>
    /// Gets or Sets PreferredProduct
    /// </summary>
    [DataMember(Name="preferredProduct", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "preferredProduct")]
    public string PreferredProduct { get; set; }

    /// <summary>
    /// Gets or Sets SupplierID
    /// </summary>
    [DataMember(Name="supplierID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supplierID")]
    public int? SupplierID { get; set; }

    /// <summary>
    /// Gets or Sets Notes
    /// </summary>
    [DataMember(Name="notes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "notes")]
    public List<FboByCompanyNotesDTO> Notes { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FBOsByCompanyDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  FboHandlerName: ").Append(FboHandlerName).Append("\n");
      sb.Append("  IsBlackListed: ").Append(IsBlackListed).Append("\n");
      sb.Append("  IsPreferred: ").Append(IsPreferred).Append("\n");
      sb.Append("  PreferredFuelerId: ").Append(PreferredFuelerId).Append("\n");
      sb.Append("  Email: ").Append(Email).Append("\n");
      sb.Append("  CopyOnDispatch: ").Append(CopyOnDispatch).Append("\n");
      sb.Append("  PreferredProduct: ").Append(PreferredProduct).Append("\n");
      sb.Append("  SupplierID: ").Append(SupplierID).Append("\n");
      sb.Append("  Notes: ").Append(Notes).Append("\n");
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
