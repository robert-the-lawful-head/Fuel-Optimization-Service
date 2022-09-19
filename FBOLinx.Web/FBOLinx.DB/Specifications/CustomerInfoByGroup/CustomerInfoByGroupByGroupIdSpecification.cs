using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerInfoByGroup
{
    public sealed class CustomerInfoByGroupByGroupIdSpecification : Specification<Models.CustomerInfoByGroup>
    {
        public CustomerInfoByGroupByGroupIdSpecification(int groupId) : base(x => x.GroupId == groupId)
        {
            AddInclude(x => x.Customer);
        }
    }
}
