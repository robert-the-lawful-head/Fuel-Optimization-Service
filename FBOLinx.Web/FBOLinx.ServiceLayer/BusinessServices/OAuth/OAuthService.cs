using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.OAuth
{
    public interface IOAuthService
    {
        Task<AccessTokens> GenerateAccessToken(DB.Models.User user, int expireMinutes = 60);
        Task<RefreshTokens> GenerateRefreshToken(DB.Models.User user, AccessTokens token);
    }

    public class OAuthService : IOAuthService
    {
        private readonly FboLinxContext _context;
        public OAuthService(FboLinxContext context)
        {
            _context = context;
        }

        public async Task<AccessTokens> GenerateAccessToken(DB.Models.User user, int expireMinutes = 60)
        {
            var accessToken = new AccessTokens
            {
                Oid = 0,
                AccessToken = GetNewToken(),
                CreatedAt = DateTime.UtcNow,
                Expired = DateTime.UtcNow.AddMinutes(expireMinutes),
                UserId = user.Oid
            };
            _context.AccessTokens.Add(accessToken);
            await _context.SaveChangesAsync();

            return accessToken;
        }

        public async Task<RefreshTokens> GenerateRefreshToken(DB.Models.User user, AccessTokens token)
        {
            var refreshToken = new RefreshTokens
            {
                CreatedAt = DateTime.UtcNow,
                Expired = DateTime.UtcNow.AddMonths(3),
                Token = GetNewToken(),
                UserId = user.Oid,
                AccessTokenId = token.Oid
            };
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public string GetNewToken(int size = 32)
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
