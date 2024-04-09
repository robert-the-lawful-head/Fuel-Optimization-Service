using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IServiceOrderItemEntityService : IRepository<ServiceOrderItem, FboLinxContext>
    {
    }

    public class ServiceOrderItemEntityService : Repository<ServiceOrderItem, FboLinxContext>, IServiceOrderItemEntityService
    {
        public ServiceOrderItemEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
