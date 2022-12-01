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

namespace FBOLinx.ServiceLayer.BusinessServices.User
{
    public interface IUserService 
    {
        public Task<DB.Models.User> GetUserByClaimedId(int claimedId);
    }

    public class UserService : IUserService
    {
        private FboLinxContext _context;
        public UserService(FboLinxContext context)
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
