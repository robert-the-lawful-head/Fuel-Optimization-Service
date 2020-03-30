using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class FuelerLinxVolumesNearByAirportResponseContent : FuelerLinxResponseContentBase
    {
        public string ICAO { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
