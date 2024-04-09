using System.Collections.Generic;
using System.Linq;
using FBOLinx.Service.Mapping.Dto;
using Geolocation;

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

        public bool AreCoordinatesInFence(double latitude, double longitude)
        {
            return FBOLinx.Core.Utilities.Geography.LocationHelper.IsPointInPolygon(new Coordinate(latitude, longitude),
                ClusterCoordinatesCollection?.Select(x => new Coordinate(x.Latitude, x.Longitude)).ToArray());
        }
    }
}