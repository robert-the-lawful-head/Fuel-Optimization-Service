using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class FuelerLinxUpliftsByLocationResponseContent : FuelerLinxResponseContentBase
    {
        public int TotalOrders { get; set; }
        public string ICAO { get; set; } = "";
    }
}
