﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using FBOLinx.Core.BaseModels.Api;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;

namespace FBOLinx.Core.BaseModels.Api
{
    public class BaseApiClient : IApiClient
    {
        private readonly Dictionary<String, String> _defaultHeaderMap = new Dictionary<String, String>();
        private RestRequest _Request;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiClient" /> class.
        /// </summary>
        /// <param name="basePath">The base path.</param>
        public BaseApiClient(String basePath = "")
        {
            BasePath = basePath;
            var options = new RestClientOptions()
            {
                MaxTimeout = 120000
            };

            RestClient = new RestClient(BasePath);
        }

        public BaseApiClient(RestClientOptions options)
        {
            BasePath = options.BaseUrl?.ToString();

            RestClient = new RestClient(options);
        }

        /// <summary>
        /// Gets or sets the base path.
        /// </summary>
        /// <value>The base path</value>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets or sets the RestClient.
        /// </summary>
        /// <value>An instance of the RestClient</value>
        public RestClient RestClient { get; set; }

        /// <summary>
        /// Gets the default header.
        /// </summary>
        public Dictionary<String, String> DefaultHeader
        {
            get { return _defaultHeaderMap; }
        }

        /// <summary>
        /// Makes the HTTP request (Sync).
        /// </summary>
        /// <param name="path">URL path.</param>
        /// <param name="method">HTTP method.</param>
        /// <param name="queryParams">Query parameters.</param>
        /// <param name="postBody">HTTP body (POST request).</param>
        /// <param name="headerParams">Header parameters.</param>
        /// <param name="formParams">Form parameters.</param>
        /// <param name="fileParams">File parameters.</param>
        /// <param name="authSettings">Authentication settings.</param>
        /// <returns>Object</returns>
        public virtual Object CallApi(String path, RestSharp.Method method, Dictionary<String, String> queryParams, String postBody,
            Dictionary<String, String> headerParams, Dictionary<String, String> formParams,
            Dictionary<String, FileParameter> fileParams, String[] authSettings)
        {
            return CallApi(path, method, queryParams, postBody, headerParams, formParams, fileParams, authSettings,
                "application/json");
        }

        public virtual Object CallApi(String path, RestSharp.Method method, Dictionary<String, String> queryParams, String postBody,
            Dictionary<String, String> headerParams, Dictionary<String, String> formParams,
            Dictionary<String, FileParameter> fileParams, String[] authSettings, string bodyContentType)
        {
            var request = GetRestRequest(path, method, queryParams, postBody, headerParams, formParams, fileParams, authSettings, bodyContentType);

            return (Object)RestClient.Execute(request);
        }

        public virtual async Task<Object> CallApiAsync(String path, RestSharp.Method method, Dictionary<String, String> queryParams, String postBody,
            Dictionary<String, String> headerParams = null, Dictionary<String, String> formParams = null,
            Dictionary<String, FileParameter> fileParams = null, String[] authSettings = null)
        {
            return await CallApiAsync(path, method, queryParams, postBody, headerParams, formParams, fileParams, authSettings,
                "application/json");
        }

