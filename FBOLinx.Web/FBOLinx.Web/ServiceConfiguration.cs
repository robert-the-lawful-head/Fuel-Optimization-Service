using System.Text;
using FBOLinx.DB.Extensions;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.Extensions;
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
using System.Threading.Tasks;
using System;

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
                        ValidateIssuer = false, // change to true when we sepparate 3rd party integration apis
                        //ValidIssuer = uri
                        ValidateAudience = false, // evualate implicaitons to 3rd party integrations if set to true.
                        //ValidAudience = fbolinxapi, 
                        ValidateIssuerSigningKey = true,                     
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true,                              
                        ClockSkew = TimeSpan.Zero                           
                    };
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.HttpContext.Request.Cookies["AuthToken"];
                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.RegisterDbConnections(configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins("https://*.fbolinx.com")
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                options.AddPolicy("LocalHost", builder => builder.WithOrigins("https://localhost:*")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());
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
            services.AddTransient<IEmailContentService, EmailContentService>();
            services.AddTransient<AssociationsService, AssociationsService>();
            services.AddTransient<DBSCANService, DBSCANService>();
            services.AddTransient<IIntegrationStatusService, IntegrationStatusService>();
            services.AddTransient<IGroupEntityService, GroupEntityService>();
            services.AddTransient<IIntegrationStatusEntityService, IntegrationStatusEntityService>();
            services.AddTransient<IOrderDetailsEntityService, OrderDetailsEntityService>();
            services.AddTransient<FuelReqPricingTemplateEntityService, FuelReqPricingTemplateEntityService>();
            services.AddTransient<IFuelReqConfirmationEntityService, FuelReqConfirmationEntityService>();
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Auth services
            services.AddScoped<JwtManager, JwtManager>();
            services.AddScoped<OAuthService, OAuthService>();
            services.AddScoped<IAPIKeyManager, APIKeyManager>();

            //Add file provider
            IFileProvider physicalProvider = new PhysicalFileProvider(System.IO.Directory.GetCurrentDirectory());

            services.AddSingleton<IFileProvider>(physicalProvider);

            //Scheduled services just for testing.  Will remove soon.
            //services.AddHostedService<SWIMPlaceholderSyncFunctionScheduledService>();
            //services.AddHostedService<AirportWatchLiveDataReprocessScheduledService>();

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
