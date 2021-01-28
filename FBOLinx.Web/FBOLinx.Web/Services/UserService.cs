using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using FBOLinx.Web.Models;
using FBOLinx.Web.Configurations;
using FBOLinx.Web.Data;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Auth;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.Web.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password, bool resetPassword);
        User CheckUserByCredentials(string username, string password);
        System.Collections.Generic.IEnumerable<User> GetAll();
        User CreateFBOLoginIfNeeded(Fbos fboRecord);
        User CreateGroupLoginIfNeeded(Group groupRecord);
    }

    public class UserService : IUserService
    {

        private readonly AppSettings _AppSettings;
        private readonly FboLinxContext _Context;
        private EncryptionService _EncryptionService;

        public UserService(FboLinxContext context, IOptions<AppSettings> appSettings, EncryptionService encryptionService)
        {
            _EncryptionService = encryptionService;
            _Context = context;
            _AppSettings = appSettings.Value;
        }

        public async Task<User> Authenticate(string username, string password, bool resetPassword = false)
        {
            var user = _Context.User.SingleOrDefault(x => x.Username == username);

            // return null if user not found
            if (user == null)
            {
                user = CheckForUserOnOldLogins(username, password, resetPassword);
                if (user == null)
                    return null;
            }
            else
            {
                var groupRecord = await _Context.Group.FirstOrDefaultAsync(x => x.Oid == user.GroupId);

                //return null if Paragon
                if (groupRecord.Isfbonetwork.GetValueOrDefault())
                    return null;

                if (!resetPassword && !_EncryptionService.VerifyHashedPassword(user.Password, password))
                {
                    return null;
                }
            }

            if (user.Active != true)
            {
                return null;
            }

            UpdateLoginCount(user);
            SetAuthToken(user);
            
            return user;
        }

        public User CheckUserByCredentials(string username, string password)
        {
            var user = _Context.User.SingleOrDefault(x => x.Username == username);
            var groupRecord = _Context.Group.Where(x => x.Oid == user.GroupId).FirstOrDefault();

            //return null if Paragon
            if (groupRecord.Isfbonetwork.GetValueOrDefault())
                return null;

            // return null if user not found
            if (user == null)
            {
                user = CheckForUserOnOldLogins(username, password);
                if (user == null)
                    return null;
            }
            else
            {
                if (!_EncryptionService.VerifyHashedPassword(user.Password, password))
                {
                    return null;
                }
            }

            if (user.Active != true)
            {
                return null;
            }
            return user;
        }

        public System.Collections.Generic.IEnumerable<User> GetAll()
        {
            // return users without passwords
            return _Context.User.Where(x => x.Password == null);
        }

        public User CreateFBOLoginIfNeeded(Fbos fboRecord)
        {
            User user = _Context.User.Where((x => x.FboId == fboRecord.Oid && x.Role == User.UserRoles.Primary))
                .FirstOrDefault();
            if (user != null)
                return user;

            if (string.IsNullOrEmpty(fboRecord.Username) || string.IsNullOrEmpty(fboRecord.Password))
                return null;
            //User doesn't exist for fbo - create it
            var contactRecord = (from fc in _Context.Fbocontacts
                join c in _Context.Contacts on fc.ContactId equals c.Oid
                where fc.Fboid == fboRecord.Oid
                select c).OrderByDescending(x => x.Primary).FirstOrDefault();
            user = new User()
            {
                FirstName = contactRecord?.FirstName,
                FboId = fboRecord.Oid,
                GroupId = fboRecord.GroupId,
                LastName = contactRecord?.LastName,
                Password = _EncryptionService.HashPassword(fboRecord.Password),
                Role = User.UserRoles.Primary,
                Username = fboRecord.Username,
                Active = true
            };
            _Context.User.Add(user);
            //fboRecord.Password = "";
            //fboRecord.Username = "";
            //_Context.Fbos.Update(fboRecord);
            _Context.SaveChanges();

            //Return the newly created user that transitioned from the FBOs table
            return user;
        }

        public User CreateGroupLoginIfNeeded(Group groupRecord)
        {
            User user = _Context.User.Where((x => x.GroupId == groupRecord.Oid && (x.Role == User.UserRoles.Conductor || x.Role == User.UserRoles.GroupAdmin))).FirstOrDefault();
            if (user != null)
                return user;

            if (string.IsNullOrEmpty(groupRecord.Username) || string.IsNullOrEmpty(groupRecord.Password))
                return null;
            //User doesn't exist for group - create it
            user = new User()
            {
                FirstName = groupRecord.GroupName,
                FboId = 0,
                GroupId = groupRecord.Oid,
                LastName = "",
                Password = _EncryptionService.HashPassword(groupRecord.Password),
                Role = (!string.IsNullOrEmpty(groupRecord.GroupName) && (groupRecord.GroupName == "FBOLinx" || groupRecord.GroupName.ToLower().Contains("fbolinx conductor"))) ? User.UserRoles.Conductor : User.UserRoles.GroupAdmin,
                Username = groupRecord.Username,
                Active = true
            };

            _Context.User.Add(user);
            //groupRecord.Password = "";
            //groupRecord.Username = "";
            //_Context.Group.Update(groupRecord);
            _Context.SaveChanges();

            //Return the newly created user that transitioned from the Group table
            return user;
        }

        #region Private Methods
        private void SetAuthToken(User user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Oid.ToString()),
                    new Claim(ClaimTypes.Role, ((short) user.Role).ToString()),
                    new Claim(ClaimTypes.GroupSid, user.GroupId.ToString()),
                    new Claim(ClaimTypes.Sid, user.FboId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
        }

        private void UpdateLoginCount(User user)
        {
            user.LoginCount = user.LoginCount.GetValueOrDefault() + 1;
            _Context.SaveChanges();
        }

        private User CheckForUserOnOldLogins(string username, string password, bool resetPassword = false)
        {
            var fbo = from f in _Context.Fbos
                      join g in _Context.Group on f.GroupId equals g.Oid
                      where g.Isfbonetwork == false && f.Username == username && (f.Password == password || resetPassword)
                      select f;

            var fboRecord = fbo.FirstOrDefault();
            
            if (fboRecord != null)
            {
                return CreateFBOLoginIfNeeded(fboRecord);
            }

            var groupRecord = _Context.Group.Where((x => x.Isfbonetwork == false && x.Username == username && (x.Password == password || resetPassword)))
                .FirstOrDefault();

            if (groupRecord != null)
            {
                return CreateGroupLoginIfNeeded(groupRecord);
            }

            return null;
        }
        #endregion

        #region Static Methods
        public static int GetClaimedUserId(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return System.Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                    .Value);
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        public static User.UserRoles GetClaimedRole(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return (User.UserRoles) System.Convert.ToInt16(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value);
            }
            catch (System.Exception)
            {
                return User.UserRoles.NotSet;
            }
        }

        public static int GetClaimedFboId(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return System.Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value);
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        public static int GetClaimedGroupId(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return System.Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.GroupSid).Value);
            }
            catch (System.Exception)
            {
                return 0;
            }
        }
        #endregion
    }
}
