using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RestSharp;

namespace FBOLinx.Core.BaseModels.Api
{
    public interface IApiClient
    {
        string BasePath { get; set; }
        RestClient RestClient { get; set; }
        Dictionary<String, String> DefaultHeader { get; }

        Object CallApi(String path, RestSharp.Method method, Dictionary<String, String> queryParams, String postBody,
            Dictionary<String, String> headerParams, Dictionary<String, String> formParams,
            Dictionary<String, FileParameter> fileParams, String[] authSettings);

        Object CallApi(String path, RestSharp.Method method, Dictionary<String, String> queryParams, String postBody,
            Dictionary<String, String> headerParams, Dictionary<String, String> formParams,
            Dictionary<String, FileParameter> fileParams, String[] authSettings, string bodyContentType);

        Task<Object> CallApiAsync(String path, RestSharp.Method method, Dictionary<String, String> queryParams, String postBody,
            Dictionary<String, String> headerParams, Dictionary<String, String> formParams,
            Dictionary<String, FileParameter> fileParams, String[] authSettings);

        Task<Object> CallApiAsync(String path, RestSharp.Method method, Dictionary<String, String> queryParams,
            String postBody,
            Dictionary<String, String> headerParams, Dictionary<String, String> formParams,
            Dictionary<String, FileParameter> fileParams, String[] authSettings, string bodyContentType);

        RestRequest GetLastRequest();
        void AddDefaultHeader(string key, string value);
        string EscapeString(string str);
        string ParameterToString(object obj);
        string Serialize(object obj);
        //string GetApiKeyWithPrefix(string apiKeyIdentifier);

        void UpdateParamsForAuth(Dictionary<String, String> queryParams, Dictionary<String, String> headerParams,
            string[] authSettings);

        object Deserialize(string content, Type type, IReadOnlyCollection<HeaderParameter> headers = null);
    }
}
