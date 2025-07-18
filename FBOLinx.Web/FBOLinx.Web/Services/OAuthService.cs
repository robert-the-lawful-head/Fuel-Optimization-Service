﻿using FBOLinx.Web.Auth;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System.Security.Claims;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.DB.Specifications.Group;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.OAuth;
using FBOLinx.DB.Specifications.User;
using FBOLinx.Service.Mapping.Dto;
using Microsoft.Extensions.Options;
using FBOLinx.Web.Configurations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;

namespace FBOLinx.Web.Services
{
    public class OAuthService
    {
        private readonly FboLinxContext _context;
        private readonly JwtManager _jwtManager;
        private readonly IGroupService _groupService;
        private readonly IFboService _fboService;
        private readonly IOAuthService _IOAuthService;
        private readonly FBOLinx.ServiceLayer.BusinessServices.User.IUserService _userService;
        private SecuritySettings _securitySettings;


        public OAuthService(FboLinxContext context, JwtManager jwtManager, IGroupService groupService, IFboService fboService, IOAuthService oAuthService, FBOLinx.ServiceLayer.BusinessServices.User.IUserService userService, IOptions<SecuritySettings> securitySettings)
        {
            _context = context;
            _jwtManager = jwtManager;
            _groupService = groupService;
            _fboService = fboService;
            _IOAuthService = oAuthService;
            _userService = userService;
            _securitySettings = securitySettings.Value;
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

            var authToken =  _jwtManager.GenerateToken(token.User.Oid, token.User.FboId, token.User.Role, token.User.GroupId);

            var refreshToken = await _IOAuthService.GenerateRefreshToken(token.User.Oid, token.Oid);

            var group = await _groupService.GetSingleBySpec(new GroupByGroupIdSpecification(token.User.GroupId.Value));
            var fbo = await _fboService.GetFbo(token.User.FboId);

            return new AuthTokenResponse(authToken, DateTime.UtcNow.AddMinutes(_securitySettings.TokenExpirationInMinutes), refreshToken.Token, refreshToken.Expired, token.User.Username, token.User.FboId, group.GroupName, token.User.GroupId.Value, token.User.Role, fbo.FboAirport.Icao, fbo.Fbo);
        }

        public AuthTokenResponse CheckAuthToken(string authToken)
        {
            var claimsPrincipal = _jwtManager.GetPrincipal(authToken);
            if (claimsPrincipal == null)
            {
                return new AuthTokenResponse(false, "Invalid access token.  Please re-authenticate the user.");
            }

            var role = Convert.ToInt32(claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.Role).Value);
            if (role != 3)
                return new AuthTokenResponse(false, "Invalid access token.  Please re-authenticate the user.");

            return new AuthTokenResponse((Core.Enums.UserRoles) role, authToken);
        }

        public async Task<ExchangeRefreshTokenResponse> ExchangeRefreshToken(ExchangeRefreshTokenRequest request)
        {
            var claimsPrincipal = _jwtManager.GetPrincipal(request.AuthToken);
            
            if (claimsPrincipal == null)
            {
                return new ExchangeRefreshTokenResponse(false, "Invalid refresh token.  Please re-authenticate the user.");
            }

            var claimedId = Convert.ToInt32(claimsPrincipal.Claims.First((c => c.Type == ClaimTypes.NameIdentifier)).Value);
            var user = await _userService.GetSingleBySpec(new UserByOidSpecification(claimedId));
            var oldRefreshToken = await _context.RefreshTokens
                                                        .Where(r => r.UserId.Equals(user.Oid) && r.Token.Equals(request.RefreshToken))
                                                        .FirstOrDefaultAsync();

            if (oldRefreshToken == null)
            {
                return new ExchangeRefreshTokenResponse(false, "Invalid refresh token.  Please re-authenticate the user.");
            }

            AccessTokens oldToken = await _context.AccessTokens.FindAsync(oldRefreshToken.AccessTokenId);

            AccessTokensDto accessToken = await _IOAuthService.GenerateAccessToken(user.Oid, _securitySettings.TokenExpirationInMinutes);

            string authToken = _jwtManager.GenerateToken(user.Oid, user.FboId, user.Role, user.GroupId);

            RefreshTokensDto refreshToken = await _IOAuthService.GenerateRefreshToken(user.Oid, accessToken?.Oid ?? user.Oid);

            if(oldToken != null) _context.AccessTokens.Remove(oldToken);
            _context.RefreshTokens.Remove(oldRefreshToken);
            await _context.SaveChangesAsync();

            var group = await _groupService.GetSingleBySpec(new GroupByGroupIdSpecification(user.GroupId.Value));
            var fbo = await _fboService.GetFbo(user.FboId);

            return new ExchangeRefreshTokenResponse(authToken, DateTime.UtcNow.AddMinutes(_securitySettings.TokenExpirationInMinutes), refreshToken.Token, refreshToken.Expired, group.GroupName, user.GroupId.Value, user.Role, fbo?.FboAirport?.Icao, fbo?.Fbo, true);
        }
    }
}
