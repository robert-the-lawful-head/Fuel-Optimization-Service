using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Orders;
using FBOLinx.ServiceLayer.DTO;
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
        private FboLinxContext _context;
        public UserService(IRepository<DB.Models.User, FboLinxContext> entityService, FboLinxContext context) : base(
             entityService)
        {
            _context = context;
        }

        public async Task<DB.Models.User> GetUserByClaimedId(int claimedId)
        {
            var user = await _context.User.FindAsync(claimedId);
            return user;
        }
    }
}
