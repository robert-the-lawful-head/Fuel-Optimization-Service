using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.BusinessServices.OAuth;
using FBOLinx.ServiceLayer.DTO.Requests.FBO;
using FBOLinx.ServiceLayer.DTO.Responses.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Auth
{
    public interface IAuthService
    {
        Task<AuthenticatedLinkResponse> CreateAuthenticatedLink(int handlerId);
    }

    public class AuthService : IAuthService
    {
        private readonly IGroupFboService _GroupFboService;
        private readonly IOAuthService _OAuthService;
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;

        public AuthService(FboLinxContext context, DegaContext degaContext,
            IGroupFboService groupFboService,
            IOAuthService oAuthService)
        {
            _context = context;
            _degaContext = degaContext;
            _GroupFboService = groupFboService;
            _OAuthService = oAuthService;
        }
        public async Task<AuthenticatedLinkResponse> CreateAuthenticatedLink(int handlerId)
        {
            var fbo = await _context.Fbos.Where(x => x.AcukwikFBOHandlerId == handlerId).FirstOrDefaultAsync();
            var importedFboEmail = new ImportedFboEmails();

            if (fbo == null)
            {
                var acukwikFbo = await _degaContext.AcukwikFbohandlerDetail.Where(x => x.HandlerId == handlerId).FirstOrDefaultAsync();
                if (acukwikFbo == null)
                {
                    importedFboEmail.Email = "FBO not found";
                    return new AuthenticatedLinkResponse() { FboEmails = importedFboEmail.Email };
                }

                importedFboEmail = await _degaContext.ImportedFboEmails.Where(x => x.AcukwikFBOHandlerId == handlerId).FirstOrDefaultAsync();
                if (importedFboEmail == null || importedFboEmail.Oid == 0)
                {
                    importedFboEmail.Email = "No email found";
                    return new AuthenticatedLinkResponse() { FboEmails = importedFboEmail.Email };
                }

                var acukwikAirport = await _degaContext.AcukwikAirports.Where(x => x.Oid == acukwikFbo.AirportId).FirstOrDefaultAsync();

                if (importedFboEmail != null)
                {
                    var newFbo = new SingleFboRequest() { Group = acukwikFbo.HandlerLongName, Icao = acukwikAirport.Icao, Iata = acukwikAirport.Iata, Fbo = acukwikFbo.HandlerLongName, AcukwikFboHandlerId = handlerId, AccountType = Core.Enums.AccountTypes.NonRevFBO, FuelDeskEmail = importedFboEmail.Email };
                    fbo = await _GroupFboService.CreateNewFbo(newFbo);

                    DB.Models.User newUser = new DB.Models.User() { FboId = fbo.Oid, Role = Core.Enums.UserRoles.NonRev, Username = importedFboEmail.Email.Trim(), FirstName = importedFboEmail.Email, GroupId = fbo.GroupId };
                    await _context.User.AddAsync(newUser);

                    Fbopreferences fbopreferences = new Fbopreferences();
                    fbopreferences = new Fbopreferences() { Fboid = fbo.Oid, EnableJetA = true, EnableSaf = false, OrderNotificationsEnabled = true };
                    await _context.Fbopreferences.AddAsync(fbopreferences);

                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(fbo.FuelDeskEmail))
                    importedFboEmail.Email = fbo.FuelDeskEmail;
                else
                {
                    importedFboEmail = await _degaContext.ImportedFboEmails.Where(x => x.AcukwikFBOHandlerId == handlerId).FirstOrDefaultAsync();
                    if (importedFboEmail == null || importedFboEmail.Oid == 0)
                    {
                        importedFboEmail.Email = "No email found";
                        return new AuthenticatedLinkResponse() { FboEmails = importedFboEmail.Email };
                    }
                }
            }

            var user = await _context.User.Where(x => x.FboId == fbo.Oid && (x.Role == Core.Enums.UserRoles.Primary || x.Role == Core.Enums.UserRoles.NonRev)).FirstOrDefaultAsync();

            //Return URL with authentication for 7 days
            AccessTokens accessToken = await _OAuthService.GenerateAccessToken(user, 10080);
            return new AuthenticatedLinkResponse() { AccessToken = accessToken.AccessToken, FboEmails = importedFboEmail.Email.Trim(), Fbo = fbo.Fbo };
        }
    }
}
