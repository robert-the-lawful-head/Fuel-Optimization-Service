using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Auth;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Configurations;
using FBOLinx.Web.Services;
using FBOLinx.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

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

            services.AddDbContext<FboLinxContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("FboLinxContext")));

            services.AddDbContext<DegaContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DegaContext")));

            services.AddDbContext<FuelerLinxContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("FuelerLinxContext")));

            services.AddDbContext<FilestorageContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("FilestorageContext")));

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder.AllowAnyOrigin()
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //            .AllowCredentials());
            //});

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
            services.AddScoped<FuelerLinxService, FuelerLinxService>();
            services.AddScoped<RampFeesService, RampFeesService>();
            services.AddScoped<IPriceDistributionService, PriceDistributionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserRoleAttribute>();
            services.AddScoped<GroupTransitionService, GroupTransitionService>();

            services.AddTransient<GroupFboService, GroupFboService>();
           
            services.AddTransient<CustomerService, CustomerService>();
            services.AddTransient<FboService, FboService>();
            services.AddTransient<GroupService, GroupService>();
            services.AddTransient<IPriceFetchingService, PriceFetchingService>();
            services.AddTransient<IPricingTemplateService, PricingTemplateService>();
            services.AddTransient<ResetPasswordService, ResetPasswordService>();
            services.AddTransient<FbopricesService, FbopricesService>();
            services.AddTransient<EmailContentService, EmailContentService>();
            services.AddTransient <AssociationsService, AssociationsService>();
            services.AddTransient<AirportFboGeofenceClustersService, AirportFboGeofenceClustersService>();
            services.AddTransient<DateTimeService, DateTimeService>();

            //Business Services
            services.AddTransient<AircraftService, AircraftService>();
            services.AddTransient<DBSCANService, DBSCANService>();
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddTransient<IMailTemplateService, MailTemplateService>();
            services.AddTransient<CustomerAircraftService, CustomerAircraftService>();
            services.AddTransient<AirportWatchService, AirportWatchService>();
            services.AddTransient<IMailService, MailService>();

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
