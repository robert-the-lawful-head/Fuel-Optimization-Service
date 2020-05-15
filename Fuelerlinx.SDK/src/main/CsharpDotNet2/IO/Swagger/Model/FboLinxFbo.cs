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
  public class FboLinxFbo {
    /// <summary>
    /// Gets or Sets Oid
    /// </summary>
    [DataMember(Name="oid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "oid")]
    public int? Oid { get; set; }

    /// <summary>
    /// Gets or Sets Fbo
    /// </summary>
    [DataMember(Name="fbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fbo")]
    public string Fbo { get; set; }

    /// <summary>
    /// Gets or Sets GroupId
    /// </summary>
    [DataMember(Name="groupId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "groupId")]
    public int? GroupId { get; set; }

    /// <summary>
    /// Gets or Sets PostedRetail
    /// </summary>
    [DataMember(Name="postedRetail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "postedRetail")]
    public double? PostedRetail { get; set; }

    /// <summary>
    /// Gets or Sets TotalCost
    /// </summary>
    [DataMember(Name="totalCost", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "totalCost")]
    public double? TotalCost { get; set; }

    /// <summary>
    /// Gets or Sets Active
    /// </summary>
    [DataMember(Name="active", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "active")]
    public bool? Active { get; set; }

    /// <summary>
    /// Gets or Sets DateActivated
    /// </summary>
    [DataMember(Name="dateActivated", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateActivated")]
    public DateTime? DateActivated { get; set; }

    /// <summary>
    /// Gets or Sets Address
    /// </summary>
    [DataMember(Name="address", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "address")]
    public string Address { get; set; }

    /// <summary>
    /// Gets or Sets City
    /// </summary>
    [DataMember(Name="city", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "city")]
    public string City { get; set; }

    /// <summary>
    /// Gets or Sets State
    /// </summary>
    [DataMember(Name="state", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "state")]
    public string State { get; set; }

    /// <summary>
    /// Gets or Sets ZipCode
    /// </summary>
    [DataMember(Name="zipCode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "zipCode")]
    public string ZipCode { get; set; }

    /// <summary>
    /// Gets or Sets Country
    /// </summary>
    [DataMember(Name="country", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "country")]
    public string Country { get; set; }

    /// <summary>
    /// Gets or Sets TotalCost100Ll
    /// </summary>
    [DataMember(Name="totalCost100Ll", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "totalCost100Ll")]
    public double? TotalCost100Ll { get; set; }

    /// <summary>
    /// Gets or Sets PostedRetail100Ll
    /// </summary>
    [DataMember(Name="postedRetail100Ll", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "postedRetail100Ll")]
    public double? PostedRetail100Ll { get; set; }

    /// <summary>
    /// Gets or Sets Suspended
    /// </summary>
    [DataMember(Name="suspended", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "suspended")]
    public bool? Suspended { get; set; }

    /// <summary>
    /// Gets or Sets GroupMargin
    /// </summary>
    [DataMember(Name="groupMargin", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "groupMargin")]
    public bool? GroupMargin { get; set; }

    /// <summary>
    /// Gets or Sets ApplyGroupMargin
    /// </summary>
    [DataMember(Name="applyGroupMargin", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applyGroupMargin")]
    public bool? ApplyGroupMargin { get; set; }

    /// <summary>
    /// Gets or Sets GroupMarginSetting
    /// </summary>
    [DataMember(Name="groupMarginSetting", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "groupMarginSetting")]
    public int? GroupMarginSetting { get; set; }

    /// <summary>
    /// Gets or Sets GroupMarginFuture
    /// </summary>
    [DataMember(Name="groupMarginFuture", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "groupMarginFuture")]
    public bool? GroupMarginFuture { get; set; }

    /// <summary>
    /// Gets or Sets GroupMarginTemplate
    /// </summary>
    [DataMember(Name="groupMarginTemplate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "groupMarginTemplate")]
    public int? GroupMarginTemplate { get; set; }

    /// <summary>
    /// Gets or Sets GroupMarginMargin
    /// </summary>
    [DataMember(Name="groupMarginMargin", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "groupMarginMargin")]
    public double? GroupMarginMargin { get; set; }

    /// <summary>
    /// Gets or Sets GroupMarginType
    /// </summary>
    [DataMember(Name="groupMarginType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "groupMarginType")]
    public int? GroupMarginType { get; set; }

    /// <summary>
    /// Gets or Sets GroupMargin100Llmargin
    /// </summary>
    [DataMember(Name="groupMargin100Llmargin", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "groupMargin100Llmargin")]
    public double? GroupMargin100Llmargin { get; set; }

    /// <summary>
    /// Gets or Sets GroupMargin100Lltype
    /// </summary>
    [DataMember(Name="groupMargin100Lltype", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "groupMargin100Lltype")]
    public int? GroupMargin100Lltype { get; set; }

    /// <summary>
    /// Gets or Sets FuelDeskEmail
    /// </summary>
    [DataMember(Name="fuelDeskEmail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelDeskEmail")]
    public string FuelDeskEmail { get; set; }

    /// <summary>
    /// Gets or Sets Website
    /// </summary>
    [DataMember(Name="website", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "website")]
    public string Website { get; set; }

    /// <summary>
    /// Gets or Sets MainPhone
    /// </summary>
    [DataMember(Name="mainPhone", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mainPhone")]
    public string MainPhone { get; set; }

    /// <summary>
    /// Gets or Sets Extension
    /// </summary>
    [DataMember(Name="extension", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "extension")]
    public string Extension { get; set; }

    /// <summary>
    /// Gets or Sets DefaultMarginTypeJetA
    /// </summary>
    [DataMember(Name="defaultMarginTypeJetA", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "defaultMarginTypeJetA")]
    public int? DefaultMarginTypeJetA { get; set; }

    /// <summary>
    /// Gets or Sets DefaultMarginType100Ll
    /// </summary>
    [DataMember(Name="defaultMarginType100Ll", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "defaultMarginType100Ll")]
    public int? DefaultMarginType100Ll { get; set; }

    /// <summary>
    /// Gets or Sets SalesTax
    /// </summary>
    [DataMember(Name="salesTax", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "salesTax")]
    public bool? SalesTax { get; set; }

    /// <summary>
    /// Gets or Sets ApplySalesTax
    /// </summary>
    [DataMember(Name="applySalesTax", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applySalesTax")]
    public bool? ApplySalesTax { get; set; }

    /// <summary>
    /// Gets or Sets LastLogin
    /// </summary>
    [DataMember(Name="lastLogin", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lastLogin")]
    public DateTime? LastLogin { get; set; }

    /// <summary>
    /// Gets or Sets PriceUpdateReminderPrompt
    /// </summary>
    [DataMember(Name="priceUpdateReminderPrompt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceUpdateReminderPrompt")]
    public bool? PriceUpdateReminderPrompt { get; set; }

    /// <summary>
    /// Gets or Sets PriceUpdateNeverPrompt
    /// </summary>
    [DataMember(Name="priceUpdateNeverPrompt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceUpdateNeverPrompt")]
    public bool? PriceUpdateNeverPrompt { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Gets or Sets InitialSetupPhase
    /// </summary>
    [DataMember(Name="initialSetupPhase", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "initialSetupPhase")]
    public bool? InitialSetupPhase { get; set; }

    /// <summary>
    /// Gets or Sets DisableCost
    /// </summary>
    [DataMember(Name="disableCost", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "disableCost")]
    public bool? DisableCost { get; set; }

    /// <summary>
    /// Gets or Sets AcukwikFBOHandlerId
    /// </summary>
    [DataMember(Name="acukwikFBOHandlerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "acukwikFBOHandlerId")]
    public int? AcukwikFBOHandlerId { get; set; }

    /// <summary>
    /// Gets or Sets Group
    /// </summary>
    [DataMember(Name="group", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "group")]
    public FboLinxGroup Group { get; set; }

    /// <summary>
    /// Gets or Sets FboAirport
    /// </summary>
    [DataMember(Name="fboAirport", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboAirport")]
    public FboAirport FboAirport { get; set; }

    /// <summary>
    /// Gets or Sets Contacts
    /// </summary>
    [DataMember(Name="contacts", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "contacts")]
    public List<FBOContact> Contacts { get; set; }

    /// <summary>
    /// Gets or Sets ContactEmail
    /// </summary>
    [DataMember(Name="contactEmail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "contactEmail")]
    public string ContactEmail { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FboLinxFbo {\n");
      sb.Append("  Oid: ").Append(Oid).Append("\n");
      sb.Append("  Fbo: ").Append(Fbo).Append("\n");
      sb.Append("  GroupId: ").Append(GroupId).Append("\n");
      sb.Append("  PostedRetail: ").Append(PostedRetail).Append("\n");
      sb.Append("  TotalCost: ").Append(TotalCost).Append("\n");
      sb.Append("  Active: ").Append(Active).Append("\n");
      sb.Append("  DateActivated: ").Append(DateActivated).Append("\n");
      sb.Append("  Address: ").Append(Address).Append("\n");
      sb.Append("  City: ").Append(City).Append("\n");
      sb.Append("  State: ").Append(State).Append("\n");
      sb.Append("  ZipCode: ").Append(ZipCode).Append("\n");
      sb.Append("  Country: ").Append(Country).Append("\n");
      sb.Append("  TotalCost100Ll: ").Append(TotalCost100Ll).Append("\n");
      sb.Append("  PostedRetail100Ll: ").Append(PostedRetail100Ll).Append("\n");
      sb.Append("  Suspended: ").Append(Suspended).Append("\n");
      sb.Append("  GroupMargin: ").Append(GroupMargin).Append("\n");
      sb.Append("  ApplyGroupMargin: ").Append(ApplyGroupMargin).Append("\n");
      sb.Append("  GroupMarginSetting: ").Append(GroupMarginSetting).Append("\n");
      sb.Append("  GroupMarginFuture: ").Append(GroupMarginFuture).Append("\n");
      sb.Append("  GroupMarginTemplate: ").Append(GroupMarginTemplate).Append("\n");
      sb.Append("  GroupMarginMargin: ").Append(GroupMarginMargin).Append("\n");
      sb.Append("  GroupMarginType: ").Append(GroupMarginType).Append("\n");
      sb.Append("  GroupMargin100Llmargin: ").Append(GroupMargin100Llmargin).Append("\n");
      sb.Append("  GroupMargin100Lltype: ").Append(GroupMargin100Lltype).Append("\n");
      sb.Append("  FuelDeskEmail: ").Append(FuelDeskEmail).Append("\n");
      sb.Append("  Website: ").Append(Website).Append("\n");
      sb.Append("  MainPhone: ").Append(MainPhone).Append("\n");
      sb.Append("  Extension: ").Append(Extension).Append("\n");
      sb.Append("  DefaultMarginTypeJetA: ").Append(DefaultMarginTypeJetA).Append("\n");
      sb.Append("  DefaultMarginType100Ll: ").Append(DefaultMarginType100Ll).Append("\n");
      sb.Append("  SalesTax: ").Append(SalesTax).Append("\n");
      sb.Append("  ApplySalesTax: ").Append(ApplySalesTax).Append("\n");
      sb.Append("  LastLogin: ").Append(LastLogin).Append("\n");
      sb.Append("  PriceUpdateReminderPrompt: ").Append(PriceUpdateReminderPrompt).Append("\n");
      sb.Append("  PriceUpdateNeverPrompt: ").Append(PriceUpdateNeverPrompt).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      sb.Append("  InitialSetupPhase: ").Append(InitialSetupPhase).Append("\n");
      sb.Append("  DisableCost: ").Append(DisableCost).Append("\n");
      sb.Append("  AcukwikFBOHandlerId: ").Append(AcukwikFBOHandlerId).Append("\n");
      sb.Append("  Group: ").Append(Group).Append("\n");
      sb.Append("  FboAirport: ").Append(FboAirport).Append("\n");
      sb.Append("  Contacts: ").Append(Contacts).Append("\n");
      sb.Append("  ContactEmail: ").Append(ContactEmail).Append("\n");
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
