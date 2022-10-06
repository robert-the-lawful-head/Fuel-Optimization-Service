using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FBOLinx.Core.Utilities.Geography;
using Geolocation;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport
{
    public class AirportPosition
    {
        public string Icao { get; set; }
        public string Iata { get; set; }
        public string Faa { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? IntlTimeZone { get; set; }
        [Column("DaylightSavingsYN")]
        [StringLength(255)]
        public string DaylightSavingsYn { get; set; }
        
        public string GetProperAirportIdentifier()
        {
                if (!string.IsNullOrEmpty(Icao))
                    return Icao;
                if (!string.IsNullOrEmpty(Faa))
                    return Faa;
                return Iata;
        }

        public double GetDistanceInMilesFromAirportPosition(double otherLatitude, double otherLongitude)
        {
            var distanceFromAirport = new Coordinates(Latitude, Longitude)
                .DistanceTo(
                    new Coordinates(otherLatitude, otherLongitude),
                    UnitOfLength.Miles
                );
            return distanceFromAirport;
        }

        public Geolocation.Coordinate GetFboCoordinate()
        {
            return new Coordinate(Latitude, Longitude);
        }
    }
}
