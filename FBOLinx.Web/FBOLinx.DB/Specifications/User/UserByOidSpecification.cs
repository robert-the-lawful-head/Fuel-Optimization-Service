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
    public sealed class UserByOidSpecification : Specification<Models.User>
    {
        public UserByOidSpecification(int oid) : base(x => x.Oid == oid)
        {
        }
    }
}
