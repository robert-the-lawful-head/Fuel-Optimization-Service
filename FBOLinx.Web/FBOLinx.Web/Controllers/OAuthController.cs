using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Web.Models.Responses;
using System;
using FBOLinx.DB.Context;
using FBOLinx.Core.Enums;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.ServiceLayer.BusinessServices.OAuth;
using FBOLinx.Service.Mapping.Dto;
using Microsoft.Extensions.Options;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OAuthController : FBOLinxControllerBase
    {
        private readonly IUserService _userService;
        private readonly FboLinxContext _context;
        private readonly Services.OAuthService _oAuthService;
        private readonly IOAuthService _iOAuthService;
        private SecuritySettings _securitySettings;

        public OAuthController(IUserService userService, Services.OAuthService oAuthService, FboLinxContext context, ILoggingService logger, IOAuthService iOAuthService, IOptions<SecuritySettings> securitySettings) : base(logger)
        {
            _userService = userService;
            _context = context;
            _iOAuthService = iOAuthService;
            _oAuthService = oAuthService;
            _securitySettings = securitySettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]OAuthRequest userParam)
        {
            if (string.IsNullOrEmpty(userParam.Username) || string.IsNullOrEmpty(userParam.Password))
                return BadRequest(new { message = "Username or password is invalid/empty" });

            var user = await _userService.GetUserByCredentials(userParam.Username, userParam.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            var partner = _context.IntegrationPartners.Where(p => p.PartnerId.Equals(new Guid(userParam.PartnerId))).FirstOrDefault();
            if (partner == null)
            {
                return BadRequest(new { message = "Incorrect partner" });
            }

            AccessTokensDto accessToken = await _iOAuthService.GenerateAccessToken(user.Oid, _securitySettings.TokenExpirationInMinutes);

            return Ok(accessToken);
        }

        [HttpPost("authtoken")]
        [AllowAnonymous]
        //[APIKey(IntegrationPartnerTypes.OtherSoftware)]
        public async Task<ActionResult<AuthTokenResponse>> GenerateAuthTokenFromAccessToken([FromBody] UserAuthTokenFromAccessTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = new AuthTokenResponse();
            if (request.AccessToken.Length < 50)
                response = await _oAuthService.GenerateAuthToken(request.AccessToken);
            else
                response = _oAuthService.CheckAuthToken(request.AccessToken);
            return Ok(response);
        }

        [HttpPost("refreshtoken")]
        [AllowAnonymous]
        [APIKey(IntegrationPartnerTypes.OtherSoftware)]
        public async Task<ActionResult<ExchangeRefreshTokenResponse>> RefreshAccessToken([FromBody] ExchangeRefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _oAuthService.ExchangeRefreshToken(request);
            return Ok(response);
        }
    }
}