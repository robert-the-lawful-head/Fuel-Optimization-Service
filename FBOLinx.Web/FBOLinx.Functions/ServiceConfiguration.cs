using FBOLinx.DB.Context;
using FBOLinx.DB.Extensions;
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
using FBOLinx.ServiceLayer.Extensions;
using FBOLinx.ServiceLayer.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FBOLinx.Functions
{
    public static class ServiceConfiguration
    {
        public static void Configure(IServiceCollection services, string degaDbConnectionString, string fbolinxDbConnectionString)
        {
            services.RegisterDBConnections(degaDbConnectionString, fbolinxDbConnectionString);
            services.RegisterEntityServices();
            services.RegisterBusinessServices();
            services.AddTransient<IGroupCustomersService, GroupCustomersService>();
            
            services.AddTransient<FAAAircraftMakeModelEntityService, FAAAircraftMakeModelEntityService>();
        }
    }
}
