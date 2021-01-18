using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<string> PostAsync(string endpoint, object request)
        {
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, data);

            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }
    }
}
