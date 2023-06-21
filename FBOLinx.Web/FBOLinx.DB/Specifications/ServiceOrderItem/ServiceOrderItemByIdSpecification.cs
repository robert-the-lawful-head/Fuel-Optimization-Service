using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.ServiceOrderItem
{
    public class ServiceOrderItemByIdSpecification : Specification<Models.ServiceOrderItem>
    {
        public ServiceOrderItemByIdSpecification(long id) : base(x => x.Oid == id)
        {
        }
    }
}
