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
    public class SWIMPlaceholderRecordsSyncFunction
    {
        private readonly ISWIMService _SWIMService;

        public SWIMPlaceholderRecordsSyncFunction(ISWIMService swimService)
        {
            _SWIMService = swimService;
        }
        
        [FunctionName("SWIMPlaceholderRecordsSyncFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *", RunOnStartup = false)] TimerInfo timer, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await _SWIMService.CreatePlaceholderRecords();
            }
            catch (Exception ex)
            {
                log.LogInformation($"SWIMPlaceholderRecordsSyncFunction error: {ex.Message}. StackTrace: {ex.StackTrace}.");
            }
            
            stopwatch.Stop();
            log.LogInformation($"SWIMPlaceholderRecordsSyncFunction executed in: {stopwatch.ElapsedMilliseconds / 1000}s.");
        }
    }
}
