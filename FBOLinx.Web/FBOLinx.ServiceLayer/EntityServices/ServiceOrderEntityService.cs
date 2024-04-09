using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IServiceOrderEntityService : IRepository<ServiceOrder, FboLinxContext>
    {
    }

    public class ServiceOrderEntityService : Repository<ServiceOrder, FboLinxContext>, IServiceOrderEntityService
    {
        public ServiceOrderEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
