using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class FuelerLinxVolumesNearByAirportResponse : FuelerLinxResponseContentBase
    {
        public List<FuelerLinxVolumesNearByAirportResponseContent> d { get; set; }
    }
}
