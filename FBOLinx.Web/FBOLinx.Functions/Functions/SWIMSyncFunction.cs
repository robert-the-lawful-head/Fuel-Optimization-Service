using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FBOLinx.Functions.Services;
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

        [FunctionName("Function1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            await _SWIMService.CreatePlaceholderFlightRecordsAndUpdateTaxiingStatus();

            stopwatch.Stop();
            log.LogInformation($"CustomerDataStatisticsFunction executed in: {stopwatch.ElapsedMilliseconds / 1000}s.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
