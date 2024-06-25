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
    public interface ICustomerInfoByFboService : IBaseDTOService<CustomerInfoByFboDto, DB.Models.CustomerInfoByFbo>
    {
    }

    public class CustomerInfoByFboService :
        BaseDTOService<CustomerInfoByFboDto, DB.Models.CustomerInfoByFbo, FboLinxContext>, ICustomerInfoByFboService
    {
        public CustomerInfoByFboService(IRepository<CustomerInfoByFbo, FboLinxContext> entityService) : base(
            entityService)
        {
        }
    }
}
