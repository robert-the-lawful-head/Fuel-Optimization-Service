using System.Collections.Generic;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class AirportFboGeofenceClusterCoordinatesDto
    {
        public int Oid { get; set; }
        public int ClusterID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<double> LongitudeLatitudeAsList { get; set; }
        public AirportFboGeofenceClustersDto Cluster { get; set; }
    }
}