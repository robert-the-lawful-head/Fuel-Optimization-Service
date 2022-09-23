using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IGroupEntityService : IRepository<FBOLinx.DB.Models.Group, FboLinxContext>
    {
    }
    public class GroupEntityService : Repository<FBOLinx.DB.Models.Group, FboLinxContext>, IGroupEntityService
    {
        public GroupEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
