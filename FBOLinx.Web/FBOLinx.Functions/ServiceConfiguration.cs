
using FBOLinx.Functions.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FBOLinx.Functions
{
    public static class ServiceConfiguration
    {
        public static void Configure(IServiceCollection services, string baseAddress, string internalAPIKey)
        {
            //Commented out as we are moving away from giving the functions application DB access
            //services.RegisterDBConnections(degaDbConnectionString, fbolinxDbConnectionString);
            //services.RegisterEntityServices();
            //services.RegisterBusinessServices();
            services.AddHttpClient("FBOLinx", client =>
            {
                client.BaseAddress = new System.Uri(baseAddress);
                client.DefaultRequestHeaders.Add("x-api-key", internalAPIKey);
            });
        }

        public static void ConfigureForLocalSettings(IFunctionsHostBuilder builder)
        {
            var configuration = BuildConfiguration(builder.GetContext().ApplicationRootPath);
            builder.Services.Configure<AzureFunctionsSettings>(configuration.GetSection(nameof(AzureFunctionsSettings)));
            Configure(builder.Services, configuration.GetSection("AzureFunctionsSettings")["WebApplicationUrl"], configuration.GetSection("AzureFunctionsSettings")["InternalAPIKey"]);

        }
        private static IConfiguration BuildConfiguration(string applicationRootPath)
        {
            var config =
                new ConfigurationBuilder()
                    .SetBasePath(applicationRootPath)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

            return config;
        }
    }
}
