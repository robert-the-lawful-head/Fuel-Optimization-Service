using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class AirportFboGeoFenceGridVM
    {
        public int AcukwikAirportId { get; set; }
        public string Icao { get; set; }
        public int FboCount { get; set; }
        public int GeoFenceCount { get; set; }

        public bool NeedsAttention
        {
            get
            {
                return FboCount > GeoFenceCount;
            }
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
