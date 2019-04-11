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

namespace FBOLinx.Web.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password, bool resetPassword);
        System.Collections.Generic.IEnumerable<User> GetAll();
        User CreateFBOLoginIfNeeded(Fbos fboRecord);
        User CreateGroupLoginIfNeeded(Group groupRecord);
    }

    public class UserService : IUserService
    {

        private readonly AppSettings _AppSettings;
        private readonly FboLinxContext _Context;

        public UserService(FboLinxContext context, IOptions<AppSettings> appSettings)
        {
            _Context = context;
            _AppSettings = appSettings.Value;
        }

        public User Authenticate(string username, string password, bool resetPassword = false)
        {
            FBOLinx.Web.Utilities.Hash hashUtility = new FBOLinx.Web.Utilities.Hash();
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
                if (!resetPassword && !hashUtility.VerifyHashedPassword(user.Password, password))
                    return null;
            }

            SetAuthToken(user);
            
            return user;
        }

        public System.Collections.Generic.IEnumerable<User> GetAll()
        {
            // return users without passwords
            return _Context.User.Where(x => x.Password == null);
        }

        public User CreateFBOLoginIfNeeded(Fbos fboRecord)
        {
            FBOLinx.Web.Utilities.Hash hashUtility = new FBOLinx.Web.Utilities.Hash();
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
                Password = hashUtility.HashPassword(fboRecord.Password),
                Role = User.UserRoles.Primary,
                Username = fboRecord.Username
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
            FBOLinx.Web.Utilities.Hash hashUtility = new FBOLinx.Web.Utilities.Hash();
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
                Password = hashUtility.HashPassword(groupRecord.Password),
                Role = (groupRecord.GroupName == "FBOLinx") ? User.UserRoles.Conductor : User.UserRoles.GroupAdmin,
                Username = groupRecord.Username
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

            // remove password before returning
            user.Password = null;
        }

        private User CheckForUserOnOldLogins(string username, string password, bool resetPassword = false)
        {
            var fboRecord = _Context.Fbos.Where((x => x.Username == username && (x.Password == password || resetPassword))).FirstOrDefault();

            if (fboRecord != null)
            {
                return CreateFBOLoginIfNeeded(fboRecord);
            }

            var groupRecord = _Context.Group.Where((x => x.Username == username && (x.Password == password || resetPassword)))
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

        public static Models.User.UserRoles GetClaimedRole(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return (Models.User.UserRoles) System.Convert.ToInt16(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value);
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
