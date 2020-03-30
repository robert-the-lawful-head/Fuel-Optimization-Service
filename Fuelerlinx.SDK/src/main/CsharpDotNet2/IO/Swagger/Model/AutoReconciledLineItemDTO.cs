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
  public class AutoReconciledLineItemDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Vendor
    /// </summary>
    [DataMember(Name="vendor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "vendor")]
    public string Vendor { get; set; }

    /// <summary>
    /// Gets or Sets InvoiceNumber
    /// </summary>
    [DataMember(Name="invoiceNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoiceNumber")]
    public string InvoiceNumber { get; set; }

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
    /// Gets or Sets InvoiceDate
    /// </summary>
    [DataMember(Name="invoiceDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "invoiceDate")]
    public string InvoiceDate { get; set; }

    /// <summary>
    /// Gets or Sets Fbo
    /// </summary>
    [DataMember(Name="fbo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fbo")]
    public string Fbo { get; set; }

    /// <summary>
    /// Gets or Sets Note
    /// </summary>
    [DataMember(Name="note", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "note")]
    public string Note { get; set; }

    /// <summary>
    /// Gets or Sets Price
    /// </summary>
    [DataMember(Name="price", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "price")]
    public double? Price { get; set; }

    /// <summary>
    /// Gets or Sets Product
    /// </summary>
    [DataMember(Name="product", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "product")]
    public string Product { get; set; }

    /// <summary>
    /// Gets or Sets Volume
    /// </summary>
    [DataMember(Name="volume", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "volume")]
    public double? Volume { get; set; }

    /// <summary>
    /// Gets or Sets ServiceDate
    /// </summary>
    [DataMember(Name="serviceDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "serviceDate")]
    public string ServiceDate { get; set; }

    /// <summary>
    /// Gets or Sets ItemCode
    /// </summary>
    [DataMember(Name="itemCode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "itemCode")]
    public string ItemCode { get; set; }

    /// <summary>
    /// Gets or Sets RequestId
    /// </summary>
    [DataMember(Name="requestId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "requestId")]
    public int? RequestId { get; set; }

    /// <summary>
    /// Gets or Sets TailNumber
    /// </summary>
    [DataMember(Name="tailNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumber")]
    public string TailNumber { get; set; }

    /// <summary>
    /// Gets or Sets ProcessId
    /// </summary>
    [DataMember(Name="processId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "processId")]
    public int? ProcessId { get; set; }

    /// <summary>
    /// Gets or Sets Discrepancy
    /// </summary>
    [DataMember(Name="discrepancy", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "discrepancy")]
    public bool? Discrepancy { get; set; }

    /// <summary>
    /// Gets or Sets ItemDescription
    /// </summary>
    [DataMember(Name="itemDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "itemDescription")]
    public string ItemDescription { get; set; }

    /// <summary>
    /// Gets or Sets Terms
    /// </summary>
    [DataMember(Name="terms", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "terms")]
    public string Terms { get; set; }

    /// <summary>
    /// Gets or Sets DueDate
    /// </summary>
    [DataMember(Name="dueDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dueDate")]
    public string DueDate { get; set; }

    /// <summary>
    /// Gets or Sets RelInvoiceNumber
    /// </summary>
    [DataMember(Name="relInvoiceNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "relInvoiceNumber")]
    public string RelInvoiceNumber { get; set; }

    /// <summary>
    /// Gets or Sets AddedAsDirect
    /// </summary>
    [DataMember(Name="addedAsDirect", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addedAsDirect")]
    public bool? AddedAsDirect { get; set; }

    /// <summary>
    /// Gets or Sets TripNumber
    /// </summary>
    [DataMember(Name="tripNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripNumber")]
    public string TripNumber { get; set; }

    /// <summary>
    /// Gets or Sets LegNumber
    /// </summary>
    [DataMember(Name="legNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "legNumber")]
    public int? LegNumber { get; set; }

    /// <summary>
    /// Gets or Sets PaymentMethod
    /// </summary>
    [DataMember(Name="paymentMethod", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paymentMethod")]
    public string PaymentMethod { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AutoReconciledLineItemDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Vendor: ").Append(Vendor).Append("\n");
      sb.Append("  InvoiceNumber: ").Append(InvoiceNumber).Append("\n");
      sb.Append("  Icao: ").Append(Icao).Append("\n");
      sb.Append("  Iata: ").Append(Iata).Append("\n");
      sb.Append("  InvoiceDate: ").Append(InvoiceDate).Append("\n");
      sb.Append("  Fbo: ").Append(Fbo).Append("\n");
      sb.Append("  Note: ").Append(Note).Append("\n");
      sb.Append("  Price: ").Append(Price).Append("\n");
      sb.Append("  Product: ").Append(Product).Append("\n");
      sb.Append("  Volume: ").Append(Volume).Append("\n");
      sb.Append("  ServiceDate: ").Append(ServiceDate).Append("\n");
      sb.Append("  ItemCode: ").Append(ItemCode).Append("\n");
      sb.Append("  RequestId: ").Append(RequestId).Append("\n");
      sb.Append("  TailNumber: ").Append(TailNumber).Append("\n");
      sb.Append("  ProcessId: ").Append(ProcessId).Append("\n");
      sb.Append("  Discrepancy: ").Append(Discrepancy).Append("\n");
      sb.Append("  ItemDescription: ").Append(ItemDescription).Append("\n");
      sb.Append("  Terms: ").Append(Terms).Append("\n");
      sb.Append("  DueDate: ").Append(DueDate).Append("\n");
      sb.Append("  RelInvoiceNumber: ").Append(RelInvoiceNumber).Append("\n");
      sb.Append("  AddedAsDirect: ").Append(AddedAsDirect).Append("\n");
      sb.Append("  TripNumber: ").Append(TripNumber).Append("\n");
      sb.Append("  LegNumber: ").Append(LegNumber).Append("\n");
      sb.Append("  PaymentMethod: ").Append(PaymentMethod).Append("\n");
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
