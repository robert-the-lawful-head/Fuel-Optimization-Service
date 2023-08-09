using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.User
{
    public sealed class OrderDetailsByFuelerLinxTransactionIdSpecification : Specification<Models.OrderDetails>
    {
        public OrderDetailsByFuelerLinxTransactionIdSpecification(int fuelerLinxTransactionId) : base(x => x.FuelerLinxTransactionId == fuelerLinxTransactionId)
        {
        }
    }
}
