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

            //just take 10 element for test 
            var responses = await _context.AirportWatchHistoricalData
                          .Where(a => a.AircraftStatus == AirportWatchHistoricalData.AircraftStatusType.Parking && a.AirportICAO == "kvny")
                          .Select(a => new AirportWatchParkingGlobAdressResponse
                          {
                              Lat = a.Latitude,
                              Long = a.Longitude
                              
                          }).Take(10).Skip(10).ToListAsync();

            foreach (var item in responses)
            {
                item.AddOrigin();
            }


            //All these static value are for Test
            var result =  DBSCAN.DBSCAN.CalculateClusters(responses, 1.5, 6);
            int AirpoirtId = _degaContext.AcukwikAirports.FirstOrDefault(a => a.Icao == "kvny").AirportId;
            
            try
            {
                _context.AirportFBOGeoFenceClusters.Add(new AirportFBOGeoFenceClusters
                {
                    AcukwikAirportID = AirpoirtId > 0 ? AirpoirtId : 0,
                });

                _context.SaveChanges();

                int ClusterID = _context.AirportFBOGeoFenceClusters.FirstOrDefault(a => a.AcukwikAirportID == AirpoirtId).OID;

                foreach (var item in result.Clusters)
                {
                    foreach (var item2 in item.Objects)
                    {
                        _context.AirportFBOGeoFenceClusterCoordinates.Add(new AirportFBOGeoFenceClusterCoordinates
                        {
                            ClusterID = ClusterID , 
                            Latitude = float.Parse(item2.Point.X.ToString()),
                            Longitude = float.Parse(item2.Point.Y.ToString())
                        });

                        _context.SaveChanges();
                    }
                }

               
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

      
}
