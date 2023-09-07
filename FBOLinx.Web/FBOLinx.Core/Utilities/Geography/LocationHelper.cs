using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Itenso.TimePeriod;

namespace FBOLinx.Core.Utilities.Geography
{
    public class LocationHelper
    {
        public static double GetLatitudeGeoLocationFromGPS(string latitudeInGPS)
        {
            //var latDirection = lat.Substring(0, 1);

            //double latitude = double.Parse(lat.Substring(1, 2)) + double.Parse(lat.Substring(4, 2)) / 60 + double.Parse(lat[7..]) / 3600;
            

            //if (latDirection != "N") latitude = -latitude;

            double latitudeResult = 0;

            if (latitudeInGPS.Contains("N") || latitudeInGPS.Contains("S"))
            {
                string[] strlatitude = latitudeInGPS.ToString().Split('-');

                latitudeResult = System.Convert.ToDouble(strlatitude[0].Replace("N", "").Replace("S", "")) + (System.Convert.ToDouble(strlatitude[1]) / 60);
                if (strlatitude[0].Contains("S"))
                    latitudeResult = -latitudeResult;
            }
            else
            {
                latitudeResult = double.Parse(latitudeInGPS);
            }


            return latitudeResult;
        }

        public static double GetLongitudeGeoLocationFromGPS(string longitudeInGPS)
        {
            //var lngDirection = lng.Substring(0, 1);

            //double longitude = lng.Length == 8 ?
            //    double.Parse(lng.Substring(1, 2)) + double.Parse(lng.Substring(4, 2)) / 60 + double.Parse(lng[6..]) / 3600 :
            //    double.Parse(lng.Substring(1, 3)) + double.Parse(lng.Substring(5, 2)) / 60 + double.Parse(lng[7..]) / 3600;


            //if (lngDirection != "E") longitude = -longitude;

            double longitudeResult = 0.0;
            if (longitudeInGPS.Contains("W") || longitudeInGPS.Contains("E"))
            {
                string[] strlongitude = longitudeInGPS.ToString().Split('-');

                longitudeResult = System.Convert.ToDouble(strlongitude[0].Replace("W", "").Replace("E", "")) +
                                  (System.Convert.ToDouble(strlongitude[1]) / 60);
                if (strlongitude[0].Contains("W"))
                    longitudeResult = -longitudeResult;
            }
            else
            {
                longitudeResult = double.Parse(longitudeInGPS);
            }

            return longitudeResult;
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

        public static bool IsPointInPolygon(Geolocation.Coordinate p, Geolocation.Coordinate[] polygon)
        {
            if ((polygon?.Length).GetValueOrDefault() < 3)
            {
                return false;
            }
            double minX = polygon[0].Latitude;
            double maxX = polygon[0].Latitude;
            double minY = polygon[0].Longitude;
            double maxY = polygon[0].Longitude;
            for (int i = 1; i < polygon.Length; i++)
            {
                Geolocation.Coordinate q = polygon[i];
                minX = Math.Min(q.Latitude, minX);
                maxX = Math.Max(q.Latitude, maxX);
                minY = Math.Min(q.Longitude, minY);
                maxY = Math.Max(q.Longitude, maxY);
            }
            if (p.Latitude < minX || p.Latitude > maxX || p.Longitude < minY || p.Longitude > maxY)
            {
                return false;
            }
            // https://wrf.ecse.rpi.edu/Research/Short_Notes/pnpoly.html
            bool inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if ((polygon[i].Longitude > p.Longitude) != (polygon[j].Longitude > p.Longitude) &&
                    p.Latitude < (polygon[j].Latitude - polygon[i].Latitude) * (p.Longitude - polygon[i].Longitude) / (polygon[j].Longitude - polygon[i].Longitude) + polygon[i].Latitude)
                {
                    inside = !inside;
                }
            }
            return inside;
        }

        public static double GetBearingDegreesBetweenTwoPoints(Geolocation.Coordinate movingFromPoint, Geolocation.Coordinate movingToPoint)
        {
            try
            {
                double x = Math.Cos(DegreesToRadians(movingFromPoint.Latitude)) * Math.Sin(DegreesToRadians(movingToPoint.Latitude)) - Math.Sin(DegreesToRadians(movingFromPoint.Latitude)) * Math.Cos(DegreesToRadians(movingToPoint.Latitude)) * Math.Cos(DegreesToRadians(movingToPoint.Longitude - movingFromPoint.Longitude));
                double y = Math.Sin(DegreesToRadians(movingToPoint.Longitude - movingFromPoint.Longitude)) * Math.Cos(DegreesToRadians(movingToPoint.Latitude));

                // Math.Atan2 can return negative value, 0 <= output value < 2*PI expected 
                var radians = (Math.Atan2(y, x) + Math.PI * 2) % (Math.PI * 2);
                var degrees = radians * (180 / Math.PI);
                return degrees;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static double DegreesToRadians(double angle)
        {
            return angle * Math.PI / 180.0d;
        }
    }
}
