using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FBOLinx.Web.Data
{
    public class DatabaseInitializer
    {
        public async void InitializeDataAsync(IServiceProvider serviceProvider)
        {
            var fboLinxContext = serviceProvider.GetRequiredService<FboLinxContext>();

            //Seed the CustomerCompanyTypes with default types
            if (!fboLinxContext.CustomerCompanyTypes.Any())
            {
                fboLinxContext.CustomerCompanyTypes.Add(new CustomerCompanyTypes()
                {
                    AllowMultiplePricingTemplates = true,
                    Name = "Contract Fuel Vendor"
                });

                fboLinxContext.CustomerCompanyTypes.Add(new CustomerCompanyTypes()
                {
                    AllowMultiplePricingTemplates = false,
                    Name = "Base"
                });

                fboLinxContext.CustomerCompanyTypes.Add(new CustomerCompanyTypes()
                {
                    AllowMultiplePricingTemplates = false,
                    Name = "CAA"
                });

                fboLinxContext.CustomerCompanyTypes.Add(new CustomerCompanyTypes()
                {
                    AllowMultiplePricingTemplates = false,
                    Name = "Transient"
                });
                fboLinxContext.SaveChanges();
            }
        }
    }
}
