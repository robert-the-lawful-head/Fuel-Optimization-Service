using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
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
        private IAirportService _AirportService;

        public FuelPriceAdjustmentCleanUpService(FuelerLinxApiService fuelerLinxApiService, IAirportService airportService)
        {
            _AirportService = airportService;
            _fuelerLinxApiService = fuelerLinxApiService;
        }

        public async Task PerformFuelPriceAdjustmentCleanUp(int fboId)
        {
            _fboId = fboId;
            await CleanUpFuelerLinx();
        }

        private async Task CleanUpFuelerLinx()
        {
            var fboAirport = await _AirportService.GetAirportForFboId(_fboId);
            await _fuelerLinxApiService.ClearQuoteCacheForFbo(fboAirport?.Icao);
        }
    }
}
