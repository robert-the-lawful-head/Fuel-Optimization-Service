using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FBOLinx.DB.Models
{
  public class AirportFBOGeoFenceClusterCoordinates
    {
        [Key]
        public int OID { get; set; }
        public int ClusterID { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
