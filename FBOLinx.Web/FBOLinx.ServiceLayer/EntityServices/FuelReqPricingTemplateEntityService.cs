using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class FuelReqPricingTemplateEntityService : Repository<FuelReqPricingTemplate, FboLinxContext>
    {
        public FuelReqPricingTemplateEntityService(FboLinxContext context) : base(context)
        {

        }
    }
}
