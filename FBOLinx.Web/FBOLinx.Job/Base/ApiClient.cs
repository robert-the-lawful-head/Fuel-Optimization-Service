﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace FBOLinx.Job.Base
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(string basePath)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(basePath);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, object request, string token = "")
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            if (token != "")
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync(endpoint, data);
        }

        public async Task<T> PostAsync<T>(string endpoint, object request, string token = "")
        {
            var responseString = PostAsync(endpoint, request, token).Result;
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseString.Content.ReadAsStringAsync().Result);
            return result;
        }

        public async Task<string> GetAsync(string endpoint, string token = "")
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            if (token != "")
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var response = await _httpClient.GetAsync(endpoint);
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
