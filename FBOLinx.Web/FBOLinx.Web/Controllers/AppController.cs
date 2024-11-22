using Microsoft.AspNetCore.Mvc;
using System;
using FBOLinx.Web.DTO;
using FBOLinx.ServiceLayer.Logging;

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : FBOLinxControllerBase
    {
        public AppController(ILoggingService logger) : base(logger)
        {
        }

        // GET: api/app/version
        [HttpGet("version")]
        public IActionResult GetAppVersion()
        {
            int year = DateTime.Now.Year;
            int appVersion = 2; // This should be changed manually
            int buildNumber = GetType().Assembly.GetName().Version.Major;
            AppVersion result = new AppVersion
            {
                Version = $"{year}.{appVersion}.{buildNumber}"
            };
            return Ok(result);
        }
        // GET: api/app/log-data
        [HttpPost("log-data")]
        public IActionResult logData([FromBody] AppLogs log)
        {
            LogRetrace(log.Title,log.Data,log.logLevel, log.logColorCode);
            return Ok();
        }
    }
}
