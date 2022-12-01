using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.PricingTemplate
{
    public class CustomerPricingTemplatesModel
    {
        public int CustomerId { get; set; }
        public int FboId { get; set; }
        public int CustomerType { get; set; }
        public string PricingTemplateName { get; set; }
    }
}
