using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.CustomerInfoByGroup
{
    public class CustomerInfoByGroupCustomerIdGroupIdSpecification : Specification<Models.CustomerInfoByGroup>
    {
        public CustomerInfoByGroupCustomerIdGroupIdSpecification(int customerId, int groupId) : base(x => x.CustomerId == customerId && x.GroupId == groupId)
        {
        }
    }
}
