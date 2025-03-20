using FBOLinx.Web.Auth;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Auth;
using System.Web;
using FBOLinx.Core.Enums;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.DB.Specifications.User;
using FBOLinx.ServiceLayer.BusinessServices.OAuth;
using Mapster;
using Microsoft.Extensions.Options;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.DB.Specifications;
using System.Collections.Generic;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : FBOLinxControllerBase
    {
        private SecuritySettings _securitySettings;
        private readonly Services.IUserService _userService;
        private readonly IOAuthService _iOAuthService;
        private readonly Services.OAuthService _oAuthService;
        private readonly FboLinxContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFboService _fboService;
        private IEncryptionService _encryptionService;
        private ResetPasswordService _ResetPasswordService;
        private IPricingTemplateService _pricingTemplateService;
        private readonly ServiceLayer.BusinessServices.User.IUserService _userBusinessService;
        private readonly IIntegrationStatusService _IntegrationStatusService;
        private IRepository<IntegrationPartners, FboLinxContext> _IntegrationPartners;
        public UsersController(Services.IUserService userService, FboLinxContext context, IHttpContextAccessor httpContextAccessor, IFboService fboService, IEncryptionService encryptionService, ResetPasswordService resetPasswordService, IPricingTemplateService pricingTemplateService, ILoggingService logger, ServiceLayer.BusinessServices.User.IUserService userBusinessService, IOAuthService iOAuthService, Services.OAuthService oAuthService, IOptions<SecuritySettings> securitySettings, IIntegrationStatusService integrationStatusService, IRepository<IntegrationPartners, FboLinxContext> integrationPartners) : base(logger)
        {
            _ResetPasswordService = resetPasswordService;
            _encryptionService = encryptionService;
            _userService = userService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _fboService = fboService;
            _pricingTemplateService = pricingTemplateService;
            _userBusinessService = userBusinessService;
            _iOAuthService = iOAuthService;
            _oAuthService = oAuthService;
            _securitySettings = securitySettings.Value;
            _IntegrationStatusService = integrationStatusService;
            _IntegrationPartners = integrationPartners;
        }

        [AllowAnonymous]
        [HttpGet("prepare-token-auth")]
        public async Task<IActionResult> PrepareTokenAuthentication([FromQuery] string token)
        {
            SetHttpOnlyCookie(_securitySettings.TokenKey, token, _securitySettings.TokenExpirationInMinutes);

            //var user = await _userBusinessService.GetSingleBySpec(new UserByOidSpecification(JwtManager.GetClaimedUserId(_httpContextAccessor)));
            //var fbo = await HandlePreLoginEvents(user.FboId, user.GroupId.GetValueOrDefault());
            //user.Fbo.Cast(fbo); 

            return Ok();
        }

        [HttpGet("current")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            var user = await _userBusinessService.GetSingleBySpec(new UserByOidSpecification(JwtManager.GetClaimedUserId(_httpContextAccessor)));
            var fbo = await HandlePreLoginEvents(user.FboId, user.GroupId.GetValueOrDefault());
            user.Fbo=fbo.Adapt<FbosDto>();

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] UserDTO userParam)
        {
            if (string.IsNullOrEmpty(userParam.Username) || string.IsNullOrEmpty(userParam.Password))
                return BadRequest(new { message = "Username or password is invalid/empty" });

            var user = await _userService.GetUserByCredentials(userParam.Username, userParam.Password, authenticate: true);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            user.Fbo = await HandlePreLoginEvents(user.FboId, user.GroupId.GetValueOrDefault());

            SetHttpOnlyCookie(_securitySettings.TokenKey, user.Token, _securitySettings.TokenExpirationInMinutes);

            if (userParam.Remember)
            {
                await _userService.SetAppUserRefreshTokens(user);
                SetHttpOnlyCookie(_securitySettings.RefreshTokenKey, user.RefreshToken.Token, _securitySettings.RefreshTokenExpirationInMinutes);
            }

            var userDto = user.Adapt<UserDTO>();
            userDto.Remember = userParam.Remember;

            if (userDto.Role == UserRoles.Conductor)
                userDto.AuthToken = user.Token;

            return Ok(userDto);
        }
        private void SetHttpOnlyCookie(string cookieName, string value, int expirationInMinutes)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes)
            };

            Response.Cookies.Append(cookieName, value, cookieOptions);
        }
        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var currentRole = JwtManager.GetClaimedRole(_httpContextAccessor);

            if (currentRole == UserRoles.Conductor)
                return Ok(user);

            if (JwtManager.GetClaimedUserId(_httpContextAccessor) == id)
                return Ok(user);

            if (currentRole == UserRoles.GroupAdmin &&
                JwtManager.GetClaimedGroupId(_httpContextAccessor) == user.GroupId)
                return Ok(user);

            if (currentRole == UserRoles.Primary &&
                JwtManager.GetClaimedFboId(_httpContextAccessor) == user.FboId)
                return Ok(user);

            return Unauthorized();
        }

        // GET: api/users/group/5
        [HttpGet("group/{groupId}")]
        [UserRole(new UserRoles[] { UserRoles.Conductor, UserRoles.GroupAdmin })]
        public async Task<IActionResult> GetUsersByGroupId([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = await _context.Group.FindAsync(groupId);

            if (group != null)
                await _userService.CreateGroupLoginIfNeeded(group);

            var users = await _context.User.Where((x => x.GroupId == groupId && x.FboId == 0)).ToListAsync();

            return Ok(users);
        }

        // GET: api/users/fbo/5
        [HttpGet("fbo/{fboId}")]
        [UserRole(new UserRoles[] { UserRoles.Conductor, UserRoles.GroupAdmin, UserRoles.Primary })]
        public async Task<IActionResult> GetUsersByFboId([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbo = await _context.Fbos.FindAsync(fboId);

            if (fbo != null)
                await _userService.CreateFBOLoginIfNeeded(fbo);

            var users = await _context.User.Where((x => x.FboId == fboId)).ToListAsync();

            return Ok(users);
        }

        // GET: api/users/roles
        [HttpGet("roles")]
        public IActionResult GetUserRoles()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roles = FBOLinx.Core.Utilities.Enums.EnumHelper.GetDescriptions(typeof(UserRoles));

            return Ok(roles);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Oid)
            {
                return BadRequest();
            }

            if (id != JwtManager.GetClaimedUserId(_httpContextAccessor) && JwtManager.GetClaimedRole(_httpContextAccessor) != UserRoles.Conductor && JwtManager.GetClaimedGroupId(_httpContextAccessor) != user.GroupId)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/users
        [HttpPost]
        [UserRole(UserRoles.Conductor, UserRoles.GroupAdmin, UserRoles.Primary)]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (JwtManager.GetClaimedRole(_httpContextAccessor) != UserRoles.Conductor && JwtManager.GetClaimedGroupId(_httpContextAccessor) != user.GroupId)
            {
                return Unauthorized();
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Oid }, user);
        }

        [AllowAnonymous]
        [HttpPost("request-reset-password")]
        public async Task<IActionResult> RequestResetPassword([FromBody] UserRequireResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.User.FirstOrDefault(u => u.Username.Equals(request.Email));
            if (user != null)
            {
                var rand = new Random();
                byte[] bytes = new byte[32];
                rand.NextBytes(bytes);

                string token = Convert.ToBase64String(bytes);

                user.ResetPasswordToken = token;
                user.ResetPasswordTokenExpiration = DateTime.UtcNow.AddDays(7);
                await _context.SaveChangesAsync();

                await _ResetPasswordService.SendResetPasswordEmailAsync(user.FirstName + " " + user.LastName, user.Username, HttpUtility.UrlEncode(token));
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("validate-reset-password-token")]
        public IActionResult ValidateResetPasswordToken([FromBody] UserResetPasswordValidateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.User.FirstOrDefault(u => u.ResetPasswordToken.Equals(request.Token));
            if (user == null || user.ResetPasswordTokenExpiration < DateTime.UtcNow)
            {
                return BadRequest();
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.User.FirstOrDefault(u => u.Username.Equals(request.Username) && u.ResetPasswordToken.Equals(request.ResetPasswordToken));
            if (user == null)
            {
                return BadRequest();
            }


            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiration = null;
            user.Password = _encryptionService.HashPassword(request.Password);

            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: api/users/newpassword
        [HttpPost("newpassword")]
        public async Task<IActionResult> PostUserUpdatePassword([FromBody] UserUpdatePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (JwtManager.GetClaimedRole(_httpContextAccessor) != UserRoles.Conductor && JwtManager.GetClaimedGroupId(_httpContextAccessor) != request.User.GroupId)
            {
                return Unauthorized();
            }
            request.User.Password = _encryptionService.HashPassword(request.NewPassword);
            _context.Entry(request.User).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(request.User.Oid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { password = request.User.Password });
        }

        // POST: api/users
        [HttpPost("run-login-checks")]
        public async Task<IActionResult> PostInitiateUser()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                int fboId = JwtManager.GetClaimedFboId(_httpContextAccessor);

                await _pricingTemplateService.FixDefaultPricingTemplate(fboId);
            }
            catch (Exception)
            {
            }

            return Ok();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        [UserRole(UserRoles.Conductor, UserRoles.GroupAdmin, UserRoles.Primary)]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (JwtManager.GetClaimedRole(_httpContextAccessor) != UserRoles.Conductor && JwtManager.GetClaimedGroupId(_httpContextAccessor) != user.GroupId)
            {
                return Unauthorized();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpGet("checkemailexists/{emailAddress}")]
        public IActionResult CheckEmailExists([FromRoute] string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(emailAddress))
                return BadRequest(new { message = "Username or password is invalid/empty" });

            var checkUser = _context.User.FirstOrDefault(s => s.Username == emailAddress);
            if (checkUser != null)
            {
                return StatusCode(409, "Email exists");
            }
            return Ok(new { message = "Clear" });
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(_securitySettings.TokenKey);
            Response.Cookies.Delete(_securitySettings.RefreshTokenKey);

            return Ok(new { message = "Logout successful" });
        }

        [HttpPost("app-refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshAppAccessToken()
        {
            Response.Cookies.Delete(_securitySettings.TokenKey);
            Response.Cookies.Delete(_securitySettings.RefreshTokenKey);

            var authToken = Request.Cookies[_securitySettings.TokenKey];
            var refreshToken = Request.Cookies[_securitySettings.RefreshTokenKey];

            var request = new ExchangeRefreshTokenRequest
            {
                AuthToken = authToken,
                RefreshToken = refreshToken
            };
            var response = await _oAuthService.ExchangeRefreshToken(request);

            if (!response.Success)
                return Unauthorized();

            SetHttpOnlyCookie(_securitySettings.RefreshTokenKey, response.RefreshToken, _securitySettings.RefreshTokenExpirationInMinutes);
            SetHttpOnlyCookie(_securitySettings.TokenKey, response.AuthToken, _securitySettings.TokenExpirationInMinutes);

            return Ok();
        }


        private bool UserExists(int id)
        {
            return _context.Group.Any(e => e.Oid == id);
        }

        private async Task<Fbos> HandlePreLoginEvents(int fboId, int groupId)
        {
            var fbo = await _fboService.GetFboModel(fboId);

            if (fbo != null)
            {
                if (fbo.GroupId == groupId)
                {
                    fbo.LastLogin = DateTime.UtcNow;
                    await _fboService.UpdateModel(fbo);

                    var x1Integration = await _IntegrationPartners.FirstOrDefaultAsync(x => x.PartnerName == "X1");

                    if (x1Integration != null)
                    {
                        var result = await _IntegrationStatusService.GetSingleBySpec(new IntegrationStatusSpecification(x1Integration.Oid, fboId));
                        if (result != null)
                            fbo.IntegrationStatus = true;
                    }
                }
            }

            try
            {
                var group = await _context.Group.FindAsync(groupId);

                if (group.IsLegacyAccount == true)
                {
                    await _fboService.DoLegacyGroupTransition(group.Oid);

                    group.IsLegacyAccount = false;
                    await _context.SaveChangesAsync();
                }

                return fbo;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return fbo;
            }
        }

    }
}