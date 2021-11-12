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
     
        public DBSCANService(FboLinxContext context, DegaContext degaContext)
        {
           
            _context = context;
            _degaContext = degaContext;
          
        }


        //get List of Lat & Long of Parked Airports in Kvny 
        public async Task GetParkingLocations()
        {
            //just take 10 element for test 
            var responses = await _context.AirportWatchHistoricalData
                          .Where(a => a.AircraftStatus == AirportWatchHistoricalData.AircraftStatusType.Parking && a.AirportICAO == "KVNY")
                          .Select(a => new AirportWatchParkingGlobAdressResponse
                          {
                              Lat = a.Latitude,
                              Long = a.Longitude
                              
                          }).Take(10).ToListAsync();

            foreach (var item in responses)
            {
                item.AddOrigin();
            }


            //All these static value are for Test
            var result =  DBSCAN.DBSCAN.CalculateClusters(responses, 1.5, 6);
         
            
            try
            {
                var AirpoirtId = await _degaContext.AcukwikAirports.FirstOrDefaultAsync(a => a.Icao == "KVNY");
                double totalLat = 0, totalLong = 0;

                //Calc Center of Cluster 
                foreach (var item in result.Clusters)
                {
                    foreach (var item2 in item.Objects)
                    {
                        totalLat += item2.Lat;
                        totalLong += item2.Long;
                    }


                    _context.AirportFBOGeoFenceClusters.Add(new AirportFBOGeoFenceClusters
                    {
                        AcukwikAirportID = AirpoirtId != null ? AirpoirtId.AirportId : 0,
                        CenterLatitude = float.Parse((totalLat / item.Objects.Count).ToString()) , 
                        CenterLongitude  = float.Parse((totalLong / item.Objects.Count).ToString())

                    });
                    await _context.SaveChangesAsync();
                }             

                var ClusterID = await _context.AirportFBOGeoFenceClusters.FirstOrDefaultAsync(a => a.AcukwikAirportID == AirpoirtId.AirportId);

                foreach (var item in result.Clusters)
                {
                    foreach (var item2 in item.Objects)
                    {
                        _context.AirportFBOGeoFenceClusterCoordinates.Add(new AirportFBOGeoFenceClusterCoordinates
                        {
                            ClusterID = ClusterID.OID , 
                            Latitude = float.Parse(item2.Point.X.ToString()),
                            Longitude = float.Parse(item2.Point.Y.ToString())
                        });

                       await _context.SaveChangesAsync();
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
