using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.Dto.Requests
{
    public class PrincingTemplateRequest
    {
        public int? currentPricingTemplateId { get; set; }
        public string name { get; set; }
    }
}
