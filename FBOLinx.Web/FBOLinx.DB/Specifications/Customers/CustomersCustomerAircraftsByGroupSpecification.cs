using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Customers
{
    public sealed class CustomersCustomerAircraftsByGroupSpecification : Specification<Models.Customers>
    {
        public CustomersCustomerAircraftsByGroupSpecification(int groupId) : base(x => !x.Suspended.HasValue || x.Suspended == false)
        {
            AddInclude(x => x.CustomerInfoByGroup.Where(c => c.GroupId == groupId));
            AddInclude(x => x.CustomerAircrafts.Where(f => f.GroupId == groupId));
        }

        public CustomersCustomerAircraftsByGroupSpecification(int groupId, List<string> tailNumbers) : base(x => !x.Suspended.HasValue || x.Suspended == false)
        {
            AddInclude(x => x.CustomerInfoByGroup.Where(c => c.GroupId == groupId));
            AddInclude(x => x.CustomerAircrafts.Where(f => f.GroupId == groupId && tailNumbers.Contains(f.TailNumber)));
        }

        public CustomersCustomerAircraftsByGroupSpecification(int groupId, int fboId) : base(x => (!x.Suspended.HasValue || x.Suspended == false) && x.CustomerInfoByGroup.Any(c => c.GroupId == groupId))
        {
            AddInclude(x => x.CustomerInfoByGroup.Where(c => c.GroupId == groupId));
            AddInclude(x => x.CustomerAircrafts.Where(f => f.GroupId == groupId));
            AddInclude(x => x.CustomCustomerType.Where(x => x.Fboid == fboId));
        }

        public CustomersCustomerAircraftsByGroupSpecification(int groupId, int fboId, int customerInfoByGroupId) : base(x => x.Oid == customerInfoByGroupId && (!x.Suspended.HasValue || x.Suspended == false))
        {
            AddInclude(x => x.CustomerInfoByGroup.Where(c => c.GroupId == groupId));
            AddInclude(x => x.CustomerAircrafts.Where(f => f.GroupId == groupId));
            AddInclude(x => x.CustomCustomerType.Where(x => x.Fboid == fboId));
        }
    }
}
