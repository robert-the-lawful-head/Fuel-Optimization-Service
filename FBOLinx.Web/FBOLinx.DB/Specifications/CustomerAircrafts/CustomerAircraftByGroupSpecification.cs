using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerAircrafts
{
    public sealed class CustomerAircraftByGroupSpecification : Specification<Models.CustomerAircrafts>
    {
        public CustomerAircraftByGroupSpecification(List<int> groupIds, int customerId) : base(x => groupIds.Contains(x.GroupId) &&
            !string.IsNullOrEmpty(x.TailNumber) 
            &&
            x.CustomerId == customerId)
        {
            AddInclude(x => x.Notes);
        }
    }
}
