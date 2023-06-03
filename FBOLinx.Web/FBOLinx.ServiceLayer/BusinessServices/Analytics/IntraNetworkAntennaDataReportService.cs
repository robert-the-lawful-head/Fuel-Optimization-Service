using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.Groups;

namespace FBOLinx.ServiceLayer.BusinessServices.Analytics
{
    public class IntraNetworkAntennaDataReportService
    {
        private IAirportWatchHistoricalDataService _AirportWatchHistoricalDataService;
        private IFboService _FboService;
        private IAirportTimeService _AirportTimeService;

        public IntraNetworkAntennaDataReportService(IAirportWatchHistoricalDataService airportWatchHistoricalDataService,
            IFboService fboService,
            IAirportTimeService airportTimeService)
        {
            _AirportTimeService = airportTimeService;
            _FboService = fboService;
            _AirportWatchHistoricalDataService = airportWatchHistoricalDataService;
        }

        public async Task GenerateReportForNetwork(int groupId, DateTime startDateTimeUtc, DateTime endDateTimeUtc)
        {
            var fbos = await _FboService.GetFbosByGroupId(groupId);
            var airports = fbos.Where(x => x.FboAirport != null && !string.IsNullOrEmpty(x.FboAirport.Icao))
                .Select(x => x.FboAirport?.Icao)
                .Distinct()
                .ToList();
            var historicalData = await _AirportWatchHistoricalDataService.GetHistoricalData(startDateTimeUtc, endDateTimeUtc, airports, null, null);

        }
    }
}
