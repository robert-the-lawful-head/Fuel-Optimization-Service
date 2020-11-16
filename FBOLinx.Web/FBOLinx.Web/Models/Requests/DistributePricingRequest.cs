using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class DistributePricingRequest
    {
        public Models.PricingTemplate PricingTemplate { get; set; }
        public Models.CustomerInfoByGroup Customer { get; set; }
        public int CustomerCompanyType { get; set; }
        public EmailContent EmailContentGreeting { get; set; }
        public EmailContent EmailContentSignature { get; set; }
        public int FboId { get; set; }
        public int GroupId { get; set; }

        public string PreviewEmail { get; set; }
    }
}
