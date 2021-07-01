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
    public interface IFuelPricingApi
    {
        /// <summary>
        /// Internal use only - delete all cached pricing for a currently authenticated company. 
        /// </summary>
        /// <returns>DeleteCurrentPricingResponse</returns>
        DeleteCurrentPricingResponse DeleteCurrentPricingForCompany ();
        /// <summary>
        /// Delete Supported Price Sheet File Tests by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteSupportedPriceSheetFileTestsResponse</returns>
        DeleteSupportedPriceSheetFileTestsResponse DeleteSupportedPriceSheetFileTests (int? id);
        /// <summary>
        /// Deletes Supported Price Sheet Files by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteSupportedPriceSheetFilesResponse</returns>
        DeleteSupportedPriceSheetFilesResponse DeleteSupportedPriceSheetFiles (int? id);
        /// <summary>
        /// Internal use only - delete all weekly pricing records for a particular fuel vendor. 
        /// </summary>
        /// <param name="fuelVendorId"></param>
        /// <returns>DeleteWeeklyPricingResponse</returns>
        DeleteWeeklyPricingResponse DeleteWeeklyPricingForFuelVendor (int? fuelVendorId);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>AssociatedDetailsResponse</returns>
        AssociatedDetailsResponse GetAssociatedDetailsForFuelOption (AssociatedDetailsRequest body);
        /// <summary>
        /// Fetch all cached pricing for the specified comma-delimited ICAOs currently available for the user. 
        /// </summary>
        /// <param name="commaDelimitedIcaos"></param>
        /// <returns>CurrentPricingResponse</returns>
        CurrentPricingResponse GetCurrentPricingForLocation (string commaDelimitedIcaos);
        /// <summary>
        /// Fetch all cached pricing for the specified comma-delimited ICAOs and flight type currently available for the user. 
        /// </summary>
        /// <param name="commaDelimitedIcaos"></param>
        /// <param name="flightType"></param>
        /// <returns>CurrentPricingResponse</returns>
        CurrentPricingResponse GetCurrentPricingForLocationAndFlightType (string commaDelimitedIcaos, string flightType);
        /// <summary>
        /// Retrieves a live quote from all vendor web services tied to the flight department&#39;s account using their default flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings. It is always recommended to do a live quote for pricing if one hasn&#39;t been done in the last few hours for the desired airports.
        /// </summary>
        /// <param name="commaDelimitedIcaos"></param>
        /// <returns>CurrentPricingResponse</returns>
        CurrentPricingResponse GetLiveQuoteForLocations (string commaDelimitedIcaos);
        /// <summary>
        /// Retrieves a live quote from all vendor web services tied to the flight department&#39;s account using the specified flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings. It is always recommended to do a live quote for pricing if one hasn&#39;t been done in the last few hours for the desired airports.
        /// </summary>
        /// <param name="commaDelimitedIcaos"></param>
        /// <param name="flightType"></param>
        /// <returns>CurrentPricingResponse</returns>
        CurrentPricingResponse GetLiveQuoteForLocationsAndFlightType (string commaDelimitedIcaos, string flightType);
        /// <summary>
        /// Retrieves a live quote from all vendor web services tied to the flight department&#39;s account using the specified flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.  Only quotes the specified fuel vendor based on the provided {fuelVendorId}. It is always recommended to do a live quote for pricing if one hasn&#39;t been done in the last few hours for the desired airports.
        /// </summary>
        /// <param name="commaDelimitedIcaos"></param>
        /// <param name="flightType"></param>
        /// <param name="fuelVendorId"></param>
        /// <returns>CurrentPricingResponse</returns>
        CurrentPricingResponse GetLiveQuoteForLocationsAndFlightTypeAndVendor (string commaDelimitedIcaos, string flightType, int? fuelVendorId);
        /// <summary>
        /// Internal use only - Fetch a quote response from the EPIC Aviation web service. 
        /// </summary>
        /// <param name="commaDelimitedAirportIdentifiers"></param>
        /// <returns>EpicQuoteResponse</returns>
        EpicQuoteResponse GetQuotFromEpic (string commaDelimitedAirportIdentifiers);
        /// <summary>
        /// Gets Supported Price Sheet File Tests by supportedPriceSheetFileId 
        /// </summary>
        /// <param name="supportedPriceSheetFileId"></param>
        /// <returns>SupportedPriceSheetFileTestsResponse</returns>
        SupportedPriceSheetFileTestsResponse GetSupportedPriceSheetFileTestsBySupportedPriceSheetFileId (int? supportedPriceSheetFileId);
        /// <summary>
        /// Gets Supported Price Sheet Files by FuelVendorId 
        /// </summary>
        /// <param name="fuelVendorId"></param>
        /// <returns>SupportedPriceSheetFileListResponse</returns>
        SupportedPriceSheetFileListResponse GetSupportedPriceSheetFilesByByFuelVendorId (int? fuelVendorId);
        /// <summary>
        /// Get Supported Price Sheet Files by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SupportedPriceSheetFilesResponse</returns>
        SupportedPriceSheetFilesResponse GetSupportedPriceSheetFilesByById (int? id);
        /// <summary>
        /// If available, will fetch records from the weekly price sheet received each week for the specified [commaDelimitedIcaos] and [fuelVendorId].  These are to be used as a fallback option when a vendor&#39;s service is unavailable. 
        /// </summary>
        /// <param name="fuelVendorId"></param>
        /// <param name="commaDelimitedIcaos"></param>
        /// <returns>WeeklyPricingListResponse</returns>
        WeeklyPricingListResponse GetWeeklyPricingForFuelerAndLocation (int? fuelVendorId, string commaDelimitedIcaos);
        /// <summary>
        /// /// If available, will fetch records from the weekly price sheet received each week for the specified [commaDelimitedIcaos].  These are to be used as a fallback option when a vendor&#39;s service is unavailable. 
        /// </summary>
        /// <param name="commaDelimitedIcaos"></param>
        /// <returns>WeeklyPricingListResponse</returns>
        WeeklyPricingListResponse GetWeeklyPricingForLocation (string commaDelimitedIcaos);
        /// <summary>
        /// Internal use only - Please use the \&quot;dispatching\&quot; API to dispatch a full fuel order.  This API method is strictly for notifying the fuel vendor. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostFuelOrderResponse</returns>
        PostFuelOrderResponse PostFuelOrder (PostFuelOrderRequest body);
        /// <summary>
        /// Post Supported Price Sheet File Tests 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostSupportedPriceSheetFileTestsResponse</returns>
        PostSupportedPriceSheetFileTestsResponse PostSupportedPriceSheetFileTests (PostSupportedPriceSheetFileTestsRequest body);
        /// <summary>
        /// Post Supported Price Sheet Files 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostSupportedPriceSheetFilesResponse</returns>
        PostSupportedPriceSheetFilesResponse PostSupportedPriceSheetFiles (PostSupportedPriceSheetFilesRequest body);
        /// <summary>
        /// Updates Supported Price Sheet File Tests 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateSupportedPriceSheetFileTestsResponse</returns>
        UpdateSupportedPriceSheetFileTestsResponse UpdateSupportedPriceSheetFileTests (UpdateSupportedPriceSheetFileTestsRequest body);
        /// <summary>
        /// Updates Supported Price Sheet Files 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateSupportedPriceSheetFilesResponse</returns>
        UpdateSupportedPriceSheetFilesResponse UpdateSupportedPriceSheetFiles (UpdateSupportedPriceSheetFilesRequest body);
        /// <summary>
        /// Internal use only - verify credentials for a vendor service. 
        /// </summary>
        /// <param name="vendorId"></param>
        /// <returns>bool?</returns>
        bool? VerifyVendorServiceCredentials (int? vendorId);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class FuelPricingApi : IFuelPricingApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FuelPricingApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public FuelPricingApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="FuelPricingApi"/> class.
        /// </summary>
        /// <returns></returns>
        public FuelPricingApi(String basePath)
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
        /// Internal use only - delete all cached pricing for a currently authenticated company. 
        /// </summary>
        /// <returns>DeleteCurrentPricingResponse</returns>            
        public DeleteCurrentPricingResponse DeleteCurrentPricingForCompany ()
        {
            
    
            var path = "/api/FuelPricing/current";
            path = path.Replace("{format}", "json");
                
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCurrentPricingForCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCurrentPricingForCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteCurrentPricingResponse) ApiClient.Deserialize(response.Content, typeof(DeleteCurrentPricingResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete Supported Price Sheet File Tests by Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteSupportedPriceSheetFileTestsResponse</returns>            
        public DeleteSupportedPriceSheetFileTestsResponse DeleteSupportedPriceSheetFileTests (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteSupportedPriceSheetFileTests");
            
    
            var path = "/api/FuelPricing/supported-price-sheet-file-tests/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSupportedPriceSheetFileTests: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSupportedPriceSheetFileTests: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteSupportedPriceSheetFileTestsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteSupportedPriceSheetFileTestsResponse), response.Headers);
        }
    
        /// <summary>
        /// Deletes Supported Price Sheet Files by Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteSupportedPriceSheetFilesResponse</returns>            
        public DeleteSupportedPriceSheetFilesResponse DeleteSupportedPriceSheetFiles (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteSupportedPriceSheetFiles");
            
    
            var path = "/api/FuelPricing/supported-price-sheet-files/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSupportedPriceSheetFiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSupportedPriceSheetFiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteSupportedPriceSheetFilesResponse) ApiClient.Deserialize(response.Content, typeof(DeleteSupportedPriceSheetFilesResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - delete all weekly pricing records for a particular fuel vendor. 
        /// </summary>
        /// <param name="fuelVendorId"></param> 
        /// <returns>DeleteWeeklyPricingResponse</returns>            
        public DeleteWeeklyPricingResponse DeleteWeeklyPricingForFuelVendor (int? fuelVendorId)
        {
            
            // verify the required parameter 'fuelVendorId' is set
            if (fuelVendorId == null) throw new ApiException(400, "Missing required parameter 'fuelVendorId' when calling DeleteWeeklyPricingForFuelVendor");
            
    
            var path = "/api/FuelPricing/weekly-pricing/by-fueler/{fuelVendorId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "fuelVendorId" + "}", ApiClient.ParameterToString(fuelVendorId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteWeeklyPricingForFuelVendor: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteWeeklyPricingForFuelVendor: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteWeeklyPricingResponse) ApiClient.Deserialize(response.Content, typeof(DeleteWeeklyPricingResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>AssociatedDetailsResponse</returns>            
        public AssociatedDetailsResponse GetAssociatedDetailsForFuelOption (AssociatedDetailsRequest body)
        {
            
    
            var path = "/api/FuelPricing/associated-details";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAssociatedDetailsForFuelOption: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAssociatedDetailsForFuelOption: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AssociatedDetailsResponse) ApiClient.Deserialize(response.Content, typeof(AssociatedDetailsResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch all cached pricing for the specified comma-delimited ICAOs currently available for the user. 
        /// </summary>
        /// <param name="commaDelimitedIcaos"></param> 
        /// <returns>CurrentPricingResponse</returns>            
        public CurrentPricingResponse GetCurrentPricingForLocation (string commaDelimitedIcaos)
        {
            
            // verify the required parameter 'commaDelimitedIcaos' is set
            if (commaDelimitedIcaos == null) throw new ApiException(400, "Missing required parameter 'commaDelimitedIcaos' when calling GetCurrentPricingForLocation");
            
    
            var path = "/api/FuelPricing/current/{commaDelimitedIcaos}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "commaDelimitedIcaos" + "}", ApiClient.ParameterToString(commaDelimitedIcaos));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCurrentPricingForLocation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCurrentPricingForLocation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CurrentPricingResponse) ApiClient.Deserialize(response.Content, typeof(CurrentPricingResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch all cached pricing for the specified comma-delimited ICAOs and flight type currently available for the user. 
        /// </summary>
        /// <param name="commaDelimitedIcaos"></param> 
        /// <param name="flightType"></param> 
        /// <returns>CurrentPricingResponse</returns>            
        public CurrentPricingResponse GetCurrentPricingForLocationAndFlightType (string commaDelimitedIcaos, string flightType)
        {
            
            // verify the required parameter 'commaDelimitedIcaos' is set
            if (commaDelimitedIcaos == null) throw new ApiException(400, "Missing required parameter 'commaDelimitedIcaos' when calling GetCurrentPricingForLocationAndFlightType");
            
            // verify the required parameter 'flightType' is set
            if (flightType == null) throw new ApiException(400, "Missing required parameter 'flightType' when calling GetCurrentPricingForLocationAndFlightType");
            
    
            var path = "/api/FuelPricing/current/{commaDelimitedIcaos}/flight-type/{flightType}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "commaDelimitedIcaos" + "}", ApiClient.ParameterToString(commaDelimitedIcaos));
path = path.Replace("{" + "flightType" + "}", ApiClient.ParameterToString(flightType));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCurrentPricingForLocationAndFlightType: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCurrentPricingForLocationAndFlightType: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CurrentPricingResponse) ApiClient.Deserialize(response.Content, typeof(CurrentPricingResponse), response.Headers);
        }
    
        /// <summary>
        /// Retrieves a live quote from all vendor web services tied to the flight department&#39;s account using their default flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings. It is always recommended to do a live quote for pricing if one hasn&#39;t been done in the last few hours for the desired airports.
        /// </summary>
        /// <param name="commaDelimitedIcaos"></param> 
        /// <returns>CurrentPricingResponse</returns>            
        public CurrentPricingResponse GetLiveQuoteForLocations (string commaDelimitedIcaos)
        {
            
            // verify the required parameter 'commaDelimitedIcaos' is set
            if (commaDelimitedIcaos == null) throw new ApiException(400, "Missing required parameter 'commaDelimitedIcaos' when calling GetLiveQuoteForLocations");
            
    
            var path = "/api/FuelPricing/live-quote/{commaDelimitedIcaos}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "commaDelimitedIcaos" + "}", ApiClient.ParameterToString(commaDelimitedIcaos));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetLiveQuoteForLocations: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetLiveQuoteForLocations: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CurrentPricingResponse) ApiClient.Deserialize(response.Content, typeof(CurrentPricingResponse), response.Headers);
        }
    
        /// <summary>
        /// Retrieves a live quote from all vendor web services tied to the flight department&#39;s account using the specified flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings. It is always recommended to do a live quote for pricing if one hasn&#39;t been done in the last few hours for the desired airports.
        /// </summary>
        /// <param name="commaDelimitedIcaos"></param> 
        /// <param name="flightType"></param> 
        /// <returns>CurrentPricingResponse</returns>            
        public CurrentPricingResponse GetLiveQuoteForLocationsAndFlightType (string commaDelimitedIcaos, string flightType)
        {
            
            // verify the required parameter 'commaDelimitedIcaos' is set
            if (commaDelimitedIcaos == null) throw new ApiException(400, "Missing required parameter 'commaDelimitedIcaos' when calling GetLiveQuoteForLocationsAndFlightType");
            
            // verify the required parameter 'flightType' is set
            if (flightType == null) throw new ApiException(400, "Missing required parameter 'flightType' when calling GetLiveQuoteForLocationsAndFlightType");
            
    
            var path = "/api/FuelPricing/live-quote/{commaDelimitedIcaos}/flight-type/{flightType}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "commaDelimitedIcaos" + "}", ApiClient.ParameterToString(commaDelimitedIcaos));
path = path.Replace("{" + "flightType" + "}", ApiClient.ParameterToString(flightType));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetLiveQuoteForLocationsAndFlightType: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetLiveQuoteForLocationsAndFlightType: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CurrentPricingResponse) ApiClient.Deserialize(response.Content, typeof(CurrentPricingResponse), response.Headers);
        }
    
        /// <summary>
        /// Retrieves a live quote from all vendor web services tied to the flight department&#39;s account using the specified flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.  Only quotes the specified fuel vendor based on the provided {fuelVendorId}. It is always recommended to do a live quote for pricing if one hasn&#39;t been done in the last few hours for the desired airports.
        /// </summary>
        /// <param name="commaDelimitedIcaos"></param> 
        /// <param name="flightType"></param> 
        /// <param name="fuelVendorId"></param> 
        /// <returns>CurrentPricingResponse</returns>            
        public CurrentPricingResponse GetLiveQuoteForLocationsAndFlightTypeAndVendor (string commaDelimitedIcaos, string flightType, int? fuelVendorId)
        {
            
            // verify the required parameter 'commaDelimitedIcaos' is set
            if (commaDelimitedIcaos == null) throw new ApiException(400, "Missing required parameter 'commaDelimitedIcaos' when calling GetLiveQuoteForLocationsAndFlightTypeAndVendor");
            
            // verify the required parameter 'flightType' is set
            if (flightType == null) throw new ApiException(400, "Missing required parameter 'flightType' when calling GetLiveQuoteForLocationsAndFlightTypeAndVendor");
            
            // verify the required parameter 'fuelVendorId' is set
            if (fuelVendorId == null) throw new ApiException(400, "Missing required parameter 'fuelVendorId' when calling GetLiveQuoteForLocationsAndFlightTypeAndVendor");
            
    
            var path = "/api/FuelPricing/live-quote/{commaDelimitedIcaos}/flight-type/{flightType}/fuel-vendor/{fuelVendorId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "commaDelimitedIcaos" + "}", ApiClient.ParameterToString(commaDelimitedIcaos));
path = path.Replace("{" + "flightType" + "}", ApiClient.ParameterToString(flightType));
path = path.Replace("{" + "fuelVendorId" + "}", ApiClient.ParameterToString(fuelVendorId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetLiveQuoteForLocationsAndFlightTypeAndVendor: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetLiveQuoteForLocationsAndFlightTypeAndVendor: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CurrentPricingResponse) ApiClient.Deserialize(response.Content, typeof(CurrentPricingResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch a quote response from the EPIC Aviation web service. 
        /// </summary>
        /// <param name="commaDelimitedAirportIdentifiers"></param> 
        /// <returns>EpicQuoteResponse</returns>            
        public EpicQuoteResponse GetQuotFromEpic (string commaDelimitedAirportIdentifiers)
        {
            
            // verify the required parameter 'commaDelimitedAirportIdentifiers' is set
            if (commaDelimitedAirportIdentifiers == null) throw new ApiException(400, "Missing required parameter 'commaDelimitedAirportIdentifiers' when calling GetQuotFromEpic");
            
    
            var path = "/api/FuelPricing/quoting/epic/{commaDelimitedAirportIdentifiers}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "commaDelimitedAirportIdentifiers" + "}", ApiClient.ParameterToString(commaDelimitedAirportIdentifiers));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetQuotFromEpic: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetQuotFromEpic: " + response.ErrorMessage, response.ErrorMessage);
    
            return (EpicQuoteResponse) ApiClient.Deserialize(response.Content, typeof(EpicQuoteResponse), response.Headers);
        }
    
        /// <summary>
        /// Gets Supported Price Sheet File Tests by supportedPriceSheetFileId 
        /// </summary>
        /// <param name="supportedPriceSheetFileId"></param> 
        /// <returns>SupportedPriceSheetFileTestsResponse</returns>            
        public SupportedPriceSheetFileTestsResponse GetSupportedPriceSheetFileTestsBySupportedPriceSheetFileId (int? supportedPriceSheetFileId)
        {
            
            // verify the required parameter 'supportedPriceSheetFileId' is set
            if (supportedPriceSheetFileId == null) throw new ApiException(400, "Missing required parameter 'supportedPriceSheetFileId' when calling GetSupportedPriceSheetFileTestsBySupportedPriceSheetFileId");
            
    
            var path = "/api/FuelPricing/supported-price-sheet-file-tests/by-supportedInvoiceImportFileId/{supportedPriceSheetFileId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "supportedPriceSheetFileId" + "}", ApiClient.ParameterToString(supportedPriceSheetFileId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedPriceSheetFileTestsBySupportedPriceSheetFileId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedPriceSheetFileTestsBySupportedPriceSheetFileId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SupportedPriceSheetFileTestsResponse) ApiClient.Deserialize(response.Content, typeof(SupportedPriceSheetFileTestsResponse), response.Headers);
        }
    
        /// <summary>
        /// Gets Supported Price Sheet Files by FuelVendorId 
        /// </summary>
        /// <param name="fuelVendorId"></param> 
        /// <returns>SupportedPriceSheetFileListResponse</returns>            
        public SupportedPriceSheetFileListResponse GetSupportedPriceSheetFilesByByFuelVendorId (int? fuelVendorId)
        {
            
            // verify the required parameter 'fuelVendorId' is set
            if (fuelVendorId == null) throw new ApiException(400, "Missing required parameter 'fuelVendorId' when calling GetSupportedPriceSheetFilesByByFuelVendorId");
            
    
            var path = "/api/FuelPricing/supported-price-sheet-files/by-fuel-vendor/{fuelVendorId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "fuelVendorId" + "}", ApiClient.ParameterToString(fuelVendorId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedPriceSheetFilesByByFuelVendorId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedPriceSheetFilesByByFuelVendorId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SupportedPriceSheetFileListResponse) ApiClient.Deserialize(response.Content, typeof(SupportedPriceSheetFileListResponse), response.Headers);
        }
    
        /// <summary>
        /// Get Supported Price Sheet Files by Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>SupportedPriceSheetFilesResponse</returns>            
        public SupportedPriceSheetFilesResponse GetSupportedPriceSheetFilesByById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetSupportedPriceSheetFilesByById");
            
    
            var path = "/api/FuelPricing/supported-price-sheet-files/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedPriceSheetFilesByById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedPriceSheetFilesByById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SupportedPriceSheetFilesResponse) ApiClient.Deserialize(response.Content, typeof(SupportedPriceSheetFilesResponse), response.Headers);
        }
    
        /// <summary>
        /// If available, will fetch records from the weekly price sheet received each week for the specified [commaDelimitedIcaos] and [fuelVendorId].  These are to be used as a fallback option when a vendor&#39;s service is unavailable. 
        /// </summary>
        /// <param name="fuelVendorId"></param> 
        /// <param name="commaDelimitedIcaos"></param> 
        /// <returns>WeeklyPricingListResponse</returns>            
        public WeeklyPricingListResponse GetWeeklyPricingForFuelerAndLocation (int? fuelVendorId, string commaDelimitedIcaos)
        {
            
            // verify the required parameter 'fuelVendorId' is set
            if (fuelVendorId == null) throw new ApiException(400, "Missing required parameter 'fuelVendorId' when calling GetWeeklyPricingForFuelerAndLocation");
            
            // verify the required parameter 'commaDelimitedIcaos' is set
            if (commaDelimitedIcaos == null) throw new ApiException(400, "Missing required parameter 'commaDelimitedIcaos' when calling GetWeeklyPricingForFuelerAndLocation");
            
    
            var path = "/api/FuelPricing/weekly-pricing/by-fueler/{fuelVendorId}/by-locations/{commaDelimitedIcaos}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "fuelVendorId" + "}", ApiClient.ParameterToString(fuelVendorId));
