using Azure.Core;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.AcukwikAirport;
using FBOLinx.DB.Specifications.Aircraft;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.DB.Specifications.User;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.BusinessServices.OAuth;
using FBOLinx.ServiceLayer.BusinessServices.User;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.Requests.FBO;
using FBOLinx.ServiceLayer.DTO.Responses.Auth;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RTools_NTS.Util;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Auth
{
    public interface IAuthService
    {
        Task<AuthenticatedLinkResponse> CreateAuthenticatedLink(int handlerId);
        Task<string> CreateNonRevAccount(int handlerId, string fbo = "");
    }

    public class AuthService : IAuthService
    {
        private readonly IGroupFboService _GroupFboService;
        private readonly IOAuthService _OAuthService;
        private readonly IFboService _fboService;
        private readonly IAcukwikFboHandlerDetailService _acukwikFbohandlerDetailService;
        private readonly IUserService _userService;
        private readonly IAirportService _airportService;
        private readonly IFboPreferencesService _fboPreferencesService;
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private IMailService _MailService;
        private IWebHostEnvironment _env;

        public AuthService(FboLinxContext context, DegaContext degaContext,
            IGroupFboService groupFboService,
            IOAuthService oAuthService,
            IFboService fboService,
            IAcukwikFboHandlerDetailService acukwikFbohandlerDetailService,
            IUserService userService,
            IAirportService airportService,
            IFboPreferencesService fboPreferencesService, IMailService mailService, IWebHostEnvironment env)
        {
            _context = context;
            _degaContext = degaContext;
            _GroupFboService = groupFboService;
            _OAuthService = oAuthService;
            _fboService = fboService;
            _acukwikFbohandlerDetailService = acukwikFbohandlerDetailService;
            _userService = userService;
            _airportService = airportService;
            _fboPreferencesService = fboPreferencesService;
            _MailService = mailService;
            _env = env;
        }
        public async Task<AuthenticatedLinkResponse> CreateAuthenticatedLink(int handlerId)
        {
            var fbo = await _fboService.GetSingleBySpec(new FboByAcukwikHandlerIdSpecification(handlerId));
            var importedFboEmail = new ImportedFboEmails();
            var acukwikFbo = await _acukwikFbohandlerDetailService.GetSingleBySpec(new AcukwikFboHandlerDetailSpecification(handlerId));

            if (fbo == null)
            {
                importedFboEmail.Email = await CreateNonRevAccount(handlerId);

                if (!importedFboEmail.Email.Contains("@"))
                    return new AuthenticatedLinkResponse() { FboEmails = importedFboEmail.Email };

                fbo = await _fboService.GetSingleBySpec(new FboByAcukwikHandlerIdSpecification(handlerId));
            }
            else
            {
                if (!String.IsNullOrEmpty(fbo.FuelDeskEmail))
                    importedFboEmail.Email = fbo.FuelDeskEmail;
                else
                {
                    if (String.IsNullOrEmpty(acukwikFbo.HandlerEmail))
                    {
                        importedFboEmail.Email = "No email found";
                        return new AuthenticatedLinkResponse() { FboEmails = importedFboEmail.Email };
                    }
                    importedFboEmail.Email = acukwikFbo.HandlerEmail;
                }
            }

            var user = await _userService.GetSingleBySpec(new PrimaryOrNonRevUserByFboIdSpecification(fbo.Oid));

            if (user == null)
            {
                user = new UserDTO() { FboId = fbo.Oid, Role = Core.Enums.UserRoles.Primary, Username = importedFboEmail.Email.Trim(), FirstName = importedFboEmail.Email, GroupId = fbo.GroupId };
                user = await _userService.AddAsync(user);
            }
            
            if (!fbo.Active.GetValueOrDefault())
            {
                fbo.AccountType = Core.Enums.AccountTypes.NonRevFBO;
                fbo.Active = true;
                user.Role = Core.Enums.UserRoles.NonRev;
                user.Active = true;

                await _fboService.UpdateAsync(fbo);
                await _userService.UpdateAsync(user);
            }
            else if (fbo.Suspended.GetValueOrDefault())
            {
                fbo.AccountType = Core.Enums.AccountTypes.NonRevFBO;
                user.Role = Core.Enums.UserRoles.NonRev;

                await _fboService.UpdateAsync(fbo);
                await _userService.UpdateAsync(user);
            }

            //Return URL with authentication for 7 days
            //AccessTokensDto accessToken = await _OAuthService.GenerateAccessToken(user, 10080);
            //return new AuthenticatedLinkResponse() { AccessToken = accessToken.AccessToken, FboEmails = importedFboEmail.Email.Trim(), Fbo = fbo.Fbo };
            return new AuthenticatedLinkResponse() { FboEmails = importedFboEmail.Email.Trim(), Fbo = fbo.Fbo };
        }

        public async Task<string> CreateNonRevAccount(int handlerId, string icao = "")
        {
            var fbo = new FbosDto();
            var importedFboEmail = new ImportedFboEmails();
            var acukwikFbo = await _acukwikFbohandlerDetailService.GetSingleBySpec(new AcukwikFboHandlerDetailSpecification(handlerId));

            if (acukwikFbo == null)
            {
                importedFboEmail.Email = "FBO not found";
                return importedFboEmail.Email;
            }

            if (String.IsNullOrEmpty(acukwikFbo.HandlerEmail))
            {
                importedFboEmail.Email = "No email found";

                if (icao == "")
                    return importedFboEmail.Email;

                if (!_env.IsProduction())
                    await SendEmailToLucas(icao, acukwikFbo.HandlerLongName);
                acukwikFbo.HandlerEmail = "N/A";
            }

            importedFboEmail.Email = acukwikFbo.HandlerEmail;

            var acukwikAirport = await _airportService.GetSingleBySpec(new AcukwikAirportByOidSpecification(acukwikFbo.AirportId.GetValueOrDefault()));

            var newFbo = new SingleFboRequest() { Group = acukwikFbo.HandlerLongName, Icao = acukwikAirport.Icao, Iata = acukwikAirport.Iata, Fbo = acukwikFbo.HandlerLongName, AcukwikFboHandlerId = handlerId, FuelDeskEmail = importedFboEmail.Email };
            fbo = await _GroupFboService.CreateNewFbo(newFbo, AccountTypes.NonRevFBO);

            UserDTO newUser = new UserDTO() { FboId = fbo.Oid, Role = Core.Enums.UserRoles.NonRev, Username = importedFboEmail.Email.Trim(), FirstName = importedFboEmail.Email, GroupId = fbo.GroupId };
            await _userService.AddAsync(newUser);

            FboPreferencesDTO fbopreferences = new FboPreferencesDTO();
            fbopreferences = new FboPreferencesDTO() { Fboid = fbo.Oid, EnableJetA = true, EnableSaf = false, OrderNotificationsEnabled = true };
            await _fboPreferencesService.AddAsync(fbopreferences);

            return importedFboEmail.Email;
        }

        private async Task SendEmailToLucas(string icao, string fbo)
        {
            FBOLinxMailMessage mailMessage = new FBOLinxMailMessage();
            string body = "Hi Lucas,<br/>";
            body += "This FBO ( " + fbo + " ) at " + icao + " is missing an email address. It has been added to the FBOlinx system with a user N/A as the username";
            mailMessage.From = new MailAddress("donotreply@fbolinx.com");
            mailMessage.To.Add(new MailAddress("lucas@fbolinx.com"));
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "Missing email for " + fbo + " at " + icao + " on FBOlinx";

            await _MailService.SendAsync(mailMessage);
        }
    }
}
