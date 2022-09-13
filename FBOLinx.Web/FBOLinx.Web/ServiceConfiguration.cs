using System.Text;
using FBOLinx.DB.Extensions;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.BusinessServices.MissedOrderLog;
using FBOLinx.ServiceLayer.BusinessServices.MissedQuoteLog;
using FBOLinx.ServiceLayer.BusinessServices.SWIM;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using FBOLinx.ServiceLayer.Extensions;
using FBOLinx.ServiceLayer.Logging;
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
using FBOLinx.ServiceLayer.BusinessServices.FuelPricing;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.BusinessServices.RampFee;
using FBOLinx.ServiceLayer.BusinessServices.Groups;

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
            
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            var appParnterSDKSettings = configuration.GetSection("AppPartnerSDKSettings");
            services.Configure<AppPartnerSDKSettings>(appParnterSDKSettings);
            var demoDataSection = configuration.GetSection("DemoData");
            services.Configure<DemoData>(demoDataSection);
            var demoData = demoDataSection.Get<DemoData>();

            // configure DI for application services

            //Business Services
            services.RegisterServices();

            services.AddScoped<ILoggingService, LoggingService>();

            services.AddScoped<RampFeesService, RampFeesService>();
            services.AddScoped<IPriceDistributionService, PriceDistributionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserRoleAttribute>();
            services.AddScoped<GroupTransitionService, GroupTransitionService>();
            services.AddTransient<GroupFboService, GroupFboService>();
            services.AddTransient<FboPreferencesService, FboPreferencesService>();
            services.AddTransient<IPriceFetchingService, PriceFetchingService>();
            services.AddTransient<ResetPasswordService, ResetPasswordService>();
            services.AddTransient<FbopricesService, FbopricesService>();
            services.AddTransient<EmailContentService, EmailContentService>();
            services.AddTransient<AssociationsService, AssociationsService>();
            services.AddTransient<AirportFboGeofenceClustersService, AirportFboGeofenceClustersService>();
            services.AddTransient<DateTimeService, DateTimeService>();
            services.AddTransient<DBSCANService, DBSCANService>();
            services.AddTransient<AirportWatchService, AirportWatchService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IMissedQuoteLogEntityService, MissedQuoteLogEntityService>();
            services.AddTransient<ISWIMService, SWIMService>();
            services.AddTransient<AirportWatchScheduledService, AirportWatchScheduledService>();
            services.AddTransient<IMissedQuoteLogService, MissedQuoteLogService>();
            services.AddTransient<IFuelReqService, FuelReqService>();
            services.AddTransient<IAirportWatchLiveDataService, AirportWatchLiveDataService>();
            services.AddTransient<ICustomerInfoByGroupService, CustomerInfoByGroupService>();
            services.AddTransient<IMissedOrderLogService, MissedOrderLogService>();
            services.AddTransient<IFboAirportsService, FboAirportsService>();
            
            services.AddHostedService<AirportWatchScheduledService>();



            //Entity Services
            services.AddTransient<CustomerEntityService, CustomerEntityService>();
            services.AddTransient<GroupEntityService, GroupEntityService>();
            services.AddTransient<CustomerInfoByGroupEntityService, CustomerInfoByGroupEntityService>();
            services.AddTransient<CustomerAircraftEntityService, CustomerAircraftEntityService>();
            services.AddTransient<IntegrationUpdatePricingLogEntityService, IntegrationUpdatePricingLogEntityService>();
            services.AddTransient<SWIMFlightLegEntityService, SWIMFlightLegEntityService>();
            services.AddTransient<SWIMFlightLegDataEntityService, SWIMFlightLegDataEntityService>();
            services.AddTransient<AirportWatchLiveDataEntityService, AirportWatchLiveDataEntityService>();
            services.AddTransient<AirportWatchHistoricalDataEntityService, AirportWatchHistoricalDataEntityService>();
            services.AddTransient<AircraftHexTailMappingEntityService, AircraftHexTailMappingEntityService>();
            services.AddTransient<FAAAircraftMakeModelEntityService, FAAAircraftMakeModelEntityService>();
            services.AddTransient<AcukwikAirportEntityService, AcukwikAirportEntityService>();
            services.AddTransient<AircraftEntityService, AircraftEntityService>();
            services.AddTransient<MissedQuoteLogEntityService, MissedQuoteLogEntityService>();
            services.AddTransient<FuelReqEntityService, FuelReqEntityService>();
            services.AddTransient<IFboEntityService, FboEntityService>();
            services.AddTransient<IFboContactsEntityService, FboContactsEntityService>();
            services.AddTransient<IFboAirportsEntityService, FboAirportsEntityService>();
            services.AddTransient<IAcukwikFbohandlerDetailEntityService, AcukwikFbohandlerDetailEntityService>();
            services.AddTransient<IFboAirportsEntityService, FboAirportsEntityService>();
            services.AddTransient<AFSAircraftEntityService, AFSAircraftEntityService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Auth services
            services.AddScoped<JwtManager, JwtManager>();
            services.AddScoped<OAuthService, OAuthService>();
            services.AddScoped<IAPIKeyManager, APIKeyManager>();

            //Add file provider
            IFileProvider physicalProvider = new PhysicalFileProvider(System.IO.Directory.GetCurrentDirectory());

            services.AddSingleton<IFileProvider>(physicalProvider);
        }
    }
}
