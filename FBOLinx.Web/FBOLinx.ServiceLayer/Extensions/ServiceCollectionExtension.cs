using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Auth;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.FuelPricing;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace FBOLinx.ServiceLayer.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<CustomerService, CustomerService>();
            services.AddTransient<GroupService, GroupService>();
            services.AddTransient<AircraftService, AircraftService>();
            services.AddScoped<FuelerLinxApiService, FuelerLinxApiService>();
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddTransient<IMailTemplateService, MailTemplateService>();
            services.AddTransient<CustomerAircraftService, CustomerAircraftService>();
            services.AddTransient<IAirportService, AirportService>();
            services.AddTransient<IFuelPriceAdjustmentCleanUpService, FuelPriceAdjustmentCleanUpService>();
            services.AddTransient<IFuelerLinxAccoutSyncingService, FuelerLinxAccoutSyncingService>();
            services.AddTransient<IFuelerLinxAircraftSyncingService, FuelerLinxAircraftSyncingService>();
            services.AddTransient<IPricingTemplateService, PricingTemplateService>();
            services.AddTransient<ICustomerMarginService, CustomerMarginService>();
            services.AddTransient<ICustomCustomerTypeService, CustomCustomerTypeService>();

            return services;
        }
    }
}
