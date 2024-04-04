using FBOLinx.Core.Utilities.Geography;
using FBOLinx.ServiceLayer.DTO;
using Fuelerlinx.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.Extensions.Airport
{
    public static class AirportWatchLivedataExtenstion
    {
        public static void FilterNearestAntennaBox(this List<AirportWatchLiveDataDto> data, List<AirportWatchDistinctBoxesDTO> airportWatchDistinctBoxes)
        {
            var dataWithDistanceFromBoxOrderedByKm = (from d in data
                                                      join b in airportWatchDistinctBoxes on d.BoxName equals b.BoxName
                                                      select new
                                                      {
                                                          d.BoxName,
                                                          d.AircraftHexCode,
                                                          distanceFromBox = DistanceHelper.GetDistanceBetweenTwoPoints(d.Latitude, d.Longitude, (double)b.Latitude, (double)b.Longitude)
                                                      }).GroupBy(item => item.AircraftHexCode).ToList();
 
            foreach (var group in dataWithDistanceFromBoxOrderedByKm)
            {
                var nearestBox = group.OrderBy(x => x.distanceFromBox).First();
                data.RemoveAll(x => x.AircraftHexCode == group.Key && x.BoxName != nearestBox.BoxName);
            }
        }
    }
}
