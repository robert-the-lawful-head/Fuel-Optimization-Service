using System;
using System.Collections.Generic;
using RestSharp;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace IO.Swagger.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ISavedTripApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteSavedTripResponse</returns>
        DeleteSavedTripResponse DeleteSavedTrip (int? id);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteSavedTripLegResponse</returns>
        DeleteSavedTripLegResponse DeleteSavedTripLeg (int? id);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SavedTripResponse</returns>
        SavedTripResponse GetSavedTripById (int? id);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SavedTripLegResponse</returns>
        SavedTripLegResponse GetSavedTripLegById (int? id);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>SavedTripResponse</returns>
        SavedTripResponse GetSavedTripsByCompanyId (int? companyId);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostSavedTripResponse</returns>
        PostSavedTripResponse PostSavedTrip (PostSavedTripRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostSavedTripLegResponse</returns>
        PostSavedTripLegResponse PostSavedTripLeg (PostSavedTripLegRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="savedTripId"></param>
        /// <param name="savedTripName"></param>
        /// <param name="savedTripCompanyId"></param>
        /// <param name="savedTripUserId"></param>
        /// <param name="savedTripUserAircraftId"></param>
        /// <param name="savedTripFuelUnitType"></param>
        /// <param name="savedTripStartingFuel"></param>
        /// <param name="savedTripMltmodelPropertiesJson"></param>
        /// <param name="savedTripSavedTripLegs"></param>
        /// <param name="savedTripAircraftDataId"></param>
        /// <param name="savedTripAircraftDataTailNumber"></param>
        /// <param name="savedTripAircraftDataMinLandingRunwayLengthUnit"></param>
        /// <param name="savedTripAircraftDataMinLandingRunwayLengthAmount"></param>
        /// <param name="savedTripAircraftDataMinLandingRunwayLengthUnitDescription"></param>
        /// <param name="savedTripAircraftDataMaintCostPerHour"></param>
        /// <param name="savedTripAircraftDataMaxRangeUnit"></param>
        /// <param name="savedTripAircraftDataMaxRangeAmount"></param>
        /// <param name="savedTripAircraftDataMaxRangeUnitDescription"></param>
        /// <param name="savedTripAircraftDataSize"></param>
        /// <param name="savedTripAircraftDataFuelBurnRate"></param>
        /// <param name="savedTripAircraftDataFuelCapacityUnit"></param>
        /// <param name="savedTripAircraftDataFuelCapacityAmount"></param>
        /// <param name="savedTripAircraftDataFuelCapacityUnitDescription"></param>
        /// <param name="savedTripAircraftDataPayloadBurnRate"></param>
        /// <param name="savedTripAircraftDataAircraftId"></param>
        /// <param name="savedTripAircraftDataMaxTakeoffWeightUnit"></param>
        /// <param name="savedTripAircraftDataMaxTakeoffWeightAmount"></param>
        /// <param name="savedTripAircraftDataMaxTakeoffWeightUnitDescription"></param>
        /// <param name="savedTripAircraftDataMaxLandingWeightUnit"></param>
        /// <param name="savedTripAircraftDataMaxLandingWeightAmount"></param>
        /// <param name="savedTripAircraftDataMaxLandingWeightUnitDescription"></param>
        /// <param name="savedTripAircraftDataAircraftCeilingUnit"></param>
        /// <param name="savedTripAircraftDataAircraftCeilingAmount"></param>
        /// <param name="savedTripAircraftDataAircraftCeilingUnitDescription"></param>
        /// <param name="savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnit"></param>
        /// <param name="savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelAmount"></param>
        /// <param name="savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnitDescription"></param>
        /// <param name="savedTripAircraftDataMinTakeoffRunwayLengthUnit"></param>
        /// <param name="savedTripAircraftDataMinTakeoffRunwayLengthAmount"></param>
        /// <param name="savedTripAircraftDataMinTakeoffRunwayLengthUnitDescription"></param>
        /// <param name="savedTripAircraftDataZeroFuelWeightUnit"></param>
        /// <param name="savedTripAircraftDataZeroFuelWeightAmount"></param>
        /// <param name="savedTripAircraftDataZeroFuelWeightUnitDescription"></param>
        /// <param name="savedTripAircraftDataBasicOperatingWeightUnit"></param>
        /// <param name="savedTripAircraftDataBasicOperatingWeightAmount"></param>
        /// <param name="savedTripAircraftDataBasicOperatingWeightUnitDescription"></param>
        /// <param name="savedTripAircraftDataQbclass"></param>
        /// <param name="savedTripAircraftDataIfrreseveUnit"></param>
        /// <param name="savedTripAircraftDataIfrreseveAmount"></param>
        /// <param name="savedTripAircraftDataIfrreseveUnitDescription"></param>
        /// <param name="savedTripAircraftDataDefault"></param>
        /// <param name="savedTripAircraftDataNormalCruiseTasUnit"></param>
        /// <param name="savedTripAircraftDataNormalCruiseTasAmount"></param>
        /// <param name="savedTripAircraftDataNormalCruiseTasUnitDescription"></param>
        /// <param name="savedTripAircraftDataApcode"></param>
        /// <param name="savedTripAircraftDataMinimumUpliftUnit"></param>
        /// <param name="savedTripAircraftDataMinimumUpliftAmount"></param>
        /// <param name="savedTripAircraftDataMinimumUpliftUnitDescription"></param>
        /// <param name="savedTripAircraftDataAircraftTypeEngineName"></param>
        /// <param name="savedTripAircraftDataFleetGroup"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAircraftId"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMake"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsModel"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnit"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsNormalCruiseTasAmount"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnitDescription"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsFuelCapacityUnit"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsFuelCapacityAmount"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsFuelCapacityUnitDescription"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnit"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthAmount"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnitDescription"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsRangeNm"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsRangePerGal"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxRangeHours"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxRangeMinutes"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsReserveMinutes"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsReserveNm"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsSize"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnit"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightAmount"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnitDescription"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnit"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxLandingWeightAmount"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnitDescription"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAircraftCeilingUnit"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAircraftCeilingAmount"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAircraftCeilingUnitDescription"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnit"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelAmount"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnitDescription"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnit"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAmount"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnitDescription"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnit"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsZeroFuelWeightAmount"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnitDescription"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnit"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsBasicOperatingWeightAmount"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnitDescription"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsFuelType"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsIfrreseveUnit"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsIfrreseveAmount"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsIfrreseveUnitDescription"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsIcao"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsOid"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsAircraftId"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsModelName"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsLengthFt"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsHeightFt"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsWingSpanFt"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsLengthFtInches"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsHeightFtInches"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsWidthFtInches"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCrewConfiguration"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPassengerConfiguration"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPressurizationPsi"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardLbs"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardGal"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalLbs"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalGal"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxRampWeightLbs"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxTakeoffWeightLbs"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsZeroFuelWeightLbs"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsBasicOperatingWeightLbs"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxLandingWeightLbs"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsCleanSpeedKnots"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsoLandingSpeedKnots"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalCruiseTasSpeedKnots"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVmoMaxOpIasSpeedKnots"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalClimbFpm"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineOutClimbFpm"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCeilingFt"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsLandingPerfFaaFieldLengthFt"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerfSlIsaBfl"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerf500020cBfl"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsRangeNm"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNumberOfEngines"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineModelS"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineThrustLbsPerEngine"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineShaftHpPerEngine"></param>
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineCommonTboHours"></param>
        /// <returns>UpdateSavedTripResponse</returns>
        UpdateSavedTripResponse UpdateSavedTrip (int? savedTripId, string savedTripName, int? savedTripCompanyId, int? savedTripUserId, int? savedTripUserAircraftId, string savedTripFuelUnitType, double? savedTripStartingFuel, string savedTripMltmodelPropertiesJson, List<Object> savedTripSavedTripLegs, int? savedTripAircraftDataId, string savedTripAircraftDataTailNumber, int? savedTripAircraftDataMinLandingRunwayLengthUnit, double? savedTripAircraftDataMinLandingRunwayLengthAmount, string savedTripAircraftDataMinLandingRunwayLengthUnitDescription, double? savedTripAircraftDataMaintCostPerHour, int? savedTripAircraftDataMaxRangeUnit, double? savedTripAircraftDataMaxRangeAmount, string savedTripAircraftDataMaxRangeUnitDescription, int? savedTripAircraftDataSize, double? savedTripAircraftDataFuelBurnRate, int? savedTripAircraftDataFuelCapacityUnit, double? savedTripAircraftDataFuelCapacityAmount, string savedTripAircraftDataFuelCapacityUnitDescription, double? savedTripAircraftDataPayloadBurnRate, int? savedTripAircraftDataAircraftId, int? savedTripAircraftDataMaxTakeoffWeightUnit, double? savedTripAircraftDataMaxTakeoffWeightAmount, string savedTripAircraftDataMaxTakeoffWeightUnitDescription, int? savedTripAircraftDataMaxLandingWeightUnit, double? savedTripAircraftDataMaxLandingWeightAmount, string savedTripAircraftDataMaxLandingWeightUnitDescription, int? savedTripAircraftDataAircraftCeilingUnit, double? savedTripAircraftDataAircraftCeilingAmount, string savedTripAircraftDataAircraftCeilingUnitDescription, int? savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnit, double? savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelAmount, string savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnitDescription, int? savedTripAircraftDataMinTakeoffRunwayLengthUnit, double? savedTripAircraftDataMinTakeoffRunwayLengthAmount, string savedTripAircraftDataMinTakeoffRunwayLengthUnitDescription, int? savedTripAircraftDataZeroFuelWeightUnit, double? savedTripAircraftDataZeroFuelWeightAmount, string savedTripAircraftDataZeroFuelWeightUnitDescription, int? savedTripAircraftDataBasicOperatingWeightUnit, double? savedTripAircraftDataBasicOperatingWeightAmount, string savedTripAircraftDataBasicOperatingWeightUnitDescription, string savedTripAircraftDataQbclass, int? savedTripAircraftDataIfrreseveUnit, double? savedTripAircraftDataIfrreseveAmount, string savedTripAircraftDataIfrreseveUnitDescription, bool? savedTripAircraftDataDefault, int? savedTripAircraftDataNormalCruiseTasUnit, double? savedTripAircraftDataNormalCruiseTasAmount, string savedTripAircraftDataNormalCruiseTasUnitDescription, string savedTripAircraftDataApcode, int? savedTripAircraftDataMinimumUpliftUnit, double? savedTripAircraftDataMinimumUpliftAmount, string savedTripAircraftDataMinimumUpliftUnitDescription, string savedTripAircraftDataAircraftTypeEngineName, string savedTripAircraftDataFleetGroup, int? savedTripAircraftDataFactorySpecificationsAircraftId, string savedTripAircraftDataFactorySpecificationsMake, string savedTripAircraftDataFactorySpecificationsModel, int? savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnit, double? savedTripAircraftDataFactorySpecificationsNormalCruiseTasAmount, string savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnitDescription, int? savedTripAircraftDataFactorySpecificationsFuelCapacityUnit, double? savedTripAircraftDataFactorySpecificationsFuelCapacityAmount, string savedTripAircraftDataFactorySpecificationsFuelCapacityUnitDescription, int? savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnit, double? savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthAmount, string savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnitDescription, double? savedTripAircraftDataFactorySpecificationsRangeNm, double? savedTripAircraftDataFactorySpecificationsRangePerGal, double? savedTripAircraftDataFactorySpecificationsMaxRangeHours, double? savedTripAircraftDataFactorySpecificationsMaxRangeMinutes, double? savedTripAircraftDataFactorySpecificationsReserveMinutes, double? savedTripAircraftDataFactorySpecificationsReserveNm, int? savedTripAircraftDataFactorySpecificationsSize, int? savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnit, double? savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightAmount, string savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnitDescription, int? savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnit, double? savedTripAircraftDataFactorySpecificationsMaxLandingWeightAmount, string savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnitDescription, int? savedTripAircraftDataFactorySpecificationsAircraftCeilingUnit, double? savedTripAircraftDataFactorySpecificationsAircraftCeilingAmount, string savedTripAircraftDataFactorySpecificationsAircraftCeilingUnitDescription, int? savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnit, double? savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelAmount, string savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnitDescription, int? savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnit, double? savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAmount, string savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnitDescription, int? savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnit, double? savedTripAircraftDataFactorySpecificationsZeroFuelWeightAmount, string savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnitDescription, int? savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnit, double? savedTripAircraftDataFactorySpecificationsBasicOperatingWeightAmount, string savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnitDescription, string savedTripAircraftDataFactorySpecificationsFuelType, int? savedTripAircraftDataFactorySpecificationsIfrreseveUnit, double? savedTripAircraftDataFactorySpecificationsIfrreseveAmount, string savedTripAircraftDataFactorySpecificationsIfrreseveUnitDescription, string savedTripAircraftDataFactorySpecificationsIcao, int? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsOid, int? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsAircraftId, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsModelName, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsLengthFt, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsHeightFt, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsWingSpanFt, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsLengthFtInches, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsHeightFtInches, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsWidthFtInches, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCrewConfiguration, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPassengerConfiguration, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPressurizationPsi, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardGal, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalGal, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxRampWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxTakeoffWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsZeroFuelWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsBasicOperatingWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxLandingWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsCleanSpeedKnots, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsoLandingSpeedKnots, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalCruiseTasSpeedKnots, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVmoMaxOpIasSpeedKnots, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalClimbFpm, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineOutClimbFpm, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCeilingFt, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsLandingPerfFaaFieldLengthFt, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerfSlIsaBfl, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerf500020cBfl, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsRangeNm, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNumberOfEngines, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineModelS, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineThrustLbsPerEngine, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineShaftHpPerEngine, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineCommonTboHours);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateSavedTripLegResponse</returns>
        UpdateSavedTripLegResponse UpdateSavedTripLeg (UpdateSavedTripLegRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class SavedTripApi : ISavedTripApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SavedTripApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public SavedTripApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="SavedTripApi"/> class.
        /// </summary>
        /// <returns></returns>
        public SavedTripApi(String basePath)
        {
            this.ApiClient = new ApiClient(basePath);
        }
    
        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public void SetBasePath(String basePath)
        {
            this.ApiClient.BasePath = basePath;
        }
    
        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public String GetBasePath(String basePath)
        {
            return this.ApiClient.BasePath;
        }
    
        /// <summary>
        /// Gets or sets the API client.
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient {get; set;}
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteSavedTripResponse</returns>            
        public DeleteSavedTripResponse DeleteSavedTrip (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteSavedTrip");
            
    
            var path = "/api/SavedTrip/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSavedTrip: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSavedTrip: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteSavedTripResponse) ApiClient.Deserialize(response.Content, typeof(DeleteSavedTripResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteSavedTripLegResponse</returns>            
        public DeleteSavedTripLegResponse DeleteSavedTripLeg (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteSavedTripLeg");
            
    
            var path = "/api/SavedTrip/legs/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSavedTripLeg: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSavedTripLeg: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteSavedTripLegResponse) ApiClient.Deserialize(response.Content, typeof(DeleteSavedTripLegResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>SavedTripResponse</returns>            
        public SavedTripResponse GetSavedTripById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetSavedTripById");
            
    
            var path = "/api/SavedTrip/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSavedTripById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSavedTripById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SavedTripResponse) ApiClient.Deserialize(response.Content, typeof(SavedTripResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>SavedTripLegResponse</returns>            
        public SavedTripLegResponse GetSavedTripLegById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetSavedTripLegById");
            
    
            var path = "/api/SavedTrip/legs/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSavedTripLegById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSavedTripLegById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SavedTripLegResponse) ApiClient.Deserialize(response.Content, typeof(SavedTripLegResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="companyId"></param> 
        /// <returns>SavedTripResponse</returns>            
        public SavedTripResponse GetSavedTripsByCompanyId (int? companyId)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetSavedTripsByCompanyId");
            
    
            var path = "/api/SavedTrip/by-company/{companyId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyId" + "}", ApiClient.ParameterToString(companyId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSavedTripsByCompanyId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSavedTripsByCompanyId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SavedTripResponse) ApiClient.Deserialize(response.Content, typeof(SavedTripResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostSavedTripResponse</returns>            
        public PostSavedTripResponse PostSavedTrip (PostSavedTripRequest body)
        {
            
    
            var path = "/api/SavedTrip";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PostSavedTrip: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostSavedTrip: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostSavedTripResponse) ApiClient.Deserialize(response.Content, typeof(PostSavedTripResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostSavedTripLegResponse</returns>            
        public PostSavedTripLegResponse PostSavedTripLeg (PostSavedTripLegRequest body)
        {
            
    
            var path = "/api/SavedTrip/legs";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PostSavedTripLeg: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostSavedTripLeg: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostSavedTripLegResponse) ApiClient.Deserialize(response.Content, typeof(PostSavedTripLegResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="savedTripId"></param> 
        /// <param name="savedTripName"></param> 
        /// <param name="savedTripCompanyId"></param> 
        /// <param name="savedTripUserId"></param> 
        /// <param name="savedTripUserAircraftId"></param> 
        /// <param name="savedTripFuelUnitType"></param> 
        /// <param name="savedTripStartingFuel"></param> 
        /// <param name="savedTripMltmodelPropertiesJson"></param> 
        /// <param name="savedTripSavedTripLegs"></param> 
        /// <param name="savedTripAircraftDataId"></param> 
        /// <param name="savedTripAircraftDataTailNumber"></param> 
        /// <param name="savedTripAircraftDataMinLandingRunwayLengthUnit"></param> 
        /// <param name="savedTripAircraftDataMinLandingRunwayLengthAmount"></param> 
        /// <param name="savedTripAircraftDataMinLandingRunwayLengthUnitDescription"></param> 
        /// <param name="savedTripAircraftDataMaintCostPerHour"></param> 
        /// <param name="savedTripAircraftDataMaxRangeUnit"></param> 
        /// <param name="savedTripAircraftDataMaxRangeAmount"></param> 
        /// <param name="savedTripAircraftDataMaxRangeUnitDescription"></param> 
        /// <param name="savedTripAircraftDataSize"></param> 
        /// <param name="savedTripAircraftDataFuelBurnRate"></param> 
        /// <param name="savedTripAircraftDataFuelCapacityUnit"></param> 
        /// <param name="savedTripAircraftDataFuelCapacityAmount"></param> 
        /// <param name="savedTripAircraftDataFuelCapacityUnitDescription"></param> 
        /// <param name="savedTripAircraftDataPayloadBurnRate"></param> 
        /// <param name="savedTripAircraftDataAircraftId"></param> 
        /// <param name="savedTripAircraftDataMaxTakeoffWeightUnit"></param> 
        /// <param name="savedTripAircraftDataMaxTakeoffWeightAmount"></param> 
        /// <param name="savedTripAircraftDataMaxTakeoffWeightUnitDescription"></param> 
        /// <param name="savedTripAircraftDataMaxLandingWeightUnit"></param> 
        /// <param name="savedTripAircraftDataMaxLandingWeightAmount"></param> 
        /// <param name="savedTripAircraftDataMaxLandingWeightUnitDescription"></param> 
        /// <param name="savedTripAircraftDataAircraftCeilingUnit"></param> 
        /// <param name="savedTripAircraftDataAircraftCeilingAmount"></param> 
        /// <param name="savedTripAircraftDataAircraftCeilingUnitDescription"></param> 
        /// <param name="savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnit"></param> 
        /// <param name="savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelAmount"></param> 
        /// <param name="savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnitDescription"></param> 
        /// <param name="savedTripAircraftDataMinTakeoffRunwayLengthUnit"></param> 
        /// <param name="savedTripAircraftDataMinTakeoffRunwayLengthAmount"></param> 
        /// <param name="savedTripAircraftDataMinTakeoffRunwayLengthUnitDescription"></param> 
        /// <param name="savedTripAircraftDataZeroFuelWeightUnit"></param> 
        /// <param name="savedTripAircraftDataZeroFuelWeightAmount"></param> 
        /// <param name="savedTripAircraftDataZeroFuelWeightUnitDescription"></param> 
        /// <param name="savedTripAircraftDataBasicOperatingWeightUnit"></param> 
        /// <param name="savedTripAircraftDataBasicOperatingWeightAmount"></param> 
        /// <param name="savedTripAircraftDataBasicOperatingWeightUnitDescription"></param> 
        /// <param name="savedTripAircraftDataQbclass"></param> 
        /// <param name="savedTripAircraftDataIfrreseveUnit"></param> 
        /// <param name="savedTripAircraftDataIfrreseveAmount"></param> 
        /// <param name="savedTripAircraftDataIfrreseveUnitDescription"></param> 
        /// <param name="savedTripAircraftDataDefault"></param> 
        /// <param name="savedTripAircraftDataNormalCruiseTasUnit"></param> 
        /// <param name="savedTripAircraftDataNormalCruiseTasAmount"></param> 
        /// <param name="savedTripAircraftDataNormalCruiseTasUnitDescription"></param> 
        /// <param name="savedTripAircraftDataApcode"></param> 
        /// <param name="savedTripAircraftDataMinimumUpliftUnit"></param> 
        /// <param name="savedTripAircraftDataMinimumUpliftAmount"></param> 
        /// <param name="savedTripAircraftDataMinimumUpliftUnitDescription"></param> 
        /// <param name="savedTripAircraftDataAircraftTypeEngineName"></param> 
        /// <param name="savedTripAircraftDataFleetGroup"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAircraftId"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMake"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsModel"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnit"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsNormalCruiseTasAmount"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnitDescription"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsFuelCapacityUnit"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsFuelCapacityAmount"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsFuelCapacityUnitDescription"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnit"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthAmount"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnitDescription"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsRangeNm"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsRangePerGal"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxRangeHours"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxRangeMinutes"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsReserveMinutes"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsReserveNm"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsSize"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnit"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightAmount"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnitDescription"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnit"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxLandingWeightAmount"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnitDescription"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAircraftCeilingUnit"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAircraftCeilingAmount"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAircraftCeilingUnitDescription"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnit"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelAmount"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnitDescription"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnit"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAmount"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnitDescription"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnit"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsZeroFuelWeightAmount"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnitDescription"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnit"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsBasicOperatingWeightAmount"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnitDescription"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsFuelType"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsIfrreseveUnit"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsIfrreseveAmount"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsIfrreseveUnitDescription"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsIcao"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsOid"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsAircraftId"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsModelName"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsLengthFt"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsHeightFt"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsWingSpanFt"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsLengthFtInches"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsHeightFtInches"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsWidthFtInches"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCrewConfiguration"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPassengerConfiguration"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPressurizationPsi"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardLbs"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardGal"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalLbs"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalGal"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxRampWeightLbs"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxTakeoffWeightLbs"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsZeroFuelWeightLbs"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsBasicOperatingWeightLbs"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxLandingWeightLbs"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsCleanSpeedKnots"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsoLandingSpeedKnots"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalCruiseTasSpeedKnots"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVmoMaxOpIasSpeedKnots"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalClimbFpm"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineOutClimbFpm"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCeilingFt"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsLandingPerfFaaFieldLengthFt"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerfSlIsaBfl"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerf500020cBfl"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsRangeNm"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNumberOfEngines"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineModelS"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineThrustLbsPerEngine"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineShaftHpPerEngine"></param> 
        /// <param name="savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineCommonTboHours"></param> 
        /// <returns>UpdateSavedTripResponse</returns>            
        public UpdateSavedTripResponse UpdateSavedTrip (int? savedTripId, string savedTripName, int? savedTripCompanyId, int? savedTripUserId, int? savedTripUserAircraftId, string savedTripFuelUnitType, double? savedTripStartingFuel, string savedTripMltmodelPropertiesJson, List<Object> savedTripSavedTripLegs, int? savedTripAircraftDataId, string savedTripAircraftDataTailNumber, int? savedTripAircraftDataMinLandingRunwayLengthUnit, double? savedTripAircraftDataMinLandingRunwayLengthAmount, string savedTripAircraftDataMinLandingRunwayLengthUnitDescription, double? savedTripAircraftDataMaintCostPerHour, int? savedTripAircraftDataMaxRangeUnit, double? savedTripAircraftDataMaxRangeAmount, string savedTripAircraftDataMaxRangeUnitDescription, int? savedTripAircraftDataSize, double? savedTripAircraftDataFuelBurnRate, int? savedTripAircraftDataFuelCapacityUnit, double? savedTripAircraftDataFuelCapacityAmount, string savedTripAircraftDataFuelCapacityUnitDescription, double? savedTripAircraftDataPayloadBurnRate, int? savedTripAircraftDataAircraftId, int? savedTripAircraftDataMaxTakeoffWeightUnit, double? savedTripAircraftDataMaxTakeoffWeightAmount, string savedTripAircraftDataMaxTakeoffWeightUnitDescription, int? savedTripAircraftDataMaxLandingWeightUnit, double? savedTripAircraftDataMaxLandingWeightAmount, string savedTripAircraftDataMaxLandingWeightUnitDescription, int? savedTripAircraftDataAircraftCeilingUnit, double? savedTripAircraftDataAircraftCeilingAmount, string savedTripAircraftDataAircraftCeilingUnitDescription, int? savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnit, double? savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelAmount, string savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnitDescription, int? savedTripAircraftDataMinTakeoffRunwayLengthUnit, double? savedTripAircraftDataMinTakeoffRunwayLengthAmount, string savedTripAircraftDataMinTakeoffRunwayLengthUnitDescription, int? savedTripAircraftDataZeroFuelWeightUnit, double? savedTripAircraftDataZeroFuelWeightAmount, string savedTripAircraftDataZeroFuelWeightUnitDescription, int? savedTripAircraftDataBasicOperatingWeightUnit, double? savedTripAircraftDataBasicOperatingWeightAmount, string savedTripAircraftDataBasicOperatingWeightUnitDescription, string savedTripAircraftDataQbclass, int? savedTripAircraftDataIfrreseveUnit, double? savedTripAircraftDataIfrreseveAmount, string savedTripAircraftDataIfrreseveUnitDescription, bool? savedTripAircraftDataDefault, int? savedTripAircraftDataNormalCruiseTasUnit, double? savedTripAircraftDataNormalCruiseTasAmount, string savedTripAircraftDataNormalCruiseTasUnitDescription, string savedTripAircraftDataApcode, int? savedTripAircraftDataMinimumUpliftUnit, double? savedTripAircraftDataMinimumUpliftAmount, string savedTripAircraftDataMinimumUpliftUnitDescription, string savedTripAircraftDataAircraftTypeEngineName, string savedTripAircraftDataFleetGroup, int? savedTripAircraftDataFactorySpecificationsAircraftId, string savedTripAircraftDataFactorySpecificationsMake, string savedTripAircraftDataFactorySpecificationsModel, int? savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnit, double? savedTripAircraftDataFactorySpecificationsNormalCruiseTasAmount, string savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnitDescription, int? savedTripAircraftDataFactorySpecificationsFuelCapacityUnit, double? savedTripAircraftDataFactorySpecificationsFuelCapacityAmount, string savedTripAircraftDataFactorySpecificationsFuelCapacityUnitDescription, int? savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnit, double? savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthAmount, string savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnitDescription, double? savedTripAircraftDataFactorySpecificationsRangeNm, double? savedTripAircraftDataFactorySpecificationsRangePerGal, double? savedTripAircraftDataFactorySpecificationsMaxRangeHours, double? savedTripAircraftDataFactorySpecificationsMaxRangeMinutes, double? savedTripAircraftDataFactorySpecificationsReserveMinutes, double? savedTripAircraftDataFactorySpecificationsReserveNm, int? savedTripAircraftDataFactorySpecificationsSize, int? savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnit, double? savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightAmount, string savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnitDescription, int? savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnit, double? savedTripAircraftDataFactorySpecificationsMaxLandingWeightAmount, string savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnitDescription, int? savedTripAircraftDataFactorySpecificationsAircraftCeilingUnit, double? savedTripAircraftDataFactorySpecificationsAircraftCeilingAmount, string savedTripAircraftDataFactorySpecificationsAircraftCeilingUnitDescription, int? savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnit, double? savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelAmount, string savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnitDescription, int? savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnit, double? savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAmount, string savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnitDescription, int? savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnit, double? savedTripAircraftDataFactorySpecificationsZeroFuelWeightAmount, string savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnitDescription, int? savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnit, double? savedTripAircraftDataFactorySpecificationsBasicOperatingWeightAmount, string savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnitDescription, string savedTripAircraftDataFactorySpecificationsFuelType, int? savedTripAircraftDataFactorySpecificationsIfrreseveUnit, double? savedTripAircraftDataFactorySpecificationsIfrreseveAmount, string savedTripAircraftDataFactorySpecificationsIfrreseveUnitDescription, string savedTripAircraftDataFactorySpecificationsIcao, int? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsOid, int? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsAircraftId, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsModelName, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsLengthFt, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsHeightFt, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsWingSpanFt, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsLengthFtInches, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsHeightFtInches, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsWidthFtInches, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCrewConfiguration, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPassengerConfiguration, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPressurizationPsi, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardGal, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalGal, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxRampWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxTakeoffWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsZeroFuelWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsBasicOperatingWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxLandingWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsCleanSpeedKnots, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsoLandingSpeedKnots, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalCruiseTasSpeedKnots, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVmoMaxOpIasSpeedKnots, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalClimbFpm, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineOutClimbFpm, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCeilingFt, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsLandingPerfFaaFieldLengthFt, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerfSlIsaBfl, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerf500020cBfl, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsRangeNm, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNumberOfEngines, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineModelS, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineThrustLbsPerEngine, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineShaftHpPerEngine, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineCommonTboHours)
        {
            
            // verify the required parameter 'savedTripId' is set
            if (savedTripId == null) throw new ApiException(400, "Missing required parameter 'savedTripId' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripName' is set
            if (savedTripName == null) throw new ApiException(400, "Missing required parameter 'savedTripName' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripCompanyId' is set
            if (savedTripCompanyId == null) throw new ApiException(400, "Missing required parameter 'savedTripCompanyId' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripUserId' is set
            if (savedTripUserId == null) throw new ApiException(400, "Missing required parameter 'savedTripUserId' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripUserAircraftId' is set
            if (savedTripUserAircraftId == null) throw new ApiException(400, "Missing required parameter 'savedTripUserAircraftId' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripFuelUnitType' is set
            if (savedTripFuelUnitType == null) throw new ApiException(400, "Missing required parameter 'savedTripFuelUnitType' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripStartingFuel' is set
            if (savedTripStartingFuel == null) throw new ApiException(400, "Missing required parameter 'savedTripStartingFuel' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripMltmodelPropertiesJson' is set
            if (savedTripMltmodelPropertiesJson == null) throw new ApiException(400, "Missing required parameter 'savedTripMltmodelPropertiesJson' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripSavedTripLegs' is set
            if (savedTripSavedTripLegs == null) throw new ApiException(400, "Missing required parameter 'savedTripSavedTripLegs' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataId' is set
            if (savedTripAircraftDataId == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataId' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataTailNumber' is set
            if (savedTripAircraftDataTailNumber == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataTailNumber' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMinLandingRunwayLengthUnit' is set
            if (savedTripAircraftDataMinLandingRunwayLengthUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMinLandingRunwayLengthUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMinLandingRunwayLengthAmount' is set
            if (savedTripAircraftDataMinLandingRunwayLengthAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMinLandingRunwayLengthAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMinLandingRunwayLengthUnitDescription' is set
            if (savedTripAircraftDataMinLandingRunwayLengthUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMinLandingRunwayLengthUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMaintCostPerHour' is set
            if (savedTripAircraftDataMaintCostPerHour == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMaintCostPerHour' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMaxRangeUnit' is set
            if (savedTripAircraftDataMaxRangeUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMaxRangeUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMaxRangeAmount' is set
            if (savedTripAircraftDataMaxRangeAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMaxRangeAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMaxRangeUnitDescription' is set
            if (savedTripAircraftDataMaxRangeUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMaxRangeUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataSize' is set
            if (savedTripAircraftDataSize == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataSize' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFuelBurnRate' is set
            if (savedTripAircraftDataFuelBurnRate == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFuelBurnRate' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFuelCapacityUnit' is set
            if (savedTripAircraftDataFuelCapacityUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFuelCapacityUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFuelCapacityAmount' is set
            if (savedTripAircraftDataFuelCapacityAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFuelCapacityAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFuelCapacityUnitDescription' is set
            if (savedTripAircraftDataFuelCapacityUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFuelCapacityUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataPayloadBurnRate' is set
            if (savedTripAircraftDataPayloadBurnRate == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataPayloadBurnRate' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataAircraftId' is set
            if (savedTripAircraftDataAircraftId == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataAircraftId' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMaxTakeoffWeightUnit' is set
            if (savedTripAircraftDataMaxTakeoffWeightUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMaxTakeoffWeightUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMaxTakeoffWeightAmount' is set
            if (savedTripAircraftDataMaxTakeoffWeightAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMaxTakeoffWeightAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMaxTakeoffWeightUnitDescription' is set
            if (savedTripAircraftDataMaxTakeoffWeightUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMaxTakeoffWeightUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMaxLandingWeightUnit' is set
            if (savedTripAircraftDataMaxLandingWeightUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMaxLandingWeightUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMaxLandingWeightAmount' is set
            if (savedTripAircraftDataMaxLandingWeightAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMaxLandingWeightAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMaxLandingWeightUnitDescription' is set
            if (savedTripAircraftDataMaxLandingWeightUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMaxLandingWeightUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataAircraftCeilingUnit' is set
            if (savedTripAircraftDataAircraftCeilingUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataAircraftCeilingUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataAircraftCeilingAmount' is set
            if (savedTripAircraftDataAircraftCeilingAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataAircraftCeilingAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataAircraftCeilingUnitDescription' is set
            if (savedTripAircraftDataAircraftCeilingUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataAircraftCeilingUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnit' is set
            if (savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelAmount' is set
            if (savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnitDescription' is set
            if (savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMinTakeoffRunwayLengthUnit' is set
            if (savedTripAircraftDataMinTakeoffRunwayLengthUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMinTakeoffRunwayLengthUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMinTakeoffRunwayLengthAmount' is set
            if (savedTripAircraftDataMinTakeoffRunwayLengthAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMinTakeoffRunwayLengthAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMinTakeoffRunwayLengthUnitDescription' is set
            if (savedTripAircraftDataMinTakeoffRunwayLengthUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMinTakeoffRunwayLengthUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataZeroFuelWeightUnit' is set
            if (savedTripAircraftDataZeroFuelWeightUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataZeroFuelWeightUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataZeroFuelWeightAmount' is set
            if (savedTripAircraftDataZeroFuelWeightAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataZeroFuelWeightAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataZeroFuelWeightUnitDescription' is set
            if (savedTripAircraftDataZeroFuelWeightUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataZeroFuelWeightUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataBasicOperatingWeightUnit' is set
            if (savedTripAircraftDataBasicOperatingWeightUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataBasicOperatingWeightUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataBasicOperatingWeightAmount' is set
            if (savedTripAircraftDataBasicOperatingWeightAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataBasicOperatingWeightAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataBasicOperatingWeightUnitDescription' is set
            if (savedTripAircraftDataBasicOperatingWeightUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataBasicOperatingWeightUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataQbclass' is set
            if (savedTripAircraftDataQbclass == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataQbclass' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataIfrreseveUnit' is set
            if (savedTripAircraftDataIfrreseveUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataIfrreseveUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataIfrreseveAmount' is set
            if (savedTripAircraftDataIfrreseveAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataIfrreseveAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataIfrreseveUnitDescription' is set
            if (savedTripAircraftDataIfrreseveUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataIfrreseveUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataDefault' is set
            if (savedTripAircraftDataDefault == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataDefault' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataNormalCruiseTasUnit' is set
            if (savedTripAircraftDataNormalCruiseTasUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataNormalCruiseTasUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataNormalCruiseTasAmount' is set
            if (savedTripAircraftDataNormalCruiseTasAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataNormalCruiseTasAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataNormalCruiseTasUnitDescription' is set
            if (savedTripAircraftDataNormalCruiseTasUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataNormalCruiseTasUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataApcode' is set
            if (savedTripAircraftDataApcode == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataApcode' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMinimumUpliftUnit' is set
            if (savedTripAircraftDataMinimumUpliftUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMinimumUpliftUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMinimumUpliftAmount' is set
            if (savedTripAircraftDataMinimumUpliftAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMinimumUpliftAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataMinimumUpliftUnitDescription' is set
            if (savedTripAircraftDataMinimumUpliftUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataMinimumUpliftUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataAircraftTypeEngineName' is set
            if (savedTripAircraftDataAircraftTypeEngineName == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataAircraftTypeEngineName' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFleetGroup' is set
            if (savedTripAircraftDataFleetGroup == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFleetGroup' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAircraftId' is set
            if (savedTripAircraftDataFactorySpecificationsAircraftId == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAircraftId' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMake' is set
            if (savedTripAircraftDataFactorySpecificationsMake == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMake' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsModel' is set
            if (savedTripAircraftDataFactorySpecificationsModel == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsModel' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnit' is set
            if (savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsNormalCruiseTasAmount' is set
            if (savedTripAircraftDataFactorySpecificationsNormalCruiseTasAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsNormalCruiseTasAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnitDescription' is set
            if (savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsFuelCapacityUnit' is set
            if (savedTripAircraftDataFactorySpecificationsFuelCapacityUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsFuelCapacityUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsFuelCapacityAmount' is set
            if (savedTripAircraftDataFactorySpecificationsFuelCapacityAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsFuelCapacityAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsFuelCapacityUnitDescription' is set
            if (savedTripAircraftDataFactorySpecificationsFuelCapacityUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsFuelCapacityUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnit' is set
            if (savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthAmount' is set
            if (savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnitDescription' is set
            if (savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsRangeNm' is set
            if (savedTripAircraftDataFactorySpecificationsRangeNm == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsRangeNm' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsRangePerGal' is set
            if (savedTripAircraftDataFactorySpecificationsRangePerGal == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsRangePerGal' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMaxRangeHours' is set
            if (savedTripAircraftDataFactorySpecificationsMaxRangeHours == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMaxRangeHours' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMaxRangeMinutes' is set
            if (savedTripAircraftDataFactorySpecificationsMaxRangeMinutes == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMaxRangeMinutes' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsReserveMinutes' is set
            if (savedTripAircraftDataFactorySpecificationsReserveMinutes == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsReserveMinutes' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsReserveNm' is set
            if (savedTripAircraftDataFactorySpecificationsReserveNm == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsReserveNm' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsSize' is set
            if (savedTripAircraftDataFactorySpecificationsSize == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsSize' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnit' is set
            if (savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightAmount' is set
            if (savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnitDescription' is set
            if (savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnit' is set
            if (savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMaxLandingWeightAmount' is set
            if (savedTripAircraftDataFactorySpecificationsMaxLandingWeightAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMaxLandingWeightAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnitDescription' is set
            if (savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAircraftCeilingUnit' is set
            if (savedTripAircraftDataFactorySpecificationsAircraftCeilingUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAircraftCeilingUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAircraftCeilingAmount' is set
            if (savedTripAircraftDataFactorySpecificationsAircraftCeilingAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAircraftCeilingAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAircraftCeilingUnitDescription' is set
            if (savedTripAircraftDataFactorySpecificationsAircraftCeilingUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAircraftCeilingUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnit' is set
            if (savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelAmount' is set
            if (savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnitDescription' is set
            if (savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnit' is set
            if (savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAmount' is set
            if (savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnitDescription' is set
            if (savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnit' is set
            if (savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsZeroFuelWeightAmount' is set
            if (savedTripAircraftDataFactorySpecificationsZeroFuelWeightAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsZeroFuelWeightAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnitDescription' is set
            if (savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnit' is set
            if (savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsBasicOperatingWeightAmount' is set
            if (savedTripAircraftDataFactorySpecificationsBasicOperatingWeightAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsBasicOperatingWeightAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnitDescription' is set
            if (savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsFuelType' is set
            if (savedTripAircraftDataFactorySpecificationsFuelType == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsFuelType' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsIfrreseveUnit' is set
            if (savedTripAircraftDataFactorySpecificationsIfrreseveUnit == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsIfrreseveUnit' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsIfrreseveAmount' is set
            if (savedTripAircraftDataFactorySpecificationsIfrreseveAmount == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsIfrreseveAmount' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsIfrreseveUnitDescription' is set
            if (savedTripAircraftDataFactorySpecificationsIfrreseveUnitDescription == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsIfrreseveUnitDescription' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsIcao' is set
            if (savedTripAircraftDataFactorySpecificationsIcao == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsIcao' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsOid' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsOid == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsOid' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsAircraftId' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsAircraftId == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsAircraftId' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsModelName' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsModelName == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsModelName' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsLengthFt' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsLengthFt == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsLengthFt' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsHeightFt' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsHeightFt == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsHeightFt' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsWingSpanFt' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsWingSpanFt == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsWingSpanFt' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsLengthFtInches' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsLengthFtInches == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsLengthFtInches' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsHeightFtInches' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsHeightFtInches == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsHeightFtInches' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsWidthFtInches' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsWidthFtInches == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsWidthFtInches' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCrewConfiguration' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCrewConfiguration == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCrewConfiguration' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPassengerConfiguration' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPassengerConfiguration == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPassengerConfiguration' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPressurizationPsi' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPressurizationPsi == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPressurizationPsi' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardLbs' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardLbs == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardLbs' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardGal' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardGal == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardGal' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalLbs' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalLbs == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalLbs' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalGal' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalGal == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalGal' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxRampWeightLbs' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxRampWeightLbs == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxRampWeightLbs' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxTakeoffWeightLbs' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxTakeoffWeightLbs == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxTakeoffWeightLbs' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsZeroFuelWeightLbs' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsZeroFuelWeightLbs == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsZeroFuelWeightLbs' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsBasicOperatingWeightLbs' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsBasicOperatingWeightLbs == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsBasicOperatingWeightLbs' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxLandingWeightLbs' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxLandingWeightLbs == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxLandingWeightLbs' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsCleanSpeedKnots' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsCleanSpeedKnots == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsCleanSpeedKnots' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsoLandingSpeedKnots' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsoLandingSpeedKnots == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsoLandingSpeedKnots' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalCruiseTasSpeedKnots' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalCruiseTasSpeedKnots == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalCruiseTasSpeedKnots' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVmoMaxOpIasSpeedKnots' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVmoMaxOpIasSpeedKnots == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVmoMaxOpIasSpeedKnots' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalClimbFpm' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalClimbFpm == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalClimbFpm' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineOutClimbFpm' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineOutClimbFpm == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineOutClimbFpm' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCeilingFt' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCeilingFt == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCeilingFt' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsLandingPerfFaaFieldLengthFt' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsLandingPerfFaaFieldLengthFt == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsLandingPerfFaaFieldLengthFt' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerfSlIsaBfl' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerfSlIsaBfl == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerfSlIsaBfl' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerf500020cBfl' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerf500020cBfl == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerf500020cBfl' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsRangeNm' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsRangeNm == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsRangeNm' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNumberOfEngines' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNumberOfEngines == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNumberOfEngines' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineModelS' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineModelS == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineModelS' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineThrustLbsPerEngine' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineThrustLbsPerEngine == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineThrustLbsPerEngine' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineShaftHpPerEngine' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineShaftHpPerEngine == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineShaftHpPerEngine' when calling UpdateSavedTrip");
            
            // verify the required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineCommonTboHours' is set
            if (savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineCommonTboHours == null) throw new ApiException(400, "Missing required parameter 'savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineCommonTboHours' when calling UpdateSavedTrip");
            
    
            var path = "/api/SavedTrip";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "SavedTrip.Id" + "}", ApiClient.ParameterToString(savedTripId));
path = path.Replace("{" + "SavedTrip.Name" + "}", ApiClient.ParameterToString(savedTripName));
path = path.Replace("{" + "SavedTrip.CompanyId" + "}", ApiClient.ParameterToString(savedTripCompanyId));
path = path.Replace("{" + "SavedTrip.UserId" + "}", ApiClient.ParameterToString(savedTripUserId));
path = path.Replace("{" + "SavedTrip.UserAircraftId" + "}", ApiClient.ParameterToString(savedTripUserAircraftId));
path = path.Replace("{" + "SavedTrip.FuelUnitType" + "}", ApiClient.ParameterToString(savedTripFuelUnitType));
path = path.Replace("{" + "SavedTrip.StartingFuel" + "}", ApiClient.ParameterToString(savedTripStartingFuel));
path = path.Replace("{" + "SavedTrip.MltmodelPropertiesJson" + "}", ApiClient.ParameterToString(savedTripMltmodelPropertiesJson));
path = path.Replace("{" + "SavedTrip.SavedTripLegs" + "}", ApiClient.ParameterToString(savedTripSavedTripLegs));
path = path.Replace("{" + "SavedTrip.AircraftData.Id" + "}", ApiClient.ParameterToString(savedTripAircraftDataId));
path = path.Replace("{" + "SavedTrip.AircraftData.TailNumber" + "}", ApiClient.ParameterToString(savedTripAircraftDataTailNumber));
path = path.Replace("{" + "SavedTrip.AircraftData.MinLandingRunwayLength.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataMinLandingRunwayLengthUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.MinLandingRunwayLength.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataMinLandingRunwayLengthAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.MinLandingRunwayLength.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataMinLandingRunwayLengthUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.MaintCostPerHour" + "}", ApiClient.ParameterToString(savedTripAircraftDataMaintCostPerHour));
path = path.Replace("{" + "SavedTrip.AircraftData.MaxRange.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataMaxRangeUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.MaxRange.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataMaxRangeAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.MaxRange.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataMaxRangeUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.Size" + "}", ApiClient.ParameterToString(savedTripAircraftDataSize));
path = path.Replace("{" + "SavedTrip.AircraftData.FuelBurnRate" + "}", ApiClient.ParameterToString(savedTripAircraftDataFuelBurnRate));
path = path.Replace("{" + "SavedTrip.AircraftData.FuelCapacity.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataFuelCapacityUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.FuelCapacity.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataFuelCapacityAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.FuelCapacity.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataFuelCapacityUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.PayloadBurnRate" + "}", ApiClient.ParameterToString(savedTripAircraftDataPayloadBurnRate));
path = path.Replace("{" + "SavedTrip.AircraftData.AircraftId" + "}", ApiClient.ParameterToString(savedTripAircraftDataAircraftId));
path = path.Replace("{" + "SavedTrip.AircraftData.MaxTakeoffWeight.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataMaxTakeoffWeightUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.MaxTakeoffWeight.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataMaxTakeoffWeightAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.MaxTakeoffWeight.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataMaxTakeoffWeightUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.MaxLandingWeight.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataMaxLandingWeightUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.MaxLandingWeight.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataMaxLandingWeightAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.MaxLandingWeight.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataMaxLandingWeightUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.AircraftCeiling.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataAircraftCeilingUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.AircraftCeiling.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataAircraftCeilingAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.AircraftCeiling.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataAircraftCeilingUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.MinTakeoffRunwayLengthAtSeaLevel.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.MinTakeoffRunwayLengthAtSeaLevel.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.MinTakeoffRunwayLengthAtSeaLevel.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.MinTakeoffRunwayLength.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataMinTakeoffRunwayLengthUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.MinTakeoffRunwayLength.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataMinTakeoffRunwayLengthAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.MinTakeoffRunwayLength.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataMinTakeoffRunwayLengthUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.ZeroFuelWeight.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataZeroFuelWeightUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.ZeroFuelWeight.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataZeroFuelWeightAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.ZeroFuelWeight.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataZeroFuelWeightUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.BasicOperatingWeight.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataBasicOperatingWeightUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.BasicOperatingWeight.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataBasicOperatingWeightAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.BasicOperatingWeight.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataBasicOperatingWeightUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.Qbclass" + "}", ApiClient.ParameterToString(savedTripAircraftDataQbclass));
path = path.Replace("{" + "SavedTrip.AircraftData.Ifrreseve.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataIfrreseveUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.Ifrreseve.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataIfrreseveAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.Ifrreseve.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataIfrreseveUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.Default" + "}", ApiClient.ParameterToString(savedTripAircraftDataDefault));
path = path.Replace("{" + "SavedTrip.AircraftData.NormalCruiseTas.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataNormalCruiseTasUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.NormalCruiseTas.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataNormalCruiseTasAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.NormalCruiseTas.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataNormalCruiseTasUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.Apcode" + "}", ApiClient.ParameterToString(savedTripAircraftDataApcode));
path = path.Replace("{" + "SavedTrip.AircraftData.MinimumUplift.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataMinimumUpliftUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.MinimumUplift.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataMinimumUpliftAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.MinimumUplift.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataMinimumUpliftUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.AircraftTypeEngineName" + "}", ApiClient.ParameterToString(savedTripAircraftDataAircraftTypeEngineName));
path = path.Replace("{" + "SavedTrip.AircraftData.FleetGroup" + "}", ApiClient.ParameterToString(savedTripAircraftDataFleetGroup));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AircraftId" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAircraftId));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.Make" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMake));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.Model" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsModel));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.NormalCruiseTas.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.NormalCruiseTas.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsNormalCruiseTasAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.NormalCruiseTas.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.FuelCapacity.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsFuelCapacityUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.FuelCapacity.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsFuelCapacityAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.FuelCapacity.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsFuelCapacityUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MinLandingRunwayLength.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MinLandingRunwayLength.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MinLandingRunwayLength.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.RangeNm" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsRangeNm));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.RangePerGal" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsRangePerGal));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MaxRangeHours" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMaxRangeHours));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MaxRangeMinutes" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMaxRangeMinutes));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.ReserveMinutes" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsReserveMinutes));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.ReserveNm" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsReserveNm));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.Size" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsSize));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MaxTakeoffWeight.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MaxTakeoffWeight.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MaxTakeoffWeight.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MaxLandingWeight.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MaxLandingWeight.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMaxLandingWeightAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MaxLandingWeight.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AircraftCeiling.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAircraftCeilingUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AircraftCeiling.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAircraftCeilingAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AircraftCeiling.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAircraftCeilingUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MinTakeoffRunwayLengthAtSeaLevel.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MinTakeoffRunwayLengthAtSeaLevel.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MinTakeoffRunwayLengthAtSeaLevel.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MinTakeoffRunwayLength.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MinTakeoffRunwayLength.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.MinTakeoffRunwayLength.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.ZeroFuelWeight.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.ZeroFuelWeight.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsZeroFuelWeightAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.ZeroFuelWeight.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.BasicOperatingWeight.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.BasicOperatingWeight.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsBasicOperatingWeightAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.BasicOperatingWeight.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.FuelType" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsFuelType));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.Ifrreseve.Unit" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsIfrreseveUnit));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.Ifrreseve.Amount" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsIfrreseveAmount));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.Ifrreseve.UnitDescription" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsIfrreseveUnitDescription));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.Icao" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsIcao));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.Oid" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsOid));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.AircraftId" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsAircraftId));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.ModelName" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsModelName));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.FuselageDimensionsLengthFt" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsLengthFt));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.FuselageDimensionsHeightFt" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsHeightFt));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.FuselageDimensionsWingSpanFt" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsWingSpanFt));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.CabinDimensionsLengthFtInches" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsLengthFtInches));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.CabinDimensionsHeightFtInches" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsHeightFtInches));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.CabinDimensionsWidthFtInches" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsWidthFtInches));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.CrewConfiguration" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCrewConfiguration));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.PassengerConfiguration" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPassengerConfiguration));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.PressurizationPsi" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPressurizationPsi));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.FuelCapacityStandardLbs" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardLbs));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.FuelCapacityStandardGal" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardGal));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.FuelCapacityOptionalLbs" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalLbs));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.FuelCapacityOptionalGal" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalGal));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.MaxRampWeightLbs" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxRampWeightLbs));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.MaxTakeoffWeightLbs" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxTakeoffWeightLbs));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.ZeroFuelWeightLbs" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsZeroFuelWeightLbs));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.BasicOperatingWeightLbs" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsBasicOperatingWeightLbs));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.MaxLandingWeightLbs" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxLandingWeightLbs));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.VsCleanSpeedKnots" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsCleanSpeedKnots));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.VsoLandingSpeedKnots" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsoLandingSpeedKnots));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.NormalCruiseTasSpeedKnots" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalCruiseTasSpeedKnots));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.VmoMaxOpIasSpeedKnots" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVmoMaxOpIasSpeedKnots));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.NormalClimbFpm" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalClimbFpm));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.EngineOutClimbFpm" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineOutClimbFpm));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.CeilingFt" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCeilingFt));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.LandingPerfFaaFieldLengthFt" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsLandingPerfFaaFieldLengthFt));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.TakeoffPerfSlIsaBfl" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerfSlIsaBfl));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.TakeoffPerf500020cBfl" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerf500020cBfl));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.RangeNm" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsRangeNm));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.NumberOfEngines" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNumberOfEngines));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.EngineModelS" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineModelS));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.EngineThrustLbsPerEngine" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineThrustLbsPerEngine));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.EngineShaftHpPerEngine" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineShaftHpPerEngine));
path = path.Replace("{" + "SavedTrip.AircraftData.FactorySpecifications.AdditionalSpecifications.EngineCommonTboHours" + "}", ApiClient.ParameterToString(savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineCommonTboHours));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSavedTrip: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSavedTrip: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateSavedTripResponse) ApiClient.Deserialize(response.Content, typeof(UpdateSavedTripResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateSavedTripLegResponse</returns>            
        public UpdateSavedTripLegResponse UpdateSavedTripLeg (UpdateSavedTripLegRequest body)
        {
            
    
            var path = "/api/SavedTrip/legs";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSavedTripLeg: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSavedTripLeg: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateSavedTripLegResponse) ApiClient.Deserialize(response.Content, typeof(UpdateSavedTripLegResponse), response.Headers);
        }
    
    }
}
