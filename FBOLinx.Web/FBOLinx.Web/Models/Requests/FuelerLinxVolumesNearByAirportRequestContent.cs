using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class FuelerLinxVolumesNearByAirportRequestContent
    {
        public string UserServiceKey { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int? DistanceMile { get; set; }
        public string ICAO { get; set; }
    }
}
