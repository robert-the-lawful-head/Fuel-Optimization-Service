using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;

namespace FBOLinx.ServiceLayer.BusinessServices.FuelPricing
{
    public interface IFuelPriceAdjustmentCleanUpService
    {
        Task PerformFuelPriceAdjustmentCleanUp(int fboId);
    }

    public class FuelPriceAdjustmentCleanUpService : IFuelPriceAdjustmentCleanUpService
    {
        private FuelerLinxApiService _fuelerLinxApiService;
        private int _fboId;

        public FuelPriceAdjustmentCleanUpService(FuelerLinxApiService fuelerLinxApiService)
        {
            _fuelerLinxApiService = fuelerLinxApiService;
        }

        public async Task PerformFuelPriceAdjustmentCleanUp(int fboId)
        {
            _fboId = fboId;
            await CleanUpFuelerLinx();
        }

        private async Task CleanUpFuelerLinx()
        {
            await _fuelerLinxApiService.ClearQuoteCacheForFbo(_fboId);
        }
    }
}
