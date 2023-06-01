using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.ServiceOrder
{
    public sealed class ServiceOrderByFboSpecification : Specification<Models.ServiceOrder>
    {
        public ServiceOrderByFboSpecification(int fboId, DateTime? startDateTimeUtc, DateTime? endDateTimUtce) : base(x => x.Fboid == fboId 
            && (!startDateTimeUtc.HasValue || x.ArrivalDateTimeUtc >= startDateTimeUtc) 
            && (!endDateTimUtce.HasValue || x.ArrivalDateTimeUtc <= endDateTimUtce))
        {
            AddInclude(x => x.ServiceOrderItems);
            AddInclude(x => x.CustomerInfoByGroup);
            AddInclude(x => x.CustomerAircraft);
        }
    }
}
