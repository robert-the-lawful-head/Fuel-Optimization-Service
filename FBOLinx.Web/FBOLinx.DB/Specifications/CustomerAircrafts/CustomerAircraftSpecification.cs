using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerAircrafts
{
    public sealed class CustomerAircraftSpecification : Specification<Models.CustomerAircrafts>
    {
        public CustomerAircraftSpecification(int id) : base(x => x.Oid == id)
        {
            AddInclude(ca => ca.Customer);
            AddInclude(ca => ca.Customer.CustomerInfoByGroup.Where(x => ca.GroupId.HasValue && x.GroupId == ca.GroupId.Value));
        }
    }
}
