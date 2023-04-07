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
        public CustomerAircraftsByGroupSpecification(int groupId) : base(x => x.GroupId == groupId)
        {
            AddInclude(ca => ca.Customer);
            AddInclude(ca => ca.Customer.CustomerInfoByGroup.Where(x => x.GroupId == groupId));
        }

        public CustomerAircraftsByGroupSpecification(int groupId, List<string> tailNumbers) : base(x => x.GroupId == groupId && (tailNumbers == null || tailNumbers.Contains(x.TailNumber)))
        {
            AddInclude(ca => ca.Customer);
            AddInclude(ca => ca.Customer.CustomerInfoByGroup.Where(x => x.GroupId == groupId));
        }
    }
}
