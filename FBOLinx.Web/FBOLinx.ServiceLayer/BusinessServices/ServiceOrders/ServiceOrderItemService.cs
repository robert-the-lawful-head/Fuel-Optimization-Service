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
{ public interface IServiceOrderItemService : IBaseDTOService<ServiceOrderItemDto, DB.Models.ServiceOrderItem>
    {
    }

    public class ServiceOrderItemService :
        BaseDTOService<ServiceOrderItemDto, DB.Models.ServiceOrderItem, FboLinxContext>, IServiceOrderItemService
    {
        public ServiceOrderItemService(IRepository<ServiceOrderItem, FboLinxContext> entityService) : base(
            entityService)
        {
        }

        public ServiceOrderItemService(IUserService entityService) : base(entityService)
        {
        }
    }
}
