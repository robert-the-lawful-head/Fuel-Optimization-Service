using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using FBOLinx.Functions;
using System.Diagnostics;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FBOLinx.Functions
{
    public class Startup : FunctionsStartup
    {
        private bool _UseLocalSettings;
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            CheckForLocalSettingUsage();
            if (_UseLocalSettings)
            {
                ServiceConfiguration.ConfigureForLocalSettings(builder, GetSqlConnectionString("DegaDB"), GetSqlConnectionString("ParagonTestDB"));
            } 
            else
            {
                ServiceConfiguration.Configure(builder.Services, GetSqlConnectionString("DegaDB"), GetSqlConnectionString("ParagonTestDB"), GetApplicationSetting("WebApplicationUrl"), GetApplicationSetting("InternalAPIKey"));  
            }

        }

        private static string GetSqlConnectionString(string name)
        {
            string conStr = System.Environment.GetEnvironmentVariable($"SQLCONNSTR_{name}", EnvironmentVariableTarget.Process); // Azure Functions App Service naming convention
            if (string.IsNullOrEmpty(conStr))
                conStr = System.Environment.GetEnvironmentVariable($"ConnectionStrings:{name}", EnvironmentVariableTarget.Process);
            return conStr;
        }

        private static string GetCustomConnectionString(string name)
        {
            string conStr = System.Environment.GetEnvironmentVariable($"CUSTOMCONNSTR_{name}", EnvironmentVariableTarget.Process); // Azure Functions App Service naming convention
            if (string.IsNullOrEmpty(conStr))
                conStr = System.Environment.GetEnvironmentVariable($"ConnectionStrings:{name}", EnvironmentVariableTarget.Process);
            return conStr;
        }

        private static string GetApplicationSetting(string name)
        {
            string conStr = System.Environment.GetEnvironmentVariable($"AzureFunctionsSettings_{name}", EnvironmentVariableTarget.Process); // Azure Functions App Service naming convention
            if (string.IsNullOrEmpty(conStr))
                conStr = System.Environment.GetEnvironmentVariable($"AzureFunctionsSettings:{name}", EnvironmentVariableTarget.Process);
            return conStr;
        }

        [Conditional("DEBUG")]
        private void CheckForLocalSettingUsage()
        {
            _UseLocalSettings = true;
        }
    }
}

