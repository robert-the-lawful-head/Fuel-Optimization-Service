using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Fbo
{
    public sealed class AllFbosByGroupIdSpecification : Specification<Models.Fbos>
    {
        public AllFbosByGroupIdSpecification(int groupId) : base(x => x.GroupId == groupId && x.Active == true)
        {
            AddInclude(x => x.FboAirport);
        }
    }
}
