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
  public class CompanyDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets CompanyName
    /// </summary>
    [DataMember(Name="companyName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyName")]
    public string CompanyName { get; set; }

    /// <summary>
    /// Gets or Sets MaxUsers
    /// </summary>
    [DataMember(Name="maxUsers", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxUsers")]
    public double? MaxUsers { get; set; }

    /// <summary>
    /// Gets or Sets MaxAircraft
    /// </summary>
    [DataMember(Name="maxAircraft", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxAircraft")]
    public double? MaxAircraft { get; set; }

    /// <summary>
    /// Gets or Sets MaxFuelers
    /// </summary>
    [DataMember(Name="maxFuelers", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "maxFuelers")]
    public double? MaxFuelers { get; set; }

    /// <summary>
    /// Gets or Sets HeaderImageUrl
    /// </summary>
    [DataMember(Name="headerImageUrl", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "headerImageUrl")]
    public string HeaderImageUrl { get; set; }

    /// <summary>
    /// Gets or Sets ShareTransactions
    /// </summary>
    [DataMember(Name="shareTransactions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "shareTransactions")]
    public bool? ShareTransactions { get; set; }

    /// <summary>
    /// Gets or Sets CertificateType
    /// </summary>
    [DataMember(Name="certificateType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "certificateType")]
    public string CertificateType { get; set; }

    /// <summary>
    /// Gets or Sets HideInFboLinx
    /// </summary>
    [DataMember(Name="hideInFboLinx", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hideInFboLinx")]
    public bool? HideInFboLinx { get; set; }

    /// <summary>
    /// Gets or Sets Active
    /// </summary>
    [DataMember(Name="active", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "active")]
    public bool? Active { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="subscriptionType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "subscriptionType")]
    public int? SubscriptionType { get; set; }

    /// <summary>
    /// Gets or Sets FaaId
    /// </summary>
    [DataMember(Name="faaId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "faaId")]
    public string FaaId { get; set; }

    /// <summary>
    /// Gets or Sets FaaCertificateNumber
    /// </summary>
    [DataMember(Name="faaCertificateNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "faaCertificateNumber")]
    public string FaaCertificateNumber { get; set; }

    /// <summary>
    /// Gets or Sets ImageFileDataId
    /// </summary>
    [DataMember(Name="imageFileDataId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "imageFileDataId")]
    public int? ImageFileDataId { get; set; }

    /// <summary>
    /// Gets or Sets CompanySettings
    /// </summary>
    [DataMember(Name="companySettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companySettings")]
    public List<CompanySettingsDTO> CompanySettings { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CompanyDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyName: ").Append(CompanyName).Append("\n");
      sb.Append("  MaxUsers: ").Append(MaxUsers).Append("\n");
      sb.Append("  MaxAircraft: ").Append(MaxAircraft).Append("\n");
      sb.Append("  MaxFuelers: ").Append(MaxFuelers).Append("\n");
      sb.Append("  HeaderImageUrl: ").Append(HeaderImageUrl).Append("\n");
      sb.Append("  ShareTransactions: ").Append(ShareTransactions).Append("\n");
      sb.Append("  CertificateType: ").Append(CertificateType).Append("\n");
      sb.Append("  HideInFboLinx: ").Append(HideInFboLinx).Append("\n");
      sb.Append("  Active: ").Append(Active).Append("\n");
      sb.Append("  SubscriptionType: ").Append(SubscriptionType).Append("\n");
      sb.Append("  FaaId: ").Append(FaaId).Append("\n");
      sb.Append("  FaaCertificateNumber: ").Append(FaaCertificateNumber).Append("\n");
      sb.Append("  ImageFileDataId: ").Append(ImageFileDataId).Append("\n");
      sb.Append("  CompanySettings: ").Append(CompanySettings).Append("\n");
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
