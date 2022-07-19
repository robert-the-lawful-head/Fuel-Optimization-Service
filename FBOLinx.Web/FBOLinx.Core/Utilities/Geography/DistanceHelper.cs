using GeoCoordinatePortable;

namespace FBOLinx.Core.Utilities.Geography
{
    public static class DistanceHelper
    {
        public const double EarthRadiusMiles = 3958.75;

        public static double GetDistanceBetweenTwoPoints(double lat1, double lng1, double lat2, double lng2)
        {
            var coord1 = new GeoCoordinate(lat1, lng1);
            var coord2 = new GeoCoordinate(lat2, lng2);

            return coord1.GetDistanceTo(coord2);
        }
        public static double MetersToMiles(double meters)
        {
            return (meters / 1609.344);
        }
        public static double MetersToNauticalMiles(double meters)
        {
            return (meters * 0.00053995680);
        }
    }
}

