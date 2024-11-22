using FBOLinx.ServiceLayer.Logging;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FBOLinx.Web.Controllers
{

    public class FBOLinxControllerBase : ControllerBase
    {
        private readonly ILoggingService _logger;
        public FBOLinxControllerBase(ILoggingService logger)
        {
            _logger = logger;
        }

        protected void HandleException(Exception exception)
        {
            _logger.LogError(exception.Message + (exception.InnerException != null ? ". Inner exception: " + exception.InnerException.Message + "***" + exception.InnerException.StackTrace : ""), exception.Message + (exception.InnerException != null ? ". Inner exception: " + exception.InnerException.Message + "***" + exception.InnerException.StackTrace : ""), ServiceLayer.Logging.LogLevel.Error, LogColorCode.Red);
        }
        protected void LogRetrace(string title, string data, ServiceLayer.Logging.LogLevel loglevel = LogLevel.Info, LogColorCode logColorCode = LogColorCode.Blue)
        {
            _logger.LogError(title,data, loglevel, logColorCode);
        }
    }
}
