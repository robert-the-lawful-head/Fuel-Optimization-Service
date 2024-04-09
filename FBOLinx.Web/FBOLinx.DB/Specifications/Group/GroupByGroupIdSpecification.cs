using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Group
{
    public class GroupByGroupIdSpecification : Specification<Models.Group>
    {
        public GroupByGroupIdSpecification(int groupId) : base(x => x.Oid == groupId)
        {
        }
    }
}
