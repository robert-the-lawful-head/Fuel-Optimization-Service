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
    public class SWIMStaticSyncFunction
    {
        private readonly ISWIMService _SWIMService;

        public SWIMStaticSyncFunction(ISWIMService swimService)
        {
            _SWIMService = swimService;
        }
        
        [FunctionName("SWIMStaticSyncFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *", RunOnStartup = false)] TimerInfo timer, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await _SWIMService.SetFlightLegsStaticData();
            }
            catch (Exception ex)
            {
                log.LogInformation($"SWIMStaticSyncFunction error: {ex.Message}. StackTrace: {ex.StackTrace}.");
            }
            
            stopwatch.Stop();
            log.LogInformation($"SWIMStaticSyncFunction executed in: {stopwatch.ElapsedMilliseconds / 1000}s.");
        }
    }
}
