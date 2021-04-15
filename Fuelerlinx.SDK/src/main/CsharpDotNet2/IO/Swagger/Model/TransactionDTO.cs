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
  public class TransactionDTO {
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
    /// Gets or Sets Fbo
    /// </summary>
    [DataMember(Name="fbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fbo")]
    public string Fbo { get; set; }

    /// <summary>
    /// Gets or Sets FuelVendorId
    /// </summary>
    [DataMember(Name="fuelVendorId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelVendorId")]
    public int? FuelVendorId { get; set; }

    /// <summary>
    /// Gets or Sets TailNumberId
    /// </summary>
    [DataMember(Name="tailNumberId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumberId")]
    public int? TailNumberId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="invoiceStatus", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoiceStatus")]
    public int? InvoiceStatus { get; set; }

    /// <summary>
    /// Gets or Sets InvoiceStatusDescription
    /// </summary>
    [DataMember(Name="invoiceStatusDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoiceStatusDescription")]
    public string InvoiceStatusDescription { get; set; }

    /// <summary>
    /// Gets or Sets TailNumber
    /// </summary>
    [DataMember(Name="tailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumber")]
    public string TailNumber { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalDateTime
    /// </summary>
    [DataMember(Name="arrivalDateTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalDateTime")]
    public DateTime? ArrivalDateTime { get; set; }

    /// <summary>
    /// Gets or Sets DepartureDateTime
    /// </summary>
    [DataMember(Name="departureDateTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureDateTime")]
    public DateTime? DepartureDateTime { get; set; }

    /// <summary>
    /// Gets or Sets ServiceDateTime
    /// </summary>
    [DataMember(Name="serviceDateTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "serviceDateTime")]
    public DateTime? ServiceDateTime { get; set; }

    /// <summary>
    /// Gets or Sets Icao
    /// </summary>
    [DataMember(Name="icao", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "icao")]
    public string Icao { get; set; }

    /// <summary>
    /// Gets or Sets Iata
    /// </summary>
    [DataMember(Name="iata", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "iata")]
    public string Iata { get; set; }

    /// <summary>
    /// Gets or Sets FuelVendor
    /// </summary>
    [DataMember(Name="fuelVendor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelVendor")]
    public string FuelVendor { get; set; }

    /// <summary>
    /// Gets or Sets DispatchedVolume
    /// </summary>
    [DataMember(Name="dispatchedVolume", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dispatchedVolume")]
    public Weight DispatchedVolume { get; set; }

    /// <summary>
    /// Gets or Sets DispatchedPricePerGallon
    /// </summary>
    [DataMember(Name="dispatchedPricePerGallon", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dispatchedPricePerGallon")]
    public double? DispatchedPricePerGallon { get; set; }

    /// <summary>
    /// Gets or Sets MarketUpdatedPricePerGallon
    /// </summary>
    [DataMember(Name="marketUpdatedPricePerGallon", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "marketUpdatedPricePerGallon")]
    public double? MarketUpdatedPricePerGallon { get; set; }

    /// <summary>
    /// Gets or Sets CreationDate
    /// </summary>
    [DataMember(Name="creationDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "creationDate")]
    public DateTime? CreationDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="timeStandard", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timeStandard")]
    public int? TimeStandard { get; set; }

    /// <summary>
    /// Gets or Sets InvoicedPricePerGallon
    /// </summary>
    [DataMember(Name="invoicedPricePerGallon", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoicedPricePerGallon")]
    public double? InvoicedPricePerGallon { get; set; }

    /// <summary>
    /// Gets or Sets InvoicedVolume
    /// </summary>
    [DataMember(Name="invoicedVolume", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoicedVolume")]
    public Weight InvoicedVolume { get; set; }

    /// <summary>
    /// Gets or Sets BasePricePerGallon
    /// </summary>
    [DataMember(Name="basePricePerGallon", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "basePricePerGallon")]
    public double? BasePricePerGallon { get; set; }

    /// <summary>
    /// Gets or Sets PostedRetail
    /// </summary>
    [DataMember(Name="postedRetail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "postedRetail")]
    public double? PostedRetail { get; set; }

    /// <summary>
    /// Gets or Sets ForcedReconcile
    /// </summary>
    [DataMember(Name="forcedReconcile", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "forcedReconcile")]
    public bool? ForcedReconcile { get; set; }

    /// <summary>
    /// Gets or Sets Archived
    /// </summary>
    [DataMember(Name="archived", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "archived")]
    public bool? Archived { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets WebDispatchId
    /// </summary>
    [DataMember(Name="webDispatchId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "webDispatchId")]
    public int? WebDispatchId { get; set; }

    /// <summary>
    /// Gets or Sets Direct
    /// </summary>
    [DataMember(Name="direct", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "direct")]
    public bool? Direct { get; set; }

    /// <summary>
    /// Gets or Sets MemoForTransactionID
    /// </summary>
    [DataMember(Name="memoForTransactionID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "memoForTransactionID")]
    public int? MemoForTransactionID { get; set; }

    /// <summary>
    /// Gets or Sets DiscrepancyCorrected
    /// </summary>
    [DataMember(Name="discrepancyCorrected", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "discrepancyCorrected")]
    public bool? DiscrepancyCorrected { get; set; }

    /// <summary>
    /// Gets or Sets PlattsPrice
    /// </summary>
    [DataMember(Name="plattsPrice", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "plattsPrice")]
    public double? PlattsPrice { get; set; }

    /// <summary>
    /// Gets or Sets CustomerName
    /// </summary>
    [DataMember(Name="customerName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "customerName")]
    public string CustomerName { get; set; }

    /// <summary>
    /// Gets or Sets TripType
    /// </summary>
    [DataMember(Name="tripType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripType")]
    public string TripType { get; set; }

    /// <summary>
    /// Gets or Sets FuelMasterId
    /// </summary>
    [DataMember(Name="fuelMasterId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelMasterId")]
    public int? FuelMasterId { get; set; }

    /// <summary>
    /// Gets or Sets IntoPlane
    /// </summary>
    [DataMember(Name="intoPlane", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "intoPlane")]
    public string IntoPlane { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Gets or Sets MailBoxForFuelRelease
    /// </summary>
    [DataMember(Name="mailBoxForFuelRelease", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mailBoxForFuelRelease")]
    public MailBoxJobDTO MailBoxForFuelRelease { get; set; }

    /// <summary>
    /// Gets or Sets InvoiceNumber
    /// </summary>
    [DataMember(Name="invoiceNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoiceNumber")]
    public string InvoiceNumber { get; set; }

    /// <summary>
    /// Gets or Sets DepartureDate
    /// </summary>
    [DataMember(Name="departureDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureDate")]
    public string DepartureDate { get; set; }

    /// <summary>
    /// Gets or Sets DepartureTime
    /// </summary>
    [DataMember(Name="departureTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureTime")]
    public string DepartureTime { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalDate
    /// </summary>
    [DataMember(Name="arrivalDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalDate")]
    public string ArrivalDate { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalTime
    /// </summary>
    [DataMember(Name="arrivalTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalTime")]
    public string ArrivalTime { get; set; }

    /// <summary>
    /// Gets or Sets ServiceDate
    /// </summary>
    [DataMember(Name="serviceDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "serviceDate")]
    public string ServiceDate { get; set; }

    /// <summary>
    /// Gets or Sets ServiceTime
    /// </summary>
    [DataMember(Name="serviceTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "serviceTime")]
    public string ServiceTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="source", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "source")]
    public int? Source { get; set; }

    /// <summary>
    /// Gets or Sets DispatchNumber
    /// </summary>
    [DataMember(Name="dispatchNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dispatchNumber")]
    public string DispatchNumber { get; set; }

    /// <summary>
    /// Gets or Sets ReportedRampFeeWaivedAt
    /// </summary>
    [DataMember(Name="reportedRampFeeWaivedAt", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reportedRampFeeWaivedAt")]
    public Weight ReportedRampFeeWaivedAt { get; set; }

    /// <summary>
    /// Gets or Sets RampFeeCallFbo
    /// </summary>
    [DataMember(Name="rampFeeCallFbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFeeCallFbo")]
    public bool? RampFeeCallFbo { get; set; }

    /// <summary>
    /// Gets or Sets ReportedRampFee
    /// </summary>
    [DataMember(Name="reportedRampFee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reportedRampFee")]
    public double? ReportedRampFee { get; set; }

    /// <summary>
    /// Gets or Sets PaymentEmail
    /// </summary>
    [DataMember(Name="paymentEmail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paymentEmail")]
    public string PaymentEmail { get; set; }

    /// <summary>
    /// Gets or Sets IsPlaceholder
    /// </summary>
    [DataMember(Name="isPlaceholder", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isPlaceholder")]
    public bool? IsPlaceholder { get; set; }

    /// <summary>
    /// Gets or Sets HasPaid
    /// </summary>
    [DataMember(Name="hasPaid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hasPaid")]
    public bool? HasPaid { get; set; }

    /// <summary>
    /// Gets or Sets DepartureDateYearFirstFormat
    /// </summary>
    [DataMember(Name="departureDateYearFirstFormat", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "departureDateYearFirstFormat")]
    public string DepartureDateYearFirstFormat { get; set; }

    /// <summary>
    /// Gets or Sets ArrivalDateYearFirstFormat
    /// </summary>
    [DataMember(Name="arrivalDateYearFirstFormat", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "arrivalDateYearFirstFormat")]
    public string ArrivalDateYearFirstFormat { get; set; }

    /// <summary>
    /// Gets or Sets ServiceDateYearFirstFormat
    /// </summary>
    [DataMember(Name="serviceDateYearFirstFormat", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "serviceDateYearFirstFormat")]
    public string ServiceDateYearFirstFormat { get; set; }

    /// <summary>
    /// Gets or Sets ScheduledTripID
    /// </summary>
    [DataMember(Name="scheduledTripID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "scheduledTripID")]
    public string ScheduledTripID { get; set; }

    /// <summary>
    /// Gets or Sets LegNumber
    /// </summary>
    [DataMember(Name="legNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legNumber")]
    public int? LegNumber { get; set; }

    /// <summary>
    /// Gets or Sets NonFuelServicesTotal
    /// </summary>
    [DataMember(Name="nonFuelServicesTotal", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "nonFuelServicesTotal")]
    public double? NonFuelServicesTotal { get; set; }

    /// <summary>
    /// Gets or Sets NonFuelServiceNames
    /// </summary>
    [DataMember(Name="nonFuelServiceNames", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "nonFuelServiceNames")]
    public string NonFuelServiceNames { get; set; }

    /// <summary>
    /// Gets or Sets FlightTypeMapping
    /// </summary>
    [DataMember(Name="flightTypeMapping", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightTypeMapping")]
    public FlightTypeMappingDTO FlightTypeMapping { get; set; }

    /// <summary>
    /// Gets or Sets TransactionNotes
    /// </summary>
    [DataMember(Name="transactionNotes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionNotes")]
    public List<TransactionNoteDTO> TransactionNotes { get; set; }

    /// <summary>
    /// Gets or Sets TransactionFuelTaxes
    /// </summary>
    [DataMember(Name="transactionFuelTaxes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionFuelTaxes")]
    public List<TransactionFuelTaxDTO> TransactionFuelTaxes { get; set; }

    /// <summary>
    /// Gets or Sets TransactionServiceFees
    /// </summary>
    [DataMember(Name="transactionServiceFees", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionServiceFees")]
    public List<TransactionServiceFeeDTO> TransactionServiceFees { get; set; }

    /// <summary>
    /// Gets or Sets TransactionAttachments
    /// </summary>
    [DataMember(Name="transactionAttachments", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionAttachments")]
    public List<TransactionAttachmentDTO> TransactionAttachments { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingIntegrationDispatchInfo
    /// </summary>
    [DataMember(Name="schedulingIntegrationDispatchInfo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "schedulingIntegrationDispatchInfo")]
    public IntegrationDispatchDTO SchedulingIntegrationDispatchInfo { get; set; }

    /// <summary>
    /// Gets or Sets TransactionLegSettings
    /// </summary>
    [DataMember(Name="transactionLegSettings", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionLegSettings")]
    public TransactionLegSettingsDTO TransactionLegSettings { get; set; }

    /// <summary>
    /// Gets or Sets TransactionDetails
    /// </summary>
    [DataMember(Name="transactionDetails", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionDetails")]
    public TransactionDetailsDTO TransactionDetails { get; set; }

    /// <summary>
    /// Gets or Sets TransactionFuelPrices
    /// </summary>
    [DataMember(Name="transactionFuelPrices", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionFuelPrices")]
    public List<TransactionFuelPriceResultDTO> TransactionFuelPrices { get; set; }

    /// <summary>
    /// Gets or Sets AirportInfo
    /// </summary>
    [DataMember(Name="airportInfo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airportInfo")]
    public AcukwikAirportDTO AirportInfo { get; set; }

    /// <summary>
    /// Gets or Sets FboHandlerDetail
    /// </summary>
    [DataMember(Name="fboHandlerDetail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboHandlerDetail")]
    public AcukwikFboHandlerDetailDTO FboHandlerDetail { get; set; }

    /// <summary>
    /// Gets or Sets CompanyFuelVendorInfo
    /// </summary>
    [DataMember(Name="companyFuelVendorInfo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyFuelVendorInfo")]
    public CompanyFuelerDTO CompanyFuelVendorInfo { get; set; }

    /// <summary>
    /// Gets or Sets AirportNotes
    /// </summary>
    [DataMember(Name="airportNotes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airportNotes")]
    public List<AirportDetailsByCompanyNotesDTO> AirportNotes { get; set; }

    /// <summary>
    /// Gets or Sets FboNotes
    /// </summary>
    [DataMember(Name="fboNotes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fboNotes")]
    public List<FboByCompanyNotesDTO> FboNotes { get; set; }

    /// <summary>
    /// Gets or Sets RampFeeNotes
    /// </summary>
    [DataMember(Name="rampFeeNotes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rampFeeNotes")]
    public List<RampFeeByCompanyNoteDTO> RampFeeNotes { get; set; }

    /// <summary>
    /// Gets or Sets PreUpliftPriceSync
    /// </summary>
    [DataMember(Name="preUpliftPriceSync", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "preUpliftPriceSync")]
    public PriceSyncTransactionDTO PreUpliftPriceSync { get; set; }

    /// <summary>
    /// Gets or Sets ChangedTripNotification
    /// </summary>
    [DataMember(Name="changedTripNotification", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "changedTripNotification")]
    public TransactionForChangedTripDTO ChangedTripNotification { get; set; }

    /// <summary>
    /// Gets or Sets TransactionAccountingData
    /// </summary>
    [DataMember(Name="transactionAccountingData", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionAccountingData")]
    public TransactionAccountingDataDTO TransactionAccountingData { get; set; }

    /// <summary>
    /// Gets or Sets TransactionAccountingTransfer
    /// </summary>
    [DataMember(Name="transactionAccountingTransfer", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionAccountingTransfer")]
    public TransactionAccountingTransferDTO TransactionAccountingTransfer { get; set; }

    /// <summary>
    /// Gets or Sets User
    /// </summary>
    [DataMember(Name="user", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "user")]
    public UserDTO User { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TransactionDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  UserId: ").Append(UserId).Append("\n");
      sb.Append("  Fbo: ").Append(Fbo).Append("\n");
      sb.Append("  FuelVendorId: ").Append(FuelVendorId).Append("\n");
      sb.Append("  TailNumberId: ").Append(TailNumberId).Append("\n");
      sb.Append("  InvoiceStatus: ").Append(InvoiceStatus).Append("\n");
      sb.Append("  InvoiceStatusDescription: ").Append(InvoiceStatusDescription).Append("\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
      sb.Append("  ArrivalDateTime: ").Append(ArrivalDateTime).Append("\n");
      sb.Append("  DepartureDateTime: ").Append(DepartureDateTime).Append("\n");
      sb.Append("  ServiceDateTime: ").Append(ServiceDateTime).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  Iata: ").Append(Iata).Append("\n");
      sb.Append("  FuelVendor: ").Append(FuelVendor).Append("\n");
      sb.Append("  DispatchedVolume: ").Append(DispatchedVolume).Append("\n");
      sb.Append("  DispatchedPricePerGallon: ").Append(DispatchedPricePerGallon).Append("\n");
      sb.Append("  MarketUpdatedPricePerGallon: ").Append(MarketUpdatedPricePerGallon).Append("\n");
      sb.Append("  CreationDate: ").Append(CreationDate).Append("\n");
      sb.Append("  TimeStandard: ").Append(TimeStandard).Append("\n");
      sb.Append("  InvoicedPricePerGallon: ").Append(InvoicedPricePerGallon).Append("\n");
      sb.Append("  InvoicedVolume: ").Append(InvoicedVolume).Append("\n");
      sb.Append("  BasePricePerGallon: ").Append(BasePricePerGallon).Append("\n");
      sb.Append("  PostedRetail: ").Append(PostedRetail).Append("\n");
      sb.Append("  ForcedReconcile: ").Append(ForcedReconcile).Append("\n");
      sb.Append("  Archived: ").Append(Archived).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  WebDispatchId: ").Append(WebDispatchId).Append("\n");
      sb.Append("  Direct: ").Append(Direct).Append("\n");
      sb.Append("  MemoForTransactionID: ").Append(MemoForTransactionID).Append("\n");
      sb.Append("  DiscrepancyCorrected: ").Append(DiscrepancyCorrected).Append("\n");
      sb.Append("  PlattsPrice: ").Append(PlattsPrice).Append("\n");
      sb.Append("  CustomerName: ").Append(CustomerName).Append("\n");
      sb.Append("  TripType: ").Append(TripType).Append("\n");
      sb.Append("  FuelMasterId: ").Append(FuelMasterId).Append("\n");
      sb.Append("  IntoPlane: ").Append(IntoPlane).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      sb.Append("  MailBoxForFuelRelease: ").Append(MailBoxForFuelRelease).Append("\n");
      sb.Append("  InvoiceNumber: ").Append(InvoiceNumber).Append("\n");
      sb.Append("  DepartureDate: ").Append(DepartureDate).Append("\n");
      sb.Append("  DepartureTime: ").Append(DepartureTime).Append("\n");
      sb.Append("  ArrivalDate: ").Append(ArrivalDate).Append("\n");
      sb.Append("  ArrivalTime: ").Append(ArrivalTime).Append("\n");
      sb.Append("  ServiceDate: ").Append(ServiceDate).Append("\n");
      sb.Append("  ServiceTime: ").Append(ServiceTime).Append("\n");
      sb.Append("  Source: ").Append(Source).Append("\n");
      sb.Append("  DispatchNumber: ").Append(DispatchNumber).Append("\n");
      sb.Append("  ReportedRampFeeWaivedAt: ").Append(ReportedRampFeeWaivedAt).Append("\n");
      sb.Append("  RampFeeCallFbo: ").Append(RampFeeCallFbo).Append("\n");
      sb.Append("  ReportedRampFee: ").Append(ReportedRampFee).Append("\n");
      sb.Append("  PaymentEmail: ").Append(PaymentEmail).Append("\n");
      sb.Append("  IsPlaceholder: ").Append(IsPlaceholder).Append("\n");
      sb.Append("  HasPaid: ").Append(HasPaid).Append("\n");
      sb.Append("  DepartureDateYearFirstFormat: ").Append(DepartureDateYearFirstFormat).Append("\n");
      sb.Append("  ArrivalDateYearFirstFormat: ").Append(ArrivalDateYearFirstFormat).Append("\n");
      sb.Append("  ServiceDateYearFirstFormat: ").Append(ServiceDateYearFirstFormat).Append("\n");
      sb.Append("  ScheduledTripID: ").Append(ScheduledTripID).Append("\n");
      sb.Append("  LegNumber: ").Append(LegNumber).Append("\n");
      sb.Append("  NonFuelServicesTotal: ").Append(NonFuelServicesTotal).Append("\n");
      sb.Append("  NonFuelServiceNames: ").Append(NonFuelServiceNames).Append("\n");
      sb.Append("  FlightTypeMapping: ").Append(FlightTypeMapping).Append("\n");
      sb.Append("  TransactionNotes: ").Append(TransactionNotes).Append("\n");
      sb.Append("  TransactionFuelTaxes: ").Append(TransactionFuelTaxes).Append("\n");
      sb.Append("  TransactionServiceFees: ").Append(TransactionServiceFees).Append("\n");
      sb.Append("  TransactionAttachments: ").Append(TransactionAttachments).Append("\n");
      sb.Append("  SchedulingIntegrationDispatchInfo: ").Append(SchedulingIntegrationDispatchInfo).Append("\n");
      sb.Append("  TransactionLegSettings: ").Append(TransactionLegSettings).Append("\n");
      sb.Append("  TransactionDetails: ").Append(TransactionDetails).Append("\n");
      sb.Append("  TransactionFuelPrices: ").Append(TransactionFuelPrices).Append("\n");
      sb.Append("  AirportInfo: ").Append(AirportInfo).Append("\n");
      sb.Append("  FboHandlerDetail: ").Append(FboHandlerDetail).Append("\n");
      sb.Append("  CompanyFuelVendorInfo: ").Append(CompanyFuelVendorInfo).Append("\n");
      sb.Append("  AirportNotes: ").Append(AirportNotes).Append("\n");
      sb.Append("  FboNotes: ").Append(FboNotes).Append("\n");
      sb.Append("  RampFeeNotes: ").Append(RampFeeNotes).Append("\n");
      sb.Append("  PreUpliftPriceSync: ").Append(PreUpliftPriceSync).Append("\n");
      sb.Append("  ChangedTripNotification: ").Append(ChangedTripNotification).Append("\n");
      sb.Append("  TransactionAccountingData: ").Append(TransactionAccountingData).Append("\n");
      sb.Append("  TransactionAccountingTransfer: ").Append(TransactionAccountingTransfer).Append("\n");
      sb.Append("  User: ").Append(User).Append("\n");
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
