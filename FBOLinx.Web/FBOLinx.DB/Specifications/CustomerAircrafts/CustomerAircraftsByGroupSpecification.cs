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
        public CustomerAircraftsByGroupSpecification(int groupId) : base(x => x.GroupId == groupId
                                                                              && (x.Customer.Oid > 0)
                                                                              && (x.Customer.CustomerInfoByGroup.Any(c => c.GroupId == groupId))
                                                                              && (!x.Customer.Suspended.HasValue || x.Customer.Suspended == false)
                                                                              )
        {
            AddInclude(ca => ca.Customer.CustomerInfoByGroup.Where(x => x.GroupId == groupId));
            AddInclude(ca => ca.Customer);
        }

        public CustomerAircraftsByGroupSpecification(int groupId, List<string> tailNumbers) : base(x => x.GroupId == groupId
            && (x.Customer.Oid > 0)
            && (x.Customer.CustomerInfoByGroup.Any(c => c.GroupId == groupId))
            && (tailNumbers == null || tailNumbers.Contains(x.TailNumber))
            && (!x.Customer.Suspended.HasValue || x.Customer.Suspended == false))
        {
            AddInclude(ca => ca.Customer.CustomerInfoByGroup.Where(x => x.GroupId == groupId));
            AddInclude(ca => ca.Customer);
        }


    }
}