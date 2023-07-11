using FBOLinx.DB.Context;
using FBOLinx.DB.Models.ServicesAndFees;
using FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees;
using FBOLinx.ServiceLayer.EntityServices;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.ServicesAndFees
{
    public interface IFboServiceTypeService
    {
        Task<List<ServiceTypeResponse>> Get(int fboId);
        Task<ServiceTypeResponse> Create(int fboId, ServiceTypeResponse serviceType);
        Task<ServiceTypeResponse> Update(int serviceTypeId, ServiceTypeResponse serviceType);
        Task<bool> Delete(int serviceTypeId);
    }
    public class FboServiceTypeService : IFboServiceTypeService
    {
        private IRepository<FboCustomServiceType, FboLinxContext> _fboCustomEntityTypeRepo;

        public FboServiceTypeService(
            IRepository<FboCustomServiceType, FboLinxContext> fboCustomEntityTypeRepo
        )
        {
            _fboCustomEntityTypeRepo = fboCustomEntityTypeRepo;
        }

        public async Task<ServiceTypeResponse> Create(int fboId, ServiceTypeResponse serviceType)
        {
            return (await _fboCustomEntityTypeRepo.AddAsync(serviceType.Adapt<FboCustomServiceType>())).Adapt<ServiceTypeResponse>();
        }

        public async Task<bool> Delete(int serviceTypeId)
        {
            return (await _fboCustomEntityTypeRepo.DeleteAsync(serviceTypeId) == null) ? false : true;
        }

        public async Task<List<ServiceTypeResponse>> Get(int fboId)
        {
            return (await _fboCustomEntityTypeRepo.Get().ToListAsync()).Adapt<List<ServiceTypeResponse>>();
        }

        public async Task<ServiceTypeResponse> Update(int serviceTypeId, ServiceTypeResponse serviceType)
        {
            var entity = serviceType.Adapt<FboCustomServiceType>();
            await _fboCustomEntityTypeRepo.UpdateAsync(entity);
            return  entity.Adapt<ServiceTypeResponse>();
        }
    }
}
