using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerInfoByGroup
{
    public sealed class CustomerInfoByGroupSpecification : Specification<Models.CustomerInfoByGroup>
    {
        public CustomerInfoByGroupSpecification(int id) : base(x => x.Oid == id)
        {
            AddInclude(x => x.Customer);
            AddInclude(x => x.Notes);
        }
    }
}
