using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerAircrafts
{
    public class CustomerAircraftByTailSpecification : Specification<Models.CustomerAircrafts>
    {
        public CustomerAircraftByTailSpecification(List<string> tailNumbers) : base(x => !string.IsNullOrEmpty(x.TailNumber) && tailNumbers.Contains(x.TailNumber))
        {
        }
    }
}
