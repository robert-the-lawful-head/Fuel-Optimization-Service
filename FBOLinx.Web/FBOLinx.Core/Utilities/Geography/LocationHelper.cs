using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.Core.Utilities.Geography
{
    public class LocationHelper
    {
        public static double GetLatitudeGeoLocationFromGPS(string lat)
        {
            var latDirection = lat.Substring(0, 1);

            double latitude = double.Parse(lat.Substring(1, 2)) + double.Parse(lat.Substring(4, 2)) / 60 + double.Parse(lat[7..]) / 3600;

            if (latDirection != "N") latitude = -latitude;

            return latitude;
        }

        public static double GetLongitudeGeoLocationFromGPS(string lng)
        {
            var lngDirection = lng.Substring(0, 1);
            
            double longitude = lng.Length == 8 ?
                double.Parse(lng.Substring(1, 2)) + double.Parse(lng.Substring(4, 2)) / 60 + double.Parse(lng[6..]) / 3600 :
                double.Parse(lng.Substring(1, 3)) + double.Parse(lng.Substring(5, 2)) / 60 + double.Parse(lng[7..]) / 3600;

            
            if (lngDirection != "E") longitude = -longitude;

            return longitude;
        }
    }
}
