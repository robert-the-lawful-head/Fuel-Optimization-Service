using System.Collections.Generic;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class AirportFboGeofenceClustersDto
    {
        public int Oid { get; set; }
        public int AcukwikAirportID { get; set; }
        public int AcukwikFBOHandlerID { get; set; }
        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }
        public string Icao { get; set; }
        public string FboName { get; set; }
        public ICollection<AirportFboGeofenceClusterCoordinatesDto> ClusterCoordinatesCollection { get; set; }
    }
}