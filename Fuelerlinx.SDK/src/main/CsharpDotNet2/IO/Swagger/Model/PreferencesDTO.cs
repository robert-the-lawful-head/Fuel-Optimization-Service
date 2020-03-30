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
  public class PreferencesDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets UserId
    /// </summary>
    [DataMember(Name="userId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userId")]
    public int? UserId { get; set; }

    /// <summary>
    /// Gets or Sets FuelOptionSorting
    /// </summary>
    [DataMember(Name="fuelOptionSorting", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelOptionSorting")]
    public int? FuelOptionSorting { get; set; }

    /// <summary>
    /// Gets or Sets IsAllInclusivePricingPrioritized
    /// </summary>
    [DataMember(Name="isAllInclusivePricingPrioritized", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isAllInclusivePricingPrioritized")]
    public bool? IsAllInclusivePricingPrioritized { get; set; }

    /// <summary>
    /// Gets or Sets IsPostedRetailSeparated
    /// </summary>
    [DataMember(Name="isPostedRetailSeparated", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isPostedRetailSeparated")]
    public bool? IsPostedRetailSeparated { get; set; }

    /// <summary>
    /// Gets or Sets OmitFromDispatchConfirmation
    /// </summary>
    [DataMember(Name="omitFromDispatchConfirmation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "omitFromDispatchConfirmation")]
    public bool? OmitFromDispatchConfirmation { get; set; }

    /// <summary>
    /// Gets or Sets FuelOn
    /// </summary>
    [DataMember(Name="fuelOn", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelOn")]
    public string FuelOn { get; set; }

    /// <summary>
    /// Gets or Sets ExcludeAllEmailsByDefault
    /// </summary>
    [DataMember(Name="excludeAllEmailsByDefault", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "excludeAllEmailsByDefault")]
    public bool? ExcludeAllEmailsByDefault { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Gets or Sets SiteMode
    /// </summary>
    [DataMember(Name="siteMode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "siteMode")]
    public int? SiteMode { get; set; }

    /// <summary>
    /// Gets or Sets ExcludeTakingFuelByDefault
    /// </summary>
    [DataMember(Name="excludeTakingFuelByDefault", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "excludeTakingFuelByDefault")]
    public bool? ExcludeTakingFuelByDefault { get; set; }

    /// <summary>
    /// Gets or Sets CollapseLegsByDefault
    /// </summary>
    [DataMember(Name="collapseLegsByDefault", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "collapseLegsByDefault")]
    public bool? CollapseLegsByDefault { get; set; }

    /// <summary>
    /// Gets or Sets CreateTransactionsForSkippedLegs
    /// </summary>
    [DataMember(Name="createTransactionsForSkippedLegs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "createTransactionsForSkippedLegs")]
    public bool? CreateTransactionsForSkippedLegs { get; set; }

    /// <summary>
    /// Gets or Sets CopyPrimaryAccountOnDispatch
    /// </summary>
    [DataMember(Name="copyPrimaryAccountOnDispatch", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "copyPrimaryAccountOnDispatch")]
    public bool? CopyPrimaryAccountOnDispatch { get; set; }

    /// <summary>
    /// Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  
    /// </summary>
    /// <value>Weight units:             0 = Gallons             1 = Pounds             2 = Kilograms             3 = Tonnes             4 = Liters    * `Gallons` - Gallons  * `Pounds` - Pounds  * `Kilograms` - Kilograms  * `Tonnes` - Tonnes  * `Liters` - Liters  </value>
    [DataMember(Name="fuelOptionsWeightUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelOptionsWeightUnit")]
    public int? FuelOptionsWeightUnit { get; set; }

    /// <summary>
    /// Gets or Sets ShowPreferredFbosOnly
    /// </summary>
    [DataMember(Name="showPreferredFbosOnly", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "showPreferredFbosOnly")]
    public bool? ShowPreferredFbosOnly { get; set; }

    /// <summary>
    /// Gets or Sets ShowPostedRetail
    /// </summary>
    [DataMember(Name="showPostedRetail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "showPostedRetail")]
    public bool? ShowPostedRetail { get; set; }

    /// <summary>
    /// Gets or Sets CopyOnVendorDispatchEmail
    /// </summary>
    [DataMember(Name="copyOnVendorDispatchEmail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "copyOnVendorDispatchEmail")]
    public bool? CopyOnVendorDispatchEmail { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="timePreference", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timePreference")]
    public int? TimePreference { get; set; }

    /// <summary>
    /// Gets or Sets IsPriceMasked
    /// </summary>
    [DataMember(Name="isPriceMasked", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isPriceMasked")]
    public bool? IsPriceMasked { get; set; }

    /// <summary>
    /// Gets or Sets AddOneGalToSkipFuel
    /// </summary>
    [DataMember(Name="addOneGalToSkipFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addOneGalToSkipFuel")]
    public bool? AddOneGalToSkipFuel { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PreferencesDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  FuelOptionSorting: ").Append(FuelOptionSorting).Append("\n");
      sb.Append("  IsAllInclusivePricingPrioritized: ").Append(IsAllInclusivePricingPrioritized).Append("\n");
      sb.Append("  IsPostedRetailSeparated: ").Append(IsPostedRetailSeparated).Append("\n");
      sb.Append("  OmitFromDispatchConfirmation: ").Append(OmitFromDispatchConfirmation).Append("\n");
      sb.Append("  FuelOn: ").Append(FuelOn).Append("\n");
      sb.Append("  ExcludeAllEmailsByDefault: ").Append(ExcludeAllEmailsByDefault).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      sb.Append("  SiteMode: ").Append(SiteMode).Append("\n");
      sb.Append("  ExcludeTakingFuelByDefault: ").Append(ExcludeTakingFuelByDefault).Append("\n");
      sb.Append("  CollapseLegsByDefault: ").Append(CollapseLegsByDefault).Append("\n");
      sb.Append("  CreateTransactionsForSkippedLegs: ").Append(CreateTransactionsForSkippedLegs).Append("\n");
      sb.Append("  CopyPrimaryAccountOnDispatch: ").Append(CopyPrimaryAccountOnDispatch).Append("\n");
      sb.Append("  FuelOptionsWeightUnit: ").Append(FuelOptionsWeightUnit).Append("\n");
      sb.Append("  ShowPreferredFbosOnly: ").Append(ShowPreferredFbosOnly).Append("\n");
      sb.Append("  ShowPostedRetail: ").Append(ShowPostedRetail).Append("\n");
      sb.Append("  CopyOnVendorDispatchEmail: ").Append(CopyOnVendorDispatchEmail).Append("\n");
      sb.Append("  TimePreference: ").Append(TimePreference).Append("\n");
      sb.Append("  IsPriceMasked: ").Append(IsPriceMasked).Append("\n");
      sb.Append("  AddOneGalToSkipFuel: ").Append(AddOneGalToSkipFuel).Append("\n");
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
