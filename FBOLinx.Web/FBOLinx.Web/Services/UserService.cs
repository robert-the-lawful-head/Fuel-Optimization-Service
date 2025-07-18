﻿using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Auth;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Auth;
using FBOLinx.Core.Enums;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.Service.Mapping.Dto;
using Mapster;
using FBOLinx.ServiceLayer.BusinessServices.OAuth;
using System;

namespace FBOLinx.Web.Services
{
    public interface IUserService
    {
        Task<FBOLinx.DB.Models.User> GetUserByCredentials(string username, string password, bool authenticate = false, bool resetPassword = false);
        Task<FBOLinx.DB.Models.User> CreateFBOLoginIfNeeded(Fbos fboRecord);
        Task<FBOLinx.DB.Models.User> CreateGroupLoginIfNeeded(Group groupRecord);
        Task SetAppUserRefreshTokens(User user);
    }

    public class UserService : IUserService
    {
        private readonly FboLinxContext _Context;
        private IEncryptionService _EncryptionService;
        private readonly JwtManager _jwtManager;
        private readonly FBOLinx.ServiceLayer.BusinessServices.User.IUserService _userService;
        private IRepository<Group, FboLinxContext> _groupRepo;
        private IOAuthService _oAuthService;


        public UserService(FboLinxContext context, IEncryptionService encryptionService, JwtManager jwtManager, FBOLinx.ServiceLayer.BusinessServices.User.IUserService userService, IRepository<Group, FboLinxContext> groupRepo, IOAuthService oAuthService)
        {
            _EncryptionService = encryptionService;
            _Context = context;
            _jwtManager = jwtManager;
            _userService = userService;
            _groupRepo = groupRepo;
            _oAuthService = oAuthService;
        }
        
        public async Task<User> GetUserByCredentials(string username, string password, bool authenticate = false, bool resetPassword = false)
        {
            FBOLinx.DB.Models.User user = await _Context.User.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
            {
                user = await CheckForUserOnOldLogins(username, password, resetPassword);
                if (user == null)
                    return null;
            }
            else
            {
                var groupRecord = await _Context.Group.Include(x => x.Fbos).FirstOrDefaultAsync(x => x.Oid == user.GroupId);
                
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
        
        public async Task<FBOLinx.DB.Models.User> CreateFBOLoginIfNeeded(Fbos fboRecord)
        {
            FBOLinx.DB.Models.User user = await _Context.User.Where((x => x.FboId == fboRecord.Oid && x.Role == UserRoles.Primary)).FirstOrDefaultAsync();
           
            if (user != null)
                return user;

            if (string.IsNullOrEmpty(fboRecord.Username) || string.IsNullOrEmpty(fboRecord.Password))
                return null;
            //User doesn't exist for fbo - create it
            var contactRecord = await (from fc in _Context.Fbocontacts
                join c in _Context.Contacts on fc.ContactId equals c.Oid
                where fc.Fboid == fboRecord.Oid
                select c).OrderByDescending(x => x.Primary).FirstOrDefaultAsync();
            user = new FBOLinx.DB.Models.User()
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
            await _Context.User.AddAsync(user);
            await _Context.SaveChangesAsync();

            return user;
        }

        public async Task<User> CreateGroupLoginIfNeeded(Group groupRecord)
        {
            User user = await _Context.User.Where(
                (x => x.GroupId == groupRecord.Oid && (x.Role == UserRoles.Conductor || x.Role == UserRoles.GroupAdmin))).FirstOrDefaultAsync();

            if (user != null)
                return user;

            if (string.IsNullOrEmpty(groupRecord.Username) || string.IsNullOrEmpty(groupRecord.Password))
                return null;
            //User doesn't exist for group - create it
            user = new FBOLinx.DB.Models.User()
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

            await _Context.User.AddAsync(user);
            await _Context.SaveChangesAsync();

            return user;
        }
        public async Task SetAppUserRefreshTokens(User user)
        {
            DB.Models.RefreshTokens oldRefreshToken = (user.RefreshToken == null)? null : await _Context.RefreshTokens.Where(r => r.UserId == user.Oid && r.Token == user.RefreshToken.Token ).FirstOrDefaultAsync();

            if (oldRefreshToken != null) _Context.RefreshTokens.Remove(oldRefreshToken);

            RefreshTokensDto refreshToken = await _oAuthService.GenerateRefreshToken(user.Oid, user.Oid);

            await _Context.SaveChangesAsync();

            user.RefreshToken = refreshToken.Adapt<RefreshTokens>();
        }
        #region Private Methods
        private void SetAuthToken(User user)
        {
            user.Token = _jwtManager.GenerateToken(user.Oid, user.FboId, user.Role, user.GroupId);
        }

        private async Task UpdateLoginCount(User user)
        {
            user.LoginCount = user.LoginCount.GetValueOrDefault() + 1;
            _Context.SaveChanges();
        }

        private async Task<User> CheckForUserOnOldLogins(string username, string password, bool resetPassword = false)
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

            var groupRecord = await _groupRepo.Where(
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