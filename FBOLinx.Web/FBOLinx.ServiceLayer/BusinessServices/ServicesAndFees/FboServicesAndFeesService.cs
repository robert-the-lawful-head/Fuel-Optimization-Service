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
        private IFboEntityService _fboEntityService;
        private IAcukwikServicesOfferedEntityService _acukwikServicesOfferedEntityService;

        public FboServicesAndFeesService(
            IRepository<FboCustomServicesAndFees, FboLinxContext> fboCustomServicesAndFeesRepo,
            IAcukwikServicesOfferedEntityService acukwikServicesOfferedEntityService,
            IFboEntityService fboEntityService)
        {
            _acukwikServicesOfferedEntityService = acukwikServicesOfferedEntityService;
            _fboCustomServicesAndFeesRepo = fboCustomServicesAndFeesRepo;
            _fboEntityService = fboEntityService;
        }

        public async Task<List<FbosServicesAndFeesResponse>> Get(int fboId)
        {
            var fbo = await _fboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));

            if (fbo == null)
                return null;

            var modifiedServices = _fboCustomServicesAndFeesRepo.Where(x => x.FboId == fboId).Include(p => p.ServiceType);
            var acukwikServicesOffered = _acukwikServicesOfferedEntityService.Where(x => x.HandlerId == (int)fbo.AcukwikFBOHandlerId);

            if (!modifiedServices.Any())
            {
                return await GetfromAcukwikServicesOffered(acukwikServicesOffered);
            }
            return await GetMergedDataFromAcukwikServicesOfferedAndCustomServicesAndFeesRepo(modifiedServices,acukwikServicesOffered);
            
        }
        private async Task<List<FbosServicesAndFeesResponse>> GetfromAcukwikServicesOffered(IQueryable<AcukwikServicesOffered> acukwikServicesOffered, IQueryable<string> notActiveServices = null)
        {
            Dictionary<string, List<ServicesAndFeesResponse>> serviceDictionary = new Dictionary<string, List<ServicesAndFeesResponse>>();

            await acukwikServicesOffered.ForEachAsync(x =>
                {
                    var serviceType = x.ServiceType;
                    var serviceAndFee = x.Adapt<ServicesAndFeesResponse>();

                    if(notActiveServices != null || notActiveServices.Count() > 0)
                        serviceAndFee.IsActive = !notActiveServices.Contains((x.HandlerId.ToString() + x.ServiceOfferedId.ToString()));

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
            var notActiveAcukwikServicesOffered = customServices.Where(x => x.ServiceActionType == ServiceActionType.NotActive && x.AcukwikServicesOfferedId != null).Select(x => x.AcukwikServicesOfferedId.ToString());

            var fbosServicesAndFeesResponseList = await this.GetfromAcukwikServicesOffered(acukwikServicesOffered,notActiveAcukwikServicesOffered);

            customServices = customServices.Where(x => x.AcukwikServicesOfferedId == null);

            Dictionary<string, List<ServicesAndFeesResponse>> serviceDictionary = new Dictionary<string, List<ServicesAndFeesResponse>>();

            await customServices.ForEachAsync(x =>
                {
                    var serviceType = x.ServiceType?.Name;
                    var serviceAndFee = x.Adapt<ServicesAndFeesResponse>();

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

            foreach (var service in serviceDictionary)
            {
                var fbosServicesAndFeesResponse = new FbosServicesAndFeesResponse()
                {
                    ServiceType = new ServiceTypeResponse()
                    {
                        IsCustom = false,
                        Name = service.Key,
                        Oid = 0
                    },
                    ServicesAndFees = service.Value
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

            var handlerId = servicesAndFees.HandlerId;
            var serviceOfferedId = servicesAndFees.ServiceOfferedId;

            AcukwikServicesOffered acukwikService = null;

            if (handlerId != null && serviceOfferedId != null)
            {
                acukwikService = await _acukwikServicesOfferedEntityService.FindByComposeKeyAsync((int)handlerId, (int)serviceOfferedId);
            }

            //new custom service
            if (acukwikService == null && customServiceAndfee == null)
            {
                var entity = servicesAndFees.Adapt<FboCustomServicesAndFees>();
                entity.Oid = 0;
                entity.FboId = fboId;
                entity.ServiceActionType = (servicesAndFees.IsActive) ? ServiceActionType.Active : ServiceActionType.NotActive;
                var createdEntity = await _fboCustomServicesAndFeesRepo.AddAsync(entity);

                return createdEntity.Adapt<ServicesAndFeesDto>();
            }
            //Update custom service
            if (acukwikService == null && customServiceAndfee != null)
            {
                var entity = servicesAndFees.Adapt<FboCustomServicesAndFees>();
                entity.ServiceActionType = (servicesAndFees.IsActive) ? ServiceActionType.Active : ServiceActionType.NotActive;
                entity.FboId = fboId;
                await _fboCustomServicesAndFeesRepo.UpdateAsync(entity);

                return entity.Adapt<ServicesAndFeesDto>();
            }
            //delete not active acuckwick record
            if (acukwikService != null && customServiceAndfee != null)
            {
                await _fboCustomServicesAndFeesRepo.DeleteAsync(servicesAndFees.Oid);
                return servicesAndFees;
            }
            //add not active acuckwick record
            if (acukwikService != null && customServiceAndfee == null)
            {
                var updatedEntity = servicesAndFees.Adapt<FboCustomServicesAndFees>();
                updatedEntity.ServiceActionType = (servicesAndFees.IsActive) ? ServiceActionType.Active : ServiceActionType.NotActive;
                updatedEntity.FboId = fboId;
                updatedEntity.Oid = 0;
                updatedEntity.AcukwikServicesOfferedId = int.Parse(servicesAndFees?.HandlerId.ToString() + servicesAndFees?.ServiceOfferedId.ToString());
                var createdEntity = await _fboCustomServicesAndFeesRepo.AddAsync(updatedEntity);

                return createdEntity.Adapt<ServicesAndFeesDto>();
            }

            return null;
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
