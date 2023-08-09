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
using System.Net.Mail;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Fbo
{
    public interface IAcukwikFboHandlerDetailService : IBaseDTOService<AcukwikFbohandlerDetailDto, AcukwikFbohandlerDetail>
    {
    }

    public class AcukwikFboHandlerDetailService :
        BaseDTOService<AcukwikFbohandlerDetailDto, AcukwikFbohandlerDetail, DegaContext>, IAcukwikFboHandlerDetailService
    {
        public AcukwikFboHandlerDetailService(IRepository<AcukwikFbohandlerDetail, DegaContext> entityService) : base(
            entityService)
        {
        }
    }
}
