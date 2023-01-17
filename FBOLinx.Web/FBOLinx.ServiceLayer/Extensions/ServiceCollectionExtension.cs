using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.Auth;
using FBOLinx.ServiceLayer.BusinessServices.CompanyPricingLog;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.DateAndTime;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.FboFeesAndTaxesService;
using FBOLinx.ServiceLayer.BusinessServices.FlightWatch;
using FBOLinx.ServiceLayer.BusinessServices.FuelPricing;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.BusinessServices.MissedOrderLog;
using FBOLinx.ServiceLayer.BusinessServices.MissedQuoteLog;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.BusinessServices.RampFee;
using FBOLinx.ServiceLayer.BusinessServices.SWIM;
using FBOLinx.ServiceLayer.BusinessServices.SWIMS;
using FBOLinx.ServiceLayer.BusinessServices.User;
using FBOLinx.ServiceLayer.Demo;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using FBOLinx.ServiceLayer.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FBOLinx.ServiceLayer.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterConfigurationSections(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            var appParnterSDKSettings = configuration.GetSection("AppPartnerSDKSettings");
            services.Configure<AppPartnerSDKSettings>(appParnterSDKSettings);
            var demoDataSection = configuration.GetSection("DemoData");
            services.Configure<DemoData>(demoDataSection);
            var demoData = demoDataSection.Get<DemoData>();

            return services;
        }

        public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<RampFeesService, RampFeesService>();
            services.AddScoped<GroupTransitionService, GroupTransitionService>();
            services.AddScoped<ILoggingService, LoggingService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<AircraftService, AircraftService>();
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
            services.AddTransient<IAirportTimeService, AirportTimeService>();
            services.AddTransient<IGroupEntityService, GroupEntityService>();
            services.AddTransient<IPriceFetchingService, PriceFetchingService>();
            services.AddTransient<IFboPricesService, FbopricesService>();
            services.AddTransient<AirportFboGeofenceClustersService, AirportFboGeofenceClustersService>();
            services.AddTransient<IAirportFboGeofenceClustersService, AirportFboGeofenceClustersService>();
            services.AddTransient<AirportWatchService, AirportWatchService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<ISWIMService, SWIMService>();
            services.AddTransient<IMissedQuoteLogService, MissedQuoteLogService>();
            services.AddTransient<IAirportWatchLiveDataService, AirportWatchLiveDataService>();
            services.AddTransient<IAirportWatchHistoricalDataService, AirportWatchHistoricalDataService>();
            services.AddTransient<ICustomerInfoByGroupService, CustomerInfoByGroupService>();
            services.AddTransient<IMissedOrderLogService, MissedOrderLogService>();
            services.AddTransient<IFboAirportsService, FboAirportsService>();
            services.AddTransient<IAirportWatchDistinctBoxesService, AirportWatchDistinctBoxesService>();
            services.AddTransient<IAirportWatchFlightLegStatusService, AirportWatchFlightLegStatusService>();
            services.AddTransient<IGroupCustomersService, GroupCustomersService>();
            services.AddTransient<ISWIMFlightLegService, SWIMFlightLegService>();
            services.AddTransient<IFlightWatchService, FlightWatchService>();
            services.AddTransient<ICompanyPricingLogService, CompanyPricingLogService>();
            services.AddTransient<DateTimeService, DateTimeService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IFboPreferencesService, FboPreferencesService>();
            services.AddTransient<IDemoFlightWatch, DemoFlightWatch>();

            services.AddScoped<FuelerLinxApiService, FuelerLinxApiService>();

            return services;
        }

        public static IServiceCollection RegisterEntityServices(this IServiceCollection services)
        {
            services.AddTransient<ICustomerAircraftEntityService, CustomerAircraftEntityService>();
            services.AddTransient<ICustomersEntityService, CustomersEntityService>();
            services.AddTransient<GroupEntityService, GroupEntityService>();
            services.AddTransient<CustomerInfoByGroupEntityService, CustomerInfoByGroupEntityService>();
            services.AddTransient<CustomerAircraftEntityService, CustomerAircraftEntityService>();
            services.AddTransient<IntegrationUpdatePricingLogEntityService, IntegrationUpdatePricingLogEntityService>();
            services.AddTransient<SWIMFlightLegEntityService, SWIMFlightLegEntityService>();
            services.AddTransient<SWIMFlightLegDataEntityService, SWIMFlightLegDataEntityService>();
            services.AddTransient<AirportWatchLiveDataEntityService, AirportWatchLiveDataEntityService>();
            services.AddTransient<AirportWatchHistoricalDataEntityService, AirportWatchHistoricalDataEntityService>();
            services.AddTransient<AircraftHexTailMappingEntityService, AircraftHexTailMappingEntityService>();
            services.AddTransient<AcukwikAirportEntityService, AcukwikAirportEntityService>();
            services.AddTransient<AircraftEntityService, AircraftEntityService>();
            services.AddTransient<IMissedQuoteLogEntityService, MissedQuoteLogEntityService>();
            services.AddTransient<FuelReqEntityService, FuelReqEntityService>();
            services.AddTransient<IFboEntityService, FboEntityService>();
            services.AddTransient<IFboContactsEntityService, FboContactsEntityService>();
            services.AddTransient<IFboAirportsEntityService, FboAirportsEntityService>();
            services.AddTransient<IAcukwikFbohandlerDetailEntityService, AcukwikFbohandlerDetailEntityService>();
            services.AddTransient<AFSAircraftEntityService, AFSAircraftEntityService>();
            services.AddTransient<FAAAircraftMakeModelEntityService, FAAAircraftMakeModelEntityService>();
            services.AddTransient<IAirportWatchDistinctBoxesEntityService, AirportWatchDistinctBoxesEntityService>();
            services.AddTransient<IAirportFboGeoFenceClusterEntityService, AirportFboGeoFenceClusterEntityService>();
            services.AddTransient<ICompanyPricingLogEntityService, CompanyPricingLogEntityService>();
            services.AddTransient<SWIMFlightLegDataErrorEntityService, SWIMFlightLegDataErrorEntityService>();
            services.AddTransient<SWIMUnrecognizedFlightLegEntityService, SWIMUnrecognizedFlightLegEntityService>();
            services.AddTransient<IFboPricesEntityService, FboPricesEntityService>();
            services.AddTransient<IFboPreferencesEntityService, FboPreferencesEntityService>();
            services.AddTransient<TableStorageLogEntityService, TableStorageLogEntityService>();

            return services;
        }
    }
}
