using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Requests.FuelReq
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
        public string FuelOn { get; set; }
        public string CustomerNotes { get; set; }
        public string PaymentMethod { get; set; }
        public int FboHandlerId { get; set; }
        public string FuelVendor { get; set; }
        public string FlightDepartment { get; set; }
        public string AircraftMakeModel { get; set; }
        public bool DemoMode { get; set; } = true;
        public bool IsCancelled { get; set; } = false;
    }
}
