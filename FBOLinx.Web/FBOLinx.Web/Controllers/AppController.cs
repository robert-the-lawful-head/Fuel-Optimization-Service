using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System;

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        public AppController()
        {
        }

        // GET: api/app/version
        [HttpGet("version")]
        public IActionResult GetAppVersion()
        {
            int year = DateTime.Now.Year;
            int appVersion = 2; // This should be changed manually
            int buildNumber = GetType().Assembly.GetName().Version.Major;
            return Ok($"{year}.{appVersion}.{buildNumber}");
        }
    }
}
