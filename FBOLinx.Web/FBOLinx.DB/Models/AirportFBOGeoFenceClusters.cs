using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
   public class AirportFboGeofenceClusters
    {

        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        public int AcukwikAirportID { get; set; }
        public int AcukwikFBOHandlerID { get; set; }
        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }
        [NotMapped]
        public string Icao { get; set; }
        [NotMapped]
        public string FboName { get; set; }

        #region Relationships
        [InverseProperty("Cluster")]
        public ICollection<AirportFboGeofenceClusterCoordinates> ClusterCoordinatesCollection { get; set; }
        #endregion
    }
}
