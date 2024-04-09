using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Analytics
{
    public class IntraNetworkVisitsReportByAirportItem
    {
        public string Icao { get; set; }
        public int AcukwikFboHandlerId { get; set; }
        public int VisitsToAirport { get; set; }
        public int VisitsToFbo { get; set; }
        public string FboName { get; set; }
    }
}
