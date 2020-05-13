using FBOLinx.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Auth
{
    public interface IRefreshTokenManager
    {
        Task<RefreshTokens> GenerateRefreshToken(User user);
        //Task<ExchangeRefreshTokenResponse> Exchange(ExchangeRefreshTokenRequest request);
    }
}