path = path.Replace("{" + "commaDelimitedIcaos" + "}", ApiClient.ParameterToString(commaDelimitedIcaos));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetWeeklyPricingForFuelerAndLocation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetWeeklyPricingForFuelerAndLocation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (WeeklyPricingListResponse) ApiClient.Deserialize(response.Content, typeof(WeeklyPricingListResponse), response.Headers);
        }
    
        /// <summary>
        /// /// If available, will fetch records from the weekly price sheet received each week for the specified [commaDelimitedIcaos].  These are to be used as a fallback option when a vendor&#39;s service is unavailable. 
        /// </summary>
        /// <param name="commaDelimitedIcaos"></param> 
        /// <returns>WeeklyPricingListResponse</returns>            
        public WeeklyPricingListResponse GetWeeklyPricingForLocation (string commaDelimitedIcaos)
        {
            
            // verify the required parameter 'commaDelimitedIcaos' is set
            if (commaDelimitedIcaos == null) throw new ApiException(400, "Missing required parameter 'commaDelimitedIcaos' when calling GetWeeklyPricingForLocation");
            
    
            var path = "/api/FuelPricing/weekly-pricing/by-locations/{commaDelimitedIcaos}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "commaDelimitedIcaos" + "}", ApiClient.ParameterToString(commaDelimitedIcaos));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetWeeklyPricingForLocation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetWeeklyPricingForLocation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (WeeklyPricingListResponse) ApiClient.Deserialize(response.Content, typeof(WeeklyPricingListResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Please use the \&quot;dispatching\&quot; API to dispatch a full fuel order.  This API method is strictly for notifying the fuel vendor. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostFuelOrderResponse</returns>            
        public PostFuelOrderResponse PostFuelOrder (PostFuelOrderRequest body)
        {
            
    
            var path = "/api/FuelPricing/fuel-order";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostFuelOrder: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostFuelOrder: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostFuelOrderResponse) ApiClient.Deserialize(response.Content, typeof(PostFuelOrderResponse), response.Headers);
        }
    
        /// <summary>
        /// Post Supported Price Sheet File Tests 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostSupportedPriceSheetFileTestsResponse</returns>            
        public PostSupportedPriceSheetFileTestsResponse PostSupportedPriceSheetFileTests (PostSupportedPriceSheetFileTestsRequest body)
        {
            
    
            var path = "/api/FuelPricing/supported-price-sheet-file-tests";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostSupportedPriceSheetFileTests: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostSupportedPriceSheetFileTests: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostSupportedPriceSheetFileTestsResponse) ApiClient.Deserialize(response.Content, typeof(PostSupportedPriceSheetFileTestsResponse), response.Headers);
        }
    
        /// <summary>
        /// Post Supported Price Sheet Files 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostSupportedPriceSheetFilesResponse</returns>            
        public PostSupportedPriceSheetFilesResponse PostSupportedPriceSheetFiles (PostSupportedPriceSheetFilesRequest body)
        {
            
    
            var path = "/api/FuelPricing/supported-price-sheet-files";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostSupportedPriceSheetFiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostSupportedPriceSheetFiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostSupportedPriceSheetFilesResponse) ApiClient.Deserialize(response.Content, typeof(PostSupportedPriceSheetFilesResponse), response.Headers);
        }
    
        /// <summary>
        /// Updates Supported Price Sheet File Tests 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateSupportedPriceSheetFileTestsResponse</returns>            
        public UpdateSupportedPriceSheetFileTestsResponse UpdateSupportedPriceSheetFileTests (UpdateSupportedPriceSheetFileTestsRequest body)
        {
            
    
            var path = "/api/FuelPricing/supported-price-sheet-file-tests";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSupportedPriceSheetFileTests: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSupportedPriceSheetFileTests: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateSupportedPriceSheetFileTestsResponse) ApiClient.Deserialize(response.Content, typeof(UpdateSupportedPriceSheetFileTestsResponse), response.Headers);
        }
    
        /// <summary>
        /// Updates Supported Price Sheet Files 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateSupportedPriceSheetFilesResponse</returns>            
        public UpdateSupportedPriceSheetFilesResponse UpdateSupportedPriceSheetFiles (UpdateSupportedPriceSheetFilesRequest body)
        {
            
    
            var path = "/api/FuelPricing/supported-price-sheet-files";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSupportedPriceSheetFiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSupportedPriceSheetFiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateSupportedPriceSheetFilesResponse) ApiClient.Deserialize(response.Content, typeof(UpdateSupportedPriceSheetFilesResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - verify credentials for a vendor service. 
        /// </summary>
        /// <param name="vendorId"></param> 
        /// <returns>bool?</returns>            
        public bool? VerifyVendorServiceCredentials (int? vendorId)
        {
            
            // verify the required parameter 'vendorId' is set
            if (vendorId == null) throw new ApiException(400, "Missing required parameter 'vendorId' when calling VerifyVendorServiceCredentials");
            
    
            var path = "/api/FuelPricing/verify-vendor-service-credentials/{vendorId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "vendorId" + "}", ApiClient.ParameterToString(vendorId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling VerifyVendorServiceCredentials: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling VerifyVendorServiceCredentials: " + response.ErrorMessage, response.ErrorMessage);
    
            return (bool?) ApiClient.Deserialize(response.Content, typeof(bool?), response.Headers);
        }
    
    }
}
