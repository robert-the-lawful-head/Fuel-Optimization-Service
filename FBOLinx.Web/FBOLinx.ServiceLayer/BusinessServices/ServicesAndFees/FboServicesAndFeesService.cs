using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models.Dega;
using FBOLinx.DB.Models.ServicesAndFees;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees;
using FBOLinx.ServiceLayer.DTO.ServicesAndFees;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.ServicesAndFees
{
    public interface IFboServicesAndFeesService
    {
        Task<List<FbosServicesAndFeesResponse>> Get(int fboId);
        Task<ServicesAndFeesDto> Create(int fboId, ServicesAndFeesDto servicesAndFees);
        Task<ServicesAndFeesDto> Update(int fboId, ServicesAndFeesDto servicesAndFees);
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
            return await GetMergedDataFromAcukwikServicesOfferedAndCustomServicesAndFeesRepo(modifiedServices,acukwikServicesOffered);
            
        }
        private async Task<List<FbosServicesAndFeesResponse>> GetfromAcukwikServicesOffered(IQueryable<AcukwikServicesOffered> acukwikServicesOffered,IQueryable<FboCustomServicesAndFees> customServices = null)
        {
            Dictionary<string, List<ServicesAndFeesResponse>> serviceDictionary = new Dictionary<string, List<ServicesAndFeesResponse>>();

            
            var notActiveAcukwikServicesOffered = (customServices == null) ? null : customServices.Where(x => x.ServiceActionType == ServiceActionType.NotActive).Select(x => x.AcukwikServicesOfferedId.ToString());

            await acukwikServicesOffered.ForEachAsync(x =>
                {
                    var serviceType = x.ServiceType;
                    var serviceAndFee = x.Adapt<ServicesAndFeesResponse>();

                    if(notActiveAcukwikServicesOffered != null || notActiveAcukwikServicesOffered?.Count() > 0)
                        serviceAndFee.IsActive = !notActiveAcukwikServicesOffered.Contains((x.HandlerId.ToString() + x.ServiceOfferedId.ToString()));

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

            var customService = (customServices == null) ? new List<FboCustomServicesAndFees>() : customServices.Where(x => x.ServiceActionType != ServiceActionType.NotActive).ToList();

            foreach(var cs in customService)
            {
                var service = await _acukwikServicesOfferedEntityService.FindByComposeKeyAsync((int)cs.AcukwikServicesOfferedId);
                serviceDictionary[service.ServiceType].Add(cs.Adapt<ServicesAndFeesResponse>());
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
        private async Task<List<FbosServicesAndFeesResponse>> GetMergedDataFromAcukwikServicesOfferedAndCustomServicesAndFeesRepo(IQueryable<FboCustomServicesAndFees> customServices, IQueryable<AcukwikServicesOffered> acukwikServicesOffered)
        {
            var customAcukwikServicesOffered = customServices.Where(x => x.ServiceTypeId == null);
            var fbosServicesAndFeesResponseList = await this.GetfromAcukwikServicesOffered(acukwikServicesOffered, customAcukwikServicesOffered);

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
                fbosServicesAndFeesResponseList.Add(fbosServicesAndFeesResponse);

            }
            return fbosServicesAndFeesResponseList;
        }
        public async Task<ServicesAndFeesDto> Create(int fboId, ServicesAndFeesDto servicesAndFees)
        {
            var entity =  servicesAndFees.Adapt<FboCustomServicesAndFees>();
            entity.ServiceActionType = (servicesAndFees.IsActive) ? ServiceActionType.Active : ServiceActionType.NotActive;
            entity.FboId = fboId;
            entity.Oid = 0;
            var createdEntity =  await _fboCustomServicesAndFeesRepo.AddAsync(entity);

            return createdEntity.Adapt<ServicesAndFeesDto>();
        }

        public async Task<ServicesAndFeesDto> Update(int fboId, ServicesAndFeesDto servicesAndFees)
        {
            var customServiceAndfee = await _fboCustomServicesAndFeesRepo.FindAsync(servicesAndFees.Oid);

            if (servicesAndFees.ServiceTypeId != null && customServiceAndfee == null) return null;

            var entity = servicesAndFees.Adapt<FboCustomServicesAndFees>();
            entity.FboId = fboId;
            entity.AcukwikServicesOfferedId = (customServiceAndfee == null) ? null: customServiceAndfee.AcukwikServicesOfferedId;

            if (servicesAndFees.ServiceTypeId != null)
            {
                await _fboCustomServicesAndFeesRepo.UpdateAsync(entity);
                return entity.Adapt<ServicesAndFeesDto>();
            }

            if (customServiceAndfee == null)
            {
                entity.Oid = 0;
                var createdEntity = await _fboCustomServicesAndFeesRepo.AddAsync(entity);

                return createdEntity.Adapt<ServicesAndFeesDto>();
            }


            var handlerId = servicesAndFees.HandlerId;
            var serviceOfferedId = servicesAndFees.ServiceOfferedId;

            var acukwikService = (handlerId == null && serviceOfferedId == null) ? null : await _acukwikServicesOfferedEntityService.FindByComposeKeyAsync((int)handlerId, (int)serviceOfferedId);

            if (acukwikService?.Service == servicesAndFees.Service)
            {
                await _fboCustomServicesAndFeesRepo.DeleteAsync(entity.Oid);
                return entity.Adapt<ServicesAndFeesDto>();
            }
            await _fboCustomServicesAndFeesRepo.UpdateAsync(entity);
            return entity.Adapt<ServicesAndFeesDto>();
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
