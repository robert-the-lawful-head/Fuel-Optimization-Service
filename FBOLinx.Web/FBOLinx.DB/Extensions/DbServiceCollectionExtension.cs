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
                    options.UseSqlServer(degaDbConnectionString);
                    // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

            if (!string.IsNullOrEmpty(fbolinxDbConnectionString))
                services.AddDbContext<FboLinxContext>(options => {
                    options.UseSqlServer(fbolinxDbConnectionString);
                    // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

            if (!string.IsNullOrEmpty(fuelerlinxDbConnectionString))
                services.AddDbContext<FuelerLinxContext>(options => {
                    options.UseSqlServer(fuelerlinxDbConnectionString);
                    // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

            if (!string.IsNullOrEmpty(fileStorageDbConnectionString))
                services.AddDbContext<FilestorageContext>(options => {
                    options.UseSqlServer(fileStorageDbConnectionString);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

            if (!string.IsNullOrEmpty(serviceLogsDbConnectionString))
                services.AddDbContext<ServiceLogsContext>(options => {
                    options.UseSqlServer(serviceLogsDbConnectionString);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

            return services;
        }
    }
}
