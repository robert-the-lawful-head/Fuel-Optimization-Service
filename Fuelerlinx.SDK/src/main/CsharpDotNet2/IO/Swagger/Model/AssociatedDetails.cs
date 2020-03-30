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
  public class AssociatedDetails {
    /// <summary>
    /// Gets or Sets AcukwikAirport
    /// </summary>
    [DataMember(Name="acukwikAirport", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "acukwikAirport")]
    public AcukwikAirportDTO AcukwikAirport { get; set; }

    /// <summary>
    /// Gets or Sets CompanySpecificRampFee
    /// </summary>
    [DataMember(Name="companySpecificRampFee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companySpecificRampFee")]
    public RampFeeByCompanyDTO CompanySpecificRampFee { get; set; }

    /// <summary>
    /// Gets or Sets CrowdSourcedRampFee
    /// </summary>
    [DataMember(Name="crowdSourcedRampFee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "crowdSourcedRampFee")]
    public MostCommonReportedRampFeeResultDTO CrowdSourcedRampFee { get; set; }

    /// <summary>
    /// Gets or Sets CompanySpecificFboDetails
    /// </summary>
    [DataMember(Name="companySpecificFboDetails", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companySpecificFboDetails")]
    public FBOsByCompanyDTO CompanySpecificFboDetails { get; set; }

    /// <summary>
    /// Gets or Sets CompanySpecificAirportDetails
    /// </summary>
    [DataMember(Name="companySpecificAirportDetails", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companySpecificAirportDetails")]
    public AirportDetailsByCompanyDTO CompanySpecificAirportDetails { get; set; }

    /// <summary>
    /// Gets or Sets CompanySpecificFuelVendorDetails
    /// </summary>
    [DataMember(Name="companySpecificFuelVendorDetails", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "companySpecificFuelVendorDetails")]
    public CompanyFuelerDTO CompanySpecificFuelVendorDetails { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AssociatedDetails {\n");
      sb.Append("  AcukwikAirport: ").Append(AcukwikAirport).Append("\n");
      sb.Append("  CompanySpecificRampFee: ").Append(CompanySpecificRampFee).Append("\n");
      sb.Append("  CrowdSourcedRampFee: ").Append(CrowdSourcedRampFee).Append("\n");
      sb.Append("  CompanySpecificFboDetails: ").Append(CompanySpecificFboDetails).Append("\n");
      sb.Append("  CompanySpecificAirportDetails: ").Append(CompanySpecificAirportDetails).Append("\n");
      sb.Append("  CompanySpecificFuelVendorDetails: ").Append(CompanySpecificFuelVendorDetails).Append("\n");
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
