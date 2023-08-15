using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerInfoByGroup
{
    public sealed class CustomerInfoByGroupCustomerAircraftsByGroupIdSpecification : Specification<Models.CustomerInfoByGroup>
    {
        public CustomerInfoByGroupCustomerAircraftsByGroupIdSpecification(int groupId) : base(x => x.GroupId == groupId && (!x.Customer.Suspended.HasValue || x.Customer.Suspended == false))
        {
            AddInclude(x => x.Customer);
            AddInclude(x => x.Customer.CustomerAircrafts.Where(f => f.GroupId == groupId));
            AddInclude(x => x.FavoriteCompany);
        }

        public CustomerInfoByGroupCustomerAircraftsByGroupIdSpecification(int groupId, int customerInfoByGroupId) : base(x => x.GroupId == groupId && x.Oid == customerInfoByGroupId && (!x.Customer.Suspended.HasValue || x.Customer.Suspended == false))
        {
            AddInclude(x => x.Customer);
            AddInclude(x => x.Customer.CustomerAircrafts.Where(f => f.GroupId == groupId));
            AddInclude(x => x.FavoriteCompany);
        }

        public CustomerInfoByGroupCustomerAircraftsByGroupIdSpecification(int groupId, int customerInfoByGroupId, List<string> tailNumbers) : base(x => x.GroupId == groupId && x.Oid == customerInfoByGroupId && (!x.Customer.Suspended.HasValue || x.Customer.Suspended == false))
        {
            AddInclude(x => x.Customer);
            AddInclude(x => x.Customer.CustomerAircrafts.Where(f => f.GroupId == groupId && tailNumbers.Contains(f.TailNumber)));
        }

        public CustomerInfoByGroupCustomerAircraftsByGroupIdSpecification(int groupId, List<string> tailNumbers) : base(x => x.GroupId == groupId && (!x.Suspended.HasValue || x.Suspended == false))
        {
            AddInclude(x => x.Customer);
            AddInclude(x => x.Customer.CustomerAircrafts.Where(f => f.GroupId == groupId && tailNumbers.Contains(f.TailNumber)));
        }
    }
}
