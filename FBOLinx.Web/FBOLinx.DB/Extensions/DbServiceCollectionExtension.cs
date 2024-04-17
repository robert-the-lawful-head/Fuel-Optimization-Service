using FBOLinx.DB.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FBOLinx.DB.Extensions
{
    public static  class DbServiceCollectionExtension
    {
        public static IServiceCollection RegisterDbConnections(this IServiceCollection services, IConfiguration configuration)
        {
            return services.RegisterDBConnections(configuration.GetConnectionString("DegaContext"),
                configuration.GetConnectionString("FboLinxContext"),
                configuration.GetConnectionString("FuelerLinxContext"),
                configuration.GetConnectionString("FilestorageContext"),
                configuration.GetConnectionString("ServiceLogsContext"));
        }

        public static IServiceCollection RegisterDBConnections(this IServiceCollection services,
            string degaDbConnectionString = "", string fbolinxDbConnectionString = "",
            string fuelerlinxDbConnectionString = "", string fileStorageDbConnectionString = "", string serviceLogsDbConnectionString = "")
        {
            if (!string.IsNullOrEmpty(degaDbConnectionString))
                services.AddDbContext<DegaContext>(options => {
                    options.UseSqlServer(degaDbConnectionString,
                        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 1));
                });

            if (!string.IsNullOrEmpty(fbolinxDbConnectionString))
                services.AddDbContext<FboLinxContext>(options => {
                    options.UseSqlServer(fbolinxDbConnectionString,
                        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 1));
                });

            if (!string.IsNullOrEmpty(fuelerlinxDbConnectionString))
                services.AddDbContext<FuelerLinxContext>(options => {
                    options.UseSqlServer(fuelerlinxDbConnectionString,
                        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 1));
                });

            if (!string.IsNullOrEmpty(fileStorageDbConnectionString))
                services.AddDbContext<FilestorageContext>(options => {
                    options.UseSqlServer(fileStorageDbConnectionString,
                        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 1));
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

            if (!string.IsNullOrEmpty(serviceLogsDbConnectionString))
                services.AddDbContext<ServiceLogsContext>(options => {
                    options.UseSqlServer(serviceLogsDbConnectionString,
                        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 1));
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

            return services;
        }
    }
}
