using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.PricingTemplate
{
    public class CustomerAircraftsPricingTemplatesModel
    {
        public int Oid { get; set; }
        public string Name { get; set; }
        public int CustomerAircraftId { get; set; }
    }
}
