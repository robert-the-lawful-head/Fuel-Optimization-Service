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
  public class CompanyFuelerSettingsDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets CompanyFuelerId
    /// </summary>
    [DataMember(Name="companyFuelerId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyFuelerId")]
    public int? CompanyFuelerId { get; set; }

    /// <summary>
    /// Gets or Sets QbvendorName
    /// </summary>
    [DataMember(Name="qbvendorName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "qbvendorName")]
    public string QbvendorName { get; set; }

    /// <summary>
    /// Gets or Sets DiscrepancyMaxPriceDif
    /// </summary>
    [DataMember(Name="discrepancyMaxPriceDif", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "discrepancyMaxPriceDif")]
    public double? DiscrepancyMaxPriceDif { get; set; }

    /// <summary>
    /// Gets or Sets IsNewlyAdded
    /// </summary>
    [DataMember(Name="isNewlyAdded", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isNewlyAdded")]
    public bool? IsNewlyAdded { get; set; }

    /// <summary>
    /// Gets or Sets UserName
    /// </summary>
    [DataMember(Name="userName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userName")]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or Sets Password
    /// </summary>
    [DataMember(Name="password", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "password")]
    public string Password { get; set; }

    /// <summary>
    /// Gets or Sets AccountNumber
    /// </summary>
    [DataMember(Name="accountNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "accountNumber")]
    public string AccountNumber { get; set; }

    /// <summary>
    /// Gets or Sets ShowBasePriceByDefault
    /// </summary>
    [DataMember(Name="showBasePriceByDefault", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "showBasePriceByDefault")]
    public bool? ShowBasePriceByDefault { get; set; }

    /// <summary>
    /// Gets or Sets Alias
    /// </summary>
    [DataMember(Name="alias", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "alias")]
    public string Alias { get; set; }

    /// <summary>
    /// Gets or Sets PaymentMethod
    /// </summary>
    [DataMember(Name="paymentMethod", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paymentMethod")]
    public string PaymentMethod { get; set; }

    /// <summary>
    /// Gets or Sets DisableWebPull
    /// </summary>
    [DataMember(Name="disableWebPull", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "disableWebPull")]
    public bool? DisableWebPull { get; set; }

    /// <summary>
    /// Gets or Sets VendorLinxGuid
    /// </summary>
    [DataMember(Name="vendorLinxGuid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "vendorLinxGuid")]
    public Guid? VendorLinxGuid { get; set; }

    /// <summary>
    /// Gets or Sets TailNumbers
    /// </summary>
    [DataMember(Name="tailNumbers", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumbers")]
    public string TailNumbers { get; set; }

    /// <summary>
    /// Gets or Sets DispatchEmail
    /// </summary>
    [DataMember(Name="dispatchEmail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dispatchEmail")]
    public string DispatchEmail { get; set; }

    /// <summary>
    /// Gets or Sets SupplierId
    /// </summary>
    [DataMember(Name="supplierId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supplierId")]
    public int? SupplierId { get; set; }

    /// <summary>
    /// Transaction Reconciliation Comparisons:             0 = Quoted price-per-unit             1 = Company-adjusted price-per-unit             2 = Quoted price-per-unit and quoted volume             3 = Company-adjusted price-per-unit and quoted volume    * `QuotedUnitPrice` - Quoted price-per-unit  * `CompanyAdjustedUnitPrice` - Company-adjusted price-per-unit  * `QuotedUnitPriceAndVolume` - Quoted price-per-unit and quoted volume  * `CompanyAdjustedUnitPriceAndVolume` - Company-adjusted price-per-unit and quoted volume  
    /// </summary>
    /// <value>Transaction Reconciliation Comparisons:             0 = Quoted price-per-unit             1 = Company-adjusted price-per-unit             2 = Quoted price-per-unit and quoted volume             3 = Company-adjusted price-per-unit and quoted volume    * `QuotedUnitPrice` - Quoted price-per-unit  * `CompanyAdjustedUnitPrice` - Company-adjusted price-per-unit  * `QuotedUnitPriceAndVolume` - Quoted price-per-unit and quoted volume  * `CompanyAdjustedUnitPriceAndVolume` - Company-adjusted price-per-unit and quoted volume  </value>
    [DataMember(Name="transactionReconciliationComparisonType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionReconciliationComparisonType")]
    public int? TransactionReconciliationComparisonType { get; set; }

    /// <summary>
    /// Gets or Sets CredentialsLastVerified
    /// </summary>
    [DataMember(Name="credentialsLastVerified", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "credentialsLastVerified")]
    public DateTime? CredentialsLastVerified { get; set; }

    /// <summary>
    /// Gets or Sets SupplierDetails
    /// </summary>
    [DataMember(Name="supplierDetails", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supplierDetails")]
    public SupplierDetailsDTO SupplierDetails { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CompanyFuelerSettingsDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CompanyFuelerId: ").Append(CompanyFuelerId).Append("\n");
      sb.Append("  QbvendorName: ").Append(QbvendorName).Append("\n");
      sb.Append("  DiscrepancyMaxPriceDif: ").Append(DiscrepancyMaxPriceDif).Append("\n");
      sb.Append("  IsNewlyAdded: ").Append(IsNewlyAdded).Append("\n");
      sb.Append("  UserName: ").Append(UserName).Append("\n");
      sb.Append("  Password: ").Append(Password).Append("\n");
      sb.Append("  AccountNumber: ").Append(AccountNumber).Append("\n");
      sb.Append("  ShowBasePriceByDefault: ").Append(ShowBasePriceByDefault).Append("\n");
      sb.Append("  Alias: ").Append(Alias).Append("\n");
      sb.Append("  PaymentMethod: ").Append(PaymentMethod).Append("\n");
      sb.Append("  DisableWebPull: ").Append(DisableWebPull).Append("\n");
      sb.Append("  VendorLinxGuid: ").Append(VendorLinxGuid).Append("\n");
      sb.Append("  TailNumbers: ").Append(TailNumbers).Append("\n");
      sb.Append("  DispatchEmail: ").Append(DispatchEmail).Append("\n");
      sb.Append("  SupplierId: ").Append(SupplierId).Append("\n");
      sb.Append("  TransactionReconciliationComparisonType: ").Append(TransactionReconciliationComparisonType).Append("\n");
      sb.Append("  CredentialsLastVerified: ").Append(CredentialsLastVerified).Append("\n");
      sb.Append("  SupplierDetails: ").Append(SupplierDetails).Append("\n");
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
