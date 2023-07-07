using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerAircrafts
{
    public sealed class CustomerAircraftSpecification : Specification<Models.CustomerAircrafts>
    {
        public CustomerAircraftSpecification(int id) : base(x => x.Oid == id)
        {
            AddInclude(x => x.Notes);
        }
    }
}
