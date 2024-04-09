using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerAircrafts
{
    public sealed class AllFbosFromAllGroupsSpecification : Specification<Models.Fbos>
    {
        public AllFbosFromAllGroupsSpecification() : base(x => x.GroupId > 1)
        {

        }
    }
}
