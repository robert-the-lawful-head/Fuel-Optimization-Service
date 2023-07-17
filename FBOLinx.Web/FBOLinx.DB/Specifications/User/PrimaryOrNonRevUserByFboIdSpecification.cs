using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.ServiceOrder
{
    public sealed class PrimaryOrNonRevUserByFboIdSpecification : Specification<Models.User>
    {
        public PrimaryOrNonRevUserByFboIdSpecification(int fboId) : base(x => x.FboId == fboId && (x.Role == Core.Enums.UserRoles.Primary || x.Role == Core.Enums.UserRoles.NonRev))
        {
        }
    }
}
