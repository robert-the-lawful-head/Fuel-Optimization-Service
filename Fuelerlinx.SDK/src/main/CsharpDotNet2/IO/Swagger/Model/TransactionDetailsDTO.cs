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
  public class TransactionDetailsDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets RequestId
    /// </summary>
    [DataMember(Name="requestId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "requestId")]
    public int? RequestId { get; set; }

    /// <summary>
    /// Gets or Sets FlightType
    /// </summary>
    [DataMember(Name="flightType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightType")]
    public string FlightType { get; set; }

    /// <summary>
    /// Gets or Sets SchedulingServiceId
    /// </summary>
    [DataMember(Name="schedulingServiceId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "schedulingServiceId")]
    public string SchedulingServiceId { get; set; }

    /// <summary>
    /// Gets or Sets FuelOn
    /// </summary>
    [DataMember(Name="fuelOn", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelOn")]
    public string FuelOn { get; set; }

    /// <summary>
    /// Gets or Sets NoFuel
    /// </summary>
    [DataMember(Name="noFuel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "noFuel")]
    public bool? NoFuel { get; set; }

    /// <summary>
    /// Gets or Sets Diverted
    /// </summary>
    [DataMember(Name="diverted", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "diverted")]
    public bool? Diverted { get; set; }

    /// <summary>
    /// Gets or Sets TaxStorageType
    /// </summary>
    [DataMember(Name="taxStorageType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "taxStorageType")]
    public int? TaxStorageType { get; set; }

    /// <summary>
    /// Gets or Sets TaxStatus
    /// </summary>
    [DataMember(Name="taxStatus", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "taxStatus")]
    public int? TaxStatus { get; set; }

    /// <summary>
    /// Gets or Sets DegaServiceId
    /// </summary>
    [DataMember(Name="degaServiceId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "degaServiceId")]
    public int? DegaServiceId { get; set; }

    /// <summary>
    /// Gets or Sets PreventSyncingWithDegaFuelOrders
    /// </summary>
    [DataMember(Name="preventSyncingWithDegaFuelOrders", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "preventSyncingWithDegaFuelOrders")]
    public bool? PreventSyncingWithDegaFuelOrders { get; set; }

    /// <summary>
    /// Gets or Sets VendorServiceId
    /// </summary>
    [DataMember(Name="vendorServiceId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "vendorServiceId")]
    public string VendorServiceId { get; set; }

    /// <summary>
    /// Gets or Sets CustomerNumber
    /// </summary>
    [DataMember(Name="customerNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "customerNumber")]
    public string CustomerNumber { get; set; }

    /// <summary>
    /// Gets or Sets ReferenceNumber
    /// </summary>
    [DataMember(Name="referenceNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "referenceNumber")]
    public string ReferenceNumber { get; set; }

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
      sb.Append("class TransactionDetailsDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  RequestId: ").Append(RequestId).Append("\n");
      sb.Append("  FlightType: ").Append(FlightType).Append("\n");
      sb.Append("  SchedulingServiceId: ").Append(SchedulingServiceId).Append("\n");
      sb.Append("  FuelOn: ").Append(FuelOn).Append("\n");
      sb.Append("  NoFuel: ").Append(NoFuel).Append("\n");
      sb.Append("  Diverted: ").Append(Diverted).Append("\n");
      sb.Append("  TaxStorageType: ").Append(TaxStorageType).Append("\n");
      sb.Append("  TaxStatus: ").Append(TaxStatus).Append("\n");
      sb.Append("  DegaServiceId: ").Append(DegaServiceId).Append("\n");
      sb.Append("  PreventSyncingWithDegaFuelOrders: ").Append(PreventSyncingWithDegaFuelOrders).Append("\n");
      sb.Append("  VendorServiceId: ").Append(VendorServiceId).Append("\n");
      sb.Append("  CustomerNumber: ").Append(CustomerNumber).Append("\n");
      sb.Append("  ReferenceNumber: ").Append(ReferenceNumber).Append("\n");
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
