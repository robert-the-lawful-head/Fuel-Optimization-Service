using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;

namespace FBOLinx.ServiceLayer.BusinessServices.FuelPricing
{
    public interface IPriceFetchingService
    {

        public Task<List<CustomerWithPricing>> GetCustomerPricingByLocationAsync(
            string icao, int customerId, FlightTypeClassifications flightTypeClassifications,
            ApplicableTaxFlights departureType = ApplicableTaxFlights.All, List<FboFeesAndTaxes> feesAndTaxes = null,
            int fboId = 0);

        public Task<List<CustomerWithPricing>> GetCustomerPricingByLocationAsync(
            string icao, int customerId, FlightTypeClassifications flightTypeClassifications,
            ApplicableTaxFlights departureType = ApplicableTaxFlights.All, List<FboFeesAndTaxes> feesAndTaxes = null, int fboId = 0, int groupId = 0, bool isAnalytics = false);

        public Task<List<CustomerWithPricing>> GetCustomerPricingAsync(int fboId, int groupId,
            int customerInfoByGroupId, List<int> pricingTemplateIds,
            FBOLinx.Core.Enums.FlightTypeClassifications flightTypeClassifications,
            ApplicableTaxFlights departureType = ApplicableTaxFlights.All, List<FboFeesAndTaxes> feesAndTaxes = null);

        public Task<PriceBreakdownDisplayTypes> GetPriceBreakdownDisplayType(int fboId);

        public Task<double> GetCurrentPostedRetail(int fboId);

        public Task<List<FbosGridViewModel>> GetAllFbosWithExpiredPricing();

        public Task NotifyFboExpiredPrices(List<string> toEmails, string fbo);
    }
}