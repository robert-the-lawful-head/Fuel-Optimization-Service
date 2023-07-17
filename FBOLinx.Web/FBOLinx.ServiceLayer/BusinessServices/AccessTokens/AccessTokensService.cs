using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.FuelRequests;
using FBOLinx.DB.Specifications.ServiceOrder;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.BusinessServices.ServiceOrders;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using FBOLinx.ServiceLayer.EntityServices;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Orders
{
    public interface IAccessTokensService : IBaseDTOService<AccessTokensDto, DB.Models.AccessTokens>
    {
    }

    public class AccessTokensService :
        BaseDTOService<AccessTokensDto, DB.Models.AccessTokens, FboLinxContext>, IAccessTokensService
    {
        public AccessTokensService(IRepository<AccessTokens, FboLinxContext> entityService) : base(
            entityService)
        {
        }
    }
}
