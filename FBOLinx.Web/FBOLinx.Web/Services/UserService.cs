using System.Linq;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Auth;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.Web.Auth;
using FBOLinx.Core.Enums;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.User;
using FBOLinx.DB.Specifications.ServiceOrder;

namespace FBOLinx.Web.Services
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByCredentials(string username, string password, bool authenticate = false, bool resetPassword = false);
        Task<UserDTO> CreateFBOLoginIfNeeded(Fbos fboRecord);
        Task<UserDTO> CreateGroupLoginIfNeeded(Group groupRecord);
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _AppSettings;
        private readonly FboLinxContext _Context;
        private IEncryptionService _EncryptionService;
        private readonly JwtManager _jwtManager;
        private readonly FBOLinx.ServiceLayer.BusinessServices.User.IUserService _userService;

        public UserService(FboLinxContext context, IOptions<AppSettings> appSettings, IEncryptionService encryptionService, JwtManager jwtManager, FBOLinx.ServiceLayer.BusinessServices.User.IUserService userService)
        {
            _EncryptionService = encryptionService;
            _Context = context;
            _AppSettings = appSettings.Value;
            _jwtManager = jwtManager;
            _userService = userService;
        }
        
        public async Task<UserDTO> GetUserByCredentials(string username, string password, bool authenticate = false, bool resetPassword = false)
        {
            UserDTO user = await _userService.GetSingleBySpec(new UserByUsernameSpecification(username));
            
            if (user == null)
            {
                user = await CheckForUserOnOldLogins(username, password, resetPassword);
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

            if (authenticate)
            {
                SetAuthToken(user);
                await UpdateLoginCount(user);
            }

            return user;
        }
        
        public async Task<UserDTO> CreateFBOLoginIfNeeded(Fbos fboRecord)
        {
            var user = await _userService.GetSingleBySpec(new PrimaryUserByFboIdSpecification(fboRecord.Oid));
           
            if (user != null)
                return user;

            if (string.IsNullOrEmpty(fboRecord.Username) || string.IsNullOrEmpty(fboRecord.Password))
                return null;
            //User doesn't exist for fbo - create it
            var contactRecord = await (from fc in _Context.Fbocontacts
                join c in _Context.Contacts on fc.ContactId equals c.Oid
                where fc.Fboid == fboRecord.Oid
                select c).OrderByDescending(x => x.Primary).FirstOrDefaultAsync();
            user = new UserDTO()
            {
                FirstName = contactRecord?.FirstName,
                FboId = fboRecord.Oid,
                GroupId = fboRecord.GroupId,
                LastName = contactRecord?.LastName,
                Password = _EncryptionService.HashPassword(fboRecord.Password),
                Role = UserRoles.Primary,
                Username = fboRecord.Username,
                Active = true
            };
            await _userService.AddAsync(user);

            //fboRecord.Password = "";
            //fboRecord.Username = "";
            //_Context.Fbos.Update(fboRecord);

            //Return the newly created user that transitioned from the FBOs table
            return user;
        }

        public async Task<UserDTO> CreateGroupLoginIfNeeded(Group groupRecord)
        {
            var user = await _userService.GetSingleBySpec(new ConductorOrGroupAdminUserByGroupIdSpecification(groupRecord.Oid));
            if (user != null)
                return user;

            if (string.IsNullOrEmpty(groupRecord.Username) || string.IsNullOrEmpty(groupRecord.Password))
                return null;
            //User doesn't exist for group - create it
            user = new UserDTO()
            {
                FirstName = groupRecord.GroupName,
                FboId = 0,
                GroupId = groupRecord.Oid,
                LastName = "",
                Password = _EncryptionService.HashPassword(groupRecord.Password),
                Role = (!string.IsNullOrEmpty(groupRecord.GroupName) && (groupRecord.GroupName == "FBOLinx" || groupRecord.GroupName.ToLower().Contains("fbolinx conductor"))) ? UserRoles.Conductor : UserRoles.GroupAdmin,
                Username = groupRecord.Username,
                Active = true
            };

            await _userService.AddAsync(user);
            //groupRecord.Password = "";
            //groupRecord.Username = "";
            //_Context.Group.Update(groupRecord);

            //Return the newly created user that transitioned from the Group table
            return user;
        }

        #region Private Methods
        private void SetAuthToken(UserDTO user)
        {
            user.Token = _jwtManager.GenerateToken(user.Oid, user.FboId, user.Role, user.GroupId);
        }

        private async Task UpdateLoginCount(UserDTO user)
        {
            user.LoginCount = user.LoginCount.GetValueOrDefault() + 1;
            await _userService.UpdateAsync(user);
        }

        private async Task<UserDTO> CheckForUserOnOldLogins(string username, string password, bool resetPassword = false)
        {
            var fbo = from f in _Context.Fbos
                      join g in _Context.Group on f.GroupId equals g.Oid
                      where g.Isfbonetwork == false && f.Username == username && (f.Password == password || resetPassword)
                      select f;

            var fboRecord = await fbo.FirstOrDefaultAsync();
            
            if (fboRecord != null)
            {
                return await CreateFBOLoginIfNeeded(fboRecord);
            }

            var groupRecord = await _Context.Group.Where(
                    (x => x.Isfbonetwork == false && x.Username == username && (x.Password == password || resetPassword))).FirstOrDefaultAsync();

            if (groupRecord != null)
            {
                return await CreateGroupLoginIfNeeded(groupRecord);
            }

            return null;
        }
        #endregion

        #region Static Methods
        
        #endregion
    }
}
