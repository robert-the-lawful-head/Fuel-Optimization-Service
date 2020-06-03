using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class CustomCustomerTypeUpdateRequest
    {
        public int FboId { get; set; }
        public int CustomerId { get; set; }
        public int PricingTemplateId { get; set; }
    }
}
