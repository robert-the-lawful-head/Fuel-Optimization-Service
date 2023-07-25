using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Orders;
using FBOLinx.ServiceLayer.BusinessServices.RefreshTokens;
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
        Task<AccessTokensDto> GenerateAccessToken(UserDTO user, int expireMinutes = 60);
        Task<RefreshTokensDto> GenerateRefreshToken(int userId, int tokenId);
    }

    public class OAuthService : IOAuthService
    {
        private readonly FboLinxContext _context;
        private readonly IAccessTokensService _accessTokensService;
        private readonly IRefreshTokensService _refreshTokensService;

        public OAuthService(FboLinxContext context, IAccessTokensService accessTokensService, IRefreshTokensService refreshTokensService)
        {
            _context = context;
            _accessTokensService = accessTokensService;
            _refreshTokensService = refreshTokensService;
        }

        public async Task<AccessTokensDto> GenerateAccessToken(UserDTO user, int expireMinutes = 60)
        {
            var accessToken = new AccessTokensDto
            {
                Oid = 0,
                AccessToken = GetNewToken(),
                CreatedAt = DateTime.UtcNow,
                Expired = DateTime.UtcNow.AddMinutes(expireMinutes),
                UserId = user.Oid
            };

            await _accessTokensService.AddAsync(accessToken);

            return accessToken;
        }

        public async Task<RefreshTokensDto> GenerateRefreshToken(int userId, int tokenId)
        {
            var refreshToken = new RefreshTokensDto
            {
                CreatedAt = DateTime.UtcNow,
                Expired = DateTime.UtcNow.AddMonths(3),
                Token = GetNewToken(),
                UserId = userId,
                AccessTokenId = tokenId
            };
            await _refreshTokensService.AddAsync(refreshToken);
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
