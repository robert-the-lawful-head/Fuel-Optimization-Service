using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class PricingUpdateRequest
    {
        public double? Retail { get; set; }
        public double? Cost { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
