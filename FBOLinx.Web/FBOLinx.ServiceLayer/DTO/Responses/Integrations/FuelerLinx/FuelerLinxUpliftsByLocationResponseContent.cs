using System.Collections.Generic;

namespace FBOLinx.ServiceLayer.DTO.Responses.Integrations.FuelerLinx
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
