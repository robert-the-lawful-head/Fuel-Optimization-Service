using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Group
{
    public class AllGroupsSpecification : Specification<Models.Group>
    {
        public AllGroupsSpecification(bool activeOnly) : base(x => activeOnly == false || x.Active == true)
        {
        }
    }
}
