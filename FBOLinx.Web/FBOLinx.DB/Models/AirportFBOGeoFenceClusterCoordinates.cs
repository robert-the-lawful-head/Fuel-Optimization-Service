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
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [NotMapped]
        public List<double> LongitudeLatitudeAsList
        {
            get
            {
                var result = new List<double>();
                result.Add(Longitude);
                result.Add(Latitude);
                return result;
            }
        }

        #region Relationships
        [ForeignKey("ClusterID")]
        [InverseProperty("ClusterCoordinatesCollection")]
        public AirportFboGeofenceClusters Cluster { get; set; }
        #endregion
    }
}
