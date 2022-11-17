using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerAircrafts
{
    public class CustomerAircraftByGroupAndTailSpecification : Specification<Models.CustomerAircrafts>
    {
        public CustomerAircraftByGroupAndTailSpecification(List<int> groupIds, List<string> tailNumbers, int customerId) : base(x => x.GroupId.HasValue && 
            groupIds.Contains(x.GroupId.Value) && 
            !string.IsNullOrEmpty(x.TailNumber) &&
            tailNumbers.Contains(x.TailNumber) &&
            x.CustomerId == customerId)
        {
        }

        public CustomerAircraftByGroupAndTailSpecification(List<int> groupIds, int customerId) : base(x => x.GroupId.HasValue &&
            groupIds.Contains(x.GroupId.Value) &&
            !string.IsNullOrEmpty(x.TailNumber) 
        &&
            //tailNumbers.Contains(x.TailNumber) &&
            x.CustomerId == customerId)
        {
        }
    }
}
