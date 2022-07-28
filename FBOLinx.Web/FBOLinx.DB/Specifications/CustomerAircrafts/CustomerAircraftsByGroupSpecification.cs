using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerAircrafts
{
    public sealed class CustomerAircraftsByGroupSpecification : Specification<Models.CustomerAircrafts>
    {
        public CustomerAircraftsByGroupSpecification(int groupId) : base(x => x.GroupId.HasValue && x.GroupId.Value == groupId)
        {
            AddInclude(ca => ca.Customer);
            AddInclude(ca => ca.Customer.CustomerInfoByGroup.Where(x => x.GroupId == groupId));
        }
    }
}
