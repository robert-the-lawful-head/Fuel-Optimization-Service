using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.User
{
    public sealed class ServiceOrderByFuelerLinxTransactionIdFboIdSpecification : Specification<Models.ServiceOrder>
    {
        public ServiceOrderByFuelerLinxTransactionIdFboIdSpecification(int fuelerLinxTransactionId, int fboId) : base(x => x.FuelerLinxTransactionId == fuelerLinxTransactionId && x.Fboid == fboId)
        {
            AddInclude(x => x.ServiceOrderItems);
            AddInclude(x => x.CustomerInfoByGroup);
            AddInclude(x => x.CustomerAircraft);
        }
    }
}
