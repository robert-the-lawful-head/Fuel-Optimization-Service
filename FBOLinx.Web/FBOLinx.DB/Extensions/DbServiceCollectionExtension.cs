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
            services.AddDbContext<FboLinxContext>(options =>{
                options.UseSqlServer(configuration.GetConnectionString("FboLinxContext"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddDbContext<DegaContext>(options =>{
                options.UseSqlServer(configuration.GetConnectionString("DegaContext"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            });

            services.AddDbContext<FuelerLinxContext>(options =>{
                options.UseSqlServer(configuration.GetConnectionString("FuelerLinxContext"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            });

            services.AddDbContext<FilestorageContext>(options =>{
                options.UseSqlServer(configuration.GetConnectionString("FilestorageContext"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            return services;
        }
    }
}
