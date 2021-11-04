using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.Web.Configurations;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Models.Responses.AirportWatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class DBSCANService
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private readonly AircraftService _aircraftService;
        private readonly FboService _fboService;
        private List<AirportWatchLiveData> _LiveDataToUpdate;
        private List<AirportWatchLiveData> _LiveDataToInsert;
        private List<AirportWatchHistoricalData> _HistoricalDataToUpdate;
        private List<AirportWatchHistoricalData> _HistoricalDataToInsert;
        private List<AirportWatchAircraftTailNumber> _TailNumberDataToInsert;
        private FuelerLinxService _fuelerLinxService;
        private IOptions<DemoData> _demoData;

        public DBSCANService(FboLinxContext context, DegaContext degaContext, AircraftService aircraftService, FboService fboService, FuelerLinxService fuelerLinxService, IOptions<DemoData> demoData)
        {
            _demoData = demoData;
            _context = context;
            _degaContext = degaContext;
            _aircraftService = aircraftService;
            _fboService = fboService;
            _fuelerLinxService = fuelerLinxService;
        }


        //get List of Lat & Long of Parked Airports in Kvny 
        public async void GetParkingLocations()
        {


            var responses = await _context.AirportWatchHistoricalData
                          .Where(a => a.AircraftStatus == AirportWatchHistoricalData.AircraftStatusType.Parking && a.AirportICAO == "kvny")
                          .Select(a => new AirportWatchParkingGlobAdressResponse
                          {
                              Lat = a.Latitude,
                              Long = a.Longitude
                          }).ToListAsync();

            //DBSCAN.DBSCAN.CalculateClusters(responses, 1.5, 6);


        }
    }

      
}
