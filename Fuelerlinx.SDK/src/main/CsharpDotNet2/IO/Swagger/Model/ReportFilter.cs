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
  public class ReportFilter {
    /// <summary>
    /// Gets or Sets StartDate
    /// </summary>
    [DataMember(Name="startDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "startDate")]
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or Sets EndDate
    /// </summary>
    [DataMember(Name="endDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "endDate")]
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or Sets TailNumberList
    /// </summary>
    [DataMember(Name="tailNumberList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tailNumberList")]
    public List<string> TailNumberList { get; set; }

    /// <summary>
    /// Gets or Sets AllowedInvoiceStatusesList
    /// </summary>
    [DataMember(Name="allowedInvoiceStatusesList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "allowedInvoiceStatusesList")]
    public List<int?> AllowedInvoiceStatusesList { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    [DataMember(Name="dateInterval", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateInterval")]
    public int? DateInterval { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    [DataMember(Name="currency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Gets or Sets MapRegion
    /// </summary>
    [DataMember(Name="mapRegion", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mapRegion")]
    public string MapRegion { get; set; }

    /// <summary>
    /// Gets or Sets State
    /// </summary>
    [DataMember(Name="state", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "state")]
    public string State { get; set; }

    /// <summary>
    /// Gets or Sets IncludedAirports
    /// </summary>
    [DataMember(Name="includedAirports", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "includedAirports")]
    public List<string> IncludedAirports { get; set; }

    /// <summary>
    /// Gets or Sets ExcludedAirports
    /// </summary>
    [DataMember(Name="excludedAirports", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "excludedAirports")]
    public List<string> ExcludedAirports { get; set; }

    /// <summary>
    /// Gets or Sets IncludedFuelVendors
    /// </summary>
    [DataMember(Name="includedFuelVendors", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "includedFuelVendors")]
    public List<int?> IncludedFuelVendors { get; set; }

    /// <summary>
    /// Gets or Sets ExcludedFuelVendors
    /// </summary>
    [DataMember(Name="excludedFuelVendors", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "excludedFuelVendors")]
    public List<int?> ExcludedFuelVendors { get; set; }

    /// <summary>
    /// Gets or Sets ExcludeArchived
    /// </summary>
    [DataMember(Name="excludeArchived", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "excludeArchived")]
    public bool? ExcludeArchived { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ReportFilter {\n");
      sb.Append("  StartDate: ").Append(StartDate).Append("\n");
      sb.Append("  EndDate: ").Append(EndDate).Append("\n");
      sb.Append("  TailNumberList: ").Append(TailNumberList).Append("\n");
      sb.Append("  AllowedInvoiceStatusesList: ").Append(AllowedInvoiceStatusesList).Append("\n");
      sb.Append("  DateInterval: ").Append(DateInterval).Append("\n");
      sb.Append("  Currency: ").Append(Currency).Append("\n");
      sb.Append("  MapRegion: ").Append(MapRegion).Append("\n");
      sb.Append("  State: ").Append(State).Append("\n");
      sb.Append("  IncludedAirports: ").Append(IncludedAirports).Append("\n");
      sb.Append("  ExcludedAirports: ").Append(ExcludedAirports).Append("\n");
      sb.Append("  IncludedFuelVendors: ").Append(IncludedFuelVendors).Append("\n");
      sb.Append("  ExcludedFuelVendors: ").Append(ExcludedFuelVendors).Append("\n");
      sb.Append("  ExcludeArchived: ").Append(ExcludeArchived).Append("\n");
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
