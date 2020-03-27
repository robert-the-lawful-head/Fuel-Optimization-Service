using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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
            string assemblyVersion1 = GetType().Assembly.GetName().Version.ToString();
            string assemblyVersion2 = Assembly.GetEntryAssembly().GetName().Version.ToString();
            string fileVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            string version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            List<string> result = new List<string>
            {
                assemblyVersion1,
                assemblyVersion2,
                fileVersion,
                version
            };
            return Ok(result);
        }
    }
}
