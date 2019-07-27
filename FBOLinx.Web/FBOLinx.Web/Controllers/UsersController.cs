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

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _UserService;
        private readonly FboLinxContext _Context;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private IFileProvider _FileProvider;
        private MailSettings _MailSettings;

        public UsersController(IUserService userService, FboLinxContext context, IHttpContextAccessor httpContextAccessor, IFileProvider fileProvider, IOptions<MailSettings> mailSettings)
        {
            _UserService = userService;
            _Context = context;
            _HttpContextAccessor = httpContextAccessor;
            _MailSettings = mailSettings.Value;
            _FileProvider = fileProvider;
        }

        [HttpGet("current")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            var user = await _Context.User.FindAsync(UserService.GetClaimedUserId(_HttpContextAccessor));

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            if (string.IsNullOrEmpty(userParam.Username) || string.IsNullOrEmpty(userParam.Password))
                return BadRequest(new { message = "Username or password is invalid/empty" });

            var user = _UserService.Authenticate(userParam.Username, userParam.Password, false);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

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

            var user = await _Context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var currentRole = UserService.GetClaimedRole(_HttpContextAccessor);

            if (currentRole == Models.User.UserRoles.Conductor)
                return Ok(user);

            if (UserService.GetClaimedUserId(_HttpContextAccessor) == id)
                return Ok(user);

            if (currentRole == Models.User.UserRoles.GroupAdmin &&
                UserService.GetClaimedGroupId(_HttpContextAccessor) == user.GroupId)
                return Ok(user);

            if (currentRole == Models.User.UserRoles.Primary &&
                UserService.GetClaimedFboId(_HttpContextAccessor) == user.FboId)
                return Ok(user);

            return Unauthorized();
        }

        // GET: api/users/group/5
        [HttpGet("group/{groupId}")]
        [UserRole(new User.UserRoles[] {Models.User.UserRoles.Conductor, Models.User.UserRoles.GroupAdmin})]
        public async Task<IActionResult> GetUsersByGroupId([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = await _Context.Group.FindAsync(groupId);

            if (group != null)
                _UserService.CreateGroupLoginIfNeeded(group);

            var users = await _Context.User.Where((x => x.GroupId == groupId && x.FboId == 0)).ToListAsync();
            
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

            var fbo = await _Context.Fbos.FindAsync(fboId);

            if (fbo != null)
                _UserService.CreateFBOLoginIfNeeded(fbo);

            var users = await _Context.User.Where((x => x.FboId == fboId)).ToListAsync();

            return Ok(users);
        }

        // GET: api/users/roles
        [HttpGet("roles")]
        public async Task<IActionResult> GetUserRoles()
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

            if (id != UserService.GetClaimedUserId(_HttpContextAccessor) && UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.Conductor && UserService.GetClaimedGroupId(_HttpContextAccessor) != user.GroupId)
            {
                return BadRequest(ModelState);
            }

            _Context.Entry(user).State = EntityState.Modified;

            try
            {
                await _Context.SaveChangesAsync();
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

            if (UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.Conductor && UserService.GetClaimedGroupId(_HttpContextAccessor) != user.GroupId)
            {
                return Unauthorized();
            }

            _Context.User.Add(user);
            await _Context.SaveChangesAsync();

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

            var user = _UserService.Authenticate(request.username, "", true);
            if (user == null)
                return BadRequest(new {message = "There are no records of that username in the system"});

            string emailAddress = "";
            if (user.Username.Contains("@"))
                emailAddress = user.Username;
            if (user.FboId > 0)
            {
                var fbo = await _Context.Fbos.FindAsync(user.FboId);
                if (fbo != null)
                    emailAddress = fbo.FuelDeskEmail;
            }
            if (string.IsNullOrEmpty(emailAddress))
                return BadRequest(new { message = "No valid email address to send to.  Please contact us for assistance." });

            ResetPasswordService service =
                new ResetPasswordService(_MailSettings, _Context, _FileProvider, _HttpContextAccessor);
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

            if (UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.Conductor && UserService.GetClaimedGroupId(_HttpContextAccessor) != request.User.GroupId)
            {
                return Unauthorized();
            }
            Utilities.Hash hashUtility = new Utilities.Hash();
            request.User.Password = hashUtility.HashPassword(request.NewPassword);
            _Context.Entry(request.User).State = EntityState.Modified;

            try
            {
                await _Context.SaveChangesAsync();
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

            return Ok(new {password = request.User.Password});
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

                int fboId = UserService.GetClaimedFboId(_HttpContextAccessor);

                var existingPricingTemplates =
                    await _Context.PricingTemplate.Where(x => x.Fboid == fboId).ToListAsync();
                if (existingPricingTemplates != null && existingPricingTemplates.Count != 0)
                    return Ok();

                //Add a default pricing template - project #1c5383
                var newTemplate = new PricingTemplate()
                {
                    Default = true,
                    Fboid = fboId,
                    Name = "Posted Retail",
                    MarginType = PricingTemplate.MarginTypes.RetailMinus,
                    Notes = ""
                };

                await _Context.PricingTemplate.AddAsync(newTemplate);
                await _Context.SaveChangesAsync();

                await AddDefaultCustomerMargins(newTemplate.Oid, 1, 500);
                await AddDefaultCustomerMargins(newTemplate.Oid, 501, 750);
                await AddDefaultCustomerMargins(newTemplate.Oid, 751, 1000);
                await AddDefaultCustomerMargins(newTemplate.Oid, 1001, 99999);
            }
            catch (System.Exception exception)
            {
                //Resume the login without issue
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

            var user = await _Context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.Conductor && UserService.GetClaimedGroupId(_HttpContextAccessor) != user.GroupId)
            {
                return Unauthorized();
            }

            _Context.User.Remove(user);
            await _Context.SaveChangesAsync();

            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _Context.Group.Any(e => e.Oid == id);
        }

        private async Task AddDefaultCustomerMargins(int priceTemplateId, double min, double max)
        {
            var newPriceTier = new PriceTiers() { Min = min, Max = max, MaxEntered = max};
            await _Context.PriceTiers.AddAsync(newPriceTier);
            await _Context.SaveChangesAsync();

            var newCustomerMargin = new CustomerMargins()
            {
                Amount = 0,
                TemplateId = priceTemplateId,
                PriceTierId = newPriceTier.Oid
            };
            await _Context.CustomerMargins.AddAsync(newCustomerMargin);
            await _Context.SaveChangesAsync();

        }
    }
}