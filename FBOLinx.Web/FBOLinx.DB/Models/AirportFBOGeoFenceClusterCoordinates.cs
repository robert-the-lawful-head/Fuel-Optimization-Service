using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FBOLinx.DB.Models
{
  public class AirportFboGeofenceClusterCoordinates
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        public int ClusterID { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
