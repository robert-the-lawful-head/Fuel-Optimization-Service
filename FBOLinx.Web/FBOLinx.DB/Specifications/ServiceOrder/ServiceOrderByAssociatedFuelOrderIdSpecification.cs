using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.ServiceOrder
{
    public class ServiceOrderByAssociatedFuelOrderIdSpecification : Specification<Models.ServiceOrder>
    {
        public ServiceOrderByAssociatedFuelOrderIdSpecification(int associatedFuelOrderId) : base(x => x.AssociatedFuelOrderId == associatedFuelOrderId)
        {
            AddInclude(x => x.ServiceOrderItems);
            AddInclude(x => x.CustomerInfoByGroup);
            AddInclude(x => x.CustomerAircraft);
        }
    }
}
