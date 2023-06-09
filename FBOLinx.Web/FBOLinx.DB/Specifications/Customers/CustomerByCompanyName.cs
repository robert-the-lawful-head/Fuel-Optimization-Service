using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Customers
{
    public class CustomerByCompanyName : Specification<Models.Customers>
    {
        public CustomerByCompanyName(string companyName) : base(x => x.Company == companyName)
        {
        }
    }
}
