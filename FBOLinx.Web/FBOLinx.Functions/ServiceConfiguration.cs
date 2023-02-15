using FBOLinx.DB.Extensions;
using FBOLinx.ServiceLayer.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;

namespace FBOLinx.Functions
{
    public static class ServiceConfiguration
    {
        public static void Configure(IServiceCollection services, string degaDbConnectionString, string fbolinxDbConnectionString, string baseAddress, string internalAPIKey)
        {
            services.RegisterDBConnections(degaDbConnectionString, fbolinxDbConnectionString);
            services.RegisterEntityServices();
            services.RegisterBusinessServices();
            services.AddHttpClient("FBOLinx", client =>
            {
                client.BaseAddress = new System.Uri(baseAddress);
                client.DefaultRequestHeaders.Add("x-api-key", internalAPIKey);
            });
        }

        public static void ConfigureForLocalSettings(IFunctionsHostBuilder builder, string degaDbConnectionString, string fbolinxDbConnectionString)
        {
            var configuration = BuildConfiguration(builder.GetContext().ApplicationRootPath);
            builder.Services.Configure<AzureFunctionsSettings>(configuration.GetSection(nameof(AzureFunctionsSettings)));
            Configure(builder.Services, degaDbConnectionString, fbolinxDbConnectionString, configuration.GetSection("AzureFunctionsSettings")["WebApplicationUrl"], configuration.GetSection("AzureFunctionsSettings")["InternalAPIKey"]);

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
