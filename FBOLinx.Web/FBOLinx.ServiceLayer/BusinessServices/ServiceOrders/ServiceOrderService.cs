using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.User;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.ServiceOrders
{
    public interface IServiceOrderService : IBaseDTOService<ServiceOrderDto, DB.Models.ServiceOrder>
    {
    }

    public class ServiceOrderService : BaseDTOService<ServiceOrderDto, DB.Models.ServiceOrder, FboLinxContext>, IServiceOrderService
    {
        public ServiceOrderService(IRepository<ServiceOrder, FboLinxContext> entityService) : base(entityService)
        {
        }

        public ServiceOrderService(IUserService entityService) : base(entityService)
        {
        }
    }
}
