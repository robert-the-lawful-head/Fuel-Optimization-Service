using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Web.Models.Responses;
using System;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OAuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly FboLinxContext _context;
        private readonly OAuthService _oAuthService;

        public OAuthController(IUserService userService, OAuthService oAuthService, FboLinxContext context)
        {
            _userService = userService;
            _context = context;
            _oAuthService = oAuthService;
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

            AccessTokens accessToken = await _oAuthService.GenerateAccessToken(user);

            return Ok(accessToken);
        }

        [HttpPost("authtoken")]
        [AllowAnonymous]
        [APIKey(IntegrationPartners.IntegrationPartnerTypes.OtherSoftware)]
        public async Task<ActionResult<AuthTokenResponse>> GenerateAuthTokenFromAccessToken([FromBody] UserAuthTokenFromAccessTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _oAuthService.GenerateAuthToken(request.AccessToken);
            return Ok(response);
        }

        [HttpPost("refreshtoken")]
        [AllowAnonymous]
        [APIKey(IntegrationPartners.IntegrationPartnerTypes.OtherSoftware)]
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