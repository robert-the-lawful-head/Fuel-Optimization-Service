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
  public class OracleAccountingExportResultDTO {
    /// <summary>
    /// Gets or Sets InvoiceHeaderIdentifier
    /// </summary>
    [DataMember(Name="invoiceHeaderIdentifier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoiceHeaderIdentifier")]
    public long? InvoiceHeaderIdentifier { get; set; }

    /// <summary>
    /// Gets or Sets BusinessUnit
    /// </summary>
    [DataMember(Name="businessUnit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "businessUnit")]
    public string BusinessUnit { get; set; }

    /// <summary>
    /// Invoice Number
    /// </summary>
    /// <value>Invoice Number</value>
    [DataMember(Name="invoiceNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoiceNumber")]
    public string InvoiceNumber { get; set; }

    /// <summary>
    /// Invoice Amount
    /// </summary>
    /// <value>Invoice Amount</value>
    [DataMember(Name="invoiceAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoiceAmount")]
    public double? InvoiceAmount { get; set; }

    /// <summary>
    /// Gets or Sets InvoiceDate
    /// </summary>
    [DataMember(Name="invoiceDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoiceDate")]
    public string InvoiceDate { get; set; }

    /// <summary>
    /// Gets or Sets Vendor
    /// </summary>
    [DataMember(Name="vendor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "vendor")]
    public string Vendor { get; set; }

    /// <summary>
    /// Gets or Sets VendorNumber
    /// </summary>
    [DataMember(Name="vendorNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "vendorNumber")]
    public string VendorNumber { get; set; }

    /// <summary>
    /// Gets or Sets SupplierSite
    /// </summary>
    [DataMember(Name="supplierSite", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supplierSite")]
    public string SupplierSite { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Gets or Sets InvoiceType
    /// </summary>
    [DataMember(Name="invoiceType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoiceType")]
    public string InvoiceType { get; set; }

    /// <summary>
    /// Gets or Sets PaymentTerms
    /// </summary>
    [DataMember(Name="paymentTerms", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paymentTerms")]
    public string PaymentTerms { get; set; }

    /// <summary>
    /// Gets or Sets PayGroup
    /// </summary>
    [DataMember(Name="payGroup", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "payGroup")]
    public string PayGroup { get; set; }

    /// <summary>
    /// Gets or Sets PaymentReasonComments
    /// </summary>
    [DataMember(Name="paymentReasonComments", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paymentReasonComments")]
    public string PaymentReasonComments { get; set; }

    /// <summary>
    /// Gets or Sets Line
    /// </summary>
    [DataMember(Name="line", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "line")]
    public long? Line { get; set; }

    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name="type", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    /// <summary>
    /// Gets or Sets Amount
    /// </summary>
    [DataMember(Name="amount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "amount")]
    public double? Amount { get; set; }

    /// <summary>
    /// Gets or Sets QuotedQty
    /// </summary>
    [DataMember(Name="quotedQty", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "quotedQty")]
    public double? QuotedQty { get; set; }

    /// <summary>
    /// Gets or Sets UnitPrice
    /// </summary>
    [DataMember(Name="unitPrice", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "unitPrice")]
    public double? UnitPrice { get; set; }

    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets DistributionCombination
    /// </summary>
    [DataMember(Name="distributionCombination", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "distributionCombination")]
    public string DistributionCombination { get; set; }

    /// <summary>
    /// Gets or Sets ContextValue
    /// </summary>
    [DataMember(Name="contextValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "contextValue")]
    public string ContextValue { get; set; }

    /// <summary>
    /// Gets or Sets AdditionalInformation
    /// </summary>
    [DataMember(Name="additionalInformation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "additionalInformation")]
    public double? AdditionalInformation { get; set; }

    /// <summary>
    /// Gets or Sets ProjectNumber
    /// </summary>
    [DataMember(Name="projectNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "projectNumber")]
    public string ProjectNumber { get; set; }

    /// <summary>
    /// Gets or Sets TripNumber
    /// </summary>
    [DataMember(Name="tripNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripNumber")]
    public string TripNumber { get; set; }

    /// <summary>
    /// Gets or Sets UpliftDate
    /// </summary>
    [DataMember(Name="upliftDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "upliftDate")]
    public string UpliftDate { get; set; }

    /// <summary>
    /// Gets or Sets AccountCode
    /// </summary>
    [DataMember(Name="accountCode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "accountCode")]
    public string AccountCode { get; set; }

    /// <summary>
    /// Gets or Sets Locations
    /// </summary>
    [DataMember(Name="locations", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "locations")]
    public string Locations { get; set; }

    /// <summary>
    /// Gets or Sets ProjectInformation
    /// </summary>
    [DataMember(Name="projectInformation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "projectInformation")]
    public string ProjectInformation { get; set; }

    /// <summary>
    /// Gets or Sets FuelerLinxTransactionId
    /// </summary>
    [DataMember(Name="fuelerLinxTransactionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelerLinxTransactionId")]
    public int? FuelerLinxTransactionId { get; set; }

    /// <summary>
    /// Gets or Sets AveragePPG
    /// </summary>
    [DataMember(Name="averagePPG", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "averagePPG")]
    public double? AveragePPG { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class OracleAccountingExportResultDTO {\n");
      sb.Append("  InvoiceHeaderIdentifier: ").Append(InvoiceHeaderIdentifier).Append("\n");
      sb.Append("  BusinessUnit: ").Append(BusinessUnit).Append("\n");
      sb.Append("  InvoiceNumber: ").Append(InvoiceNumber).Append("\n");
      sb.Append("  InvoiceAmount: ").Append(InvoiceAmount).Append("\n");
      sb.Append("  InvoiceDate: ").Append(InvoiceDate).Append("\n");
      sb.Append("  Vendor: ").Append(Vendor).Append("\n");
      sb.Append("  VendorNumber: ").Append(VendorNumber).Append("\n");
      sb.Append("  SupplierSite: ").Append(SupplierSite).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      sb.Append("  InvoiceType: ").Append(InvoiceType).Append("\n");
      sb.Append("  PaymentTerms: ").Append(PaymentTerms).Append("\n");
      sb.Append("  PayGroup: ").Append(PayGroup).Append("\n");
      sb.Append("  PaymentReasonComments: ").Append(PaymentReasonComments).Append("\n");
      sb.Append("  Line: ").Append(Line).Append("\n");
      sb.Append("  Type: ").Append(Type).Append("\n");
      sb.Append("  Amount: ").Append(Amount).Append("\n");
      sb.Append("  QuotedQty: ").Append(QuotedQty).Append("\n");
      sb.Append("  UnitPrice: ").Append(UnitPrice).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  DistributionCombination: ").Append(DistributionCombination).Append("\n");
      sb.Append("  ContextValue: ").Append(ContextValue).Append("\n");
      sb.Append("  AdditionalInformation: ").Append(AdditionalInformation).Append("\n");
      sb.Append("  ProjectNumber: ").Append(ProjectNumber).Append("\n");
      sb.Append("  TripNumber: ").Append(TripNumber).Append("\n");
      sb.Append("  UpliftDate: ").Append(UpliftDate).Append("\n");
      sb.Append("  AccountCode: ").Append(AccountCode).Append("\n");
      sb.Append("  Locations: ").Append(Locations).Append("\n");
      sb.Append("  ProjectInformation: ").Append(ProjectInformation).Append("\n");
      sb.Append("  FuelerLinxTransactionId: ").Append(FuelerLinxTransactionId).Append("\n");
      sb.Append("  AveragePPG: ").Append(AveragePPG).Append("\n");
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
