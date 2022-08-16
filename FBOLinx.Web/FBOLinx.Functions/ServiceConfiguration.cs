using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.FuelPricing;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.BusinessServices.MissedOrderLog;
using FBOLinx.ServiceLayer.BusinessServices.MissedQuoteLog;
using FBOLinx.ServiceLayer.BusinessServices.RampFee;
using FBOLinx.ServiceLayer.BusinessServices.SWIM;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using FBOLinx.ServiceLayer.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FBOLinx.Functions
{
    public static class ServiceConfiguration
    {
        public static void Configure(IServiceCollection services, string degaDbConnectionString, string fbolinxDbConnectionString)
        {
            services.AddDbContext<DegaContext>(options => {
                options.UseSqlServer(degaDbConnectionString);
            });
            services.AddDbContext<FboLinxContext>(options => {
                options.UseSqlServer(fbolinxDbConnectionString);
            });

            // services.AddTransient<SWIMFlightLegEntityService, SWIMFlightLegEntityService>();
            // services.AddTransient<SWIMFlightLegDataEntityService, SWIMFlightLegDataEntityService>();
            // services.AddTransient<AirportWatchLiveDataEntityService, AirportWatchLiveDataEntityService>();
            // services.AddTransient<AircraftHexTailMappingEntityService, AircraftHexTailMappingEntityService>();
            // services.AddTransient<AirportWatchHistoricalDataEntityService, AirportWatchHistoricalDataEntityService>();
            // services.AddTransient<AcukwikAirportEntityService, AcukwikAirportEntityService>();
            // services.AddTransient<ICustomerAircraftEntityService, CustomerAircraftEntityService>();
            // services.AddTransient<AircraftEntityService, AircraftEntityService>();
            // services.AddTransient<ILoggingService, LoggingService>();
            // services.AddTransient<AirportWatchService, AirportWatchService>();
            // services.AddTransient<ISWIMService, SWIMService>();

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

            services.AddScoped<ILoggingService, LoggingService>();

            services.AddScoped<RampFeesService, RampFeesService>();
            services.AddScoped<GroupTransitionService, GroupTransitionService>();
            services.AddTransient<IPriceFetchingService, PriceFetchingService>();
            services.AddTransient<FbopricesService, FbopricesService>();
            services.AddTransient<AirportFboGeofenceClustersService, AirportFboGeofenceClustersService>();
            services.AddTransient<AirportWatchService, AirportWatchService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IMissedQuoteLogEntityService, MissedQuoteLogEntityService>();
            services.AddTransient<ISWIMService, SWIMService>();
            services.AddTransient<IMissedQuoteLogService, MissedQuoteLogService>();
            services.AddTransient<IFuelReqService, FuelReqService>();
            services.AddTransient<IAirportWatchLiveDataService, AirportWatchLiveDataService>();
            services.AddTransient<ICustomerInfoByGroupService, CustomerInfoByGroupService>();
            services.AddTransient<IMissedOrderLogService, MissedOrderLogService>();
            services.AddTransient<IFboAirportsService, FboAirportsService>();

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
        }
    }
}
