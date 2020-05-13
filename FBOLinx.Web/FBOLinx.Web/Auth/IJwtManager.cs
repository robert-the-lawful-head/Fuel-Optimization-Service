using FBOLinx.Web.Models;
using FBOLinx.Web.Models.Responses;
using FBOLinx.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FBOLinx.Web.Auth
{
    public interface IJwtManager
    {
        string GenerateToken(int id, string name, string username, int? companyId, int expireMinutes = 10080);
        ClaimsPrincipal GetPrincipal(string token);
        Task<UserAuthTokenResponse> GetUserAccessInformation(string username, string password, IUserService userService, IRefreshTokenManager refreshTokenManager);
        Task<UserAuthTokenResponse> GetUserAccessInformation(int userId, IRefreshTokenManager refreshTokenManager);

        Task<User> GetClaimedUser();

        string GetCurrentAuthToken();
    }
}
