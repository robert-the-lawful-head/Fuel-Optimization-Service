using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Models;

namespace FBOLinx.Web.Models.Requests
{
    public class DistributePricingRequest
    {
        public PricingTemplate PricingTemplate { get; set; }
        public CustomerInfoByGroup Customer { get; set; }
        public int CustomerCompanyType { get; set; }
        public EmailContent EmailContentGreeting { get; set; }
        public EmailContent EmailContentSignature { get; set; }
        public int FboId { get; set; }
        public int GroupId { get; set; }

        public string PreviewEmail { get; set; }
    }
}
