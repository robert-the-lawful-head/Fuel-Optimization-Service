using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models.Dega;
using FBOLinx.DB.Models.ServicesAndFees;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.ServiceLayer.DTO.ServicesAndFees;
using FBOLinx.ServiceLayer.EntityServices;
using Mapster;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.ServicesAndFees
{
    public interface IFboServicesAndFeesService
    {
        Task<List<ServicesAndFeesDto>> Get(int fboId);
        Task<ServicesAndFeesDto> Create(int fboId, ServicesAndFeesDto servicesAndFees);
        Task<bool> Update(int fboId, ServicesAndFeesDto servicesAndFees, int? handlerId, int? serviceOfferedId);
        Task<bool> Delete(int ServiceAdnFeesId, int? handlerId, int? serviceOfferedId);
    }
    public class FboServicesAndFeesService : IFboServicesAndFeesService
    {
        private IRepository<FboCustomServicesAndFees, FboLinxContext> _fboCustomServicesAndFeesRepo;
        private IFboEntityService _fboEntityService;
        private IAcukwikServicesOfferedEntityService _acukwikServicesOfferedEntityService;
        private const int defaultDataLimit = 5000;
        public FboServicesAndFeesService(
            IRepository<FboCustomServicesAndFees, FboLinxContext> fboCustomServicesAndFeesRepo,
            IAcukwikServicesOfferedEntityService acukwikServicesOfferedEntityService,
            IFboEntityService fboEntityService)
        {
            _acukwikServicesOfferedEntityService = acukwikServicesOfferedEntityService;
            _fboCustomServicesAndFeesRepo = fboCustomServicesAndFeesRepo;
            _fboEntityService = fboEntityService;
        }

        public async Task<List<ServicesAndFeesDto>> Get(int fboId)
        {
            var fbo = await _fboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));

            if (fbo == null)
                return null;

            var modifiedServices = _fboCustomServicesAndFeesRepo.Where(x => x.FboId == fboId);
            var acukwikServicesOffered = _acukwikServicesOfferedEntityService.Where(x => x.HandlerId == (int)fbo.AcukwikFBOHandlerId);
            if (!modifiedServices.Any())
            {
                return await GetfromAcukwikServicesOffered(acukwikServicesOffered);
            }
            return await GetMergedDataFromAcukwikServicesOfferedAndCustomServicesAndFeesRepo(modifiedServices,acukwikServicesOffered);
            
        }
        private async Task<List<ServicesAndFeesDto>> GetfromAcukwikServicesOffered(IQueryable<AcukwikServicesOffered> acukwikServicesOffered)
        {
            return await acukwikServicesOffered.Select(x =>
                    x.Adapt<ServicesAndFeesDto>()
                ).ToListAsync();
        }
        private async Task<List<ServicesAndFeesDto>> GetMergedDataFromAcukwikServicesOfferedAndCustomServicesAndFeesRepo(IQueryable<FboCustomServicesAndFees> modifiedServices, IQueryable<AcukwikServicesOffered> acukwikServicesOffered)
        {
            var deletedAndModifiedServices = await modifiedServices.Where(x => x.ServiceActionType == ServiceActionType.Deleted || x.ServiceActionType == ServiceActionType.Updated).Select(x => x.AcukwikServicesOfferedId.ToString()).ToListAsync();

            // generating Id manually until is set on dega db
            var customNotModifiedSrvices = acukwikServicesOffered.Where(x => !deletedAndModifiedServices.Contains((x.HandlerId.ToString() + x.ServiceOfferedId.ToString())));

            var notModifiedServiceResult = await customNotModifiedSrvices.Select(x =>
                   x.Adapt<ServicesAndFeesDto>()
               ).ToListAsync();

            var customModifiedServices = await modifiedServices.Select(x =>
                   x.Adapt<ServicesAndFeesDto>()
               ).ToListAsync();

            return notModifiedServiceResult.Concat(customModifiedServices).ToList();
        }
        public async Task<ServicesAndFeesDto> Create(int fboId, ServicesAndFeesDto servicesAndFees)
        {
            var entity =  servicesAndFees.Adapt<FboCustomServicesAndFees>();
            entity.ServiceActionType = ServiceActionType.New;
            entity.FboId = fboId;
            var createdEntity =  await _fboCustomServicesAndFeesRepo.AddAsync(entity);

            return createdEntity.Adapt<ServicesAndFeesDto>();
        }

        public async Task<bool> Update(int fboId, ServicesAndFeesDto servicesAndFees, int? handlerId, int? serviceOfferedId)
        {
            var customServiceAndfee = await _fboCustomServicesAndFeesRepo.FindAsync(servicesAndFees.Oid);

            if(customServiceAndfee != null)
            {
                customServiceAndfee.Service = servicesAndFees.Service;
                customServiceAndfee.ServiceType = servicesAndFees.ServiceType;
                await _fboCustomServicesAndFeesRepo.UpdateAsync(customServiceAndfee);
                return true;
            }

            if (handlerId == null && serviceOfferedId == null)
                return false;

            var defaultServiceAndFee = await _acukwikServicesOfferedEntityService.FindByComposeKeyAsync((int)handlerId,(int)serviceOfferedId);

            if(defaultServiceAndFee != null)
            {
                var entity = servicesAndFees.Adapt<FboCustomServicesAndFees>();
                entity.ServiceActionType = ServiceActionType.Updated;
                entity.FboId = fboId;
                entity.AcukwikServicesOfferedId = int.Parse(handlerId.ToString()+serviceOfferedId.ToString());
                await _fboCustomServicesAndFeesRepo.AddAsync(entity);
                return true;
            }

            return false;
        }
        public async Task<bool> Delete(int ServiceAdnFeesId,int? handlerId, int? serviceOfferedId)
        {
            if (await _fboCustomServicesAndFeesRepo.DeleteAsync(ServiceAdnFeesId) != null)
            {
                return true;
            }

            if(handlerId == null && serviceOfferedId == null)
                return false;

            var defaultServiceAndFee = await _acukwikServicesOfferedEntityService.FindByComposeKeyAsync((int)handlerId,(int)serviceOfferedId);

            if (defaultServiceAndFee != null)
            {
                var entity = new FboCustomServicesAndFees()
                {
                    ServiceActionType = ServiceActionType.Deleted,
                    AcukwikServicesOfferedId = ServiceAdnFeesId
                };
              
                await _fboCustomServicesAndFeesRepo.AddAsync(entity);
                return true;
            }

            return false;
        }
    }
}
