using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.CustomerInfoByGroup
{
    public class CustomerInfoByGroupByFboIdSpecification : Specification<Models.CustomerInfoByGroup>
    {
        public CustomerInfoByGroupByFboIdSpecification(int groupId, int fboId, int customerInfoByGroupId) : base(x => x.GroupId == groupId! && (customerInfoByGroupId == 0 || x.Oid == customerInfoByGroupId) && (!x.Customer.Suspended.HasValue || !x.Customer.Suspended.Value))
        {
            AddInclude(x => x.Customer);
            AddInclude(x => x.Customer.CustomCustomerType.Where(x => x.Fboid == fboId));
            //AddInclude(x => x.Customer.CustomerContacts);
        }
    }
}
