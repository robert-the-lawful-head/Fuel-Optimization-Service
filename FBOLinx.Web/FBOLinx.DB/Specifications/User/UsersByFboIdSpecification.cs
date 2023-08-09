using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.User
{
    public sealed class UsersByFboIdSpecification : Specification<Models.User>
    {
        public UsersByFboIdSpecification(int fboId) : base(x => x.FboId == fboId)
        {
        }
    }
}
