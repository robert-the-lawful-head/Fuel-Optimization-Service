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
  public class ReportDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets IsFavorite
    /// </summary>
    [DataMember(Name="isFavorite", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isFavorite")]
    public bool? IsFavorite { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets DataOptions
    /// </summary>
    [DataMember(Name="dataOptions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dataOptions")]
    public ReportDataOptions DataOptions { get; set; }

    /// <summary>
    /// Gets or Sets CreationDateUtc
    /// </summary>
    [DataMember(Name="creationDateUtc", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "creationDateUtc")]
    public DateTime? CreationDateUtc { get; set; }

    /// <summary>
    /// Gets or Sets ChartOptions
    /// </summary>
    [DataMember(Name="chartOptions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "chartOptions")]
    public ChartOptions ChartOptions { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ReportDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  IsFavorite: ").Append(IsFavorite).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  DataOptions: ").Append(DataOptions).Append("\n");
      sb.Append("  CreationDateUtc: ").Append(CreationDateUtc).Append("\n");
      sb.Append("  ChartOptions: ").Append(ChartOptions).Append("\n");
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
