using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.FboGeofence
{
    public class FboGeofenceClustersViewModel
    {
        public int Oid { get; set; }
        public int AcukwikAirportID { get; set; }
        public int AcukwikFBOHandlerID { get; set; }
        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }
        public string Icao { get; set; }
        public string Fbo { get; set; }
    }
}
