using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Requests
{
    public class PricingUpdateRequest
    {
        public double? Retail { get; set; }
        public double? Cost { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
