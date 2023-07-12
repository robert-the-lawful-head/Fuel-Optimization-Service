using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Requests.ServiceOrder
{
    public class ServiceOrderRequest
    {
        public DateTime? Eta { get; set; }
        public DateTime? Etd { get; set; }
        public string TimeStandard { get; set; }
        public int? SourceId { get; set; }
        public int? CompanyId { get; set; }
        public string TailNumber { get; set; }
        public string Email { get; set; }
        public int? FboLinxFuelOrderId { get; set; } = 0;
        public string FuelOn { get; set; }
        public string FuelVendor { get; set; }
        public List<string> ServiceNames { get; set; } = new List<string>();
    }
}
