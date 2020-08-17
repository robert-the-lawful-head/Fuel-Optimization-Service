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
  public class FuelVendorDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets FuelerName
    /// </summary>
    [DataMember(Name="fuelerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerName")]
    public string FuelerName { get; set; }

    /// <summary>
    /// Gets or Sets FuelerType
    /// </summary>
    [DataMember(Name="fuelerType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerType")]
    public string FuelerType { get; set; }

    /// <summary>
    /// Gets or Sets CreationDate
    /// </summary>
    [DataMember(Name="creationDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "creationDate")]
    public DateTime? CreationDate { get; set; }

    /// <summary>
    /// Gets or Sets ChangeDate
    /// </summary>
    [DataMember(Name="changeDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "changeDate")]
    public DateTime? ChangeDate { get; set; }

    /// <summary>
    /// Gets or Sets EMail
    /// </summary>
    [DataMember(Name="eMail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "eMail")]
    public string EMail { get; set; }

    /// <summary>
    /// Gets or Sets ProcessName
    /// </summary>
    [DataMember(Name="processName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "processName")]
    public string ProcessName { get; set; }

    /// <summary>
    /// Gets or Sets IsAllInclusive
    /// </summary>
    [DataMember(Name="isAllInclusive", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isAllInclusive")]
    public bool? IsAllInclusive { get; set; }

    /// <summary>
    /// Gets or Sets WebLink
    /// </summary>
    [DataMember(Name="webLink", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "webLink")]
    public string WebLink { get; set; }

    /// <summary>
    /// Gets or Sets FuelerPhone
    /// </summary>
    [DataMember(Name="fuelerPhone", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerPhone")]
    public string FuelerPhone { get; set; }

    /// <summary>
    /// Gets or Sets ContactEmail
    /// </summary>
    [DataMember(Name="contactEmail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "contactEmail")]
    public string ContactEmail { get; set; }

    /// <summary>
    /// Gets or Sets ImagePath
    /// </summary>
    [DataMember(Name="imagePath", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "imagePath")]
    public string ImagePath { get; set; }

    /// <summary>
    /// Gets or Sets PullFlag
    /// </summary>
    [DataMember(Name="pullFlag", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pullFlag")]
    public bool? PullFlag { get; set; }

    /// <summary>
    /// Gets or Sets EmailToCC
    /// </summary>
    [DataMember(Name="emailToCC", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "emailToCC")]
    public string EmailToCC { get; set; }

    /// <summary>
    /// Gets or Sets IsInternational
    /// </summary>
    [DataMember(Name="isInternational", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isInternational")]
    public bool? IsInternational { get; set; }

    /// <summary>
    /// Gets or Sets Enabled
    /// </summary>
    [DataMember(Name="enabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Gets or Sets FbolinxId
    /// </summary>
    [DataMember(Name="fbolinxId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fbolinxId")]
    public int? FbolinxId { get; set; }

    /// <summary>
    /// Gets or Sets Show
    /// </summary>
    [DataMember(Name="show", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "show")]
    public bool? Show { get; set; }

    /// <summary>
    /// Gets or Sets IsWebBasedAutoRecon
    /// </summary>
    [DataMember(Name="isWebBasedAutoRecon", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isWebBasedAutoRecon")]
    public bool? IsWebBasedAutoRecon { get; set; }

    /// <summary>
    /// Gets or Sets IsPricingAlwaysShown
    /// </summary>
    [DataMember(Name="isPricingAlwaysShown", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isPricingAlwaysShown")]
    public bool? IsPricingAlwaysShown { get; set; }

    /// <summary>
    /// Gets or Sets InternationalEmail
    /// </summary>
    [DataMember(Name="internationalEmail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "internationalEmail")]
    public string InternationalEmail { get; set; }

    /// <summary>
    /// Gets or Sets UseOldService
    /// </summary>
    [DataMember(Name="useOldService", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "useOldService")]
    public bool? UseOldService { get; set; }

    /// <summary>
    /// Gets or Sets ServiceUrl
    /// </summary>
    [DataMember(Name="serviceUrl", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "serviceUrl")]
    public string ServiceUrl { get; set; }

    /// <summary>
    /// Gets or Sets IsMessagesEnabled
    /// </summary>
    [DataMember(Name="isMessagesEnabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isMessagesEnabled")]
    public bool? IsMessagesEnabled { get; set; }

    /// <summary>
    /// Gets or Sets IsVendorLinxEnabled
    /// </summary>
    [DataMember(Name="isVendorLinxEnabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isVendorLinxEnabled")]
    public bool? IsVendorLinxEnabled { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FuelVendorDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  FuelerName: ").Append(FuelerName).Append("\n");
      sb.Append("  FuelerType: ").Append(FuelerType).Append("\n");
      sb.Append("  CreationDate: ").Append(CreationDate).Append("\n");
      sb.Append("  ChangeDate: ").Append(ChangeDate).Append("\n");
      sb.Append("  EMail: ").Append(EMail).Append("\n");
      sb.Append("  ProcessName: ").Append(ProcessName).Append("\n");
      sb.Append("  IsAllInclusive: ").Append(IsAllInclusive).Append("\n");
      sb.Append("  WebLink: ").Append(WebLink).Append("\n");
      sb.Append("  FuelerPhone: ").Append(FuelerPhone).Append("\n");
      sb.Append("  ContactEmail: ").Append(ContactEmail).Append("\n");
      sb.Append("  ImagePath: ").Append(ImagePath).Append("\n");
      sb.Append("  PullFlag: ").Append(PullFlag).Append("\n");
      sb.Append("  EmailToCC: ").Append(EmailToCC).Append("\n");
      sb.Append("  IsInternational: ").Append(IsInternational).Append("\n");
      sb.Append("  Enabled: ").Append(Enabled).Append("\n");
      sb.Append("  FbolinxId: ").Append(FbolinxId).Append("\n");
      sb.Append("  Show: ").Append(Show).Append("\n");
      sb.Append("  IsWebBasedAutoRecon: ").Append(IsWebBasedAutoRecon).Append("\n");
      sb.Append("  IsPricingAlwaysShown: ").Append(IsPricingAlwaysShown).Append("\n");
      sb.Append("  InternationalEmail: ").Append(InternationalEmail).Append("\n");
      sb.Append("  UseOldService: ").Append(UseOldService).Append("\n");
      sb.Append("  ServiceUrl: ").Append(ServiceUrl).Append("\n");
      sb.Append("  IsMessagesEnabled: ").Append(IsMessagesEnabled).Append("\n");
      sb.Append("  IsVendorLinxEnabled: ").Append(IsVendorLinxEnabled).Append("\n");
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
