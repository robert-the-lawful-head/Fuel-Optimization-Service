using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.DTO;

namespace FBOLinx.Web.Models.Responses
{
    public class PriceLookupResponse
    {
        public string Template { get; set; }
        public string Company { get; set; }
        public string MakeModel { get; set; }
        public List<CustomerWithPricing> PricingList { get; set; }
        public RampFees RampFee { get; set; }
    }
}
