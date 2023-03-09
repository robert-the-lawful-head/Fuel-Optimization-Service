using FBOLinx.ServiceLayer.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FBOLinx.Web.Controllers
{

    public class FBOLinxControllerBase : ControllerBase
    {
        private readonly ILoggingService _logger;
        public FBOLinxControllerBase(ILoggingService logger)
        {
            _logger = logger;
        }

        protected void HandleExceptionAsync(Exception exception)
        {
            _logger.LogError(exception.Message + (exception.InnerException != null ? ". Inner exception: " + exception.InnerException.Message + "***" + exception.InnerException.StackTrace : ""), exception.Message + (exception.InnerException != null ? ". Inner exception: " + exception.InnerException.Message + "***" + exception.InnerException.StackTrace : ""), ServiceLayer.Logging.LogLevel.Error, LogColorCode.Red);
        }
    }
}
