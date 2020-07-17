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
    public interface IFuelVendorApi
    {
        /// <summary>
        /// Delete the company-specific details of a fuel vendor based on the provided {companyFuelerId}. 
        /// </summary>
        /// <param name="companyFuelerId"></param>
        /// <returns>DeleteCompanyFuelerResponse</returns>
        DeleteCompanyFuelerResponse DeleteCompanyFueler (int? companyFuelerId);
        /// <summary>
        /// Delete a company-specific note for the provided {companyFuelerId} record. 
        /// </summary>
        /// <param name="companyFuelerId"></param>
        /// <param name="noteId"></param>
        /// <returns>DeleteCompanyFuelerNotesResponse</returns>
        DeleteCompanyFuelerNotesResponse DeleteCompanyFuelerNotes (int? companyFuelerId, int? noteId);
        /// <summary>
        /// Delete a price adjustment for a company fueler. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteCompanyFuelerPriceAdjustmentResponse</returns>
        DeleteCompanyFuelerPriceAdjustmentResponse DeleteCompanyFuelerPriceAdjustment (int? id);
        /// <summary>
        /// Delete a captured price sheet for a company&#39;s fuel vendor. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteCompanyFuelerPriceSheetFileCaptureResponse</returns>
        DeleteCompanyFuelerPriceSheetFileCaptureResponse DeleteCompanyFuelerPriceSheetFileCapture (int? id);
        /// <summary>
        /// Delete a company-specific settings record for a fuel vendor. 
        /// </summary>
        /// <param name="companyFuelerId"></param>
        /// <param name="settingsId"></param>
        /// <returns>DeleteCompanyFuelerSettingsResponse</returns>
        DeleteCompanyFuelerSettingsResponse DeleteCompanyFuelerSettings (int? companyFuelerId, int? settingsId);
        /// <summary>
        /// Delete fuel vendor payment information by fuel vendor id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>FuelVendorPaymentInformationDTO</returns>
        FuelVendorPaymentInformationDTO DeletePaymentInformationByFuelVendorId (int? id);
        /// <summary>
        /// Fetch a company-specific record tied to the fuel vendor for the provided {fuelVendorId}. 
        /// </summary>
        /// <param name="fuelVendorId"></param>
        /// <returns>CompanyFuelerResponse</returns>
        CompanyFuelerResponse GetCompanyFuelerByFuelerId (int? fuelVendorId);
        /// <summary>
        /// Fetch a company-specific fuel vendor record for the provided {companyFuelerId}. 
        /// </summary>
        /// <param name="companyFuelerId"></param>
        /// <returns>CompanyFuelerResponse</returns>
        CompanyFuelerResponse GetCompanyFuelerById (int? companyFuelerId);
        /// <summary>
        /// Fetch all company-specific records for the authenticated company. 
        /// </summary>
        /// <returns>CompanyFuelerListResponse</returns>
        CompanyFuelerListResponse GetCompanyFuelerList ();
        /// <summary>
        /// Fetch the company-specific notes for a particular fuel vendor based on the provided {companyFuelerId}. 
        /// </summary>
        /// <param name="companyFuelerId"></param>
        /// <returns>CompanyFuelerNotesResponse</returns>
        CompanyFuelerNotesResponse GetCompanyFuelerNotes (int? companyFuelerId);
        /// <summary>
        /// Get all price adjustments for a company fueler. 
        /// </summary>
        /// <param name="companyFuelerId"></param>
        /// <returns>CompanyFuelerPriceAdjustmentListResponse</returns>
        CompanyFuelerPriceAdjustmentListResponse GetCompanyFuelerPriceAdjustmentList (int? companyFuelerId);
        /// <summary>
        /// Get recently captured price sheet by it&#39;s {id} 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>PostCompanyFuelerPriceSheetFileCaptureResponse</returns>
        PostCompanyFuelerPriceSheetFileCaptureResponse GetCompanyFuelerPriceSheetFileCapture (int? id);
        /// <summary>
        /// Fetch the company-specific settings for the specified {companyFuelerId} record. 
        /// </summary>
        /// <param name="companyFuelerId"></param>
        /// <returns>CompanyFuelerSettingsResponse</returns>
        CompanyFuelerSettingsResponse GetCompanyFuelerSettings (int? companyFuelerId);
        /// <summary>
        /// Get fuel vendor payment information by fuel vendor id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>FuelVendorPaymentInformationDTO</returns>
        FuelVendorPaymentInformationDTO GetPaymentInformationByFuelVendorId (int? id);
        /// <summary>
        /// Add a company-specific record for a fuel vendor.  These details are unique for each flight department. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostCompanyFuelerResponse</returns>
        PostCompanyFuelerResponse PostCompanyFueler (PostCompanyFuelerRequest body);
        /// <summary>
        /// Add a new company-specific note for a fuel vendor. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostCompanyFuelerNotesResponse</returns>
        PostCompanyFuelerNotesResponse PostCompanyFuelerNotes (PostCompanyFuelerNotesRequest body);
        /// <summary>
        /// Add a new price adjustment for a company fueler.  This price adjustment will be applied to the user&#39;s own adjusted price section when reviewing prices. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostCompanyFuelerPriceAdjustmentResponse</returns>
        PostCompanyFuelerPriceAdjustmentResponse PostCompanyFuelerPriceAdjustment (PostCompanyFuelerPriceAdjustmentRequest body);
        /// <summary>
        /// Add a recently captured price sheet for a company&#39;s fuel vendor. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostCompanyFuelerPriceSheetFileCaptureResponse</returns>
        PostCompanyFuelerPriceSheetFileCaptureResponse PostCompanyFuelerPriceSheetFileCapture (PostCompanyFuelerPriceSheetFileCaptureRequest body);
        /// <summary>
        /// Add a company-specific settings record for a fuel vendor. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostCompanyFuelerSettingsResponse</returns>
        PostCompanyFuelerSettingsResponse PostCompanyFuelerSettings (PostCompanyFuelerSettingsRequest body);
        /// <summary>
        /// Add a fuel vendor payment information 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostPaymentInformationResponse</returns>
        PostPaymentInformationResponse PostPaymentInformation (PostPaymentInformationRequest body);
        /// <summary>
        /// Update a fuel vendor payment information 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>FuelVendorPaymentInformationDTO</returns>
        FuelVendorPaymentInformationDTO PutPaymentInformation (FuelVendorPaymentInformationDTO body);
        /// <summary>
        /// Update the company-specific details of a fuel vendor.  These details are unique for each flight department. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateCompanyFuelerResponse</returns>
        UpdateCompanyFuelerResponse UpdateCompanyFueler (UpdateCompanyFuelerRequest body);
        /// <summary>
        /// Update an existing company-specific note for a fuel vendor. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateCompanyFuelerNotesResponse</returns>
        UpdateCompanyFuelerNotesResponse UpdateCompanyFuelerNotes (UpdateCompanyFuelerNotesRequest body);
        /// <summary>
        /// Update a captured price sheet for a company&#39;s fuel vendor. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateCompanyFuelerPriceSheetFileCaptureResponse</returns>
        UpdateCompanyFuelerPriceSheetFileCaptureResponse UpdateCompanyFuelerPriceSheetFileCapture (UpdateCompanyFuelerPriceSheetFileCaptureRequest body);
        /// <summary>
        /// Update a company-specific settings record for a fuel vendor. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateCompanyFuelerSettingsResponse</returns>
        UpdateCompanyFuelerSettingsResponse UpdateCompanyFuelerSettings (UpdateCompanyFuelerSettingsRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class FuelVendorApi : IFuelVendorApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FuelVendorApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public FuelVendorApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="FuelVendorApi"/> class.
        /// </summary>
        /// <returns></returns>
        public FuelVendorApi(String basePath)
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
        /// Delete the company-specific details of a fuel vendor based on the provided {companyFuelerId}. 
        /// </summary>
        /// <param name="companyFuelerId"></param> 
        /// <returns>DeleteCompanyFuelerResponse</returns>            
        public DeleteCompanyFuelerResponse DeleteCompanyFueler (int? companyFuelerId)
        {
            
            // verify the required parameter 'companyFuelerId' is set
            if (companyFuelerId == null) throw new ApiException(400, "Missing required parameter 'companyFuelerId' when calling DeleteCompanyFueler");
            
    
            var path = "/api/FuelVendor/company-specific/{companyFuelerId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyFuelerId" + "}", ApiClient.ParameterToString(companyFuelerId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyFueler: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyFueler: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteCompanyFuelerResponse) ApiClient.Deserialize(response.Content, typeof(DeleteCompanyFuelerResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a company-specific note for the provided {companyFuelerId} record. 
        /// </summary>
        /// <param name="companyFuelerId"></param> 
        /// <param name="noteId"></param> 
        /// <returns>DeleteCompanyFuelerNotesResponse</returns>            
        public DeleteCompanyFuelerNotesResponse DeleteCompanyFuelerNotes (int? companyFuelerId, int? noteId)
        {
            
            // verify the required parameter 'companyFuelerId' is set
            if (companyFuelerId == null) throw new ApiException(400, "Missing required parameter 'companyFuelerId' when calling DeleteCompanyFuelerNotes");
            
            // verify the required parameter 'noteId' is set
            if (noteId == null) throw new ApiException(400, "Missing required parameter 'noteId' when calling DeleteCompanyFuelerNotes");
            
    
            var path = "/api/FuelVendor/company-specific/{companyFuelerId}/notes/{noteId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyFuelerId" + "}", ApiClient.ParameterToString(companyFuelerId));
path = path.Replace("{" + "noteId" + "}", ApiClient.ParameterToString(noteId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyFuelerNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyFuelerNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteCompanyFuelerNotesResponse) ApiClient.Deserialize(response.Content, typeof(DeleteCompanyFuelerNotesResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a price adjustment for a company fueler. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteCompanyFuelerPriceAdjustmentResponse</returns>            
        public DeleteCompanyFuelerPriceAdjustmentResponse DeleteCompanyFuelerPriceAdjustment (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteCompanyFuelerPriceAdjustment");
            
    
            var path = "/api/FuelVendor/company-specific/price-adjustment/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyFuelerPriceAdjustment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyFuelerPriceAdjustment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteCompanyFuelerPriceAdjustmentResponse) ApiClient.Deserialize(response.Content, typeof(DeleteCompanyFuelerPriceAdjustmentResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a captured price sheet for a company&#39;s fuel vendor. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteCompanyFuelerPriceSheetFileCaptureResponse</returns>            
        public DeleteCompanyFuelerPriceSheetFileCaptureResponse DeleteCompanyFuelerPriceSheetFileCapture (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteCompanyFuelerPriceSheetFileCapture");
            
    
            var path = "/api/FuelVendor/company-specific/price-sheet-capture/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyFuelerPriceSheetFileCapture: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyFuelerPriceSheetFileCapture: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteCompanyFuelerPriceSheetFileCaptureResponse) ApiClient.Deserialize(response.Content, typeof(DeleteCompanyFuelerPriceSheetFileCaptureResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a company-specific settings record for a fuel vendor. 
        /// </summary>
        /// <param name="companyFuelerId"></param> 
        /// <param name="settingsId"></param> 
        /// <returns>DeleteCompanyFuelerSettingsResponse</returns>            
        public DeleteCompanyFuelerSettingsResponse DeleteCompanyFuelerSettings (int? companyFuelerId, int? settingsId)
        {
            
            // verify the required parameter 'companyFuelerId' is set
            if (companyFuelerId == null) throw new ApiException(400, "Missing required parameter 'companyFuelerId' when calling DeleteCompanyFuelerSettings");
            
            // verify the required parameter 'settingsId' is set
            if (settingsId == null) throw new ApiException(400, "Missing required parameter 'settingsId' when calling DeleteCompanyFuelerSettings");
            
    
            var path = "/api/FuelVendor/company-specific/{companyFuelerId}/settings/{settingsId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyFuelerId" + "}", ApiClient.ParameterToString(companyFuelerId));
path = path.Replace("{" + "settingsId" + "}", ApiClient.ParameterToString(settingsId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyFuelerSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyFuelerSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteCompanyFuelerSettingsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteCompanyFuelerSettingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete fuel vendor payment information by fuel vendor id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>FuelVendorPaymentInformationDTO</returns>            
        public FuelVendorPaymentInformationDTO DeletePaymentInformationByFuelVendorId (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeletePaymentInformationByFuelVendorId");
            
    
            var path = "/api/FuelVendor/{id}/delete-payment-info";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeletePaymentInformationByFuelVendorId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeletePaymentInformationByFuelVendorId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FuelVendorPaymentInformationDTO) ApiClient.Deserialize(response.Content, typeof(FuelVendorPaymentInformationDTO), response.Headers);
        }
    
        /// <summary>
        /// Fetch a company-specific record tied to the fuel vendor for the provided {fuelVendorId}. 
        /// </summary>
        /// <param name="fuelVendorId"></param> 
        /// <returns>CompanyFuelerResponse</returns>            
        public CompanyFuelerResponse GetCompanyFuelerByFuelerId (int? fuelVendorId)
        {
            
            // verify the required parameter 'fuelVendorId' is set
            if (fuelVendorId == null) throw new ApiException(400, "Missing required parameter 'fuelVendorId' when calling GetCompanyFuelerByFuelerId");
            
    
            var path = "/api/FuelVendor/company-specific/by-fueler/{fuelVendorId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerByFuelerId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerByFuelerId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyFuelerResponse) ApiClient.Deserialize(response.Content, typeof(CompanyFuelerResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a company-specific fuel vendor record for the provided {companyFuelerId}. 
        /// </summary>
        /// <param name="companyFuelerId"></param> 
        /// <returns>CompanyFuelerResponse</returns>            
        public CompanyFuelerResponse GetCompanyFuelerById (int? companyFuelerId)
        {
            
            // verify the required parameter 'companyFuelerId' is set
            if (companyFuelerId == null) throw new ApiException(400, "Missing required parameter 'companyFuelerId' when calling GetCompanyFuelerById");
            
    
            var path = "/api/FuelVendor/company-specific/{companyFuelerId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyFuelerId" + "}", ApiClient.ParameterToString(companyFuelerId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyFuelerResponse) ApiClient.Deserialize(response.Content, typeof(CompanyFuelerResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch all company-specific records for the authenticated company. 
        /// </summary>
        /// <returns>CompanyFuelerListResponse</returns>            
        public CompanyFuelerListResponse GetCompanyFuelerList ()
        {
            
    
            var path = "/api/FuelVendor/company-specific/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyFuelerListResponse) ApiClient.Deserialize(response.Content, typeof(CompanyFuelerListResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch the company-specific notes for a particular fuel vendor based on the provided {companyFuelerId}. 
        /// </summary>
        /// <param name="companyFuelerId"></param> 
        /// <returns>CompanyFuelerNotesResponse</returns>            
        public CompanyFuelerNotesResponse GetCompanyFuelerNotes (int? companyFuelerId)
        {
            
            // verify the required parameter 'companyFuelerId' is set
            if (companyFuelerId == null) throw new ApiException(400, "Missing required parameter 'companyFuelerId' when calling GetCompanyFuelerNotes");
            
    
            var path = "/api/FuelVendor/company-specific/{companyFuelerId}/notes";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyFuelerId" + "}", ApiClient.ParameterToString(companyFuelerId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyFuelerNotesResponse) ApiClient.Deserialize(response.Content, typeof(CompanyFuelerNotesResponse), response.Headers);
        }
    
        /// <summary>
        /// Get all price adjustments for a company fueler. 
        /// </summary>
        /// <param name="companyFuelerId"></param> 
        /// <returns>CompanyFuelerPriceAdjustmentListResponse</returns>            
        public CompanyFuelerPriceAdjustmentListResponse GetCompanyFuelerPriceAdjustmentList (int? companyFuelerId)
        {
            
            // verify the required parameter 'companyFuelerId' is set
            if (companyFuelerId == null) throw new ApiException(400, "Missing required parameter 'companyFuelerId' when calling GetCompanyFuelerPriceAdjustmentList");
            
    
            var path = "/api/FuelVendor/company-specific/{companyFuelerId}/price-adjustment/list";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyFuelerId" + "}", ApiClient.ParameterToString(companyFuelerId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerPriceAdjustmentList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerPriceAdjustmentList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyFuelerPriceAdjustmentListResponse) ApiClient.Deserialize(response.Content, typeof(CompanyFuelerPriceAdjustmentListResponse), response.Headers);
        }
    
        /// <summary>
        /// Get recently captured price sheet by it&#39;s {id} 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>PostCompanyFuelerPriceSheetFileCaptureResponse</returns>            
        public PostCompanyFuelerPriceSheetFileCaptureResponse GetCompanyFuelerPriceSheetFileCapture (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetCompanyFuelerPriceSheetFileCapture");
            
    
            var path = "/api/FuelVendor/company-specific/price-sheet-capture/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerPriceSheetFileCapture: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerPriceSheetFileCapture: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostCompanyFuelerPriceSheetFileCaptureResponse) ApiClient.Deserialize(response.Content, typeof(PostCompanyFuelerPriceSheetFileCaptureResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch the company-specific settings for the specified {companyFuelerId} record. 
        /// </summary>
        /// <param name="companyFuelerId"></param> 
        /// <returns>CompanyFuelerSettingsResponse</returns>            
        public CompanyFuelerSettingsResponse GetCompanyFuelerSettings (int? companyFuelerId)
        {
            
            // verify the required parameter 'companyFuelerId' is set
            if (companyFuelerId == null) throw new ApiException(400, "Missing required parameter 'companyFuelerId' when calling GetCompanyFuelerSettings");
            
    
            var path = "/api/FuelVendor/company-specific/{companyFuelerId}/settings";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyFuelerId" + "}", ApiClient.ParameterToString(companyFuelerId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyFuelerSettingsResponse) ApiClient.Deserialize(response.Content, typeof(CompanyFuelerSettingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Get fuel vendor payment information by fuel vendor id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>FuelVendorPaymentInformationDTO</returns>            
        public FuelVendorPaymentInformationDTO GetPaymentInformationByFuelVendorId (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetPaymentInformationByFuelVendorId");
            
    
            var path = "/api/FuelVendor/{id}/get-payment-info";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetPaymentInformationByFuelVendorId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetPaymentInformationByFuelVendorId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FuelVendorPaymentInformationDTO) ApiClient.Deserialize(response.Content, typeof(FuelVendorPaymentInformationDTO), response.Headers);
        }
    
        /// <summary>
        /// Add a company-specific record for a fuel vendor.  These details are unique for each flight department. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostCompanyFuelerResponse</returns>            
        public PostCompanyFuelerResponse PostCompanyFueler (PostCompanyFuelerRequest body)
        {
            
    
            var path = "/api/FuelVendor/company-specific";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyFueler: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyFueler: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostCompanyFuelerResponse) ApiClient.Deserialize(response.Content, typeof(PostCompanyFuelerResponse), response.Headers);
        }
    
        /// <summary>
        /// Add a new company-specific note for a fuel vendor. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostCompanyFuelerNotesResponse</returns>            
        public PostCompanyFuelerNotesResponse PostCompanyFuelerNotes (PostCompanyFuelerNotesRequest body)
        {
            
    
            var path = "/api/FuelVendor/company-specific/notes";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyFuelerNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyFuelerNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostCompanyFuelerNotesResponse) ApiClient.Deserialize(response.Content, typeof(PostCompanyFuelerNotesResponse), response.Headers);
        }
    
        /// <summary>
        /// Add a new price adjustment for a company fueler.  This price adjustment will be applied to the user&#39;s own adjusted price section when reviewing prices. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostCompanyFuelerPriceAdjustmentResponse</returns>            
        public PostCompanyFuelerPriceAdjustmentResponse PostCompanyFuelerPriceAdjustment (PostCompanyFuelerPriceAdjustmentRequest body)
        {
            
    
            var path = "/api/FuelVendor/company-specific/price-adjustment";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyFuelerPriceAdjustment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyFuelerPriceAdjustment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostCompanyFuelerPriceAdjustmentResponse) ApiClient.Deserialize(response.Content, typeof(PostCompanyFuelerPriceAdjustmentResponse), response.Headers);
        }
    
        /// <summary>
        /// Add a recently captured price sheet for a company&#39;s fuel vendor. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostCompanyFuelerPriceSheetFileCaptureResponse</returns>            
        public PostCompanyFuelerPriceSheetFileCaptureResponse PostCompanyFuelerPriceSheetFileCapture (PostCompanyFuelerPriceSheetFileCaptureRequest body)
        {
            
    
            var path = "/api/FuelVendor/company-specific/price-sheet-capture";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyFuelerPriceSheetFileCapture: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyFuelerPriceSheetFileCapture: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostCompanyFuelerPriceSheetFileCaptureResponse) ApiClient.Deserialize(response.Content, typeof(PostCompanyFuelerPriceSheetFileCaptureResponse), response.Headers);
        }
    
        /// <summary>
        /// Add a company-specific settings record for a fuel vendor. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostCompanyFuelerSettingsResponse</returns>            
        public PostCompanyFuelerSettingsResponse PostCompanyFuelerSettings (PostCompanyFuelerSettingsRequest body)
        {
            
    
            var path = "/api/FuelVendor/company-specific/settings";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyFuelerSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyFuelerSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostCompanyFuelerSettingsResponse) ApiClient.Deserialize(response.Content, typeof(PostCompanyFuelerSettingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Add a fuel vendor payment information 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostPaymentInformationResponse</returns>            
        public PostPaymentInformationResponse PostPaymentInformation (PostPaymentInformationRequest body)
        {
            
    
            var path = "/api/FuelVendor/add-payment-info";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostPaymentInformation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostPaymentInformation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostPaymentInformationResponse) ApiClient.Deserialize(response.Content, typeof(PostPaymentInformationResponse), response.Headers);
        }
    
        /// <summary>
        /// Update a fuel vendor payment information 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>FuelVendorPaymentInformationDTO</returns>            
        public FuelVendorPaymentInformationDTO PutPaymentInformation (FuelVendorPaymentInformationDTO body)
        {
            
    
            var path = "/api/FuelVendor/update-payment-info";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PutPaymentInformation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PutPaymentInformation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FuelVendorPaymentInformationDTO) ApiClient.Deserialize(response.Content, typeof(FuelVendorPaymentInformationDTO), response.Headers);
        }
    
        /// <summary>
        /// Update the company-specific details of a fuel vendor.  These details are unique for each flight department. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateCompanyFuelerResponse</returns>            
        public UpdateCompanyFuelerResponse UpdateCompanyFueler (UpdateCompanyFuelerRequest body)
        {
            
    
            var path = "/api/FuelVendor/company-specific";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyFueler: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyFueler: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateCompanyFuelerResponse) ApiClient.Deserialize(response.Content, typeof(UpdateCompanyFuelerResponse), response.Headers);
        }
    
        /// <summary>
        /// Update an existing company-specific note for a fuel vendor. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateCompanyFuelerNotesResponse</returns>            
        public UpdateCompanyFuelerNotesResponse UpdateCompanyFuelerNotes (UpdateCompanyFuelerNotesRequest body)
        {
            
    
            var path = "/api/FuelVendor/company-specific/notes";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyFuelerNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyFuelerNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateCompanyFuelerNotesResponse) ApiClient.Deserialize(response.Content, typeof(UpdateCompanyFuelerNotesResponse), response.Headers);
        }
    
        /// <summary>
        /// Update a captured price sheet for a company&#39;s fuel vendor. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateCompanyFuelerPriceSheetFileCaptureResponse</returns>            
        public UpdateCompanyFuelerPriceSheetFileCaptureResponse UpdateCompanyFuelerPriceSheetFileCapture (UpdateCompanyFuelerPriceSheetFileCaptureRequest body)
        {
            
    
            var path = "/api/FuelVendor/company-specific/price-sheet-capture";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyFuelerPriceSheetFileCapture: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyFuelerPriceSheetFileCapture: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateCompanyFuelerPriceSheetFileCaptureResponse) ApiClient.Deserialize(response.Content, typeof(UpdateCompanyFuelerPriceSheetFileCaptureResponse), response.Headers);
        }
    
        /// <summary>
        /// Update a company-specific settings record for a fuel vendor. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateCompanyFuelerSettingsResponse</returns>            
        public UpdateCompanyFuelerSettingsResponse UpdateCompanyFuelerSettings (UpdateCompanyFuelerSettingsRequest body)
        {
            
    
            var path = "/api/FuelVendor/company-specific/settings";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyFuelerSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyFuelerSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateCompanyFuelerSettingsResponse) ApiClient.Deserialize(response.Content, typeof(UpdateCompanyFuelerSettingsResponse), response.Headers);
        }
    
    }
}
