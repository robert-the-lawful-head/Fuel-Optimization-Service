using System.Text;
using FBOLinx.DB.Extensions;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.BusinessServices.MissedOrderLog;
using FBOLinx.ServiceLayer.BusinessServices.MissedQuoteLog;
using FBOLinx.ServiceLayer.BusinessServices.SWIM;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using FBOLinx.ServiceLayer.Extensions;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.ServiceLayer.ScheduledService;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Extensions;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.TableStorage;
using FBOLinx.TableStorage.EntityServices;
using System.Text.Json.Serialization;

namespace FBOLinx.Web
{
    public static class ServiceConfiguration
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.ConfigureSwagger();
            services.AddMemoryCache();

            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.RegisterDbConnections(configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins("https://*.fbolinx.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                options.AddPolicy("LocalHost", builder => builder.WithOrigins("https://localhost:5001").AllowAnyMethod().AllowAnyHeader());
            }
            );
            
            // configure DI for application services

            services.RegisterConfigurationSections(configuration);
            services.RegisterEntityServices();
            services.RegisterBusinessServices();
            services.RegisterMappingConfiguration();
            

            //Services to be migrated to the ServiceLayer and moved into services.RegisterBusinessServices().
            services.AddScoped<IPriceDistributionService, PriceDistributionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserRoleAttribute>();
            services.AddTransient<GroupFboService, GroupFboService>();
            services.AddTransient<ResetPasswordService, ResetPasswordService>();
            services.AddTransient<EmailContentService, EmailContentService>();
            services.AddTransient<AssociationsService, AssociationsService>();
            services.AddTransient<DBSCANService, DBSCANService>();
            services.AddTransient<IIntegrationStatusService, IntegrationStatusService>();
            services.AddTransient<IGroupEntityService, GroupEntityService>();
            services.AddTransient<IIntegrationStatusEntityService, IntegrationStatusEntityService>();
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Auth services
            services.AddScoped<JwtManager, JwtManager>();
            services.AddScoped<OAuthService, OAuthService>();
            services.AddScoped<IAPIKeyManager, APIKeyManager>();

            //Add file provider
            IFileProvider physicalProvider = new PhysicalFileProvider(System.IO.Directory.GetCurrentDirectory());

            services.AddSingleton<IFileProvider>(physicalProvider);

            //Scheduled service just for testing.  Will remove soon.
            //services.AddHostedService<SWIMPlaceholderSyncFunctionScheduledService>();

            ConfigureAzureTableStorageEntityServices(services, configuration);
        }

        private static void ConfigureAzureTableStorageEntityServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureTableStorageSettings>(configuration.GetSection("AzureTableStorageSettings"));
            
            services.AddTransient<BlobStorageService, BlobStorageService>();
            services.AddTransient<AirportWatchDataTableEntityService, AirportWatchDataTableEntityService>();
        }
    }
}
