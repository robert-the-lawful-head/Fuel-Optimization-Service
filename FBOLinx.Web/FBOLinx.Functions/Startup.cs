using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using FBOLinx.Functions;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FBOLinx.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            ServiceConfiguration.Configure(builder.Services, GetSqlConnectionString("DegaContext"), GetSqlConnectionString("FboLinxContext"));
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
    }
}

