using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Analytics
{
    public class IntraNetworkVisitsReportItem
    {
        public string GroupName { get; set; }
        public string TailNumber { get; set; }
        public string Company { get; set; }
        public string AircraftType { get; set; }
        public string AircraftTypeCode { get; set; }
        public int CustomerInfoByGroupId { get; set; }
        public List<string> FlightNumbers { get; set; }

        public List<IntraNetworkVisitsReportByAirportItem> VisitsByAirport { get; set; } =
            new List<IntraNetworkVisitsReportByAirportItem>();
    }
}
