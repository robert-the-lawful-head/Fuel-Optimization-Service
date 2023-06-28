using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models.Dega;
using FBOLinx.DB.Models.ServicesAndFees;
using FBOLinx.ServiceLayer.DTO.ServicesAndFees;
using FBOLinx.ServiceLayer.EntityServices;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.ServicesAndFees
{
    public interface IFboServicesAndFeesService
    {
        Task<List<ServicesAndFeesDto>> Get(int fboId);
    }
    public class FboServicesAndFeesService : IFboServicesAndFeesService
    {
        private IRepository<FboCustomServicesAndFees, FboLinxContext> _fboCustomServicesAndFeesRepo;
        private IRepository<AcukwikServicesOffered, DegaContext> _acukwikServicesOfferedRepo;
        public FboServicesAndFeesService(
            IRepository<FboCustomServicesAndFees, FboLinxContext> fboCustomServicesAndFeesRepo,
            IRepository<AcukwikServicesOffered, DegaContext> acukwikServicesOfferedRepo)
        {
            _acukwikServicesOfferedRepo = acukwikServicesOfferedRepo;
            _fboCustomServicesAndFeesRepo = fboCustomServicesAndFeesRepo;
        }

        public async Task<List<ServicesAndFeesDto>> Get(int fboId)
        {
            var modifiedServices = _fboCustomServicesAndFeesRepo.Where(x => x.FboId == fboId);
            var acukwikServicesOffered = _acukwikServicesOfferedRepo.Get().Take(1000);

            if (!modifiedServices.Any())
            {
                return await acukwikServicesOffered.Select(x =>
                    //x.Adapt<ServicesAndFeesDto>()
                    new ServicesAndFeesDto
                    {
                        //Oid must be replace for PK once is done on DB side
                        Oid = x.HandlerId.ToString() + x.ServiceOfferedId.ToString(),
                        Service = x.Service,
                        ServiceType = x.ServiceType,
                    }
                ).ToListAsync();
            }

            var deletedAndModifiedServices = await modifiedServices.Where(x => x.ServiceActionType == ServiceActionType.Deleted || x.ServiceActionType == ServiceActionType.Updated).Select(x => x.AcukwikServicesOfferedId.ToString()).ToListAsync();

            // generating Id manually until is set on dega db
            var customNotModifiedSrvices = acukwikServicesOffered.Where(x => !deletedAndModifiedServices.Contains((x.HandlerId.ToString() + x.ServiceOfferedId.ToString())));

            var notModifiedServiceResult = await customNotModifiedSrvices.Select(x =>
                   //x.Adapt<ServicesAndFeesDto>()
                   new ServicesAndFeesDto
                   {
                       Oid = x.HandlerId.ToString() + x.ServiceOfferedId.ToString(),
                       Service = x.Service,
                       ServiceType = x.ServiceType,
                   }
               ).ToListAsync();

            var customModifiedServices = await modifiedServices.Select(x =>
                   //x.Adapt<ServicesAndFeesDto>()
                   new ServicesAndFeesDto
                   {
                       Oid = x.Oid.ToString(),
                       Service = x.Service,
                       ServiceType = x.ServiceType,
                   }
               ).ToListAsync();

            return notModifiedServiceResult.Concat(customModifiedServices).ToList();
        }
        public async Task<List<ServicesAndFeesDto>> GetByServiceType(int fboId, string serviceType)
        {
            var modifiedServices = _fboCustomServicesAndFeesRepo.Where(x => x.FboId == fboId);
            var acukwikServicesOffered = _acukwikServicesOfferedRepo.Get();

            if (!modifiedServices.Any())
            {
                return await acukwikServicesOffered.Select(x =>
                    //x.Adapt<ServicesAndFeesDto>()
                    new ServicesAndFeesDto
                    {
                        //Oid must be replace for PK once is done on DB side
                        Oid = x.HandlerId.ToString() + x.ServiceOfferedId.ToString(),
                        Service = x.Service,
                        ServiceType = x.ServiceType,
                    }
                ).ToListAsync();
            }

            var deletedAndModifiedServices = await modifiedServices.Where(x => x.ServiceActionType == ServiceActionType.Deleted || x.ServiceActionType == ServiceActionType.Updated).Select(x => x.AcukwikServicesOfferedId.ToString()).ToListAsync();

            // generating Id manually until is set on dega db
            var customNotModifiedSrvices = acukwikServicesOffered.Where(x => !deletedAndModifiedServices.Contains((x.HandlerId.ToString() + x.ServiceOfferedId.ToString())));

            var notModifiedServiceResult = await customNotModifiedSrvices.Select(x =>
                   x.Adapt<ServicesAndFeesDto>()
               ).ToListAsync();

            var customModifiedServices = await modifiedServices.Select(x =>
                   //x.Adapt<ServicesAndFeesDto>()
                   new ServicesAndFeesDto
                   {
                       Oid = x.Oid.ToString(),
                       Service = x.Service,
                       ServiceType = x.ServiceType,
                   }
               ).ToListAsync();

            return notModifiedServiceResult.Concat(customModifiedServices).ToList();
        }
    }
}
