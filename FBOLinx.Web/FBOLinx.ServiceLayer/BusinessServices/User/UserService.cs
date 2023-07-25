using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.User
{
    public interface IUserService : IBaseDTOService<UserDTO, DB.Models.User>
    {
        public Task<DB.Models.User> GetUserByClaimedId(int claimedId);
    }

    public class UserService :
        BaseDTOService<UserDTO, DB.Models.User, FboLinxContext>, IUserService
    {
        public UserService(IRepository<DB.Models.User, FboLinxContext> entityService) : base(
             entityService)
        {
        }

        public async Task<DB.Models.User> GetUserByClaimedId(int claimedId)
        {
            var user = await _EntityService.FindAsync(claimedId);
            return user;
        }
    }
}