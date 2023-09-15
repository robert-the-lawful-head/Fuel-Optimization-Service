using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models.Dega;
using FBOLinx.DB.Models.ServicesAndFees;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees;
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
        Task<List<FbosServicesAndFeesResponse>> Get(int fboId);
        Task<ServicesAndFeesResponse> Create(int fboId, ServicesAndFeesDto servicesAndFees);
        Task<ServicesAndFeesResponse> Update(int fboId, ServicesAndFeesDto servicesAndFees);
        Task<bool> Delete(int ServiceAdnFeesId, int? handlerId, int? serviceOfferedId);
    }
    public class FboServicesAndFeesService : IFboServicesAndFeesService
    {
        private IRepository<FboCustomServicesAndFees, FboLinxContext> _fboCustomServicesAndFeesRepo;
        private IRepository<FboCustomServiceType, FboLinxContext> _fboCustomServiceTypeRepo;
        private IFboEntityService _fboEntityService;
        private IAcukwikServicesOfferedEntityService _acukwikServicesOfferedEntityService;

        public FboServicesAndFeesService(
            IRepository<FboCustomServicesAndFees, FboLinxContext> fboCustomServicesAndFeesRepo,
            IRepository<FboCustomServiceType, FboLinxContext> fboCustomServiceTypeRepo,
            IAcukwikServicesOfferedEntityService acukwikServicesOfferedEntityService,
            IFboEntityService fboEntityService)
        {
            _acukwikServicesOfferedEntityService = acukwikServicesOfferedEntityService;
            _fboCustomServicesAndFeesRepo = fboCustomServicesAndFeesRepo;
            _fboCustomServiceTypeRepo = fboCustomServiceTypeRepo;
            _fboEntityService = fboEntityService;
        }

        public async Task<List<FbosServicesAndFeesResponse>> Get(int fboId)
        {
            var fbo = await _fboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));

            if (fbo == null)
                return null;

            var modifiedServices = _fboCustomServicesAndFeesRepo.Where(x => x.FboId == fboId).Include(p => p.ServiceType);
            var serviceTypes = _fboCustomServiceTypeRepo.Where(x => x.FboId == fboId);
            var acukwikServicesOffered = _acukwikServicesOfferedEntityService.Where(x => x.HandlerId == (int)fbo.AcukwikFBOHandlerId);

            if (!modifiedServices.Any() && !serviceTypes.Any())
            {
                return await GetfromAcukwikServicesOffered(acukwikServicesOffered);
            }

            var fbosServicesAndFeesResponseList = await GetfromAcukwikServicesOffered(acukwikServicesOffered, modifiedServices);

            return await GetCustomServicesAndFees(fbosServicesAndFeesResponseList);
            
        }
        private async Task<List<FbosServicesAndFeesResponse>> GetfromAcukwikServicesOffered(IQueryable<AcukwikServicesOffered> acukwikServicesOffered,IQueryable<FboCustomServicesAndFees> customServices = null)
        {
            Dictionary<string, List<ServicesAndFeesResponse>> serviceDictionary = new Dictionary<string, List<ServicesAndFeesResponse>>();


            
            var notActiveAcukwikServicesOffered = (customServices == null) ? null : customServices.Where(x => x.ServiceActionType == ServiceActionType.NotActive && x.ServiceTypeId == null);

            await acukwikServicesOffered.ForEachAsync(x =>
                {
                    var serviceType = x.ServiceType;
                    var serviceAndFee = x.Adapt<ServicesAndFeesResponse>();

                    if (notActiveAcukwikServicesOffered != null)
                    {
                        var notactiveService = notActiveAcukwikServicesOffered.Where(cs => cs.Service == x.Service);
                        if (notactiveService.Any())
                        {
                            serviceAndFee.IsActive = false;
                            serviceAndFee.Oid = notactiveService.FirstOrDefault().Oid;
                        }
                    }

                    if (serviceDictionary.ContainsKey(serviceType))
                    {
                        serviceDictionary[serviceType].Add(serviceAndFee);
                    }
                    else
                    {
                        serviceDictionary.Add(serviceType, new List<ServicesAndFeesResponse>() { serviceAndFee });
                    }
                }
            );

            var customService = (customServices == null) ? new List<FboCustomServicesAndFees>() : customServices.Where(x => x.ServiceTypeId == null).ToList();

            foreach (var cs in customService)
            {
                if (cs.AcukwikServicesOfferedId == null) continue;
                var service = await _acukwikServicesOfferedEntityService.FindByComposeKeyAsync((int)cs.AcukwikServicesOfferedId);
                if (service.Service == cs.Service) continue;
                var resouce = cs.Adapt<ServicesAndFeesResponse>();
                resouce.HandlerId = service.HandlerId;
                resouce.ServiceOfferedId = service.ServiceOfferedId;
                serviceDictionary[service.ServiceType].Add(resouce);
            }

            var fbosServicesAndFeesResponseList = new List<FbosServicesAndFeesResponse>();

            foreach (var service in serviceDictionary )
            {
                var fbosServicesAndFeesResponse = new FbosServicesAndFeesResponse()
                {
                    ServiceType =  new ServiceTypeResponse()
                    {
                        IsCustom = false,
                        Name = service.Key,
                        Oid = 0
                    },
                    ServicesAndFees = service.Value
                };
                fbosServicesAndFeesResponseList.Add( fbosServicesAndFeesResponse );
            }
            return fbosServicesAndFeesResponseList;
        }
        private async Task<List<FbosServicesAndFeesResponse>> GetCustomServicesAndFees(List<FbosServicesAndFeesResponse> customServicesAndFees)
        {
            var customServiceTypes = await _fboCustomServiceTypeRepo.Get()
                .Include(x => x.FboCustomServicesAndFees)
                .Include(x => x.CreatedByUser).ToListAsync();


            foreach (var service in customServiceTypes)
            {
                var fbosServicesAndFeesResponse = new FbosServicesAndFeesResponse()
                {
                    ServiceType = new ServiceTypeResponse()
                    {
                        IsCustom = true,
                        Name = service.Name,
                        Oid = service.Oid,
                    },
                    ServicesAndFees = service.FboCustomServicesAndFees.Adapt<List<ServicesAndFeesResponse>>()
                };
                customServicesAndFees.Add(fbosServicesAndFeesResponse);

            }
            return customServicesAndFees;
        }
        public async Task<ServicesAndFeesResponse> Create(int fboId, ServicesAndFeesDto servicesAndFees)
        {
            var entity =  servicesAndFees.Adapt<FboCustomServicesAndFees>();
            entity.ServiceActionType = (servicesAndFees.IsActive) ? ServiceActionType.Active : ServiceActionType.NotActive;
            entity.FboId = fboId;
            entity.Oid = 0;
            var createdEntity =  await _fboCustomServicesAndFeesRepo.AddAsync(entity);
            return createdEntity.Adapt<ServicesAndFeesResponse>();
        }

        public async Task<ServicesAndFeesResponse> Update(int fboId, ServicesAndFeesDto servicesAndFees)
        {
            var customServiceAndfee = await _fboCustomServicesAndFeesRepo.FindAsync(servicesAndFees.Oid);

            if (servicesAndFees.ServiceTypeId != null && customServiceAndfee == null) return null;

            var entity = servicesAndFees.Adapt<FboCustomServicesAndFees>();
            entity.FboId = fboId;

            if (servicesAndFees.ServiceTypeId != null)
            {
                await _fboCustomServicesAndFeesRepo.UpdateAsync(entity);
                return entity.Adapt<ServicesAndFeesResponse>();
            }

            if (customServiceAndfee == null)
            {
                entity.Oid = 0;
                var createdEntity = await _fboCustomServicesAndFeesRepo.AddAsync(entity);

                return createdEntity.Adapt<ServicesAndFeesResponse>();
            }

            var handlerId = servicesAndFees.HandlerId;
            var serviceOfferedId = servicesAndFees.ServiceOfferedId;

            var acukwikService = (handlerId == null && serviceOfferedId == null) ? null : await _acukwikServicesOfferedEntityService.FindByComposeKeyAsync((int)handlerId, (int)serviceOfferedId);

            if (acukwikService?.Service == servicesAndFees.Service)
            {
                await _fboCustomServicesAndFeesRepo.DeleteAsync(entity.Oid);
                return entity.Adapt<ServicesAndFeesResponse>();
            }
            await _fboCustomServicesAndFeesRepo.UpdateAsync(entity);
            return entity.Adapt<ServicesAndFeesResponse>();
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
