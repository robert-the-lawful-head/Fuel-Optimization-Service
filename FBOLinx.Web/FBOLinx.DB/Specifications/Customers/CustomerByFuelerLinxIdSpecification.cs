using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Customers
{
    public sealed class CustomerByFuelerLinxIdSpecification : Specification<Models.Customers>
    {
        public CustomerByFuelerLinxIdSpecification(int fuelerLinxId) : base(x => x.FuelerlinxId.HasValue && x.FuelerlinxId.Value == fuelerLinxId)
        {
            
        }
    }
}
