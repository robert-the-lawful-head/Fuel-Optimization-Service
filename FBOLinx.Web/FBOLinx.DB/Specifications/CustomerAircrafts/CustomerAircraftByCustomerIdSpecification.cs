using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerAircrafts
{
    public class CustomerAircraftByCustomerIdSpecification: Specification<Models.CustomerAircrafts>
    {
        public CustomerAircraftByCustomerIdSpecification(int customerId) : base(x => x.CustomerId == customerId)
        {
        }

        public CustomerAircraftByCustomerIdSpecification(int customerId, List<string> tailNumberList) : base(x => x.CustomerId == customerId && tailNumberList.Contains(x.TailNumber))
        {
        }
    }
}
