using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IOrderDetailsEntityService : IRepository<OrderDetails, FboLinxContext>
    {
    }

    public class OrderDetailsEntityService : Repository<OrderDetails, FboLinxContext>, IOrderDetailsEntityService
    {
        public OrderDetailsEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
