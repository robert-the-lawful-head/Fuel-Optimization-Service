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
  public class AveragePriceByAirportDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets AveragePricePerGallon
    /// </summary>
    [DataMember(Name="averagePricePerGallon", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "averagePricePerGallon")]
    public double? AveragePricePerGallon { get; set; }

    /// <summary>
    /// Gets or Sets AirportIdentifier
    /// </summary>
    [DataMember(Name="airportIdentifier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "airportIdentifier")]
    public string AirportIdentifier { get; set; }

    /// <summary>
    /// Gets or Sets CompanyId
    /// </summary>
    [DataMember(Name="companyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companyId")]
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or Sets CreationDate
    /// </summary>
    [DataMember(Name="creationDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "creationDate")]
    public DateTime? CreationDate { get; set; }

    /// <summary>
    /// Gets or Sets MinimumPricePerGallon
    /// </summary>
    [DataMember(Name="minimumPricePerGallon", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minimumPricePerGallon")]
    public double? MinimumPricePerGallon { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AveragePriceByAirportDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  AveragePricePerGallon: ").Append(AveragePricePerGallon).Append("\n");
      sb.Append("  AirportIdentifier: ").Append(AirportIdentifier).Append("\n");
      sb.Append("  CompanyId: ").Append(CompanyId).Append("\n");
      sb.Append("  CreationDate: ").Append(CreationDate).Append("\n");
      sb.Append("  MinimumPricePerGallon: ").Append(MinimumPricePerGallon).Append("\n");
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
