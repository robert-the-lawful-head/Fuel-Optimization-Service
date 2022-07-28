using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Auth;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.FboFeesAndTaxesService;
using FBOLinx.ServiceLayer.BusinessServices.FuelPricing;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.Extensions.DependencyInjection;

namespace FBOLinx.ServiceLayer.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddTransient<CustomerService, CustomerService>();
            services.AddTransient<GroupService, GroupService>();
            services.AddTransient<AircraftService, AircraftService>();
            services.AddScoped<FuelerLinxApiService, FuelerLinxApiService>();
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddTransient<IMailTemplateService, MailTemplateService>();
            services.AddTransient<ICustomerAircraftService, CustomerAircraftService>();
            services.AddTransient<IAirportService, AirportService>();
            services.AddTransient<IFuelPriceAdjustmentCleanUpService, FuelPriceAdjustmentCleanUpService>();
            services.AddTransient<IFuelerLinxAccoutSyncingService, FuelerLinxAccoutSyncingService>();
            services.AddTransient<IFuelerLinxAircraftSyncingService, FuelerLinxAircraftSyncingService>();
            services.AddTransient<IPricingTemplateService, PricingTemplateService>();
            services.AddTransient<IPricingTemplateGridService, PricingTemplateService>();
            services.AddTransient<IPricingTemplateAttachmentService, PricingTemplateAttachmentService>();
            services.AddTransient<ICustomerMarginService, CustomerMarginService>();
            services.AddTransient<ICustomCustomerTypeService, CustomCustomerTypeService>();
            services.AddTransient<IPricingTemplateEntityService, PricingTemplateEntityService>();
            services.AddTransient<ICustomerTypesEntityService, CustomerTypesEntityService>();
            services.AddTransient<ICustomerMarginsEntityService, CustomerMarginsEntityService>();
            services.AddTransient<ICustomerAircraftEntityService, CustomerAircraftEntityService>();
            services.AddTransient<ICustomerInfoByGroupEntityService, CustomerInfoByGroupEntityService>();
            services.AddTransient<IFbolinxPricingTemplateAttachmentsEntityService, FbolinxPricingTemplateAttachmentsEntityService>();
            services.AddTransient<IFboFeesAndTaxesService, FboFeesAndTaxesService>();
            services.AddTransient<IFboService, FboService>();
            services.AddTransient<IntegrationUpdatePricingLogService, IntegrationUpdatePricingLogService>();
            services.AddTransient<IFuelReqService, FuelReqService>();

            return services;
        }
    }
}
