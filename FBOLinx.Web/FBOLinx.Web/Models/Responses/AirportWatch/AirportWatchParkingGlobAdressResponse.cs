using DBSCAN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses.AirportWatch
{
    public class AirportWatchParkingGlobAdressResponse : DBSCAN.IPointData
    {
        public AirportWatchParkingGlobAdressResponse()
        {
           

        }

        private static Point origin;
        public double Lat { get; set; }
        public double Long { get; set; }

        public void AddOrigin ()
        {
            origin = new Point(Lat, Long);
        }
        public ref readonly Point Point => ref origin;
    }
}
