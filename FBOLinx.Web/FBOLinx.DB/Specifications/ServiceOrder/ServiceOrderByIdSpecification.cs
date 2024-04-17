using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.User
{
    public sealed class ServiceOrderByIdSpecification : Specification<Models.ServiceOrder>
    {
        public ServiceOrderByIdSpecification(int id) : base(x => x.Oid == id)
        {
            AddInclude(x => x.ServiceOrderItems);
            AddInclude(x => x.CustomerInfoByGroup);
            AddInclude(x => x.CustomerAircraft);
        }
    }
}
