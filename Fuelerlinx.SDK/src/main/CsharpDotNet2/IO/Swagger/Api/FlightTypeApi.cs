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
    public interface IFlightTypeApi
    {
        /// <summary>
        /// Internal use only - Delete a flight type mapping record by it&#39;s [id]. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteFlightTypeMappingResponse</returns>
        DeleteFlightTypeMappingResponse DeleteFlightTypeMapping (int? id);
        /// <summary>
        /// Fetch a flight type mapping by it&#39;s name.  This is used to identify if a flight type is private or commercial based on various custom names provided by scheduling systems. 
        /// </summary>
        /// <param name="flightTypeName"></param>
        /// <returns>FlightTypeMappingResponse</returns>
        FlightTypeMappingResponse GetFlightTypeMappingByName (string flightTypeName);
        /// <summary>
        /// Internal use only - Fetch all flight type mappings. 
        /// </summary>
        /// <returns>FlightTypeMappingListResponse</returns>
        FlightTypeMappingListResponse GetFlightTypeMappingList ();
        /// <summary>
        /// Internal use only - Add a flight type mapping record. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostFlightTypeMappingResponse</returns>
        PostFlightTypeMappingResponse PostFlightTypeMapping (PostFlightTypeMappingRequest body);
        /// <summary>
        /// Internal use only - Update a flight type mapping record. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateFlightTypeMappingResponse</returns>
        UpdateFlightTypeMappingResponse UpdateFlightTypeMapping (UpdateFlightTypeMappingRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class FlightTypeApi : IFlightTypeApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlightTypeApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public FlightTypeApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="FlightTypeApi"/> class.
        /// </summary>
        /// <returns></returns>
        public FlightTypeApi(String basePath)
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
        /// Internal use only - Delete a flight type mapping record by it&#39;s [id]. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteFlightTypeMappingResponse</returns>            
        public DeleteFlightTypeMappingResponse DeleteFlightTypeMapping (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteFlightTypeMapping");
            
    
            var path = "/api/FlightType/mapping/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteFlightTypeMapping: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteFlightTypeMapping: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteFlightTypeMappingResponse) ApiClient.Deserialize(response.Content, typeof(DeleteFlightTypeMappingResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a flight type mapping by it&#39;s name.  This is used to identify if a flight type is private or commercial based on various custom names provided by scheduling systems. 
        /// </summary>
        /// <param name="flightTypeName"></param> 
        /// <returns>FlightTypeMappingResponse</returns>            
        public FlightTypeMappingResponse GetFlightTypeMappingByName (string flightTypeName)
        {
            
            // verify the required parameter 'flightTypeName' is set
            if (flightTypeName == null) throw new ApiException(400, "Missing required parameter 'flightTypeName' when calling GetFlightTypeMappingByName");
            
    
            var path = "/api/FlightType/mapping/by-flight-name/{flightTypeName}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "flightTypeName" + "}", ApiClient.ParameterToString(flightTypeName));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetFlightTypeMappingByName: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetFlightTypeMappingByName: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FlightTypeMappingResponse) ApiClient.Deserialize(response.Content, typeof(FlightTypeMappingResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch all flight type mappings. 
        /// </summary>
        /// <returns>FlightTypeMappingListResponse</returns>            
        public FlightTypeMappingListResponse GetFlightTypeMappingList ()
        {
            
    
            var path = "/api/FlightType/list/mapping";
            path = path.Replace("{format}", "json");
                
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetFlightTypeMappingList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetFlightTypeMappingList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FlightTypeMappingListResponse) ApiClient.Deserialize(response.Content, typeof(FlightTypeMappingListResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Add a flight type mapping record. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostFlightTypeMappingResponse</returns>            
        public PostFlightTypeMappingResponse PostFlightTypeMapping (PostFlightTypeMappingRequest body)
        {
            
    
            var path = "/api/FlightType/mapping";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostFlightTypeMapping: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostFlightTypeMapping: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostFlightTypeMappingResponse) ApiClient.Deserialize(response.Content, typeof(PostFlightTypeMappingResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update a flight type mapping record. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateFlightTypeMappingResponse</returns>            
        public UpdateFlightTypeMappingResponse UpdateFlightTypeMapping (UpdateFlightTypeMappingRequest body)
        {
            
    
            var path = "/api/FlightType/mapping";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFlightTypeMapping: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFlightTypeMapping: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateFlightTypeMappingResponse) ApiClient.Deserialize(response.Content, typeof(UpdateFlightTypeMappingResponse), response.Headers);
        }
    
    }
}
