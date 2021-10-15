using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class CustomerAircraftsWithTemplateRequest
    {
        public string TailNumber { get; set; }
        public int? PricingTemplateId { get; set; }
        public AirCrafts.AircraftSizes? Size { get; set; }
        public int AircraftId { get; set; }
    }
}
