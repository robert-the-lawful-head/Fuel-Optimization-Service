using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.User
{
    public sealed class ConductorOrGroupAdminUserByGroupIdSpecification : Specification<Models.User>
    {
        public ConductorOrGroupAdminUserByGroupIdSpecification(int groupId) : base(x => x.GroupId == groupId && (x.Role == UserRoles.Conductor || x.Role == UserRoles.GroupAdmin))
        {
        }
    }
}
