using FBOLinx.ServiceLayer.Logging;

namespace FBOLinx.Web.DTO
{
    public class AppLogs
    {
        public string Title { get; set; }
        public string Data { get; set; }
        public LogColorCode logColorCode { get; set; } = LogColorCode.Blue;
        public LogLevel logLevel { get; set; } = LogLevel.Info;
    }
}
