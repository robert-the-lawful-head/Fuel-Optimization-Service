using FBOLinx.Web.Auth;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System.Security.Claims;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.DB.Specifications.Group;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;

namespace FBOLinx.Web.Services
{
    public class OAuthService
    {
        private readonly FboLinxContext _context;
        private readonly JwtManager _jwtManager;
        private readonly IGroupService _groupService;
        private readonly IFboService _fboService;

        public OAuthService(FboLinxContext context, JwtManager jwtManager, IGroupService groupService, IFboService fboService)
        {
            _context = context;
            _jwtManager = jwtManager;
            _groupService = groupService;
            _fboService = fboService;
        }

        public async Task<AccessTokens> GenerateAccessToken(User user, int expireMinutes = 60)
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

        public async Task<AuthTokenResponse> GenerateAuthToken(string accessToken)
        {
            var token = await _context.AccessTokens.Where(a => a.AccessToken.Equals(accessToken) && a.Expired > DateTime.UtcNow)
                                                .Include(a => a.User)
                                                .FirstOrDefaultAsync();
            if (token == null || token.User == null)
            {
                return new AuthTokenResponse(false,
                    "The provided access token is invalid.  Please ensure you are using the token within 1 hour of receiving it.");
            }
            var user = token.User;

            var authToken =  _jwtManager.GenerateToken(user.Oid, user.FboId, user.Role, user.GroupId);

            var refreshToken = await GenerateRefreshToken(user, token);

            var group = await _groupService.GetSingleBySpec(new GroupByGroupIdSpecification(user.GroupId.Value));
            var fbo = await _fboService.GetFbo(user.FboId);

            return new AuthTokenResponse(authToken, DateTime.UtcNow.AddMinutes(10080), refreshToken.Token, refreshToken.Expired, user.Username, user.FboId, group.GroupName, user.GroupId.Value, user.Role, fbo.FboAirport.Icao, fbo.Fbo);
        }

        public async Task<ExchangeRefreshTokenResponse> ExchangeRefreshToken(ExchangeRefreshTokenRequest request)
        {
            var claimsPrincipal = _jwtManager.GetPrincipal(request.AuthToken);

            if (claimsPrincipal == null)
            {
                return new ExchangeRefreshTokenResponse(false, "Invalid refresh token.  Please re-authenticate the user.");
            }

            var claimedId = Convert.ToInt32(claimsPrincipal.Claims.First((c => c.Type == ClaimTypes.NameIdentifier)).Value);
            var user = await _context.User.FindAsync(claimedId);
            var oldRefreshToken = await _context.RefreshTokens
                                                        .Where(r => r.UserId.Equals(user.Oid) && r.Token.Equals(request.RefreshToken))
                                                        .FirstOrDefaultAsync();

            if (oldRefreshToken == null)
            {
                return new ExchangeRefreshTokenResponse(false, "Invalid refresh token.  Please re-authenticate the user.");
            }

            AccessTokens oldToken = await _context.AccessTokens.FindAsync(oldRefreshToken.AccessTokenId);

            AccessTokens accessToken = await GenerateAccessToken(user, 10080);

            string authToken = _jwtManager.GenerateToken(user.Oid, user.FboId, user.Role, user.GroupId);

            RefreshTokens refreshToken = await GenerateRefreshToken(user, accessToken);

            _context.AccessTokens.Remove(oldToken);
            _context.RefreshTokens.Remove(oldRefreshToken);
            await _context.SaveChangesAsync();

            var group = await _groupService.GetSingleBySpec(new GroupByGroupIdSpecification(user.GroupId.Value));
            var fbo = await _fboService.GetFbo(user.FboId);

            return new ExchangeRefreshTokenResponse(authToken, DateTime.UtcNow.AddMinutes(10080), refreshToken.Token, refreshToken.Expired, group.GroupName, user.GroupId.Value, user.Role, fbo.FboAirport.Icao, fbo.Fbo, true);
        }

        public async Task<RefreshTokens> GenerateRefreshToken(User user, AccessTokens token)
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
