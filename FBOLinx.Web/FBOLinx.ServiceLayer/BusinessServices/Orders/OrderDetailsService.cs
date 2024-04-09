using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.FuelRequests;
using FBOLinx.DB.Specifications.User;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.BusinessServices.ServiceOrders;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using FBOLinx.ServiceLayer.EntityServices;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Orders
{
    public interface IOrderDetailsService : IBaseDTOService<OrderDetailsDto, DB.Models.OrderDetails>
    {
    }

    public class OrderDetailsService :
        BaseDTOService<OrderDetailsDto, DB.Models.OrderDetails, FboLinxContext>, IOrderDetailsService
    {
        public OrderDetailsService(IRepository<OrderDetails, FboLinxContext> entityService) : base(
            entityService)
        {
        }
    }
}
