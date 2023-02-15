using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.SWIM;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace FBOLinx.Functions
{
    public class SWIMPlaceholderRecordsSyncFunction
    {
        private readonly ISWIMService _SWIMService;
        private readonly HttpClient _httpClient;
        public SWIMPlaceholderRecordsSyncFunction(ISWIMService swimService, IHttpClientFactory httpClientFactory)
        {
            _SWIMService = swimService;
            _httpClient = httpClientFactory.CreateClient("FBOLinx");
        }
        
        [FunctionName("SWIMRecentAndUpcomingFlightLegsSyncFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *", RunOnStartup = false)] TimerInfo timer, ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                var endpoint = "/api/swim/sync-flight-legs";
                
                _httpClient.PostAsync(endpoint, null);
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
