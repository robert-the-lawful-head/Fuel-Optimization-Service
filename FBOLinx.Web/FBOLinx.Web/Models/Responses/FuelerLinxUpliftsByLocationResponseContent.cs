using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class FuelerLinxUpliftsByLocationResponseContent : FuelerLinxResponseContentBase
    {
        public List<FuelerLinxUpliftsByLocationByMonth> TotalOrdersByMonth { get; set; }
        public string ICAO { get; set; } = "";
    }

    #region Objects

    public class FuelerLinxUpliftsByLocationByMonth
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
    }
    #endregion
}
