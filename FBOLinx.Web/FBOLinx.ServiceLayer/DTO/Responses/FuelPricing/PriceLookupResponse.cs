using System.Collections.Generic;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.DTO.Responses.FuelPricing
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
