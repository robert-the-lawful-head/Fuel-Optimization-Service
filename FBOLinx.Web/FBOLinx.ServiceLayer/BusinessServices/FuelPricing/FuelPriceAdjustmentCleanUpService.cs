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
        private FuelerLinxService _fuelerLinxService;
        private int _fboId;

        public FuelPriceAdjustmentCleanUpService(FuelerLinxService fuelerLinxService)
        {
            _fuelerLinxService = fuelerLinxService;
        }

        public async Task PerformFuelPriceAdjustmentCleanUp(int fboId)
        {
            _fboId = fboId;
            await CleanUpFuelerLinx();
        }

        private async Task CleanUpFuelerLinx()
        {
            await _fuelerLinxService.ClearQuoteCacheForFbo(_fboId);
        }
    }
}
