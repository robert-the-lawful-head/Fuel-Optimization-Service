using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Configurations;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using FBOLinx.Web.Models.Responses;
using System.Security.Cryptography;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly FboLinxContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileProvider _fileProvider;
        private readonly MailSettings _MailSettings;
        private IServiceProvider _Services;

        public UsersController(IUserService userService, FboLinxContext context, IHttpContextAccessor httpContextAccessor, IFileProvider fileProvider, IOptions<MailSettings> mailSettings, IServiceProvider services)
        {
            _userService = userService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _MailSettings = mailSettings.Value;
            _fileProvider = fileProvider;
            _Services = services;
        }

        [HttpGet("current")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            var user = await _context.User.FindAsync(UserService.GetClaimedUserId(_httpContextAccessor));

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] User userParam)
        {
            if (string.IsNullOrEmpty(userParam.Username) || string.IsNullOrEmpty(userParam.Password))
                return BadRequest(new { message = "Username or password is invalid/empty" });

            var user = _userService.Authenticate(userParam.Username, userParam.Password, false);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            var fbo = _context.Fbos.FirstOrDefault(f => f.GroupId == user.GroupId && f.Oid == user.FboId);
            if (fbo != null)
            {
                fbo.LastLogin = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            try
            {
                var group = _context.Group.Find(user.GroupId);

                if (group.IsLegacyAccount == true)
                {
                    await DoLegacyGroupTransition(group.Oid);

                    group.IsLegacyAccount = false;
                    await _context.SaveChangesAsync();
                }
            } catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }

            return Ok(user);
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

            var currentRole = UserService.GetClaimedRole(_httpContextAccessor);

            if (currentRole == Models.User.UserRoles.Conductor)
                return Ok(user);

            if (UserService.GetClaimedUserId(_httpContextAccessor) == id)
                return Ok(user);

            if (currentRole == Models.User.UserRoles.GroupAdmin &&
                UserService.GetClaimedGroupId(_httpContextAccessor) == user.GroupId)
                return Ok(user);

            if (currentRole == Models.User.UserRoles.Primary &&
                UserService.GetClaimedFboId(_httpContextAccessor) == user.FboId)
                return Ok(user);

            return Unauthorized();
        }

        // GET: api/users/group/5
        [HttpGet("group/{groupId}")]
        [UserRole(new User.UserRoles[] { Models.User.UserRoles.Conductor, Models.User.UserRoles.GroupAdmin })]
        public async Task<IActionResult> GetUsersByGroupId([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = await _context.Group.FindAsync(groupId);

            if (group != null)
                _userService.CreateGroupLoginIfNeeded(group);

            var users = await _context.User.Where((x => x.GroupId == groupId && x.FboId == 0)).ToListAsync();

            return Ok(users);
        }

        // GET: api/users/fbo/5
        [HttpGet("fbo/{fboId}")]
        [UserRole(new User.UserRoles[] { Models.User.UserRoles.Conductor, Models.User.UserRoles.GroupAdmin, Models.User.UserRoles.Primary })]
        public async Task<IActionResult> GetUsersByFboId([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbo = await _context.Fbos.FindAsync(fboId);

            if (fbo != null)
                _userService.CreateFBOLoginIfNeeded(fbo);

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

            var roles = Utilities.Enum.GetDescriptions(typeof(Models.User.UserRoles));

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

            if (id != UserService.GetClaimedUserId(_httpContextAccessor) && UserService.GetClaimedRole(_httpContextAccessor) != Models.User.UserRoles.Conductor && UserService.GetClaimedGroupId(_httpContextAccessor) != user.GroupId)
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
        [UserRole(Models.User.UserRoles.Conductor, Models.User.UserRoles.GroupAdmin, Models.User.UserRoles.Primary)]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (UserService.GetClaimedRole(_httpContextAccessor) != Models.User.UserRoles.Conductor && UserService.GetClaimedGroupId(_httpContextAccessor) != user.GroupId)
            {
                return Unauthorized();
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Oid }, user);
        }

        // POST: api/users/resetpassword
        [AllowAnonymous]
        [HttpPost("resetpassword")]
        public async Task<IActionResult> PostResetPassword([FromBody] UserResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userService.Authenticate(request.username, "", true);
            if (user == null)
                return BadRequest(new { message = "There are no records of that username in the system" });

            string emailAddress = "";
            if (user.Username.Contains("@"))
                emailAddress = user.Username;
            if (user.FboId > 0)
            {
                var fbo = await _context.Fbos.FindAsync(user.FboId);
                if (fbo != null)
                    emailAddress = fbo.FuelDeskEmail;
            }
            if (string.IsNullOrEmpty(emailAddress))
                return BadRequest(new { message = "No valid email address to send to.  Please contact us for assistance." });

            ResetPasswordService service =
                new ResetPasswordService(_MailSettings, _context, _fileProvider, _httpContextAccessor);
            await service.SendResetPasswordEmailAsync(user, emailAddress);

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

            if (UserService.GetClaimedRole(_httpContextAccessor) != Models.User.UserRoles.Conductor && UserService.GetClaimedGroupId(_httpContextAccessor) != request.User.GroupId)
            {
                return Unauthorized();
            }
            Utilities.Hash hashUtility = new Utilities.Hash();
            request.User.Password = hashUtility.HashPassword(request.NewPassword);
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

                int fboId = UserService.GetClaimedFboId(_httpContextAccessor);

                PricingTemplateService pricingTemplateService = new PricingTemplateService(_context);

                await pricingTemplateService.FixDefaultPricingTemplate(fboId);
            }
            catch (Exception)
            {
            }

            return Ok();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        [UserRole(Models.User.UserRoles.Conductor, Models.User.UserRoles.GroupAdmin, Models.User.UserRoles.Primary)]
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

            if (UserService.GetClaimedRole(_httpContextAccessor) != Models.User.UserRoles.Conductor && UserService.GetClaimedGroupId(_httpContextAccessor) != user.GroupId)
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

        private bool UserExists(int id)
        {
            return _context.Group.Any(e => e.Oid == id);
        }

        private async Task DoLegacyGroupTransition(int groupId)
        {
            var customerMargins = await (from cm in _context.CustomerMargins
                                         join pt in _context.PricingTemplate on new { cm.TemplateId, marginType = (short)1 } equals new { TemplateId = pt.Oid, marginType = (short)pt.MarginType }
                                         where cm.Amount < 0
                                         select cm).ToListAsync();
            foreach (var cm in customerMargins)
            {
                cm.Amount = -cm.Amount;
            }
            _context.CustomerMargins.UpdateRange(customerMargins);
            await _context.SaveChangesAsync();

            var deleteAircraftPricesTask = Task.Run(async () =>
            {
                using (var scope = _Services.CreateScope())
                {
                    var groupTransitionService = scope.ServiceProvider.GetRequiredService<GroupTransitionService>();
                    await groupTransitionService.PerformLegacyGroupTransition(groupId);                    
                }
            });
        }
    }
}
