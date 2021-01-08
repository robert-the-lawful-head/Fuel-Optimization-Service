using System.Text;
using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Auth;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Configurations;
using FBOLinx.Web.Data;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;

namespace FBOLinx.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
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
                options.UseSqlServer(Configuration.GetConnectionString("FboLinxContext")));

            services.AddDbContext<DegaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DegaContext")));

            services.AddDbContext<FuelerLinxContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("FuelerLinxContext")));

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

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            var appParnterSDKSettings = Configuration.GetSection("AppPartnerSDKSettings");
            services.Configure<AppPartnerSDKSettings>(appParnterSDKSettings);

            // configure DI for application services
            services.AddScoped<FuelerLinxService, FuelerLinxService>();
            services.AddScoped<RampFeesService, RampFeesService>();
            services.AddScoped<PriceDistributionService, PriceDistributionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserRoleAttribute>();
            services.AddScoped<GroupTransitionService, GroupTransitionService>();

            services.AddTransient<GroupFboService, GroupFboService>();
            services.AddTransient<CustomerService, CustomerService>();
            services.AddTransient<FboService, FboService>();
            services.AddTransient<PriceFetchingService, PriceFetchingService>();
            services.AddTransient<ResetPasswordService, ResetPasswordService>();

            //Business Services
            services.AddTransient<AircraftService, AircraftService>();
            services.AddTransient<EncryptionService, EncryptionService>();
            services.AddTransient<MailTemplateService, MailTemplateService>();
            services.AddTransient<CustomerAircraftService, CustomerAircraftService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Auth services
            services.AddScoped<JwtManager, JwtManager>();
            services.AddScoped<OAuthService, OAuthService>();
            services.AddScoped<IAPIKeyManager, APIKeyManager>();

            //Add file provider
            IFileProvider physicalProvider = new PhysicalFileProvider(System.IO.Directory.GetCurrentDirectory());

            services.AddSingleton<IFileProvider>(physicalProvider);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            //app.UseIdentityServer();

            // global cors policy
            if (env.IsDevelopment())
                app.UseCors("LocalHost");
            else
                app.UseCors("CorsPolicy");

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller}/{action=Index}/{id?}");
            //});

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
