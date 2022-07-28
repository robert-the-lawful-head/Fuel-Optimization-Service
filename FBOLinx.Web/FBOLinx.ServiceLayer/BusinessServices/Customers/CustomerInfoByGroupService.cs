using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.Customers
{
    public interface ICustomerInfoByGroupService : IBaseDTOService<CustomerInfoByGroupDTO, CustomerInfoByGroup>
    {
    }

    public class CustomerInfoByGroupService : BaseDTOService<CustomerInfoByGroupDTO, DB.Models.CustomerInfoByGroup, FboLinxContext>, ICustomerInfoByGroupService
    {
        public CustomerInfoByGroupService(IRepository<CustomerInfoByGroup, FboLinxContext> entityService) : base(entityService)
        {
        }
    }
}
