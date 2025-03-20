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
        Task<List<FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees.ServiceTypeResponse>> Get(int fboId);
        Task<FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees.ServiceTypeResponse> Create(int fboId, FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees.ServiceTypeResponse serviceType);
        Task<FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees.ServiceTypeResponse> Update(int serviceTypeId, FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees.ServiceTypeResponse serviceType);
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

        public async Task<FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees.ServiceTypeResponse> Create(int fboId, FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees.ServiceTypeResponse serviceType)
        {
            var entity = serviceType.Adapt<FboCustomServiceType>();
            entity.FboId = fboId;
            return (await _fboCustomEntityTypeRepo.AddAsync(entity)).Adapt<FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees.ServiceTypeResponse>();
        }

        public async Task<bool> Delete(int serviceTypeId)
        {
            var entity = await _fboCustomEntityTypeRepo.Get().Include(x => x.FboCustomServicesAndFees).Where(x => x.Oid == serviceTypeId).FirstOrDefaultAsync();

             await _fboCustomEntityTypeRepo.DeleteAsync(entity);

            return true;
        }

        public async Task<List<FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees.ServiceTypeResponse>> Get(int fboId)
        {
            return (await _fboCustomEntityTypeRepo.Get().ToListAsync()).Adapt<List<FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees.ServiceTypeResponse>>();
        }

        public async Task<FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees.ServiceTypeResponse> Update(int serviceTypeId, FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees.ServiceTypeResponse serviceType)
        {
            var entity = await _fboCustomEntityTypeRepo.FindAsync(serviceTypeId);

            if (entity == null)
                return null;
            entity.Name = serviceType.Name;
            await _fboCustomEntityTypeRepo.UpdateAsync(entity);
            return  entity.Adapt<FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees.ServiceTypeResponse>();
        }
    }
}
