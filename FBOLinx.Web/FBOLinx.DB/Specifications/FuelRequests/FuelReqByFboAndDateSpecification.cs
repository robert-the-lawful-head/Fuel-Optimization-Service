using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.FuelRequests
{
    public sealed class FuelReqByFboAndDateSpecification : Specification<DB.Models.FuelReq>
    {
        public FuelReqByFboAndDateSpecification(int fboId, DateTime startDateTime, DateTime endDateTime) : base(x => x.Fboid.HasValue && x.Fboid.Value == fboId
                                                                                                                    && x.Eta >= startDateTime && x.Eta <= endDateTime)
        {
            AddInclude(x => x.CustomerAircraft);
            AddInclude(x => x.FuelReqPricingTemplate);
            AddInclude(x => x.Fbo);
            AddInclude(x => x.ServiceOrder);
        }
    }
}
