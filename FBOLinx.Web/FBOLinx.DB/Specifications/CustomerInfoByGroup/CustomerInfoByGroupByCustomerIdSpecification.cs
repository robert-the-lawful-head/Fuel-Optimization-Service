using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerInfoByGroup
{
    public class CustomerInfoByGroupByCustomerIdSpecification : Specification<Models.CustomerInfoByGroup>
    {
        public CustomerInfoByGroupByCustomerIdSpecification(int customerId) : base(x => x.CustomerId == customerId)
        {
        }
    }
}
