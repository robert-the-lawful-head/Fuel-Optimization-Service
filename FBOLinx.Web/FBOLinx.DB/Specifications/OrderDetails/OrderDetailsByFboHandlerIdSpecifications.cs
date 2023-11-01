using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.OrderDetails
{
    public sealed class OrderDetailsByFboHandlerIdSpecifications : Specification<Models.OrderDetails>
    {
        public OrderDetailsByFboHandlerIdSpecifications(int fboHandlerId) : base(x => x.FboHandlerId == fboHandlerId)
        {
        }
    }
}
