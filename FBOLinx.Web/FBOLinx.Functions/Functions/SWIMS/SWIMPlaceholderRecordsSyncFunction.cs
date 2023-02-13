using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.SWIM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http; // for HttpClient

namespace FBOLinx.Functions
{
    public class SWIMPlaceholderRecordsSyncFunction
    {
        private readonly ISWIMService _SWIMService;
        static readonly HttpClient client = new HttpClient(); // is this where we should instantiate the HttpClient?


        public SWIMPlaceholderRecordsSyncFunction(ISWIMService swimService)
        {
            _SWIMService = swimService;
        }
        
        [FunctionName("SWIMRecentAndUpcomingFlightLegsSyncFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *", RunOnStartup = false)] TimerInfo timer, ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                // original code --------------------
                // await _SWIMService.SyncRecentAndUpcomingFlightLegs();

                // build out url to call sync-flight-legs function
                var domain = System.Environment.GetEnvironmentVariable("WebApplicationUrl");
                var endpoint = "/api/swim/sync-flight-legs";
                var url = $"{domain}{endpoint}";

                // append internal api key to url
                var xApiKey = System.Environment.GetEnvironmentVariable("InternalAPIKey");
                url += $"?x-api-key={xApiKey}";
                
                // use HttpClient PostAsync to call sync-flight-legs function with url and InternalAPIKey
                var response = await client.PostAsync(url, null);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                log.LogInformation($"SWIMRecentAndUpcomingFlightLegsSyncFunction error: {ex.Message}. StackTrace: {ex.StackTrace}.");
            }
            
            stopwatch.Stop();
            log.LogInformation($"SWIMRecentAndUpcomingFlightLegsSyncFunction executed in: {stopwatch.ElapsedMilliseconds / 1000}s.");
        }
    }
}
