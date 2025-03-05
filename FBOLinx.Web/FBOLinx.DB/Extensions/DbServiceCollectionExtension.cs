using FBOLinx.DB.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace FBOLinx.DB.Extensions
{
    public static  class DbServiceCollectionExtension
    {
        public static IServiceCollection RegisterDbConnections(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDBConnection<DegaContext>(configuration.GetConnectionString("DegaContext"));
            services.RegisterDBConnection<FboLinxContext>(configuration.GetConnectionString("FboLinxContext"));
            services.RegisterDBConnection<FuelerLinxContext>(configuration.GetConnectionString("FuelerLinxContext"));
            services.RegisterDBConnection<FilestorageContext>(configuration.GetConnectionString("FilestorageContext"));
            services.RegisterDBConnection<ServiceLogsContext>(configuration.GetConnectionString("ServiceLogsContext"));
            services.RegisterDBConnection<FlightDataContext>(configuration.GetConnectionString("FlightDataContext"));

            return services;
        }

        public static IServiceCollection RegisterDBConnection<T>(this IServiceCollection services, string connectionString) where T : DbContext {
            
            if (string.IsNullOrEmpty(connectionString))
                return services;

            services.AddDbContext<T>(options => {
                options.UseSqlServer(connectionString,
                    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 1));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            return services;
        }
    }
}
