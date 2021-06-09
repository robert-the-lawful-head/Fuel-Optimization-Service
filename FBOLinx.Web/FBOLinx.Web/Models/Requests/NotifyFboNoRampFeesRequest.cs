using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class NotifyFboNoRampFeesRequest : NotifyFboExpiredPricingRequest
    {
        public string CustomerName { get; set; }
        public string ICAO { get; set; }
    }
}
