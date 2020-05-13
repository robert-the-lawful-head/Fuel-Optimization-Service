using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Models.Responses;
using FBOLinx.Web.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FBOLinx.Web.Auth
{
    public class RefreshTokenManager : IRefreshTokenManager
    {
        private IJwtManager _JWTManager;
        private FboLinxContext _context;

        public RefreshTokenManager(IJwtManager jwtManager, FboLinxContext context)
        {
            _context = context;
            _JWTManager = jwtManager;
        }

        public async Task<RefreshTokens> GenerateRefreshToken(User user)
        {
            var refreshToken = new RefreshTokens
            {
                CreatedAt = DateTime.UtcNow,
                Expired = DateTime.UtcNow.AddMonths(3),
                Token = GenerateToken(),
                UserId = user.Oid
            };
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<ExchangeRefreshTokenResponse> Exchange(ExchangeRefreshTokenRequest request)
        {
            var claimsPrincipal = _JWTManager.GetPrincipal(request.AuthToken);

            if (claimsPrincipal != null)
            {
                var claimedId = Convert.ToInt32(claimsPrincipal.Claims.First((c => c.Type == "UserID")));
                var user = await _context.User.FindAsync(claimedId);
                var validRefreshToken = await _context.RefreshTokens.Where(r => r.UserId.Equals(user.Oid) && r.Token.Equals(request.RefreshToken)).FirstAsync();
                if (validRefreshToken != null && validRefreshToken.Oid > 0)
                {
                    var jwtToken = _JWTManager.GenerateToken(user.Oid, string.Format("{0} {1}", user.FirstName, user.LastName), user.Username, user.FboId);
                    var refreshToken = await GenerateRefreshToken(user);
                    _context.RefreshTokens.Remove(validRefreshToken);
                    await _context.SaveChangesAsync();
                    return new ExchangeRefreshTokenResponse(jwtToken, DateTime.UtcNow.AddMinutes(10080), refreshToken.Token, refreshToken.Expired, true);

                }
            }
            return new ExchangeRefreshTokenResponse(false, "Invalid refresh token.  Please re-authenticate the user.");
        }

        private string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
