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
    public interface IFileDataApi
    {
        /// <summary>
        /// Internal use only - Delete image file data by {id}. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteImageFileDataResponse</returns>
        DeleteImageFileDataResponse DeleteImageFileData (int? id);
        /// <summary>
        /// Internal use only - Delete a file captured during an import. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteImportFileCaptureResponse</returns>
        DeleteImportFileCaptureResponse DeleteImportFileCapture (int? id);
        /// <summary>
        /// Deletes job file data by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteJobFileDataResponse</returns>
        DeleteJobFileDataResponse DeleteJobFileData (int? id);
        /// <summary>
        /// Delete price sheet file data by the provided {id}. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeletePriceSheetFileDataResponse</returns>
        DeletePriceSheetFileDataResponse DeletePriceSheetFileData (int? id);
        /// <summary>
        /// Internal use only - Delete a supported invoice file template by it&#39;s Id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteSupportedInvoiceFileTemplateResponse</returns>
        DeleteSupportedInvoiceFileTemplateResponse DeleteSupportedInvoiceFileDataTemplate (int? id);
        /// <summary>
        /// Delete transaction file data by the provided {id}. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteTransactionFileDataResponse</returns>
        DeleteTransactionFileDataResponse DeleteTransactionFileData (int? id);
        /// <summary>
        /// Internal use only - Fetch image file data by {id}. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ImageFileDataResponse</returns>
        ImageFileDataResponse GetImageFileDataById (int? id);
        /// <summary>
        /// Internal use only - Fetch a captured file import by Id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ImportFileCaptureResponse</returns>
        ImportFileCaptureResponse GetImportFileCaptureById (int? id);
        /// <summary>
        /// Fetch price sheet file data captured during an upload of pricing info. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>PriceSheetFileDataResponse</returns>
        PriceSheetFileDataResponse GetPriceSheetFileData (int? id);
        /// <summary>
        /// Internal use only - Fetch a supported invoice file template by fuel vendor and company. 
        /// </summary>
        /// <param name="fuelerProcessName"></param>
        /// <param name="companyId"></param>
        /// <returns>SupportedInvoiceFileTemplateResponse</returns>
        SupportedInvoiceFileTemplateResponse GetSupportedInvoiceFileTemplatesByCompany (string fuelerProcessName, int? companyId);
        /// <summary>
        /// Internal use only - Fetch a supported invoice file template by fuel vendor. 
        /// </summary>
        /// <param name="fuelerProcessName"></param>
        /// <returns>SupportedInvoiceFileTemplateResponse</returns>
        SupportedInvoiceFileTemplateResponse GetSupportedInvoiceFileTemplatesByFuelVendor (string fuelerProcessName);
        /// <summary>
        /// Fetch transaction file data for an invoice, receipt, or fuel release. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TransactionFileDataResponse</returns>
        TransactionFileDataResponse GetTransactionFileData (int? id);
        /// <summary>
        /// Gets job file data by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JobFileDataResponse</returns>
        JobFileDataResponse GetsJobFileDataById (int? id);
        /// <summary>
        /// Internal use only - Post new image file data. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostImageFileDataResponse</returns>
        PostImageFileDataResponse PostImageFileData (PostImageFileDataRequest body);
        /// <summary>
        /// Internal use only - Add a captured file that was recently imported. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostImportFileCaptureResponse</returns>
        PostImportFileCaptureResponse PostImportFileCapture (PostImportFileCaptureRequest body);
        /// <summary>
        /// Post job file data 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostJobFileDataResponse</returns>
        PostJobFileDataResponse PostJobFileData (PostJobFileDataRequest body);
        /// <summary>
        /// Add price sheet file data for an uploaded fuel price sheet.  The file data should be passed as a base64 string.  This is for capturing purposes only and will NOT update pricing. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostPriceSheetFileDataResponse</returns>
        PostPriceSheetFileDataResponse PostPriceSheetFileData (PostPriceSheetFileDataRequest body);
        /// <summary>
        /// Internal use only - Add a supported invoice file template. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostSupportedInvoiceFileTemplateResponse</returns>
        PostSupportedInvoiceFileTemplateResponse PostSupportedInvoiceFileTemplate (PostSupportedInvoiceFileTemplateRequest body);
        /// <summary>
        /// Add transaction file data for an invoice, receipt, or fuel release.  The file data should be passed as a base64 string. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostTransactionFileDataResponse</returns>
        PostTransactionFileDataResponse PostTransactionFileData (PostTransactionFileDataRequest body);
        /// <summary>
        /// Internal use only - Update an existing record of image file data. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns>UpdateImageFileDataResponse</returns>
        UpdateImageFileDataResponse UpdateImageFileData (int? id, UpdateImageFileDataRequest body);
        /// <summary>
        /// Internal use only - Update a file captured during an import. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns>UpdateImportFileCaptureResponse</returns>
        UpdateImportFileCaptureResponse UpdateImportFileCapture (int? id, UpdateImportFileCaptureRequest body);
        /// <summary>
        /// Updates job file data 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateJobFileDataResponse</returns>
        UpdateJobFileDataResponse UpdateJobFileData (UpdateJobFileDataRequest body);
        /// <summary>
        /// Update price sheet file data for an uploaded fuel price sheet.  This is for capturing purposes only and will NOT update pricing. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdatePriceSheetFileDataResponse</returns>
        UpdatePriceSheetFileDataResponse UpdatePriceSheetFileData (UpdatePriceSheetFileDataRequest body);
        /// <summary>
        /// Internal use only - Update a supported invoice file template. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateSupportedInvoiceFileTemplateResponse</returns>
        UpdateSupportedInvoiceFileTemplateResponse UpdateSupportedInvoiceFileDataTemplate (UpdateSupportedFileTemplateRequest body);
        /// <summary>
        /// Update transaction file data for an invoice, receipt, or fuel release. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateTransactionFileDataResponse</returns>
        UpdateTransactionFileDataResponse UpdateTransactionFileData (UpdateTransactionFileDataRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class FileDataApi : IFileDataApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDataApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public FileDataApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDataApi"/> class.
        /// </summary>
        /// <returns></returns>
        public FileDataApi(String basePath)
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
        /// Internal use only - Delete image file data by {id}. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteImageFileDataResponse</returns>            
        public DeleteImageFileDataResponse DeleteImageFileData (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteImageFileData");
            
    
            var path = "/api/FileData/image-file-data/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteImageFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteImageFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteImageFileDataResponse) ApiClient.Deserialize(response.Content, typeof(DeleteImageFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Delete a file captured during an import. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteImportFileCaptureResponse</returns>            
        public DeleteImportFileCaptureResponse DeleteImportFileCapture (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteImportFileCapture");
            
    
            var path = "/api/FileData/import-file-capture/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteImportFileCapture: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteImportFileCapture: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteImportFileCaptureResponse) ApiClient.Deserialize(response.Content, typeof(DeleteImportFileCaptureResponse), response.Headers);
        }
    
        /// <summary>
        /// Deletes job file data by Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteJobFileDataResponse</returns>            
        public DeleteJobFileDataResponse DeleteJobFileData (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteJobFileData");
            
    
            var path = "/api/FileData/job-file-data/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteJobFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteJobFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteJobFileDataResponse) ApiClient.Deserialize(response.Content, typeof(DeleteJobFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete price sheet file data by the provided {id}. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeletePriceSheetFileDataResponse</returns>            
        public DeletePriceSheetFileDataResponse DeletePriceSheetFileData (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeletePriceSheetFileData");
            
    
            var path = "/api/FileData/price-sheet-file-data/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeletePriceSheetFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeletePriceSheetFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeletePriceSheetFileDataResponse) ApiClient.Deserialize(response.Content, typeof(DeletePriceSheetFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Delete a supported invoice file template by it&#39;s Id. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteSupportedInvoiceFileTemplateResponse</returns>            
        public DeleteSupportedInvoiceFileTemplateResponse DeleteSupportedInvoiceFileDataTemplate (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteSupportedInvoiceFileDataTemplate");
            
    
            var path = "/api/FileData/invoice-file/supported-template/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSupportedInvoiceFileDataTemplate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSupportedInvoiceFileDataTemplate: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteSupportedInvoiceFileTemplateResponse) ApiClient.Deserialize(response.Content, typeof(DeleteSupportedInvoiceFileTemplateResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete transaction file data by the provided {id}. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteTransactionFileDataResponse</returns>            
        public DeleteTransactionFileDataResponse DeleteTransactionFileData (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteTransactionFileData");
            
    
            var path = "/api/FileData/transaction-file-data/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTransactionFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTransactionFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteTransactionFileDataResponse) ApiClient.Deserialize(response.Content, typeof(DeleteTransactionFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch image file data by {id}. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>ImageFileDataResponse</returns>            
        public ImageFileDataResponse GetImageFileDataById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetImageFileDataById");
            
    
            var path = "/api/FileData/image-file-data/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetImageFileDataById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetImageFileDataById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ImageFileDataResponse) ApiClient.Deserialize(response.Content, typeof(ImageFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch a captured file import by Id. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>ImportFileCaptureResponse</returns>            
        public ImportFileCaptureResponse GetImportFileCaptureById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetImportFileCaptureById");
            
    
            var path = "/api/FileData/import-file-capture/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetImportFileCaptureById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetImportFileCaptureById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ImportFileCaptureResponse) ApiClient.Deserialize(response.Content, typeof(ImportFileCaptureResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch price sheet file data captured during an upload of pricing info. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>PriceSheetFileDataResponse</returns>            
        public PriceSheetFileDataResponse GetPriceSheetFileData (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetPriceSheetFileData");
            
    
            var path = "/api/FileData/price-sheet-file-data/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetPriceSheetFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetPriceSheetFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PriceSheetFileDataResponse) ApiClient.Deserialize(response.Content, typeof(PriceSheetFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch a supported invoice file template by fuel vendor and company. 
        /// </summary>
        /// <param name="fuelerProcessName"></param> 
        /// <param name="companyId"></param> 
        /// <returns>SupportedInvoiceFileTemplateResponse</returns>            
        public SupportedInvoiceFileTemplateResponse GetSupportedInvoiceFileTemplatesByCompany (string fuelerProcessName, int? companyId)
        {
            
            // verify the required parameter 'fuelerProcessName' is set
            if (fuelerProcessName == null) throw new ApiException(400, "Missing required parameter 'fuelerProcessName' when calling GetSupportedInvoiceFileTemplatesByCompany");
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetSupportedInvoiceFileTemplatesByCompany");
            
    
            var path = "/api/FileData/invoice-file/supported-template/fueler/{fuelerProcessName}/company/{companyId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "fuelerProcessName" + "}", ApiClient.ParameterToString(fuelerProcessName));
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedInvoiceFileTemplatesByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedInvoiceFileTemplatesByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SupportedInvoiceFileTemplateResponse) ApiClient.Deserialize(response.Content, typeof(SupportedInvoiceFileTemplateResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch a supported invoice file template by fuel vendor. 
        /// </summary>
        /// <param name="fuelerProcessName"></param> 
        /// <returns>SupportedInvoiceFileTemplateResponse</returns>            
        public SupportedInvoiceFileTemplateResponse GetSupportedInvoiceFileTemplatesByFuelVendor (string fuelerProcessName)
        {
            
            // verify the required parameter 'fuelerProcessName' is set
            if (fuelerProcessName == null) throw new ApiException(400, "Missing required parameter 'fuelerProcessName' when calling GetSupportedInvoiceFileTemplatesByFuelVendor");
            
    
            var path = "/api/FileData/invoice-file/supported-template/fueler/{fuelerProcessName}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "fuelerProcessName" + "}", ApiClient.ParameterToString(fuelerProcessName));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedInvoiceFileTemplatesByFuelVendor: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedInvoiceFileTemplatesByFuelVendor: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SupportedInvoiceFileTemplateResponse) ApiClient.Deserialize(response.Content, typeof(SupportedInvoiceFileTemplateResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch transaction file data for an invoice, receipt, or fuel release. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>TransactionFileDataResponse</returns>            
        public TransactionFileDataResponse GetTransactionFileData (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetTransactionFileData");
            
    
            var path = "/api/FileData/transaction-file-data/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TransactionFileDataResponse) ApiClient.Deserialize(response.Content, typeof(TransactionFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Gets job file data by Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>JobFileDataResponse</returns>            
        public JobFileDataResponse GetsJobFileDataById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetsJobFileDataById");
            
    
            var path = "/api/FileData/job-file-data/by-id/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetsJobFileDataById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetsJobFileDataById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (JobFileDataResponse) ApiClient.Deserialize(response.Content, typeof(JobFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Post new image file data. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostImageFileDataResponse</returns>            
        public PostImageFileDataResponse PostImageFileData (PostImageFileDataRequest body)
        {
            
    
            var path = "/api/FileData/image-file-data";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostImageFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostImageFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostImageFileDataResponse) ApiClient.Deserialize(response.Content, typeof(PostImageFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Add a captured file that was recently imported. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostImportFileCaptureResponse</returns>            
        public PostImportFileCaptureResponse PostImportFileCapture (PostImportFileCaptureRequest body)
        {
            
    
            var path = "/api/FileData/import-file-capture";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostImportFileCapture: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostImportFileCapture: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostImportFileCaptureResponse) ApiClient.Deserialize(response.Content, typeof(PostImportFileCaptureResponse), response.Headers);
        }
    
        /// <summary>
        /// Post job file data 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostJobFileDataResponse</returns>            
        public PostJobFileDataResponse PostJobFileData (PostJobFileDataRequest body)
        {
            
    
            var path = "/api/FileData/job-file-data";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostJobFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostJobFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostJobFileDataResponse) ApiClient.Deserialize(response.Content, typeof(PostJobFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Add price sheet file data for an uploaded fuel price sheet.  The file data should be passed as a base64 string.  This is for capturing purposes only and will NOT update pricing. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostPriceSheetFileDataResponse</returns>            
        public PostPriceSheetFileDataResponse PostPriceSheetFileData (PostPriceSheetFileDataRequest body)
        {
            
    
            var path = "/api/FileData/price-sheet-file-data";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostPriceSheetFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostPriceSheetFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostPriceSheetFileDataResponse) ApiClient.Deserialize(response.Content, typeof(PostPriceSheetFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Add a supported invoice file template. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostSupportedInvoiceFileTemplateResponse</returns>            
        public PostSupportedInvoiceFileTemplateResponse PostSupportedInvoiceFileTemplate (PostSupportedInvoiceFileTemplateRequest body)
        {
            
    
            var path = "/api/FileData/invoice-file/supported-template";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostSupportedInvoiceFileTemplate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostSupportedInvoiceFileTemplate: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostSupportedInvoiceFileTemplateResponse) ApiClient.Deserialize(response.Content, typeof(PostSupportedInvoiceFileTemplateResponse), response.Headers);
        }
    
        /// <summary>
        /// Add transaction file data for an invoice, receipt, or fuel release.  The file data should be passed as a base64 string. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostTransactionFileDataResponse</returns>            
        public PostTransactionFileDataResponse PostTransactionFileData (PostTransactionFileDataRequest body)
        {
            
    
            var path = "/api/FileData/transaction-file-data";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostTransactionFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostTransactionFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostTransactionFileDataResponse) ApiClient.Deserialize(response.Content, typeof(PostTransactionFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update an existing record of image file data. 
        /// </summary>
        /// <param name="id"></param> 
        /// <param name="body"></param> 
        /// <returns>UpdateImageFileDataResponse</returns>            
        public UpdateImageFileDataResponse UpdateImageFileData (int? id, UpdateImageFileDataRequest body)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UpdateImageFileData");
            
    
            var path = "/api/FileData/image-file-data/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateImageFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateImageFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateImageFileDataResponse) ApiClient.Deserialize(response.Content, typeof(UpdateImageFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update a file captured during an import. 
        /// </summary>
        /// <param name="id"></param> 
        /// <param name="body"></param> 
        /// <returns>UpdateImportFileCaptureResponse</returns>            
        public UpdateImportFileCaptureResponse UpdateImportFileCapture (int? id, UpdateImportFileCaptureRequest body)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UpdateImportFileCapture");
            
    
            var path = "/api/FileData/import-file-capture/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateImportFileCapture: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateImportFileCapture: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateImportFileCaptureResponse) ApiClient.Deserialize(response.Content, typeof(UpdateImportFileCaptureResponse), response.Headers);
        }
    
        /// <summary>
        /// Updates job file data 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateJobFileDataResponse</returns>            
        public UpdateJobFileDataResponse UpdateJobFileData (UpdateJobFileDataRequest body)
        {
            
    
            var path = "/api/FileData/job-file-data";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateJobFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateJobFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateJobFileDataResponse) ApiClient.Deserialize(response.Content, typeof(UpdateJobFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Update price sheet file data for an uploaded fuel price sheet.  This is for capturing purposes only and will NOT update pricing. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdatePriceSheetFileDataResponse</returns>            
        public UpdatePriceSheetFileDataResponse UpdatePriceSheetFileData (UpdatePriceSheetFileDataRequest body)
        {
            
    
            var path = "/api/FileData/price-sheet-file-data";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdatePriceSheetFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdatePriceSheetFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdatePriceSheetFileDataResponse) ApiClient.Deserialize(response.Content, typeof(UpdatePriceSheetFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update a supported invoice file template. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateSupportedInvoiceFileTemplateResponse</returns>            
        public UpdateSupportedInvoiceFileTemplateResponse UpdateSupportedInvoiceFileDataTemplate (UpdateSupportedFileTemplateRequest body)
        {
            
    
            var path = "/api/FileData/invoice-file/supported-template";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSupportedInvoiceFileDataTemplate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSupportedInvoiceFileDataTemplate: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateSupportedInvoiceFileTemplateResponse) ApiClient.Deserialize(response.Content, typeof(UpdateSupportedInvoiceFileTemplateResponse), response.Headers);
        }
    
        /// <summary>
        /// Update transaction file data for an invoice, receipt, or fuel release. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateTransactionFileDataResponse</returns>            
        public UpdateTransactionFileDataResponse UpdateTransactionFileData (UpdateTransactionFileDataRequest body)
        {
            
    
            var path = "/api/FileData/transaction-file-data";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTransactionFileData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTransactionFileData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateTransactionFileDataResponse) ApiClient.Deserialize(response.Content, typeof(UpdateTransactionFileDataResponse), response.Headers);
        }
    
    }
}
