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
  public class CompanyAccountSettingsDTO {
    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets ExcludeRampFeesFromSavings
    /// </summary>
    [DataMember(Name="excludeRampFeesFromSavings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "excludeRampFeesFromSavings")]
    public bool? ExcludeRampFeesFromSavings { get; set; }

    /// <summary>
    /// Gets or Sets FuelRequestEmailModification
    /// </summary>
    [DataMember(Name="fuelRequestEmailModification", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelRequestEmailModification")]
    public bool? FuelRequestEmailModification { get; set; }

    /// <summary>
    /// Gets or Sets HomeBase
    /// </summary>
    [DataMember(Name="homeBase", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "homeBase")]
    public string HomeBase { get; set; }

    /// <summary>
    /// Gets or Sets PriceSync
    /// </summary>
    [DataMember(Name="priceSync", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "priceSync")]
    public bool? PriceSync { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="schedulingIntegrationType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "schedulingIntegrationType")]
    public int? SchedulingIntegrationType { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingIntegrationTypeDescription
    /// </summary>
    [DataMember(Name="schedulingIntegrationTypeDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "schedulingIntegrationTypeDescription")]
    public string SchedulingIntegrationTypeDescription { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingUserName
    /// </summary>
    [DataMember(Name="schedulingUserName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "schedulingUserName")]
    public string SchedulingUserName { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingPassword
    /// </summary>
    [DataMember(Name="schedulingPassword", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "schedulingPassword")]
    public string SchedulingPassword { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingDataPath
    /// </summary>
    [DataMember(Name="schedulingDataPath", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "schedulingDataPath")]
    public string SchedulingDataPath { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingServerName
    /// </summary>
    [DataMember(Name="schedulingServerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "schedulingServerName")]
    public string SchedulingServerName { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingAccountNumber
    /// </summary>
    [DataMember(Name="schedulingAccountNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "schedulingAccountNumber")]
    public string SchedulingAccountNumber { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingDaysOut
    /// </summary>
    [DataMember(Name="schedulingDaysOut", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "schedulingDaysOut")]
    public int? SchedulingDaysOut { get; set; }

    /// <summary>
    /// Gets or Sets UsesMechTurkForReconciling
    /// </summary>
    [DataMember(Name="usesMechTurkForReconciling", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "usesMechTurkForReconciling")]
    public bool? UsesMechTurkForReconciling { get; set; }

    /// <summary>
    /// Gets or Sets OmitVendorInPriceDisplay
    /// </summary>
    [DataMember(Name="omitVendorInPriceDisplay", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "omitVendorInPriceDisplay")]
    public bool? OmitVendorInPriceDisplay { get; set; }

    /// <summary>
    /// Gets or Sets IsWebPulledPricingDisabled
    /// </summary>
    [DataMember(Name="isWebPulledPricingDisabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isWebPulledPricingDisabled")]
    public bool? IsWebPulledPricingDisabled { get; set; }

    /// <summary>
    /// Gets or Sets IsVATRecoveryEnabled
    /// </summary>
    [DataMember(Name="isVATRecoveryEnabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isVATRecoveryEnabled")]
    public bool? IsVATRecoveryEnabled { get; set; }

    /// <summary>
    /// Gets or Sets DisableCrowdSourcedRampFees
    /// </summary>
    [DataMember(Name="disableCrowdSourcedRampFees", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "disableCrowdSourcedRampFees")]
    public bool? DisableCrowdSourcedRampFees { get; set; }

    /// <summary>
    /// Gets or Sets ReconcileMatchingInvoicedTransactions
    /// </summary>
    [DataMember(Name="reconcileMatchingInvoicedTransactions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reconcileMatchingInvoicedTransactions")]
    public bool? ReconcileMatchingInvoicedTransactions { get; set; }

    /// <summary>
    /// Gets or Sets KeepManualPriceVendorWhenInvoiced
    /// </summary>
    [DataMember(Name="keepManualPriceVendorWhenInvoiced", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "keepManualPriceVendorWhenInvoiced")]
    public bool? KeepManualPriceVendorWhenInvoiced { get; set; }

    /// <summary>
    /// Gets or Sets MatchInvoicedTransactionsToScheduling
    /// </summary>
    [DataMember(Name="matchInvoicedTransactionsToScheduling", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "matchInvoicedTransactionsToScheduling")]
    public bool? MatchInvoicedTransactionsToScheduling { get; set; }

    /// <summary>
    /// Gets or Sets ForceReconcileDiscrepancyInFavor
    /// </summary>
    [DataMember(Name="forceReconcileDiscrepancyInFavor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "forceReconcileDiscrepancyInFavor")]
    public bool? ForceReconcileDiscrepancyInFavor { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CompanyAccountSettingsDTO {\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  ExcludeRampFeesFromSavings: ").Append(ExcludeRampFeesFromSavings).Append("\n");
      sb.Append("  FuelRequestEmailModification: ").Append(FuelRequestEmailModification).Append("\n");
      sb.Append("  HomeBase: ").Append(HomeBase).Append("\n");
      sb.Append("  PriceSync: ").Append(PriceSync).Append("\n");
      sb.Append("  SchedulingIntegrationType: ").Append(SchedulingIntegrationType).Append("\n");
      sb.Append("  SchedulingIntegrationTypeDescription: ").Append(SchedulingIntegrationTypeDescription).Append("\n");
      sb.Append("  SchedulingUserName: ").Append(SchedulingUserName).Append("\n");
      sb.Append("  SchedulingPassword: ").Append(SchedulingPassword).Append("\n");
      sb.Append("  SchedulingDataPath: ").Append(SchedulingDataPath).Append("\n");
      sb.Append("  SchedulingServerName: ").Append(SchedulingServerName).Append("\n");
      sb.Append("  SchedulingAccountNumber: ").Append(SchedulingAccountNumber).Append("\n");
      sb.Append("  SchedulingDaysOut: ").Append(SchedulingDaysOut).Append("\n");
      sb.Append("  UsesMechTurkForReconciling: ").Append(UsesMechTurkForReconciling).Append("\n");
      sb.Append("  OmitVendorInPriceDisplay: ").Append(OmitVendorInPriceDisplay).Append("\n");
      sb.Append("  IsWebPulledPricingDisabled: ").Append(IsWebPulledPricingDisabled).Append("\n");
      sb.Append("  IsVATRecoveryEnabled: ").Append(IsVATRecoveryEnabled).Append("\n");
      sb.Append("  DisableCrowdSourcedRampFees: ").Append(DisableCrowdSourcedRampFees).Append("\n");
      sb.Append("  ReconcileMatchingInvoicedTransactions: ").Append(ReconcileMatchingInvoicedTransactions).Append("\n");
      sb.Append("  KeepManualPriceVendorWhenInvoiced: ").Append(KeepManualPriceVendorWhenInvoiced).Append("\n");
      sb.Append("  MatchInvoicedTransactionsToScheduling: ").Append(MatchInvoicedTransactionsToScheduling).Append("\n");
      sb.Append("  ForceReconcileDiscrepancyInFavor: ").Append(ForceReconcileDiscrepancyInFavor).Append("\n");
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
