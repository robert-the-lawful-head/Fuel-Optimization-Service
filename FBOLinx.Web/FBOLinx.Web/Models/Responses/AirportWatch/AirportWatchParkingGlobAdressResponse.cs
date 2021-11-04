using DBSCAN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses.AirportWatch
{
    public class AirportWatchParkingGlobAdressResponse : DBSCAN.IPointData
    {
        public double Lat { get; set; }
        public double Long { get; set; }


      
        public ref readonly Point Point => throw new NotImplementedException();
    }
}
