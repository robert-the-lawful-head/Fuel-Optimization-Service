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
  public class Summary {
    /// <summary>
    /// Gets or Sets DistanceResultsCollection
    /// </summary>
    [DataMember(Name="distanceResultsCollection", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "distanceResultsCollection")]
    public List<DistanceResults> DistanceResultsCollection { get; set; }

    /// <summary>
    /// Gets or Sets FuelBurnProfileCollection
    /// </summary>
    [DataMember(Name="fuelBurnProfileCollection", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fuelBurnProfileCollection")]
    public List<FuelBurnProfile> FuelBurnProfileCollection { get; set; }

    /// <summary>
    /// Gets or Sets TripTimeResultsCollection
    /// </summary>
    [DataMember(Name="tripTimeResultsCollection", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripTimeResultsCollection")]
    public List<TripTimeResults> TripTimeResultsCollection { get; set; }

    /// <summary>
    /// Gets or Sets TripCostsResultsCollection
    /// </summary>
    [DataMember(Name="tripCostsResultsCollection", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tripCostsResultsCollection")]
    public List<TripCostResults> TripCostsResultsCollection { get; set; }

    /// <summary>
    /// Gets or Sets OptionalFuelCollection
    /// </summary>
    [DataMember(Name="optionalFuelCollection", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "optionalFuelCollection")]
    public List<FuelBurnProfile> OptionalFuelCollection { get; set; }

    /// <summary>
    /// Gets or Sets RecommendationSummaryCollection
    /// </summary>
    [DataMember(Name="recommendationSummaryCollection", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "recommendationSummaryCollection")]
    public List<RecommendationSummary> RecommendationSummaryCollection { get; set; }

    /// <summary>
    /// Gets or Sets FlightPlanDataCollection
    /// </summary>
    [DataMember(Name="flightPlanDataCollection", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "flightPlanDataCollection")]
    public List<IFlightPlannerRouteResponseData> FlightPlanDataCollection { get; set; }

    /// <summary>
    /// Gets or Sets FailureMessage
    /// </summary>
    [DataMember(Name="failureMessage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "failureMessage")]
    public string FailureMessage { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Summary {\n");
      sb.Append("  DistanceResultsCollection: ").Append(DistanceResultsCollection).Append("\n");
      sb.Append("  FuelBurnProfileCollection: ").Append(FuelBurnProfileCollection).Append("\n");
      sb.Append("  TripTimeResultsCollection: ").Append(TripTimeResultsCollection).Append("\n");
      sb.Append("  TripCostsResultsCollection: ").Append(TripCostsResultsCollection).Append("\n");
      sb.Append("  OptionalFuelCollection: ").Append(OptionalFuelCollection).Append("\n");
      sb.Append("  RecommendationSummaryCollection: ").Append(RecommendationSummaryCollection).Append("\n");
      sb.Append("  FlightPlanDataCollection: ").Append(FlightPlanDataCollection).Append("\n");
      sb.Append("  FailureMessage: ").Append(FailureMessage).Append("\n");
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
