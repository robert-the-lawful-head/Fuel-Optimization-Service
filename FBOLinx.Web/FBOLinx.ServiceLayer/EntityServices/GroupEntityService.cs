using FBOLinx.DB.Context;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class GroupEntityService : FBOLinxBaseEntityService<DB.Models.Group, DTO.GroupDTO, int>, IEntityService<DB.Models.Group, DTO.GroupDTO, int>
    {
        public GroupEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
