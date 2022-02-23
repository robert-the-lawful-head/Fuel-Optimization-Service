using FBOLinx.DB.Context;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class CustomerEntityService : FBOLinxBaseEntityService<DB.Models.Customers, DTO.CustomerDTO, int>, IEntityService<DB.Models.Customers, DTO.CustomerDTO, int>
    {
        public CustomerEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
