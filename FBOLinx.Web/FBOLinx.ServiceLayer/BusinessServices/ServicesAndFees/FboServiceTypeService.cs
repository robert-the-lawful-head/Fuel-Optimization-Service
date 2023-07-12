using FBOLinx.DB.Context;
using FBOLinx.DB.Models.ServicesAndFees;
using FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
            var entity = serviceType.Adapt<FboCustomServiceType>();
            entity.FboId = fboId;
            return (await _fboCustomEntityTypeRepo.AddAsync(entity)).Adapt<ServiceTypeResponse>();
        }

        public async Task<bool> Delete(int serviceTypeId)
        {
            var entity = await _fboCustomEntityTypeRepo.Get().Include(x => x.FboCustomServicesAndFees).Where(x => x.Oid == serviceTypeId).FirstOrDefaultAsync();

             await _fboCustomEntityTypeRepo.DeleteAsync(entity);

            return true;
        }

        public async Task<List<ServiceTypeResponse>> Get(int fboId)
        {
            return (await _fboCustomEntityTypeRepo.Get().ToListAsync()).Adapt<List<ServiceTypeResponse>>();
        }

        public async Task<ServiceTypeResponse> Update(int serviceTypeId, ServiceTypeResponse serviceType)
        {
            var entity = await _fboCustomEntityTypeRepo.FindAsync(serviceTypeId);

            if (entity == null)
                return null;
            entity.Name = serviceType.Name;
            await _fboCustomEntityTypeRepo.UpdateAsync(entity);
            return  entity.Adapt<ServiceTypeResponse>();
        }
    }
}
