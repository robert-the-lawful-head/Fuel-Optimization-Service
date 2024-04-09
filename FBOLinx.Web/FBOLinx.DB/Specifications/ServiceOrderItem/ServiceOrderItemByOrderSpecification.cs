using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.ServiceOrderItem
{
    public class ServiceOrderItemByOrderSpecification : Specification<Models.ServiceOrderItem>
    {
        public ServiceOrderItemByOrderSpecification(int serviceOrderId) : base(x => x.ServiceOrderId == serviceOrderId)
        {
        }
    }
}
