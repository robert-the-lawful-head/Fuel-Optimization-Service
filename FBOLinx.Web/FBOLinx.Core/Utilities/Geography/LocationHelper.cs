using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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

        public static double ConvertDMSToDEG(string dms)
        {
            string[] dms_Array = Regex.Split(dms, @"[^\d]+");
            var degrees = double.Parse(dms_Array[1]);
            var minutes = double.Parse(dms_Array[2]);
            var seconds = double.Parse(dms_Array[3]);
            var direction = dms[0];

            var deg = degrees + (minutes / 60) + (seconds / 3600);

            if (direction == 'S' || direction == 'W')
                deg = deg * -1;

            return deg;
        }
    }
}
