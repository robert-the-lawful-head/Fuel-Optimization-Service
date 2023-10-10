using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.OrderDetails
{
    public sealed class OrderDetailsByFuelerLinxTransactionIdFboHandlerIdSpecification : Specification<Models.OrderDetails>
    {
        public OrderDetailsByFuelerLinxTransactionIdFboHandlerIdSpecification(int fuelerLinxTransactionId, int fboHandlerId) : base(x => x.FuelerLinxTransactionId == fuelerLinxTransactionId && x.FboHandlerId == fboHandlerId)
        {
        }
    }
}
