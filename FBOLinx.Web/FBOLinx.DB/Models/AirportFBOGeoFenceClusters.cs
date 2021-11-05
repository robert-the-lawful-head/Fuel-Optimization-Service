using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.DB.Models
{
   public class AirportFBOGeoFenceClusters
    {
        public int OID { get; set; }
        public int AcukwikAirportID { get; set; }
        public int AcukwikFBOHandlerID { get; set; }
        public float CenterLatitude { get; set; }
        public float CenterLongitude { get; set; }
    }
}
