using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Utilities.Enums;
using StackifyLib;
using StackifyLib.Models;

namespace FBOLinx.ServiceLayer.Logging
{
    public interface ILoggingService
    {
        void LogError(string title, string body, LogLevel level, LogColorCode? color = null);
    }

    public class LoggingService: ILoggingService
    {
        public const string AppName = "fbolinx.web";

        public void LogError(string title, string body, LogLevel level, LogColorCode? color = null)
        {
            LogMsg msg = new LogMsg();
            msg.Ex = StackifyError.New(new ApplicationException(body));
            msg.AppDetails = new LogMsgGroup() { AppName = AppName, Env = Config.Environment };
            msg.Msg = title;
            msg.Level = Enum.GetName(typeof(LogLevel), level);
            if (color != null)
            {
                msg.data = StackifyLib.Utils.HelperFunctions.SerializeDebugData(new { color = EnumHelper.GetDescription(color) }, true);
            }
            Logger.QueueLogObject(msg);
        }
    }
}
