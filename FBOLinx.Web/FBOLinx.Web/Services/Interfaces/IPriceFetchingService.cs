using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.Web.DTO;
using FBOLinx.Web.ViewModels;

namespace FBOLinx.Web.Services.Interfaces
{
    public interface IPriceFetchingService
    {
        Task<List<CustomerWithPricing>> GetCustomerPricingByLocationAsync(
            string icao, int customerId, FlightTypeClassifications flightTypeClassifications,
            ApplicableTaxFlights departureType = ApplicableTaxFlights.All, List<FboFeesAndTaxes> feesAndTaxes = null,
            int fboId = 0);

        Task<List<CustomerWithPricing>> GetCustomerPricingAsync(int fboId, int groupId,
            int customerInfoByGroupId, List<int> pricingTemplateIds,
            FBOLinx.Core.Enums.FlightTypeClassifications flightTypeClassifications,
            ApplicableTaxFlights departureType = ApplicableTaxFlights.All, List<FboFeesAndTaxes> feesAndTaxes = null);

        Task<List<PricingTemplate>> GetAllPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId,
            int groupId, int pricingTemplateId = 0);

        Task<List<PricingTemplate>> GetStandardPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId,
            int groupId, int pricingTemplateId = 0);

        Task<List<PricingTemplate>> GetTailSpecificPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer,
            int fboId, int groupId, int pricingTemplateId = 0);

        Task<List<PricingTemplatesGridViewModel>> GetPricingTemplates(int fboId, int groupId);

        Task<PriceDistributionService.PriceBreakdownDisplayTypes> GetPriceBreakdownDisplayType(int fboId);

        Task<double> GetCurrentPostedRetail(int fboId);
    }
}