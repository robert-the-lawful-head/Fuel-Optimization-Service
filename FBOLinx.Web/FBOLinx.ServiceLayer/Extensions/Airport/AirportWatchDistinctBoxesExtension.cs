using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport;
using System.Collections.Generic;
using System.Linq;

namespace FBOLinx.ServiceLayer.Extensions.Airport
{
    public static class AirportWatchDistinctBoxesExtension
    {
        public static List<AirportWatchDistinctBoxesDTO> SetAirportDefaultLongLat(this List<AirportWatchDistinctBoxesDTO> airportWatchDistinctBoxes, List<AirportPosition> airports)
        {
            return (from ad in airportWatchDistinctBoxes
                    join a in airports on new { Icao = ad.AirportICAO } equals new { Icao = (string.IsNullOrEmpty(a.Icao) ? a.Faa : a.Icao) }
                    select new
                    {
                        distinctBox = ad,
                        airportPosition = a
                    }).Select(x =>
                    {
                        x.distinctBox.Latitude = (x.distinctBox.Latitude == null) ? x.airportPosition.Latitude : x.distinctBox.Latitude;
                        x.distinctBox.Longitude = (x.distinctBox.Longitude == null) ? x.airportPosition.Longitude : x.distinctBox.Longitude;
                        return x.distinctBox;
                    }).Where(a => a.AirportICAO != null || a.Latitude != null).ToList();
        }
    }
}
