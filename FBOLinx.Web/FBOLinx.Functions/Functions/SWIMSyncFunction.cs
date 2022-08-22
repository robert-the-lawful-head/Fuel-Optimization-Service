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

namespace FBOLinx.Functions
{
    public class SWIMSyncFunction
    {
        private readonly ISWIMService _SWIMService;

        public SWIMSyncFunction(ISWIMService swimService)
        {
            _SWIMService = swimService;
        }

        //[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req
        [FunctionName("SWIMSyncFunction")]
        public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo timer, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await _SWIMService.CreatePlaceholderRecordsAndUpdateFlightStatus();
                await _SWIMService.UpdateFlightLegs();
            }
            catch (Exception ex)
            {
                log.LogInformation($"SWIMSyncFunction error: {ex.Message}. StackTrace: {ex.StackTrace}.");
            }
            
            stopwatch.Stop();
            log.LogInformation($"SWIMSyncFunction executed in: {stopwatch.ElapsedMilliseconds / 1000}s.");
        }
    }
}
