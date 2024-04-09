using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;

namespace FBOLinx.Web.Models.Requests
{
    public class CustomerAircraftsWithTemplateRequest
    {
        public string TailNumber { get; set; }
        public int? PricingTemplateId { get; set; }
        public AircraftSizes? Size { get; set; }
        public int AircraftId { get; set; }
    }
}
