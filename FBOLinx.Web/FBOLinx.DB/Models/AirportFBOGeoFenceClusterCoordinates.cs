using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.DB.Models
{
  public class AirportFBOGeoFenceClusterCoordinates
    {
        public int OID { get; set; }
        public int ClusterID { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
