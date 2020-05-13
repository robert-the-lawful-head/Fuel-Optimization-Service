using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class AccessTokenService
    {
        private readonly FboLinxContext _context;
        public AccessTokenService(FboLinxContext context)
        {
            _context = context;
        }

        public async Task<AccessTokens> GetUserFromValidToken(string accessToken)
        {
            var result = await _context.AccessTokens.Where(a => a.AccessToken.Equals(accessToken) && a.Expired > DateTime.UtcNow)
                                                .Include(a => a.User)
                                                .FirstOrDefaultAsync();
            return result;
        }
    }
}
