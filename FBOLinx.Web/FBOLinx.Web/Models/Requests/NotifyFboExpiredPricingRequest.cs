using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class NotifyFboExpiredPricingRequest
    {
        public List<string> ToEmails { get; set; }
        public string FBO { get; set; }
    }
}
