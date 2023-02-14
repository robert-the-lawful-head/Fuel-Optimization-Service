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
using Fuelerlinx.SDK;

namespace FBOLinx.Functions
{
    public class SWIMPlaceholderRecordsSyncFunction
    {
        private readonly ISWIMService _SWIMService;
        static readonly HttpClient client = new HttpClient(); // is this where we should instantiate the HttpClient?
        const string InternalAPIKey = "C82FFE95-848B-46AB-A8D6-5116BFA2AB07";


        public SWIMPlaceholderRecordsSyncFunction(ISWIMService swimService)
        {
            _SWIMService = swimService;
        }
        
        [FunctionName("SWIMRecentAndUpcomingFlightLegsSyncFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *", RunOnStartup = true)] TimerInfo timer, ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                var domain = System.Environment.GetEnvironmentVariable("ConnectionStrings:WebApplicationUrl", EnvironmentVariableTarget.Process);
                var endpoint = "/api/swim/sync-flight-legs";

                client.BaseAddress = new Uri(domain);
                client.DefaultRequestHeaders.Add("x-api-key", InternalAPIKey);

                // use HttpClient PostAsync to call sync-flight-legs function with url and InternalAPIKey
                client.PostAsync(endpoint, null);
                
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
