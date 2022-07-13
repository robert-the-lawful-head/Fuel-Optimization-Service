using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StackifyLib;
using StackifyLib.Models;

namespace FBOLinx.ServiceLayer.Logging
{
    public class LoggingService
    {
        public const string AppName = "FBOLinx";

        public async Task LogMessage(string message, string stackTrace, string level)
        {
            LogMsg msg = new LogMsg();
            msg.Ex = StackifyError.New(new ApplicationException(message));
            msg.AppDetails = new LogMsgGroup() { AppName = AppName, Env = Config.Environment };
            msg.data = StackifyLib.Utils.HelperFunctions.SerializeDebugData(new { color = "red" }, true);
            msg.Msg = "My log message";
            msg.Level = "ERROR";
            Logger.QueueLogObject(msg);
        }
    }
}
