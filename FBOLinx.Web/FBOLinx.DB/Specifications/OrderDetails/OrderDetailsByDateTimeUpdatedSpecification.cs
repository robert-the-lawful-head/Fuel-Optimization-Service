using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.User
{
    public sealed class OrderDetailsByDateTimeUpdatedSpecification : Specification<Models.OrderDetails>
    {
        public OrderDetailsByDateTimeUpdatedSpecification(DateTime startingDateTime) : base(x => x.DateTimeUpdated >= startingDateTime)
        {
        }
    }
}
