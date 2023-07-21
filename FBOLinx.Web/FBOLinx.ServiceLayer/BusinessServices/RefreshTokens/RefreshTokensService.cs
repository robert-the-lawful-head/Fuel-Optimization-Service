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

namespace FBOLinx.ServiceLayer.BusinessServices.RefreshTokens
{
    public interface IRefreshTokensService : IBaseDTOService<RefreshTokensDto, DB.Models.RefreshTokens>
    {
    }

    public class RefreshTokensService :
        BaseDTOService<RefreshTokensDto, DB.Models.RefreshTokens, FboLinxContext>, IRefreshTokensService
    {
        public RefreshTokensService(IRepository<DB.Models.RefreshTokens, FboLinxContext> entityService) : base(
            entityService)
        {
        }
    }
}
