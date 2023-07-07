using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models.Dega;
using FBOLinx.DB.Models.ServicesAndFees;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.ServiceLayer.DTO.ServicesAndFees;
using FBOLinx.ServiceLayer.EntityServices;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.ServicesAndFees
{
    public interface IFboServicesAndFeesService
    {
        Task<List<ServicesAndFeesDto>> Get(int fboId);
        Task<bool> Update(int ServiceAdnFeesId, ServicesAndFeesDto servicesAndFees);
    }
    public class FboServicesAndFeesService : IFboServicesAndFeesService
    {
        private IRepository<FboCustomServicesAndFees, FboLinxContext> _fboCustomServicesAndFeesRepo;
        private IFboEntityService _fboEntityService;
        private IRepository<AcukwikServicesOffered, DegaContext> _acukwikServicesOfferedRepo;
        private const int defaultDataLimit = 5000;
        public FboServicesAndFeesService(
            IRepository<FboCustomServicesAndFees, FboLinxContext> fboCustomServicesAndFeesRepo,
            IRepository<AcukwikServicesOffered, DegaContext> acukwikServicesOfferedRepo,
            IFboEntityService fboEntityService)
        {
            _acukwikServicesOfferedRepo = acukwikServicesOfferedRepo;
            _fboCustomServicesAndFeesRepo = fboCustomServicesAndFeesRepo;
            _fboEntityService = fboEntityService;
        }

        public async Task<List<ServicesAndFeesDto>> Get(int fboId)
        {
            var fbo = await _fboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));

            if (fbo == null)
                return null;

            var modifiedServices = _fboCustomServicesAndFeesRepo.Where(x => x.FboId == fboId);
            var acukwikServicesOffered = _acukwikServicesOfferedRepo.Where(x => x.HandlerId == fbo.AcukwikFBOHandlerId);
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

        public Task<bool> Update(int ServiceAdnFeesId, ServicesAndFeesDto servicesAndFees)
        {
            throw new System.NotImplementedException();
        }
    }
}
