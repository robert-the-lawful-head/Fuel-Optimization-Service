using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.CustomerInfoByGroup
{
    public sealed class CustomerInfoByGroupCustomerAircraftsByGroupIdNotCheckingSuspendedSpecification : Specification<Models.CustomerInfoByGroup>
    {
        public CustomerInfoByGroupCustomerAircraftsByGroupIdNotCheckingSuspendedSpecification(int groupId) : base(x => x.GroupId == groupId)
        {
            AddInclude(x => x.Customer);
            AddInclude(x => x.Customer.CustomerAircrafts.Where(f => f.GroupId == groupId));
        }
    }
}
