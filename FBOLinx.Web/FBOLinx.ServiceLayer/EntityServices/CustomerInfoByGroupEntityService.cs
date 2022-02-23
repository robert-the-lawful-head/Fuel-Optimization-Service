using FBOLinx.DB.Context;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class CustomerInfoByGroupEntityService : FBOLinxBaseEntityService<DB.Models.CustomerInfoByGroup, DTO.CustomerInfoByGroupDTO, int>, IEntityService<DB.Models.CustomerInfoByGroup, DTO.CustomerInfoByGroupDTO, int>
    {
        public CustomerInfoByGroupEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
