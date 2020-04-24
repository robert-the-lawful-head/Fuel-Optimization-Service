using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class FuelReqRequest
    {
        public string Icao { get; set; }
        public DateTime? Eta { get; set; }
        public DateTime? Etd { get; set; }
        public string Notes { get; set; }
        public double? FuelEstCost { get; set; }
        public double? FuelEstWeight { get; set; }
        public string TimeStandard { get; set; }
        public int? SourceId { get; set; }
        public int? CompanyId { get; set; }
        public string TailNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
