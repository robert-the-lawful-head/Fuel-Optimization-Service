using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Analytics
{
    public class IntraNetworkAntennaDataReportItem
    {
        public string TailNumber { get; set; }
        public string Company { get; set; }
        public string FaaRegisteredOwner { get; set; }
        public string AircraftType { get; set; }
        public int CustomerId { get; set; }
        public int CustomerInfoByGroupId { get; set; }

        public List<IntraNetworkAntennaDataReportVisitsItem> AntennaDataReportVisitsCollection { get; set; } =
            new List<IntraNetworkAntennaDataReportVisitsItem>();
    }
}
