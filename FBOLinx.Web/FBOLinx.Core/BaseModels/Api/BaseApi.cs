using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using static SendGrid.BaseClient;
using Method = RestSharp.Method;

namespace FBOLinx.Core.BaseModels.Api
{
    public class BaseApi
    {
        public IApiClient ApiClient { get; set; }

        protected T CallApiPOST<T>(string path, string postBody, Dictionary<String, String> headerParams = null)
        {
            var queryParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            if (headerParams == null)
                headerParams = new Dictionary<string, string>();

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            RestResponse response = (RestResponse)ApiClient.CallApi(path, Method.Post, queryParams, postBody, headerParams,
                formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new BaseApiException((int)response.StatusCode, "Error calling " + path + ": " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new BaseApiException((int)response.StatusCode, "Error calling " + path + ": " + response.ErrorMessage, response.ErrorMessage);

            return (T)this.ApiClient.Deserialize(response.Content, typeof(T), response.Headers);
        }

        protected async Task<T> CallApiPOSTAsync<T>(string path, string postBody, Dictionary<String, String> headerParams = null, Dictionary<string, string> formParams = null, string bodyContentType = null)
        {
            var queryParams = new Dictionary<String, String>();
            if (formParams == null)
                formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            if (headerParams == null)
                headerParams = new Dictionary<string, string>();

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            RestResponse response = (RestResponse)(await ApiClient.CallApiAsync(path, Method.Post, queryParams, postBody, headerParams,
                formParams, fileParams, authSettings, bodyContentType));

            if (((int)response.StatusCode) >= 400)
                throw new BaseApiException((int)response.StatusCode, "Error calling " + path + ": " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new BaseApiException((int)response.StatusCode, "Error calling " + path + ": " + response.ErrorMessage, response.ErrorMessage);

            return (T)this.ApiClient.Deserialize(response.Content, typeof(T), response.Headers);
        }

        protected async Task<T> CallApiPUTAsync<T>(string path, string postBody, Dictionary<String, String> headerParams = null, Dictionary<string, string> formParams = null, string bodyContentType = null)
        {
            var queryParams = new Dictionary<String, String>();
            if (formParams == null)
                formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            if (headerParams == null)
                headerParams = new Dictionary<string, string>();

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            RestResponse response = (RestResponse)(await ApiClient.CallApiAsync(path, Method.Put, queryParams, postBody, headerParams,
                formParams, fileParams, authSettings, bodyContentType));

            if (((int)response.StatusCode) >= 400)
                throw new BaseApiException((int)response.StatusCode, "Error calling " + path + ": " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new BaseApiException((int)response.StatusCode, "Error calling " + path + ": " + response.ErrorMessage, response.ErrorMessage);

            return (T)this.ApiClient.Deserialize(response.Content, typeof(T), response.Headers);
        }

        protected T CallApiGET<T>(string path, Dictionary<String, String> headerParams = null)
        {
            var queryParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            if (headerParams == null)
                headerParams = new Dictionary<string, string>();

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            RestResponse response = (RestResponse)ApiClient.CallApi(path, Method.Get, queryParams, null, headerParams,
                formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new BaseApiException((int)response.StatusCode,
                    "Error calling " + path + ": " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new BaseApiException((int)response.StatusCode,
                    "Error calling " + path + ": " + response.ErrorMessage, response.ErrorMessage);

            return (T)this.ApiClient.Deserialize(response.Content, typeof(T), response.Headers);
        }

        protected async Task<T> CallApiGETAsync<T>(string path, Dictionary<String, String> headerParams = null)
        {
            var queryParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            if (headerParams == null)
                headerParams = new Dictionary<string, string>();

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            RestResponse response = (RestResponse)(await ApiClient.CallApiAsync(path, Method.Get, queryParams, null, headerParams,
                formParams, fileParams, authSettings));

            if (((int)response.StatusCode) >= 400)
                throw new BaseApiException((int)response.StatusCode,
                    "Error calling " + path + ": " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new BaseApiException((int)response.StatusCode,
                    "Error calling " + path + ": " + response.ErrorMessage, response.ErrorMessage);

            return (T)this.ApiClient.Deserialize(response.Content, typeof(T), response.Headers);
        }
    }
}
