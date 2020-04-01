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
  public class TransactionGeneralInfoResultDTO {
    /// <summary>
    /// Gets or Sets OID
    /// </summary>
    [DataMember(Name="OID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "OID")]
    public int? OID { get; set; }

    /// <summary>
    /// Gets or Sets TripNumber
    /// </summary>
    [DataMember(Name="TripNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "TripNumber")]
    public string TripNumber { get; set; }

    /// <summary>
    /// Gets or Sets LegNumber
    /// </summary>
    [DataMember(Name="LegNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "LegNumber")]
    public int? LegNumber { get; set; }

    /// <summary>
    /// Gets or Sets TailNumber
    /// </summary>
    [DataMember(Name="TailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "TailNumber")]
    public string TailNumber { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalDate
    /// </summary>
    [DataMember(Name="ArrivalDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ArrivalDate")]
    public string ArrivalDate { get; set; }

    /// <summary>
    /// Gets or Sets DepartureDate
    /// </summary>
    [DataMember(Name="DepartureDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "DepartureDate")]
    public string DepartureDate { get; set; }

    /// <summary>
    /// Gets or Sets ServiceDate
    /// </summary>
    [DataMember(Name="ServiceDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ServiceDate")]
    public string ServiceDate { get; set; }

    /// <summary>
    /// Gets or Sets CreationDate
    /// </summary>
    [DataMember(Name="CreationDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "CreationDate")]
    public string CreationDate { get; set; }

    /// <summary>
    /// Gets or Sets ICAO
    /// </summary>
    [DataMember(Name="ICAO", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ICAO")]
    public string ICAO { get; set; }

    /// <summary>
    /// Gets or Sets Fueler
    /// </summary>
    [DataMember(Name="Fueler", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Fueler")]
    public string Fueler { get; set; }

    /// <summary>
    /// Gets or Sets QuotedPPG
    /// </summary>
    [DataMember(Name="QuotedPPG", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "QuotedPPG")]
    public double? QuotedPPG { get; set; }

    /// <summary>
    /// Gets or Sets InvoicedPPG
    /// </summary>
    [DataMember(Name="InvoicedPPG", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "InvoicedPPG")]
    public double? InvoicedPPG { get; set; }

    /// <summary>
    /// Gets or Sets QuotedFuelAmount
    /// </summary>
    [DataMember(Name="QuotedFuelAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "QuotedFuelAmount")]
    public int? QuotedFuelAmount { get; set; }

    /// <summary>
    /// Gets or Sets InvoicedFuelAmount
    /// </summary>
    [DataMember(Name="InvoicedFuelAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "InvoicedFuelAmount")]
    public double? InvoicedFuelAmount { get; set; }

    /// <summary>
    /// Gets or Sets FBO
    /// </summary>
    [DataMember(Name="FBO", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FBO")]
    public string FBO { get; set; }

    /// <summary>
    /// Gets or Sets Price
    /// </summary>
    [DataMember(Name="Price", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Price")]
    public double? Price { get; set; }

    /// <summary>
    /// Gets or Sets FuelAmount
    /// </summary>
    [DataMember(Name="FuelAmount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FuelAmount")]
    public double? FuelAmount { get; set; }

    /// <summary>
    /// Gets or Sets HasAttachments
    /// </summary>
    [DataMember(Name="HasAttachments", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "HasAttachments")]
    public bool? HasAttachments { get; set; }

    /// <summary>
    /// Gets or Sets InvoiceNumber
    /// </summary>
    [DataMember(Name="InvoiceNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "InvoiceNumber")]
    public string InvoiceNumber { get; set; }

    /// <summary>
    /// Gets or Sets InvoiceStatus
    /// </summary>
    [DataMember(Name="InvoiceStatus", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "InvoiceStatus")]
    public string InvoiceStatus { get; set; }

    /// <summary>
    /// Gets or Sets AddedByImport
    /// </summary>
    [DataMember(Name="AddedByImport", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "AddedByImport")]
    public bool? AddedByImport { get; set; }

    /// <summary>
    /// Gets or Sets DiscrepancyInImport
    /// </summary>
    [DataMember(Name="DiscrepancyInImport", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "DiscrepancyInImport")]
    public bool? DiscrepancyInImport { get; set; }

    /// <summary>
    /// Gets or Sets OriginalQuotedPPG
    /// </summary>
    [DataMember(Name="OriginalQuotedPPG", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "OriginalQuotedPPG")]
    public double? OriginalQuotedPPG { get; set; }

    /// <summary>
    /// Gets or Sets IsArchived
    /// </summary>
    [DataMember(Name="IsArchived", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "IsArchived")]
    public bool? IsArchived { get; set; }

    /// <summary>
    /// Gets or Sets FuelMasterID
    /// </summary>
    [DataMember(Name="FuelMasterID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FuelMasterID")]
    public int? FuelMasterID { get; set; }

    /// <summary>
    /// Gets or Sets FuelerID
    /// </summary>
    [DataMember(Name="FuelerID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "FuelerID")]
    public int? FuelerID { get; set; }

    /// <summary>
    /// Gets or Sets TaxStatus
    /// </summary>
    [DataMember(Name="TaxStatus", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "TaxStatus")]
    public int? TaxStatus { get; set; }

    /// <summary>
    /// Gets or Sets CompanyName
    /// </summary>
    [DataMember(Name="CompanyName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "CompanyName")]
    public string CompanyName { get; set; }

    /// <summary>
    /// Gets or Sets MemoFor
    /// </summary>
    [DataMember(Name="MemoFor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "MemoFor")]
    public int? MemoFor { get; set; }

    /// <summary>
    /// Gets or Sets RequestedBy
    /// </summary>
    [DataMember(Name="RequestedBy", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "RequestedBy")]
    public string RequestedBy { get; set; }

    /// <summary>
    /// Gets or Sets AdditionalFees
    /// </summary>
    [DataMember(Name="AdditionalFees", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "AdditionalFees")]
    public string AdditionalFees { get; set; }

    /// <summary>
    /// Gets or Sets AdditionalFeesTotal
    /// </summary>
    [DataMember(Name="AdditionalFeesTotal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "AdditionalFeesTotal")]
    public double? AdditionalFeesTotal { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalTime
    /// </summary>
    [DataMember(Name="ArrivalTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ArrivalTime")]
    public string ArrivalTime { get; set; }

    /// <summary>
    /// Gets or Sets DepartureTime
    /// </summary>
    [DataMember(Name="DepartureTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "DepartureTime")]
    public string DepartureTime { get; set; }

    /// <summary>
    /// Gets or Sets HasPaid
    /// </summary>
    [DataMember(Name="HasPaid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "HasPaid")]
    public bool? HasPaid { get; set; }

    /// <summary>
    /// Gets or Sets HasInvoicedBills
    /// </summary>
    [DataMember(Name="HasInvoicedBills", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "HasInvoicedBills")]
    public bool? HasInvoicedBills { get; set; }

    /// <summary>
    /// Gets or Sets CanPay
    /// </summary>
    [DataMember(Name="CanPay", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "CanPay")]
    public bool? CanPay { get; set; }

    /// <summary>
    /// Gets or Sets ACHMerchantProfileID
    /// </summary>
    [DataMember(Name="ACHMerchantProfileID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ACHMerchantProfileID")]
    public string ACHMerchantProfileID { get; set; }

    /// <summary>
    /// Gets or Sets CreditCardMerchantProfileID
    /// </summary>
    [DataMember(Name="CreditCardMerchantProfileID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "CreditCardMerchantProfileID")]
    public string CreditCardMerchantProfileID { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TransactionGeneralInfoResultDTO {\n");
      sb.Append("  OID: ").Append(OID).Append("\n");
      sb.Append("  TripNumber: ").Append(TripNumber).Append("\n");
      sb.Append("  LegNumber: ").Append(LegNumber).Append("\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
      sb.Append("  ArrivalDate: ").Append(ArrivalDate).Append("\n");
      sb.Append("  DepartureDate: ").Append(DepartureDate).Append("\n");
      sb.Append("  ServiceDate: ").Append(ServiceDate).Append("\n");
      sb.Append("  CreationDate: ").Append(CreationDate).Append("\n");
      sb.Append("  ICAO: ").Append(ICAO).Append("\n");
      sb.Append("  Fueler: ").Append(Fueler).Append("\n");
      sb.Append("  QuotedPPG: ").Append(QuotedPPG).Append("\n");
      sb.Append("  InvoicedPPG: ").Append(InvoicedPPG).Append("\n");
      sb.Append("  QuotedFuelAmount: ").Append(QuotedFuelAmount).Append("\n");
      sb.Append("  InvoicedFuelAmount: ").Append(InvoicedFuelAmount).Append("\n");
      sb.Append("  FBO: ").Append(FBO).Append("\n");
      sb.Append("  Price: ").Append(Price).Append("\n");
      sb.Append("  FuelAmount: ").Append(FuelAmount).Append("\n");
      sb.Append("  HasAttachments: ").Append(HasAttachments).Append("\n");
      sb.Append("  InvoiceNumber: ").Append(InvoiceNumber).Append("\n");
      sb.Append("  InvoiceStatus: ").Append(InvoiceStatus).Append("\n");
      sb.Append("  AddedByImport: ").Append(AddedByImport).Append("\n");
      sb.Append("  DiscrepancyInImport: ").Append(DiscrepancyInImport).Append("\n");
      sb.Append("  OriginalQuotedPPG: ").Append(OriginalQuotedPPG).Append("\n");
      sb.Append("  IsArchived: ").Append(IsArchived).Append("\n");
      sb.Append("  FuelMasterID: ").Append(FuelMasterID).Append("\n");
      sb.Append("  FuelerID: ").Append(FuelerID).Append("\n");
      sb.Append("  TaxStatus: ").Append(TaxStatus).Append("\n");
      sb.Append("  CompanyName: ").Append(CompanyName).Append("\n");
      sb.Append("  MemoFor: ").Append(MemoFor).Append("\n");
      sb.Append("  RequestedBy: ").Append(RequestedBy).Append("\n");
      sb.Append("  AdditionalFees: ").Append(AdditionalFees).Append("\n");
      sb.Append("  AdditionalFeesTotal: ").Append(AdditionalFeesTotal).Append("\n");
      sb.Append("  ArrivalTime: ").Append(ArrivalTime).Append("\n");
      sb.Append("  DepartureTime: ").Append(DepartureTime).Append("\n");
      sb.Append("  HasPaid: ").Append(HasPaid).Append("\n");
      sb.Append("  HasInvoicedBills: ").Append(HasInvoicedBills).Append("\n");
      sb.Append("  CanPay: ").Append(CanPay).Append("\n");
      sb.Append("  ACHMerchantProfileID: ").Append(ACHMerchantProfileID).Append("\n");
      sb.Append("  CreditCardMerchantProfileID: ").Append(CreditCardMerchantProfileID).Append("\n");
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
