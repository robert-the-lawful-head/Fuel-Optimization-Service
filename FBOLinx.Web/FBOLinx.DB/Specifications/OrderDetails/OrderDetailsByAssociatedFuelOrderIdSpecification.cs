using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.OrderDetails
{
    public sealed class OrderDetailsByAssociatedFuelOrderIdSpecification : Specification<Models.OrderDetails>
    {
        public OrderDetailsByAssociatedFuelOrderIdSpecification(int associatedFuelOrderId) : base(x => x.AssociatedFuelOrderId == associatedFuelOrderId)
        {
        }
    }
}
