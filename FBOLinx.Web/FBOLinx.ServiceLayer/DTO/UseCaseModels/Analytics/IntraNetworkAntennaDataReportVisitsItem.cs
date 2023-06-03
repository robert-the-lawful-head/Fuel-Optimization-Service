using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Analytics
{
    public class IntraNetworkAntennaDataReportVisitsItem
    {
        public string Icao { get; set; }
        public int AcuwkikFboHandlerId { get; set; }
        public int VisitsToAirport { get; set; }
        public int VisitsToFbo { get; set; }
    }
}
