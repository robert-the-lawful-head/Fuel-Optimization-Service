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
  public class PreferenceDTO {
    /// <summary>
    /// Gets or Sets UserID
    /// </summary>
    [DataMember(Name="UserID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "UserID")]
    public int? UserID { get; set; }

    /// <summary>
    /// Gets or Sets FuelOptionSorting
    /// </summary>
    [DataMember(Name="FuelOptionSorting", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FuelOptionSorting")]
    public int? FuelOptionSorting { get; set; }

    /// <summary>
    /// Gets or Sets IsAllInclusivePricingPrioritized
    /// </summary>
    [DataMember(Name="IsAllInclusivePricingPrioritized", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "IsAllInclusivePricingPrioritized")]
    public bool? IsAllInclusivePricingPrioritized { get; set; }

    /// <summary>
    /// Gets or Sets SortByAllInclusivePricing
    /// </summary>
    [DataMember(Name="SortByAllInclusivePricing", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "SortByAllInclusivePricing")]
    public bool? SortByAllInclusivePricing { get; set; }

    /// <summary>
    /// Gets or Sets IsPriceMasked
    /// </summary>
    [DataMember(Name="IsPriceMasked", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "IsPriceMasked")]
    public bool? IsPriceMasked { get; set; }

    /// <summary>
    /// Gets or Sets OmitFromDispatchConfirmation
    /// </summary>
    [DataMember(Name="OmitFromDispatchConfirmation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "OmitFromDispatchConfirmation")]
    public bool? OmitFromDispatchConfirmation { get; set; }

    /// <summary>
    /// Gets or Sets FuelOn
    /// </summary>
    [DataMember(Name="FuelOn", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FuelOn")]
    public string FuelOn { get; set; }

    /// <summary>
    /// Gets or Sets ExcludeAllEmailsByDefault
    /// </summary>
    [DataMember(Name="ExcludeAllEmailsByDefault", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ExcludeAllEmailsByDefault")]
    public bool? ExcludeAllEmailsByDefault { get; set; }

    /// <summary>
    /// Site Modes:             Live = 0,             Test = 1,             Local = 2,             Beta = 3,             Staging = 4,             Classic = 5,             Rollback = 6    * `Live` - Live  * `Test` - Test  * `Local` - Local  * `Beta` - Beta  * `Staging` - Staging  * `Classic` - Classic  * `Rollback` - Rollback  
    /// </summary>
    /// <value>Site Modes:             Live = 0,             Test = 1,             Local = 2,             Beta = 3,             Staging = 4,             Classic = 5,             Rollback = 6    * `Live` - Live  * `Test` - Test  * `Local` - Local  * `Beta` - Beta  * `Staging` - Staging  * `Classic` - Classic  * `Rollback` - Rollback  </value>
    [DataMember(Name="SiteMode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "SiteMode")]
    public int? SiteMode { get; set; }

    /// <summary>
    /// Gets or Sets HandlerEmails
    /// </summary>
    [DataMember(Name="HandlerEmails", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "HandlerEmails")]
    public List<UserEmailDTO> HandlerEmails { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="Currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Gets or Sets ExcludeTakingFuelByDefault
    /// </summary>
    [DataMember(Name="ExcludeTakingFuelByDefault", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ExcludeTakingFuelByDefault")]
    public bool? ExcludeTakingFuelByDefault { get; set; }

    /// <summary>
    /// Gets or Sets CollapseLegsByDefault
    /// </summary>
    [DataMember(Name="CollapseLegsByDefault", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "CollapseLegsByDefault")]
    public bool? CollapseLegsByDefault { get; set; }

    /// <summary>
    /// Gets or Sets CreateTransactionsForSkippedLegs
    /// </summary>
    [DataMember(Name="CreateTransactionsForSkippedLegs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "CreateTransactionsForSkippedLegs")]
    public bool? CreateTransactionsForSkippedLegs { get; set; }

    /// <summary>
    /// Gets or Sets AddOneGalToSkipFuel
    /// </summary>
    [DataMember(Name="AddOneGalToSkipFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "AddOneGalToSkipFuel")]
    public bool? AddOneGalToSkipFuel { get; set; }

    /// <summary>
    /// Gets or Sets CopyPrimaryAccountOnDispatch
    /// </summary>
    [DataMember(Name="CopyPrimaryAccountOnDispatch", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "CopyPrimaryAccountOnDispatch")]
    public bool? CopyPrimaryAccountOnDispatch { get; set; }

    /// <summary>
    /// Gets or Sets ShowPreferredFBOsOnly
    /// </summary>
    [DataMember(Name="ShowPreferredFBOsOnly", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ShowPreferredFBOsOnly")]
    public bool? ShowPreferredFBOsOnly { get; set; }

    /// <summary>
    /// Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  
    /// </summary>
    /// <value>Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  </value>
    [DataMember(Name="FuelOptionsWeightUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FuelOptionsWeightUnit")]
    public int? FuelOptionsWeightUnit { get; set; }

    /// <summary>
    /// Gets or Sets ShowPostedRetail
    /// </summary>
    [DataMember(Name="ShowPostedRetail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ShowPostedRetail")]
    public bool? ShowPostedRetail { get; set; }

    /// <summary>
    /// Gets or Sets CCOnVendorDispatchEmail
    /// </summary>
    [DataMember(Name="CCOnVendorDispatchEmail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "CCOnVendorDispatchEmail")]
    public bool? CCOnVendorDispatchEmail { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="TimePreference", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "TimePreference")]
    public int? TimePreference { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PreferenceDTO {\n");
      sb.Append("  UserID: ").Append(UserID).Append("\n");
      sb.Append("  FuelOptionSorting: ").Append(FuelOptionSorting).Append("\n");
      sb.Append("  IsAllInclusivePricingPrioritized: ").Append(IsAllInclusivePricingPrioritized).Append("\n");
      sb.Append("  SortByAllInclusivePricing: ").Append(SortByAllInclusivePricing).Append("\n");
      sb.Append("  IsPriceMasked: ").Append(IsPriceMasked).Append("\n");
      sb.Append("  OmitFromDispatchConfirmation: ").Append(OmitFromDispatchConfirmation).Append("\n");
      sb.Append("  FuelOn: ").Append(FuelOn).Append("\n");
      sb.Append("  ExcludeAllEmailsByDefault: ").Append(ExcludeAllEmailsByDefault).Append("\n");
      sb.Append("  SiteMode: ").Append(SiteMode).Append("\n");
      sb.Append("  HandlerEmails: ").Append(HandlerEmails).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      sb.Append("  ExcludeTakingFuelByDefault: ").Append(ExcludeTakingFuelByDefault).Append("\n");
      sb.Append("  CollapseLegsByDefault: ").Append(CollapseLegsByDefault).Append("\n");
      sb.Append("  CreateTransactionsForSkippedLegs: ").Append(CreateTransactionsForSkippedLegs).Append("\n");
      sb.Append("  AddOneGalToSkipFuel: ").Append(AddOneGalToSkipFuel).Append("\n");
      sb.Append("  CopyPrimaryAccountOnDispatch: ").Append(CopyPrimaryAccountOnDispatch).Append("\n");
      sb.Append("  ShowPreferredFBOsOnly: ").Append(ShowPreferredFBOsOnly).Append("\n");
      sb.Append("  FuelOptionsWeightUnit: ").Append(FuelOptionsWeightUnit).Append("\n");
      sb.Append("  ShowPostedRetail: ").Append(ShowPostedRetail).Append("\n");
      sb.Append("  CCOnVendorDispatchEmail: ").Append(CCOnVendorDispatchEmail).Append("\n");
      sb.Append("  TimePreference: ").Append(TimePreference).Append("\n");
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
