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
    public sealed class UserByUsernameSpecification : Specification<Models.User>
    {
        public UserByUsernameSpecification(string username) : base(x => x.Username == username)
        {
        }
    }
}