        public virtual async Task<Object> CallApiAsync(String path, RestSharp.Method method, Dictionary<String, String> queryParams, String postBody,
            Dictionary<String, String> headerParams, Dictionary<String, String> formParams,
            Dictionary<String, FileParameter> fileParams, String[] authSettings, string bodyContentType)
        {
            _Request = GetRestRequest(path, method, queryParams, postBody, headerParams, formParams, fileParams, authSettings, bodyContentType);

            var response = (RestResponse)(await RestClient.ExecuteAsync(_Request));

            if (response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
            {
                if (response.ErrorException != null)
                    throw new Exception(path, response.ErrorException);
                else
                    throw new Exception(path + ": " + response.Content);
            }

            return (Object)response;

        }

        public RestRequest GetLastRequest()
        {
            return _Request;
        }

        /// <summary>
        /// Makes the HTTP request with x-www-form-urlencoded content-type (Sync).
        /// </summary>
        /// <param name="path">URL path.</param>
        /// <param name="method">HTTP method.</param>
        /// <param name="queryParams">Query parameters.</param>
        /// <param name="postBody">HTTP body (POST request).</param>
        /// <param name="headerParams">Header parameters.</param>
        /// <param name="formParams">Form parameters.</param>
        /// <param name="fileParams">File parameters.</param>
        /// <param name="authSettings">Authentication settings.</param>
        /// <returns>Object</returns>
        public Object CallFormUrlEncodedApi(String path, RestSharp.Method method, Dictionary<String, String> queryParams, String postBody,
            Dictionary<String, String> headerParams, Dictionary<String, String> formParams,
            Dictionary<String, FileParameter> fileParams, String[] authSettings)
        {

            var request = new RestRequest(path, method);

            UpdateParamsForAuth(queryParams, headerParams, authSettings);

            // add default header, if any
            foreach (var defaultHeader in _defaultHeaderMap)
                request.AddHeader(defaultHeader.Key, defaultHeader.Value);

            // add header parameter, if any
            foreach (var param in headerParams)
                request.AddHeader(param.Key, param.Value);

            // add query parameter, if any
            foreach (var param in queryParams)
                request.AddParameter(param.Key, param.Value, ParameterType.GetOrPost);

            // add form parameter, if any
            foreach (var param in formParams)
                request.AddParameter(param.Key, param.Value, ParameterType.GetOrPost);

            // add file parameter, if any
            foreach (var param in fileParams)
                request.AddFile(param.Value.Name, param.Value.FileName, param.Value.ContentType);

            if (postBody != null) // http body (model) parameter
            {
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", postBody, ParameterType.RequestBody);
            }

            return (Object)RestClient.Execute(request);

        }

        /// <summary>
        /// Add default header.
        /// </summary>
        /// <param name="key">Header field name.</param>
        /// <param name="value">Header field value.</param>
        /// <returns></returns>
        public void AddDefaultHeader(string key, string value)
        {
            _defaultHeaderMap.Add(key, value);
        }

        /// <summary>
        /// Escape string (url-encoded).
        /// </summary>
        /// <param name="str">String to be escaped.</param>
        /// <returns>Escaped string.</returns>
        public string EscapeString(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// If parameter is DateTime, output in a formatted string (default ISO 8601), customizable with Configuration.DateTime.
        /// If parameter is a list of string, join the list with ",".
        /// Otherwise just return the string.
        /// </summary>
        /// <param name="obj">The parameter (header, path, query, form).</param>
        /// <returns>Formatted string.</returns>
        public string ParameterToString(object obj)
        {
            //if (obj is DateTime)
            //    // Return a formatted date string - Can be customized with Configuration.DateTimeFormat
            //    // Defaults to an ISO 8601, using the known as a Round-trip date/time pattern ("o")
            //    // https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#Anchor_8
            //    // For example: 2009-06-15T13:45:30.0000000
            //    return ((DateTime)obj).ToString(BaseApiConfiguration.DateTimeFormat);
            //else 
            if (obj is List<string>)
                return String.Join(",", (obj as List<string>).ToArray());
            else
                return Convert.ToString(obj);
        }

        /// <summary>
        /// Deserialize the JSON string into a proper object.
        /// </summary>
        /// <param name="content">HTTP body (e.g. string, JSON).</param>
        /// <param name="type">Object type.</param>
        /// <param name="headers">HTTP headers.</param>
        /// <returns>Object representation of the JSON string.</returns>
        public object Deserialize(string content, Type type, IReadOnlyCollection<HeaderParameter> headers = null)
        {
            if (type == typeof(Object)) // return an object
            {
                return content;
            }

            //if (type == typeof(Stream))
            //{
            //    var filePath = String.IsNullOrEmpty(BaseApiConfiguration.TempFolderPath)
            //        ? Path.GetTempPath()
            //        : BaseApiConfiguration.TempFolderPath;

            //    var fileName = filePath + Guid.NewGuid();
            //    if (headers != null)
            //    {
            //        var regex = new Regex(@"Content-Disposition:.*filename=['""]?([^'""\s]+)['""]?$");
            //        var match = regex.Match(headers.ToString());
            //        if (match.Success)
            //            fileName = filePath + match.Value.Replace("\"", "").Replace("'", "");
            //    }
            //    File.WriteAllText(fileName, content);
            //    return new FileStream(fileName, FileMode.Open);

            //}

            if (type.Name.StartsWith("System.Nullable`1[[System.DateTime")) // return a datetime object
            {
                return DateTime.Parse(content, null, System.Globalization.DateTimeStyles.RoundtripKind);
            }

            if (type == typeof(String) || type.Name.StartsWith("System.Nullable")) // return primitive type
            {
                return ConvertType(content, type);
            }

            // at this point, it must be a model (json)
            try
            {
                return JsonConvert.DeserializeObject(content, type);
            }
            catch (IOException e)
            {
                throw new BaseApiException(500, e.Message);
            }
        }

        /// <summary>
        /// Serialize an object into JSON string.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns>JSON string.</returns>
        public string Serialize(object obj)
        {
            try
            {
                return obj != null ? JsonConvert.SerializeObject(obj) : null;
            }
            catch (Exception e)
            {
                throw new BaseApiException(500, e.Message);
            }
        }

        /// <summary>
        /// Get the API key with prefix.
        /// </summary>
        /// <param name="apiKeyIdentifier">API key identifier (authentication scheme).</param>
        /// <returns>API key with prefix.</returns>
        //public string GetApiKeyWithPrefix(string apiKeyIdentifier)
        //{
        //    var apiKeyValue = "";
        //    BaseApiConfiguration.ApiKey.TryGetValue(apiKeyIdentifier, out apiKeyValue);
        //    var apiKeyPrefix = "";
        //    if (BaseApiConfiguration.ApiKeyPrefix.TryGetValue(apiKeyIdentifier, out apiKeyPrefix))
        //        return apiKeyPrefix + " " + apiKeyValue;
        //    else
        //        return apiKeyValue;
        //}

        /// <summary>
        /// Update parameters based on authentication.
        /// </summary>
        /// <param name="queryParams">Query parameters.</param>
        /// <param name="headerParams">Header parameters.</param>
        /// <param name="authSettings">Authentication settings.</param>
        public void UpdateParamsForAuth(Dictionary<String, String> queryParams, Dictionary<String, String> headerParams, string[] authSettings)
        {
            if (authSettings == null || authSettings.Length == 0)
                return;

            foreach (string auth in authSettings)
            {
                // determine which one to use
                switch (auth)
                {
                    default:
                        //TODO show warning about security definition not found
                        break;
                }
            }
        }

        #region Private Methods
        private RestRequest GetRestRequest(string path, Method method, Dictionary<string, string> queryParams, string postBody,
            Dictionary<string, string> headerParams, Dictionary<string, string> formParams, Dictionary<string, FileParameter> fileParams, string[] authSettings, string bodyContentType)
        {
            var request = new RestRequest(path, method);

            UpdateParamsForAuth(queryParams, headerParams, authSettings);

            // add default header, if any
            if (_defaultHeaderMap != null)
                foreach (var defaultHeader in _defaultHeaderMap)
                    request.AddHeader(defaultHeader.Key, defaultHeader.Value);

            // add header parameter, if any
            if (headerParams != null)
                foreach (var param in headerParams)
                    request.AddHeader(param.Key, param.Value);

            // add query parameter, if any
            if (queryParams != null)
                foreach (var param in queryParams)
                    request.AddParameter(param.Key, param.Value, ParameterType.GetOrPost);

            // add form parameter, if any
            if (formParams != null)
                foreach (var param in formParams)
                    request.AddParameter(param.Key, param.Value, ParameterType.GetOrPost);

            // add file parameter, if any
            if (fileParams != null)
                foreach (var param in fileParams)
                    request.AddFile(param.Value.Name, param.Value.FileName, param.Value.ContentType);

            if (postBody != null) // http body (model) parameter
            {
                if (!string.IsNullOrEmpty(bodyContentType) && bodyContentType.ToLower() != "application/json")
                {
                    if (!request.Parameters.Any(p => p.Name == "Content-Type"))
                        request.AddHeader("Content-Type", bodyContentType);
                    request.AddBody(postBody, bodyContentType);
                }
                else
                {
                    request.AddHeader("Accept", "application/json");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", postBody, ParameterType.RequestBody);
                }
            }

            return request;
        }
        #endregion

        /// <summary>
        /// Encode string in base64 format.
        /// </summary>
        /// <param name="text">String to be encoded.</param>
        /// <returns>Encoded string.</returns>
        public static string Base64Encode(string text)
        {
            var textByte = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(textByte);
        }

        /// <summary>
        /// Dynamically cast the object into target type.
        /// </summary>
        /// <param name="fromObject">Object to be casted</param>
        /// <param name="toObject">Target type</param>
        /// <returns>Casted object</returns>
        public static Object ConvertType(Object fromObject, Type toObject)
        {
            return Convert.ChangeType(fromObject, toObject);
        }
    }
}
